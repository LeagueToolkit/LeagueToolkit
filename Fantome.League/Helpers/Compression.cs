using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Fantome.Libraries.League.Helpers.Compression
{
    /// <summary>
    /// A static class which contains static methods to compress/decompress GZip and ZLib
    /// </summary>
    public static class Compression
    {
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
    }
}
