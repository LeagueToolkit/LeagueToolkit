using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Helpers.Structures;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Utils.Extensions;

internal static class BinaryWriterExtensions
{
    public static void WriteColor(this BinaryWriter writer, Color color, ColorFormat format)
    {
        Span<byte> buffer = stackalloc byte[Color.GetFormatSize(format)];
        color.Write(buffer, format);
        writer.Write(buffer);
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

    public static void WriteBox(this BinaryWriter writer, Box box)
    {
        writer.WriteVector3(box.Min);
        writer.WriteVector3(box.Max);
    }

    public static void WriteSphere(this BinaryWriter writer, Sphere sphere)
    {
        writer.WriteVector3(sphere.Position);
        writer.Write(sphere.Radius);
    }

    public static void WritePaddedString(this BinaryWriter writer, string value, int length)
    {
        if (value.Length > length)
            throw new ArgumentException(
                $"{nameof(value.Length)}: {value.Length} is greater than {nameof(length)}: {length}"
            );

        Span<char> data = stackalloc char[length];
        data.Clear();
        value.AsSpan().CopyTo(data);
        writer.Write(data);
    }

    public static void WriteNullTerminatedString(this BinaryWriter writer, string value)
    {
        writer.Write(Encoding.ASCII.GetBytes(value));
        writer.Write((byte)0);
    }
}
