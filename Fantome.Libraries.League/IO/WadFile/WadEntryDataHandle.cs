using System;
using System.IO;
using System.IO.Compression;
using ZstdSharp;

namespace Fantome.Libraries.League.IO.WadFile
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

            switch (this._entry.Type)
            {
                case WadEntryType.GZipCompressed:
                {
                    return new GZipStream(wadStream, CompressionMode.Decompress, true);
                }
                case WadEntryType.ZStandardCompressed:
                {
                    return new ZstdStream(wadStream, ZstdStreamMode.Decompress, true);
                }
                case WadEntryType.Uncompressed:
                {
                    return wadStream;
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

            switch (this._entry.Type)
            {
                case WadEntryType.GZipCompressed:
                {
                    MemoryStream uncompressedStream = new MemoryStream((int)this._entry.UncompressedSize);
                    using (GZipStream gzipStream = new GZipStream(wadStream, CompressionMode.Decompress, true))
                    {
                        gzipStream.CopyTo(uncompressedStream);
                    }
                    return uncompressedStream;
                }
                case WadEntryType.ZStandardCompressed:
                {
                    MemoryStream uncompressedStream = new MemoryStream((int)this._entry.UncompressedSize);
                    using (ZstdStream zstdStream = new ZstdStream(wadStream, ZstdStreamMode.Decompress, true))
                    {
                        zstdStream.CopyTo(uncompressedStream);
                    }
                    return uncompressedStream;
                }
                case WadEntryType.Uncompressed:
                {
                    byte[] uncompressedData = new byte[this._entry.UncompressedSize];

                    if (wadStream.Read(uncompressedData, 0, uncompressedData.Length) != uncompressedData.Length)
                    {
                        throw new IOException("Failed to read Wad Entry data");
                    }

                    return new MemoryStream(uncompressedData);
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
