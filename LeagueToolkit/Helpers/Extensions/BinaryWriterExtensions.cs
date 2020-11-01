using LeagueToolkit.Helpers.Structures;
using System;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.Helpers.Extensions
{
    internal static class BinaryWriterExtensions
    {
        public static void WriteColor(this BinaryWriter writer, Color color, ColorFormat format)
        {
            if (format == ColorFormat.RgbU8)
            {
                writer.Write((byte)(color.R * 255));
                writer.Write((byte)(color.G * 255));
                writer.Write((byte)(color.B * 255));
            }
            else if (format == ColorFormat.RgbaU8)
            {
                writer.Write((byte)(color.R * 255));
                writer.Write((byte)(color.G * 255));
                writer.Write((byte)(color.B * 255));
                writer.Write((byte)(color.A * 255));
            }
            else if (format == ColorFormat.RgbF32)
            {
                writer.Write(color.R);
                writer.Write(color.G);
                writer.Write(color.B);
            }
            else if (format == ColorFormat.RgbaF32)
            {
                writer.Write(color.R);
                writer.Write(color.G);
                writer.Write(color.B);
                writer.Write(color.A);
            }
            else if (format == ColorFormat.BgrU8)
            {
                writer.Write((byte)(color.B * 255));
                writer.Write((byte)(color.G * 255));
                writer.Write((byte)(color.R * 255));
            }
            else if (format == ColorFormat.BgraU8)
            {
                writer.Write((byte)(color.B * 255));
                writer.Write((byte)(color.G * 255));
                writer.Write((byte)(color.R * 255));
                writer.Write((byte)(color.A * 255));
            }
            else if (format == ColorFormat.BgrF32)
            {
                writer.Write(color.B);
                writer.Write(color.G);
                writer.Write(color.R);
            }
            else if (format == ColorFormat.BgraF32)
            {
                writer.Write(color.B);
                writer.Write(color.G);
                writer.Write(color.R);
                writer.Write(color.A);
            }
            else
            {
                throw new ArgumentException("Unsupported format", nameof(format));
            }
        }

        public static void WriteVector2(this BinaryWriter writer, Vector2 vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
        }
        public static void WriteVector3(this BinaryWriter writer, Vector3 vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }
        public static void WriteVector4(this BinaryWriter writer, Vector4 vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
            writer.Write(vector.W);
        }

        public static void WriteQuaternion(this BinaryWriter writer, Quaternion quaternion)
        {
            writer.Write(quaternion.X);
            writer.Write(quaternion.Y);
            writer.Write(quaternion.Z);
            writer.Write(quaternion.W);
        }

        public static void WriteMatrix4x4RowMajor(this BinaryWriter writer, Matrix4x4 matrix)
        {
            writer.Write(matrix.M11);
            writer.Write(matrix.M12);
            writer.Write(matrix.M13);
            writer.Write(matrix.M14);
            writer.Write(matrix.M21);
            writer.Write(matrix.M22);
            writer.Write(matrix.M23);
            writer.Write(matrix.M24);
            writer.Write(matrix.M31);
            writer.Write(matrix.M32);
            writer.Write(matrix.M33);
            writer.Write(matrix.M34);
            writer.Write(matrix.M41);
            writer.Write(matrix.M42);
            writer.Write(matrix.M43);
            writer.Write(matrix.M44);
        }
    }
}
