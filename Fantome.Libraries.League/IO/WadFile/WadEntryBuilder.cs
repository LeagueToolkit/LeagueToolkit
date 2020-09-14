using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ZstdSharp;

namespace Fantome.Libraries.League.IO.WadFile
{
    public class WadEntryBuilder
    {
        public WadEntryType EntryType { get; private set; }
        public ulong PathXXHash { get; private set; }

        public Stream DataStream { get; private set; }
        public int CompressedSize { get; private set; }
        public int UncompressedSize { get; private set; }

        public string FileRedirection { get; private set; }

        public byte[] Sha256Checksum { get; private set; }

        public WadEntryBuilder()
        {

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
            this.Sha256Checksum = ComputeSha256Checksum(stream);

            return this;
        }
        public WadEntryBuilder WithGZipDataStream(Stream stream, int compressedSize, int uncompressedSize)
        {
            this.EntryType = WadEntryType.GZipCompressed;
            this.DataStream = stream;
            this.CompressedSize = compressedSize;
            this.UncompressedSize = uncompressedSize;
            this.Sha256Checksum = ComputeSha256Checksum(stream);

            return this;
        }
        public WadEntryBuilder WithUncompressedDataStream(Stream stream)
        {
            this.EntryType = WadEntryType.Uncompressed;
            this.DataStream = stream;
            this.CompressedSize = this.UncompressedSize = (int)stream.Length;
            this.Sha256Checksum = ComputeSha256Checksum(stream);

            return this;
        }
    
        public WadEntryBuilder WithFileRedirection(string fileRedirection)
        {
            this.EntryType = WadEntryType.FileRedirection;
            this.FileRedirection = fileRedirection;
            this.CompressedSize = this.UncompressedSize = fileRedirection.Length + 4;
            this.Sha256Checksum = new byte[8];

            return this;
        }

        private byte[] ComputeSha256Checksum(Stream stream)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(stream).Take(8).ToArray();
            }
        }
    }
}
