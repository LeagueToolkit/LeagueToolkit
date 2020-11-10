using LeagueToolkit.Helpers.Structures;
using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Helpers.Extensions
{
    internal static class BinaryReaderColorExtensions
    {
        public static Color ReadColor(this BinaryReader reader, ColorFormat format)
        {
            if (format == ColorFormat.RgbU8)
            {
                float r = reader.ReadByte() / 255f;
                float g = reader.ReadByte() / 255f;
                float b = reader.ReadByte() / 255f;
                return new Color(r, g, b);
            }
            else if (format == ColorFormat.RgbaU8)
            {
                float r = reader.ReadByte() / 255f;
                float g = reader.ReadByte() / 255f;
                float b = reader.ReadByte() / 255f;
                float a = reader.ReadByte() / 255f;
                return new Color(r, g, b, a);
            }
            else if (format == ColorFormat.RgbF32)
            {
                float r = reader.ReadSingle();
                float g = reader.ReadSingle();
                float b = reader.ReadSingle();
                return new Color(r, g, b);
            }
            else if (format == ColorFormat.RgbaF32)
            {
                float r = reader.ReadSingle();
                float g = reader.ReadSingle();
                float b = reader.ReadSingle();
                float a = reader.ReadSingle();
                return new Color(r, g, b, a);
            }
            else if (format == ColorFormat.BgrU8)
            {
                float b = reader.ReadByte() / 255f;
                float g = reader.ReadByte() / 255f;
                float r = reader.ReadByte() / 255f;
                return new Color(r, g, b);
            }
            else if (format == ColorFormat.BgraU8)
            {
                float b = reader.ReadByte() / 255f;
                float g = reader.ReadByte() / 255f;
                float r = reader.ReadByte() / 255f;
                float a = reader.ReadByte() / 255f;
                return new Color(r, g, b, a);
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

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }
        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Matrix4x4 ReadMatrix4x4RowMajor(this BinaryReader reader)
        {
            return new Matrix4x4
            {
                M11 = reader.ReadSingle(),
                M12 = reader.ReadSingle(),
                M13 = reader.ReadSingle(),
                M14 = reader.ReadSingle(),
                M21 = reader.ReadSingle(),
                M22 = reader.ReadSingle(),
                M23 = reader.ReadSingle(),
                M24 = reader.ReadSingle(),
                M31 = reader.ReadSingle(),
                M32 = reader.ReadSingle(),
                M33 = reader.ReadSingle(),
                M34 = reader.ReadSingle(),
                M41 = reader.ReadSingle(),
                M42 = reader.ReadSingle(),
                M43 = reader.ReadSingle(),
                M44 = reader.ReadSingle()
            };
        }

        public static string ReadPaddedString(this BinaryReader reader, int length)
        {
            return Encoding.ASCII.GetString(reader.ReadBytes(length)).Replace("\0", "");
        }
        public static string ReadZeroTerminatedString(this BinaryReader reader)
        {
            string returnString = "";

            while (true)
            {
                char c = reader.ReadChar();
                if (c == 0)
                {
                    break;
                }

                returnString += c;
            }

            return returnString;
        }
    }
}
