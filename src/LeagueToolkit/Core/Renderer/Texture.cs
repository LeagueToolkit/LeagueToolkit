using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using BCnEncoder.Shared.ImageFiles;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Renderer
{
    public sealed class Texture
    {
        public IReadOnlyList<ReadOnlyMemory2D<ColorRgba32>> Mips { get; }
        private readonly Memory2D<ColorRgba32>[] _mips;

        internal Texture(Memory2D<ColorRgba32>[] mips)
        {
            this._mips = mips;
            this.Mips = this._mips.Select<Memory2D<ColorRgba32>, ReadOnlyMemory2D<ColorRgba32>>(x => x).ToArray();
        }

        public static Texture Load(Stream stream)
        {
            Guard.IsNotNull(stream, nameof(stream));

            return IdentifyFileFormat(stream) switch
            {
                TextureFileFormat.DDS => LoadDds(stream),
                TextureFileFormat.TEX => LoadTex(stream),
                TextureFileFormat.Unknown
                    => throw new InvalidOperationException("Cannot load unknown textue file format"),
            };
        }

        public static Texture LoadDds(Stream stream)
        {
            Guard.IsNotNull(stream, nameof(stream));

            BcDecoder decoder = new();
            DdsFile ddsFile = DdsFile.Load(stream);

            ColorRgba32[][] mips = decoder.DecodeAllMipMaps(ddsFile);
            Memory2D<ColorRgba32>[] mipsMemory = new Memory2D<ColorRgba32>[mips.Length];
            for (int i = 0; i < mips.Length; i++)
            {
                DdsMipMap mip = ddsFile.Faces[0].MipMaps[i];
                mipsMemory[i] = new(mips[i], (int)mip.Height, (int)mip.Width);
            }

            return new(mipsMemory);
        }

        public static Texture LoadTex(Stream stream)
        {
            Guard.IsNotNull(stream, nameof(stream));

            BcDecoder decoder = new();
            using BinaryReader br = new(stream);

            uint magic = br.ReadUInt32();
            if (magic is not 0x00584554)
                throw new InvalidOperationException($"Invalid TEX magic: {magic}");

            ushort width = br.ReadUInt16();
            ushort height = br.ReadUInt16();

            byte isExtendedFormatMaybe = br.ReadByte();
            ExtendedTextureFormat format = MapExtendedTextureFormat(br.ReadByte());
            byte resourceTypeMaybe = br.ReadByte(); // 0=texture 1=cubemap 2=surface 3=volumetexture
            TextureFlags flags = (TextureFlags)br.ReadByte();

            CompressionFormat compressionFormat = GetCompressionFormat(format);
            int blockSize = decoder.GetBlockSize(compressionFormat);

            int mipMapCount = flags.HasFlag(TextureFlags.HasMipMaps)
                ? (int)(Math.Floor(Math.Log2(Math.Max(height, width))) + 1f)
                : 1;

            // Seek to end because mipmaps are stored in reverse order (from smallest)
            // We will be reading them in reverse
            br.BaseStream.Seek(0, SeekOrigin.End);

            Memory2D<ColorRgba32>[] mipMaps = new Memory2D<ColorRgba32>[mipMapCount];
            for (int i = 0; i < mipMapCount; i++)
            {
                // Calculate dimensions of current mipmap
                int currentWidth = Math.Max(width >> i, 1);
                int currentHeight = Math.Max(height >> i, 1);
                decoder.GetBlockCount(currentWidth, currentHeight, out int widthInBlocks, out int heightInBlocks);

                int mipMapSize = widthInBlocks * heightInBlocks * blockSize;
                byte[] mipMapBuffer = ArrayPool<byte>.Shared.Rent(mipMapSize);

                // Seek to start of mipmap and read it into buffer
                br.BaseStream.Seek(-mipMapSize, SeekOrigin.Current);
                int bytesRead = br.Read(mipMapBuffer.AsSpan()[..mipMapSize]);
                if (bytesRead != mipMapSize)
                    throw new IOException($"Failed to read mip: {i}, size: {mipMapSize}, bytesRead: {bytesRead}");

                // Decode buffer and seek back
                ColorRgba32[] mipMapData = decoder.DecodeRaw(
                    mipMapBuffer,
                    currentWidth,
                    currentHeight,
                    compressionFormat
                );
                br.BaseStream.Seek(-mipMapSize, SeekOrigin.Current);

                // Return buffer and add mipmap
                ArrayPool<byte>.Shared.Return(mipMapBuffer);
                mipMaps[i] = new(mipMapData, currentHeight, currentWidth);
            }

            return new(mipMaps);
        }

        private static ExtendedTextureFormat MapExtendedTextureFormat(byte format)
        {
            // League uses BC1 as default format for some reason, we throw
            return format switch
            {
                1 => ExtendedTextureFormat.ETC1,
                2 or 3 => ExtendedTextureFormat.ETC2_EAC,
                10 or 11 => ExtendedTextureFormat.BC1,
                12 => ExtendedTextureFormat.BC3,
                20 => ExtendedTextureFormat.RGBA8,
                _ => throw new NotImplementedException($"Unsupported texture format: {format}")
            };
        }

        private static CompressionFormat GetCompressionFormat(ExtendedTextureFormat extendedFormat)
        {
            return extendedFormat switch
            {
                ExtendedTextureFormat.RGBA8 => CompressionFormat.Rgba,
                ExtendedTextureFormat.ETC1 => throw new NotImplementedException(),
                ExtendedTextureFormat.ETC2_EAC => throw new NotImplementedException(),
                ExtendedTextureFormat.BC1 => CompressionFormat.Bc1,
                ExtendedTextureFormat.BC3 => CompressionFormat.Bc3,
            };
        }

        private static TextureFileFormat IdentifyFileFormat(Stream stream)
        {
            if (IsDds(stream))
                return TextureFileFormat.DDS;
            else if (IsTex(stream))
                return TextureFileFormat.TEX;
            else
                return TextureFileFormat.Unknown;
        }

        private static bool IsDds(Stream stream)
        {
            using BinaryReader br = new(stream, Encoding.UTF8, true);

            uint magic = br.ReadUInt32();
            stream.Position -= 4;

            return magic == 0x20534444u; // "DDS "
        }

        private static bool IsTex(Stream stream)
        {
            using BinaryReader br = new(stream, Encoding.UTF8, true);

            uint magic = br.ReadUInt32();
            stream.Position -= 4;

            return magic == 0x00584554; // "TEX\0"
        }
    }

    internal enum TextureFileFormat
    {
        DDS,
        TEX,
        Unknown
    }

    public enum ExtendedTextureFormat
    {
        RGBA8,
        ETC1,
        ETC2_EAC,
        BC1,
        BC3
    }

    [Flags]
    internal enum TextureFlags : byte
    {
        HasMipMaps = 1 << 0
    }
}
