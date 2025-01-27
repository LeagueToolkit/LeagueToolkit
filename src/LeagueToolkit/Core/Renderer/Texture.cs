using System;
using System.IO;
using System.Text;
using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using BCnEncoder.Shared.ImageFiles;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;

namespace LeagueToolkit.Core.Renderer
{
    /// <summary>
    /// Represents a Texture
    /// </summary>
    public sealed class Texture
    {
        public Memory2D<ColorRgba32>[] Mips { get; set; }

        public Texture(Memory2D<ColorRgba32>[] mips)
        {
            this.Mips = mips;
        }

        /// <summary>
        /// Creates a new <see cref="Texture"/> object by reading it from the specified stream
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        /// <returns>The created <see cref="Texture"/> object</returns>
        public static Texture Load(Stream stream)
        {
            Guard.IsNotNull(stream, nameof(stream));

            return IdentifyFileFormat(stream) switch
            {
                TextureFileFormat.DDS => LoadDds(stream),
                TextureFileFormat.TEX => LoadTex(stream),
                _ => throw new InvalidOperationException("Cannot load unknown texture file format")
            };
        }

        /// <summary>
        /// Creates a new <see cref="Texture"/> object by reading it from the specified stream
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        /// <returns>The created <see cref="Texture"/> object</returns>
        public static Texture LoadDds(Stream stream)
        {
            Guard.IsNotNull(stream, nameof(stream));

            BcDecoder decoder = new();
            DdsFile ddsFile = DdsFile.Load(stream);
            Memory2D<ColorRgba32>[] mipsMemory = decoder.DecodeAllMipMaps2D(ddsFile);

            return new(mipsMemory);
        }

        /// <summary>
        /// Creates a new <see cref="Texture"/> object by reading it from the specified stream
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        /// <returns>The created <see cref="Texture"/> object</returns>
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
                (int widthInBlocks, int heightInBlocks) = CalculateBlockCount(format, currentWidth, currentHeight);

                int mipMapSize = widthInBlocks * heightInBlocks * blockSize;
                using MemoryOwner<byte> mipMapBufferOwner = MemoryOwner<byte>.Allocate(mipMapSize);

                // Seek to start of mipmap and read it into buffer
                br.BaseStream.Seek(-mipMapSize, SeekOrigin.Current);
                int bytesRead = br.Read(mipMapBufferOwner.Span);
                if (bytesRead != mipMapSize)
                    throw new IOException($"Failed to read mip: {i}, size: {mipMapSize}, bytesRead: {bytesRead}");

                // Decode buffer and seek back
                ColorRgba32[] mipMapData = decoder.DecodeRaw(
                    mipMapBufferOwner.Memory,
                    currentWidth,
                    currentHeight,
                    compressionFormat
                );
                br.BaseStream.Seek(-mipMapSize, SeekOrigin.Current);

                // Add mipmap
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
                20 => ExtendedTextureFormat.BGRA8,
                _ => throw new NotImplementedException($"Unsupported extended texture format: {format}")
            };
        }

        private static CompressionFormat GetCompressionFormat(ExtendedTextureFormat extendedFormat)
        {
            return extendedFormat switch
            {
                ExtendedTextureFormat.BGRA8 => CompressionFormat.Bgra,
                ExtendedTextureFormat.ETC1 => throw new NotImplementedException(),
                ExtendedTextureFormat.ETC2_EAC => throw new NotImplementedException(),
                ExtendedTextureFormat.BC1 => CompressionFormat.Bc1WithAlpha,
                ExtendedTextureFormat.BC3 => CompressionFormat.Bc3,
                _ => throw new NotImplementedException($"Unsupported extended texture format: {extendedFormat}")
            };
        }

        /// <summary>
        /// Identifies the texture file format from the specified stream
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> which contains the <see cref="Texture"/></param>
        /// <returns>The texture file format</returns>
        public static TextureFileFormat IdentifyFileFormat(Stream stream)
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

        private static (int widthInBlocks, int heightInBlocks) CalculateBlockCount(
            ExtendedTextureFormat format,
            int pixelWidth,
            int pixelHeight
        )
        {
            int blockLength = format == ExtendedTextureFormat.BGRA8 ? 1 : 4;
            int widthInBlocks = (pixelWidth + blockLength - 1) / blockLength;
            int heightInBlocks = (pixelHeight + blockLength - 1) / blockLength;

            return (widthInBlocks, heightInBlocks);
        }
    }

    /// <summary>
    /// Represents the type of a texture file format
    /// </summary>
    public enum TextureFileFormat
    {
        /// <summary>
        /// The texture is stored in DDS format
        /// </summary>
        DDS,

        /// <summary>
        /// The texture is stored in League of Legends TEX format
        /// </summary>
        TEX,

        /// <summary>
        /// The texture is stored in an unknown file format
        /// </summary>
        Unknown
    }

    internal enum ExtendedTextureFormat
    {
        BGRA8,
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

    public enum TextureFilter
    {
        None = 0,
        Nearest = 1,
        Linear = 2
    }

    public enum TextureAddress
    {
        Wrap = 0,
        Clamp = 1
    }
}
