using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SixLabors.ImageSharp.PixelFormats;
using TexSharp.Containers.Dds;
using TexSharp.Containers.Tex;
using TexSharp.Formats;

namespace LeagueToolkit.Core.Renderer
{
    /// <summary>
    /// Represents a Texture
    /// </summary>
    public sealed class Texture
    {
        public Memory2D<Rgba32>[] Mips { get; set; }

        public Texture(Memory2D<Rgba32>[] mips)
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

            byte[] fileData;
            if (stream is MemoryStream ms)
            {
                fileData = ms.ToArray();
            }
            else
            {
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                fileData = memoryStream.ToArray();
            }

            DdsReader ddsReader = new(fileData);
            if (ddsReader.Format == DdsFormat.Unknown)
            {
                string fourCc = ddsReader.Header.PixelFormat.FourCC != 0
                    ? Encoding.ASCII.GetString(BitConverter.GetBytes(ddsReader.Header.PixelFormat.FourCC))
                    : "0";
                throw new NotSupportedException($"DDS texture format is not supported (FourCC: {fourCc})");
            }

            uint[][] mipsData = ddsReader.DecodeAllMips();
            Memory2D<Rgba32>[] mipsMemory = new Memory2D<Rgba32>[ddsReader.MipLevels];

            for (int i = 0; i < ddsReader.MipLevels; i++)
            {
                int currentWidth = Math.Max((int)ddsReader.Header.Width >> i, 1);
                int currentHeight = Math.Max((int)ddsReader.Header.Height >> i, 1);

                Rgba32[] rgbaPixels = MemoryMarshal.Cast<uint, Rgba32>(mipsData[i]).ToArray();
                mipsMemory[i] = new(rgbaPixels, currentHeight, currentWidth);
            }

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

            byte[] fileData;
            if (stream is MemoryStream ms)
            {
                fileData = ms.ToArray();
            }
            else
            {
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                fileData = memoryStream.ToArray();
            }

            TexReader texReader = new(fileData);
            if (texReader.Format is not (TexFormat.Bc1 or TexFormat.Bc1_Alt or TexFormat.Bc3 or TexFormat.Bc5 or TexFormat.Bc7 or TexFormat.Bgra8 or TexFormat.Rgba16f))
            {
                throw new NotSupportedException($"TEX texture format is not supported: {texReader.Format}");
            }

            uint[][] mipsData = texReader.Format == TexFormat.Rgba16f
                ? texReader.DecodeAllMips(Rgba16fColorMapping.SignedNormalizedOpaque)
                : texReader.DecodeAllMips();
            Memory2D<Rgba32>[] mipsMemory = new Memory2D<Rgba32>[texReader.MipLevels];

            for (int i = 0; i < texReader.MipLevels; i++)
            {
                int currentWidth = Math.Max(texReader.Width >> i, 1);
                int currentHeight = Math.Max(texReader.Height >> i, 1);

                Rgba32[] rgbaPixels = MemoryMarshal.Cast<uint, Rgba32>(mipsData[i]).ToArray();
                mipsMemory[i] = new(rgbaPixels, currentHeight, currentWidth);
            }

            return new(mipsMemory);
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
