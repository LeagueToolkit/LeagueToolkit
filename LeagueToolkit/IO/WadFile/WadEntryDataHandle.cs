using System;
using System.IO;
using System.IO.Compression;
using ZstdSharp;

namespace LeagueToolkit.IO.WadFile
{
    public struct WadEntryDataHandle
    {
        private WadEntry _entry;

        internal WadEntryDataHandle(WadEntry entry)
        {
            this._entry = entry;
        }

        public Stream GetCompressedStream()
        {
            Stream wadStream = this._entry._wad._stream;

            // Seek to entry data
            wadStream.Seek(this._entry._dataOffset, SeekOrigin.Begin);

            // Read compressed data to a buffer
            byte[] compressedData = new byte[this._entry.CompressedSize];
            wadStream.Read(compressedData, 0, this._entry.CompressedSize);

            switch (this._entry.Type)
            {
                case WadEntryType.GZipCompressed:
                case WadEntryType.ZStandardCompressed:
                case WadEntryType.Uncompressed:
                {
                    return new MemoryStream(compressedData);
                }
                case WadEntryType.FileRedirection:
                {
                    throw new InvalidOperationException("Cannot open a handle to a File Redirection");
                }
                default:
                {
                    throw new InvalidOperationException("Invalid Wad Entry type: " + this._entry.Type);
                }
            }
        }

        public Stream GetDecompressedStream()
        {
            Stream wadStream = this._entry._wad._stream;

            // Seek to entry data
            wadStream.Seek(this._entry._dataOffset, SeekOrigin.Begin);

            // Read compressed data to a buffer
            byte[] compressedData = new byte[this._entry.CompressedSize];
            wadStream.Read(compressedData, 0, this._entry.CompressedSize);

            switch (this._entry.Type)
            {
                case WadEntryType.GZipCompressed:
                {
                    MemoryStream uncompressedStream = new MemoryStream(this._entry.UncompressedSize);
                    using MemoryStream compressedStream = new MemoryStream(compressedData);
                    using GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);

                    gzipStream.CopyTo(uncompressedStream);

                    return uncompressedStream;
                }
                case WadEntryType.ZStandardCompressed:
                {
                    byte[] decompressedData = Zstd.Decompress(compressedData, this._entry.UncompressedSize);

                    return new MemoryStream(decompressedData);
                }
                case WadEntryType.Uncompressed:
                {
                    return new MemoryStream(compressedData);
                }
                case WadEntryType.FileRedirection:
                {
                    throw new InvalidOperationException("Cannot open a handle to a File Redirection");
                }
                default:
                {
                    throw new InvalidOperationException("Invalid Wad Entry type: " + this._entry.Type);
                }
            }
        }
    }
}
