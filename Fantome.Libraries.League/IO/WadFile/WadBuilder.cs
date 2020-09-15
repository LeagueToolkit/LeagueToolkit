using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Text;
using ZstdSharp;

namespace Fantome.Libraries.League.IO.WadFile
{
    public class WadBuilder
    {
        public ReadOnlyDictionary<ulong, WadEntryBuilder> Entries { get; private set; }

        private Dictionary<ulong, WadEntryBuilder> _entries = new();

        public WadBuilder()
        {
            this.Entries = new(this._entries);
        }

        public WadBuilder WithEntry(WadEntryBuilder entryBuilder)
        {
            if (this._entries.ContainsKey(entryBuilder.PathXXHash))
            {
                throw new InvalidOperationException("An entry with the same XXHash has already been added: " + entryBuilder.PathXXHash);
            }
            else
            {
                this._entries.Add(entryBuilder.PathXXHash, entryBuilder);
            }

            return this;
        }

        public void Build(string fileLocation) => Build(File.OpenWrite(fileLocation));
        public void Build(Stream stream)
        {
            using Wad wad = new Wad();

            // Seek after Header and TOC
            stream.Seek(Wad.HEADER_SIZE_V3 + (this._entries.Count * WadEntry.TOC_SIZE_V3), SeekOrigin.Current);

            // Write data streams into the stream and save offsets
            foreach (WadEntryBuilder entryBuilder in this._entries.Values)
            {
                WriteEntryData(stream, entryBuilder);

                entryBuilder.DataStream.Close();
            }

            foreach (WadEntryBuilder entryBuilder in this._entries.Values)
            {
                wad.AddEntry(new WadEntry(
                    entryBuilder.PathXXHash,
                    entryBuilder.CompressedSize,
                    entryBuilder.UncompressedSize,
                    entryBuilder.EntryType,
                    entryBuilder.Sha256Checksum,
                    entryBuilder.FileRedirection,
                    entryBuilder._dataOffset)
                );
            }

            wad.Write(stream);
        }

        private void WriteEntryData(Stream stream, WadEntryBuilder entryBuilder)
        {
            // If we're writing a File Stream then we need to compress it first
            if (entryBuilder._isFileDataStream)
            {
                int uncompressedSize = (int)entryBuilder.DataStream.Length;
                MemoryStream compressedStream = new MemoryStream();
                if (entryBuilder.EntryType == WadEntryType.GZipCompressed)
                {
                    using GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Compress);

                    entryBuilder.DataStream.CopyTo(gzipStream);
                    entryBuilder.DataStream = compressedStream;
                    entryBuilder.UncompressedSize = uncompressedSize;
                    entryBuilder.CompressedSize = (int)compressedStream.Length;
                }
                else if (entryBuilder.EntryType == WadEntryType.ZStandardCompressed)
                {
                    using ZstdStream zstdStream = new ZstdStream(compressedStream, ZstdStreamMode.Compress);

                    entryBuilder.DataStream.CopyTo(zstdStream);
                    entryBuilder.DataStream = compressedStream;
                    entryBuilder.UncompressedSize = uncompressedSize;
                    entryBuilder.CompressedSize = (int)compressedStream.Length;
                }
                else if (entryBuilder.EntryType == WadEntryType.Uncompressed)
                {
                    entryBuilder.CompressedSize = entryBuilder.UncompressedSize = (int)entryBuilder.DataStream.Length;
                }
                else
                {
                    throw new InvalidOperationException("Cannot have a File Redirection entry with a data stream");
                }

                entryBuilder.ComputeSha256Checksum();
            }

            entryBuilder._dataOffset = (int)stream.Position;
        
            // Write data
            if (entryBuilder.EntryType == WadEntryType.FileRedirection)
            {
                stream.Write(BitConverter.GetBytes(entryBuilder.FileRedirection.Length));
                stream.Write(Encoding.UTF8.GetBytes(entryBuilder.FileRedirection));
            }
            else
            {
                entryBuilder.DataStream.CopyTo(stream);
            }
        }
    }
}
