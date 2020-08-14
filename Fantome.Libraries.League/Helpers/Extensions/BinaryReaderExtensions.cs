using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.Helpers.Extensions
{
    public static class BinaryReaderColorExtensions
    {
        public static Color ReadColor(this BinaryReader reader, ColorFormat format)
        {
            if (format == ColorFormat.RgbU8) return new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            else if (format == ColorFormat.RgbaU8) return new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            else if (format == ColorFormat.RgbF32) return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            else if (format == ColorFormat.RgbaF32) return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            else if (format == ColorFormat.BgrU8)
            {
                float b = reader.ReadByte() / 255;
                float g = reader.ReadByte() / 255;
                float r = reader.ReadByte() / 255;
                return new Color(r, g, b);
            }
            else if (format == ColorFormat.BgraU8)
            {
                float b = reader.ReadByte() / 255;
                float g = reader.ReadByte() / 255;
                float r = reader.ReadByte() / 255;
                float a = reader.ReadByte() / 255;
                return new Color(r, g, b);
            }
            else if (format == ColorFormat.BgrF32)
            {
                float b = reader.ReadSingle();
                float g = reader.ReadSingle();
                float r = reader.ReadSingle();
                return new Color(r, g, b);
            }
            else if (format == ColorFormat.BgraF32)
            {
                float b = reader.ReadSingle();
                float g = reader.ReadSingle();
                float r = reader.ReadSingle();
                float a = reader.ReadSingle();
                return new Color(r, g, b, a);
            }
            else
            {
                throw new ArgumentException("Unsupported format", nameof(format));
            }
        }

        public static string ReadPaddedString(this BinaryReader reader, int length)
        {
            return Encoding.ASCII.GetString(reader.ReadBytes(length)).Replace("\0", "");
        }
    }
}
