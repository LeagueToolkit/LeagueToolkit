using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using LeagueToolkit.Helpers.Exceptions;

namespace LeagueToolkit.IO.TEXFile;

public class TEX
{
    public enum TEXFormat : byte
    {
        UNK1 = 1,
        UNK2 = 2,
        UNK3 = 3,
        DXT1 = 0xA,
        DXT5 = 0xC
    }
    public struct TEXHeader
    {
        public ushort Width;
        public ushort Height;
        public TEXFormat format;
        public bool hasMipmaps;
    }

    public const string TEX_MAGIC = "TEX\0";

    public TEXHeader header { get; }
    public byte[] textureBuffer { get; }
    public byte[][] mipMapsBuffer { get; } // buffer for mipmaps (if present), from largest to smallest mipmap
    public int mipMapCount { get; }

    private const int MIN_WIDTH = 4;
    private const int MIN_HEIGHT = 4;

    public TEX(Stream stream)
    {
        using BinaryReader br = new BinaryReader(stream);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
        if (magic != TEX_MAGIC)
        {
            throw new InvalidFileSignatureException();
        }

        TEXHeader texHeader = new()
        {
            Width = br.ReadUInt16(),
            Height = br.ReadUInt16()
        };
        stream.Seek(1, SeekOrigin.Current); // unknown, always 1
        texHeader.format = (TEXFormat)br.ReadByte();
        if (texHeader.format is not (TEXFormat.DXT1 or TEXFormat.DXT5))
        {
            throw new InvalidOperationException($"Unsupported TEX format value {texHeader.format}");
        }
        stream.Seek(1, SeekOrigin.Current); // unknown, always 0
        texHeader.hasMipmaps = br.ReadBoolean();
        header = texHeader;

        mipMapCount = header.hasMipmaps ? (int) Math.Log(Math.Max(header.Width, header.Height), 2) : 0;
        if (mipMapCount > 0)
        {
            mipMapsBuffer = new byte[mipMapCount][];
            // mipmaps are written in order from smallest to largest
            for (int i = mipMapCount; i > 0; i--)
            {
                int currentWidth = Math.Max(header.Width / (1 << i), MIN_WIDTH);
                int currentHeight = Math.Max(header.Height / (1 << i), MIN_HEIGHT);
                mipMapsBuffer[i - 1] = br.ReadBytes(currentWidth * currentHeight);
            }
        }

        textureBuffer = br.ReadBytes(header.Width * header.Height);
    }

    public void ToDds(Stream stream, bool leaveOpen = true)
    {
        using BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen);

        int dwFlags = 0x00001007 | 0x00080000; // DDS_HEADER_FLAGS_TEXTURE | DDS_HEADER_FLAGS_LINEARSIZE
        int dwCaps = 0x00001000; // DDS_SURFACE_FLAGS_TEXTURE
        if (header.hasMipmaps)
        {
            dwFlags |= 0x00020000; // DDS_HEADER_FLAGS_MIPMAP
            dwCaps |= 0x00400008; // DDS_SURFACE_FLAGS_MIPMAP
        }
        bw.Write(Encoding.ASCII.GetBytes("DDS ")); // magic
        bw.Write(124); // header size
        bw.Write(dwFlags);
        bw.Write((int)header.Height);
        bw.Write((int)header.Width);
        bw.Write(header.Width * header.Height); // dwPitchOrLinearSize
        bw.Seek(4, SeekOrigin.Current);
        bw.Write(mipMapCount + 1);
        bw.Seek(4 * 11, SeekOrigin.Current);
        bw.Write(32); // DDS_PIXELFORMAT struct size
        bw.Write(4); // dwFlags = DDS_FOURCC
        bw.Write(Encoding.ASCII.GetBytes(header.format == TEXFormat.DXT5 ? "DXT5" : "DXT1"));
        bw.Seek(4 * 5, SeekOrigin.Current);
        bw.Write(dwCaps);
        bw.Seek(4 * 4, SeekOrigin.Current);

        bw.Write(textureBuffer);
        for (int i = 0; i < mipMapCount; i++)
        {
            bw.Write(mipMapsBuffer[i]);
        }
    }
}
