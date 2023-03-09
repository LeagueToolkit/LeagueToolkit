using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Helpers.Structures;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Utils.Extensions;

internal static class BinaryReaderColorExtensions
{
    public static Color ReadColor(this BinaryReader reader, ColorFormat format)
    {
        Span<byte> buffer = stackalloc byte[Color.GetFormatSize(format)];
        reader.Read(buffer);
        return Color.Read(buffer, format);
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

    public static Box ReadBox(this BinaryReader reader)
    {
        return new(reader.ReadVector3(), reader.ReadVector3());
    }

    public static Sphere ReadSphere(this BinaryReader reader)
    {
        Vector3 position = reader.ReadVector3();
        float radius = reader.ReadSingle();

        return new(position, radius);
    }

    public static string ReadPaddedString(this BinaryReader reader, int length)
    {
        return Encoding.ASCII.GetString(reader.ReadBytes(length).TakeWhile(b => !b.Equals(0)).ToArray());
    }

    public static string ReadNullTerminatedString(this BinaryReader reader)
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
