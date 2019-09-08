using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using ZstdNet;

namespace Fantome.Libraries.League.Helpers.Compression
{
    /// <summary>
    /// A static class which contains static methods to compress/decompress GZip and ZLib
    /// </summary>
    public static class Compression
    {
        /// <summary>
        /// Decompresses the specified GZip Data
        /// </summary>
        /// <param name="buffer">Data to decompress</param>
        /// <returns>Decompressed Data</returns>
        public static byte[] DecompressGZip(byte[] buffer)
        {
            using (MemoryStream decompressedBuffer = new MemoryStream())
            {
                using (MemoryStream compressedBuffer = new MemoryStream(buffer))
                {
                    using (GZipStream gzipBuffer = new GZipStream(compressedBuffer, CompressionMode.Decompress))
                    {
                        gzipBuffer.CopyTo(decompressedBuffer);
                    }
                }
                return decompressedBuffer.ToArray();
            }
        }

        /// <summary>
        /// Compresses the specified Data
        /// </summary>
        /// <param name="buffer">Data to compress</param>
        /// <returns>Compressed Data</returns>
        public static byte[] CompressGZip(byte[] buffer)
        {
            using (MemoryStream compressedBuffer = new MemoryStream())
            {
                using (MemoryStream uncompressedBuffer = new MemoryStream(buffer))
                {
                    using (GZipStream gzipBuffer = new GZipStream(compressedBuffer, CompressionMode.Compress))
                    {
                        uncompressedBuffer.CopyTo(gzipBuffer);
                    }
                }
                return compressedBuffer.ToArray();
            }
        }

        /// <summary>
        /// Decompresses the specified ZStandard Data
        /// </summary>
        /// <param name="buffer">Data to decompress</param>
        public static byte[] DecompressZStandard(byte[] buffer)
        {
            using (Decompressor decompressor = new Decompressor())
            {
                return decompressor.Unwrap(buffer);
            }
        }

        /// <summary>
        /// Compresses the specified Data
        /// </summary>
        /// <param name="buffer">Data to compress</param>
        public static byte[] CompressZStandard(byte[] buffer)
        {
            using (Compressor compressor = new Compressor())
            {
                return compressor.Wrap(buffer);
            }
        }
    }
}
