using LeagueToolkit.Helpers;
using LeagueToolkit.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using XXHash3NET;
using ZstdSharp;

namespace LeagueToolkit.IO.WadFile
{
    public class WadEntryBuilder
    {
        public WadEntryType EntryType { get; private set; }
        public ulong PathXXHash { get; private set; }

        public Stream DataStream { get; internal set; }
        public int CompressedSize { get; internal set; }
        public int UncompressedSize { get; internal set; }
        internal bool _isGenericDataStream = false;
        internal uint _dataOffset;

        public string FileRedirection { get; private set; }

        public WadEntryChecksumType ChecksumType { get; internal set; }
        public byte[] Checksum { get; internal set; }

        public WadEntryBuilder(WadEntryChecksumType checksumType)
        {
            this.ChecksumType = checksumType;
        }
        public WadEntryBuilder(WadEntry entry)
        {
            this.ChecksumType = entry.ChecksumType;
            WithPathXXHash(entry.XXHash);

            switch (entry.Type)
            {
                case WadEntryType.Uncompressed: WithUncompressedDataStream(entry.GetDataHandle().GetDecompressedStream()); break;
                case WadEntryType.GZipCompressed: WithGZipDataStream(entry.GetDataHandle().GetCompressedStream(), entry.CompressedSize, entry.UncompressedSize); break;
                case WadEntryType.ZStandardCompressed: WithZstdDataStream(entry.GetDataHandle().GetCompressedStream(), entry.CompressedSize, entry.UncompressedSize); break;
                case WadEntryType.FileRedirection: WithFileRedirection(entry.FileRedirection); break;
            }
        }
        public WadEntryBuilder WithPath(string path)
        {
            return WithPathXXHash(XXHash.XXH64(Encoding.UTF8.GetBytes(path.ToLower())));
        }
        public WadEntryBuilder WithPathXXHash(ulong hash)
        {
            this.PathXXHash = hash;

            return this;
        }

        public WadEntryBuilder WithZstdDataStream(Stream stream, int compressedSize, int uncompressedSize)
        {
            this.EntryType = WadEntryType.ZStandardCompressed;
            this.DataStream = stream;
            this.CompressedSize = compressedSize;
            this.UncompressedSize = uncompressedSize;

            ComputeChecksum();

            return this;
        }
        public WadEntryBuilder WithGZipDataStream(Stream stream, int compressedSize, int uncompressedSize)
        {
            this.EntryType = WadEntryType.GZipCompressed;
            this.DataStream = stream;
            this.CompressedSize = compressedSize;
            this.UncompressedSize = uncompressedSize;

            ComputeChecksum();

            return this;
        }
        public WadEntryBuilder WithUncompressedDataStream(Stream stream)
        {
            this.EntryType = WadEntryType.Uncompressed;
            this.DataStream = stream;
            this.CompressedSize = this.UncompressedSize = (int)stream.Length;

            ComputeChecksum();

            return this;
        }
        public WadEntryBuilder WithFileDataStream(string fileLocation) => WithFileDataStream(File.OpenRead(fileLocation));
        public WadEntryBuilder WithFileDataStream(FileStream stream)
        {
            string filePath = stream.Name;

            return WithGenericDataStream(filePath, stream);
        }
        public WadEntryBuilder WithGenericDataStream(string path, Stream stream)
        {
            this.EntryType = Utilities.GetExtensionWadCompressionType(Path.GetExtension(path));
            this.DataStream = stream;
            this._isGenericDataStream = true;
            this.Checksum = new byte[8];

            return this;
        }

        public WadEntryBuilder WithFileRedirection(string fileRedirection)
        {
            this.EntryType = WadEntryType.FileRedirection;
            this.FileRedirection = fileRedirection;
            this.CompressedSize = this.UncompressedSize = fileRedirection.Length + 4;
            this.Checksum = new byte[8];

            return this;
        }

        internal void ComputeChecksum()
        {
            if(this.ChecksumType == WadEntryChecksumType.SHA256)
            {
                using (SHA256 sha = SHA256.Create())
                {
                    this.DataStream.Seek(0, SeekOrigin.Begin);

                    this.Checksum = sha.ComputeHash(this.DataStream).Take(8).ToArray();
                }
            }
            else if(this.ChecksumType == WadEntryChecksumType.XXHash3)
            {
                byte[] data = new byte[this.DataStream.Length];

                this.DataStream.Seek(0, SeekOrigin.Begin);
                this.DataStream.Read(data, 0, data.Length);

                this.Checksum = BitConverter.GetBytes(XXHash3.Hash64(data));
            }
        }
    }
}