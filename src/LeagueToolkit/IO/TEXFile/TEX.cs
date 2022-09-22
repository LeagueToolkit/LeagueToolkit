using System;
using System.IO;
using System.Text;
using LeagueToolkit.Helpers.Exceptions;

namespace LeagueToolkit.IO.TEXFile
{
    public class TEX
    {
        public enum TEXFormat : byte
        {
            UNK1 = 1,
            UNK2 = 2,
            UNK3 = 3,
            DXT1 = 0xA,
            DXT5 = 0xC,
            RGBA8 = 0x14
        }

        public struct TEXHeader
        {
            public ushort width;
            public ushort height;
            public TEXFormat format;
            public bool hasMipmaps;
        }

        public const string TEX_MAGIC = "TEX\0";

        public TEXHeader Header { get; }
        public byte[] TextureBuffer { get; }
        public byte[][] MipMapsBuffer { get; } // buffer for mipmaps (if present), from largest to smallest mipmap
        public int MipMapCount { get; }

        private static int GetFormatBlockSize(TEXFormat format) => format switch
        {
            TEXFormat.DXT1 => 4,
            TEXFormat.DXT5 => 4,
            TEXFormat.RGBA8 => 1,
            _ => 1
        };

        private static int GetFormatBytesPerBlock(TEXFormat format) => format switch
        {
            TEXFormat.DXT1 => 8,
            TEXFormat.DXT5 => 16,
            TEXFormat.RGBA8 => 4,
            _ => 1
        };

        public TEX(Stream stream)
        {
            using BinaryReader br = new BinaryReader(stream, Encoding.UTF8, true);

            string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (magic != TEX_MAGIC)
            {
                throw new InvalidFileSignatureException();
            }

            TEXHeader texHeader = new()
            {
                width = br.ReadUInt16(),
                height = br.ReadUInt16()
            };
            stream.Seek(1, SeekOrigin.Current); // unknown, always 1
            texHeader.format = (TEXFormat)br.ReadByte();
            if (texHeader.format is not (TEXFormat.DXT1 or TEXFormat.DXT5 or TEXFormat.RGBA8))
            {
                throw new InvalidOperationException($"Unsupported TEX format value {texHeader.format}");
            }

            stream.Seek(1, SeekOrigin.Current); // unknown, always 0
            texHeader.hasMipmaps = br.ReadBoolean();
            this.Header = texHeader;
            int blockSize = GetFormatBlockSize(this.Header.format);
            int bytesPerBlock = GetFormatBytesPerBlock(this.Header.format);

            this.MipMapCount = Header.hasMipmaps ? (int)Math.Log(Math.Max(Header.width, Header.height), 2) : 0;
            if (this.MipMapCount > 0)
            {
                MipMapsBuffer = new byte[this.MipMapCount][];
                // mipmaps are written in order from smallest to largest
                for (int i = this.MipMapCount; i > 0; i--)
                {
                    int currentWidth = Math.Max(this.Header.width / (1 << i), 1);
                    int currentHeight = Math.Max(this.Header.height / (1 << i), 1);
                    int blockWidth = (currentWidth + blockSize - 1) / blockSize;
                    int blockHeight = (currentHeight + blockSize - 1) / blockSize;
                    int currentSize = bytesPerBlock * blockWidth * blockHeight;
                    MipMapsBuffer[i - 1] = br.ReadBytes(currentSize);
                }
            }

            this.TextureBuffer = br.ReadBytes(Math.Max(this.Header.width * this.Header.height * bytesPerBlock / (blockSize * blockSize), bytesPerBlock));
        }

        public void ToDds(string fileLocation) => ToDds(File.Create(fileLocation), false);
        public void ToDds(Stream stream, bool leaveOpen = true)
        {
            if (this.Header.format is not (TEXFormat.DXT1 or TEXFormat.DXT5 or TEXFormat.RGBA8))
            {
                throw new InvalidOperationException($"Cannot convert TEX format {this.Header.format} to DDS format.");
            }

            using BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen);

            int dwFlags = 0x00001007; // DDS_HEADER_FLAGS_TEXTURE
            int dwCaps = 0x00001000; // DDS_SURFACE_FLAGS_TEXTURE
            if (this.Header.hasMipmaps)
            {
                dwFlags |= 0x00020000; // DDS_HEADER_FLAGS_MIPMAP
                dwCaps |= 0x00400008; // DDS_SURFACE_FLAGS_MIPMAP
            }

            bw.Write(Encoding.ASCII.GetBytes("DDS ")); // magic
            bw.Write(124); // header size
            bw.Write(dwFlags);
            bw.Write((int)this.Header.height);
            bw.Write((int)this.Header.width);
            bw.Seek(4 * 2, SeekOrigin.Current);
            bw.Write(this.MipMapCount + 1);
            bw.Seek(4 * 11, SeekOrigin.Current);
            bw.Write(32); // DDS_PIXELFORMAT struct size
            if (this.Header.format == TEXFormat.RGBA8)
            {
                bw.Write(0x41); // dwFlags = DDS_RGBA
                bw.Seek(4, SeekOrigin.Current); // don't write any fourCC value
                bw.Write(8 * 4); // dwRGBBitCount
                bw.Write(0x000000ff); // dwRBitMask
                bw.Write(0x0000ff00); // dwGBitMask
                bw.Write(0x00ff0000); // dwBBitMask
                bw.Write(0xff000000); // dwABitMask
            }
            else
            {
                bw.Write(4); // dwFlags = DDS_FOURCC
                bw.Write(Encoding.ASCII.GetBytes(this.Header.format == TEXFormat.DXT5 ? "DXT5" : "DXT1"));
                bw.Seek(4 * 5, SeekOrigin.Current);
            }

            bw.Write(dwCaps);
            bw.Seek(4 * 4, SeekOrigin.Current);

            bw.Write(this.TextureBuffer);
            for (int i = 0; i < this.MipMapCount; i++)
            {
                bw.Write(this.MipMapsBuffer[i]);
            }
        }

        public void Write(string fileLocation) => Write(File.Create(fileLocation), false);
        public void Write(Stream stream, bool leaveOpen = true)
        {
            using BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII, leaveOpen);
            bw.Write(Encoding.ASCII.GetBytes(TEX_MAGIC));
            bw.Write(Header.width);
            bw.Write(Header.height);
            bw.Write((byte)1); // unknown
            bw.Write((byte)Header.format);
            bw.Write((byte)0); // unknown
            bw.Write(Header.hasMipmaps);

            // mipmaps are written in order from smallest to largest
            for (int i = MipMapCount; i > 0; i--)
            {
                bw.Write(MipMapsBuffer[i - 1]);
            }

            bw.Write(TextureBuffer);
        }
    }
}
