using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.Helpers.Extensions
{
    public static class BinaryWriterExtensions
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
    }
}
