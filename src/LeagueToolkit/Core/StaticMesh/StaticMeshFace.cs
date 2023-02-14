using LeagueToolkit.Helpers.Structures;
using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using LeagueToolkit.Helpers.Extensions;

namespace LeagueToolkit.Core.StaticMesh;

public readonly struct StaticMeshFace
{
    public ushort VertexId0 { get; init; }
    public ushort VertexId1 { get; init; }
    public ushort VertexId2 { get; init; }

    public Vector2 UV0 { get; init; }
    public Vector2 UV1 { get; init; }
    public Vector2 UV2 { get; init; }

    public Color Color0 { get; init; }
    public Color Color1 { get; init; }
    public Color Color2 { get; init; }

    public StaticMeshFace((ushort, ushort, ushort) indices, (Vector2, Vector2, Vector2) uvs)
    {
        (this.VertexId0, this.VertexId1, this.VertexId2) = indices;
        (this.UV0, this.UV1, this.UV2) = uvs;

        this.Color0 = Color.One;
        this.Color1 = Color.One;
        this.Color2 = Color.One;
    }

    public StaticMeshFace(
        (ushort, ushort, ushort) indices,
        (Vector2, Vector2, Vector2) uvs,
        (Color, Color, Color) colors
    )
    {
        (this.VertexId0, this.VertexId1, this.VertexId2) = indices;
        (this.UV0, this.UV1, this.UV2) = uvs;
        (this.Color0, this.Color1, this.Color2) = colors;
    }

    internal static StaticMeshFace ReadBinary(BinaryReader br)
    {
        // indices are 16-bit internally
        (ushort, ushort, ushort) indices = ((ushort)br.ReadUInt32(), (ushort)br.ReadUInt32(), (ushort)br.ReadUInt32());

        // ignore material because it's not used
        br.BaseStream.Seek(64, SeekOrigin.Current);

        ReadOnlySpan<float> uvs =
            stackalloc float[] {
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle()
            };

        return new(indices, (new(uvs[0], uvs[3]), new(uvs[1], uvs[4]), new(uvs[2], uvs[5])));
    }

    public static StaticMeshFace ReadAscii(StreamReader sr)
    {
        string[] input = sr.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        var indices = (ushort.Parse(input[1]), ushort.Parse(input[2]), ushort.Parse(input[3]));

        return new(
            indices,
            (
                new(
                    float.Parse(input[5], CultureInfo.InvariantCulture),
                    float.Parse(input[6], CultureInfo.InvariantCulture)
                ),
                new(
                    float.Parse(input[7], CultureInfo.InvariantCulture),
                    float.Parse(input[8], CultureInfo.InvariantCulture)
                ),
                new(
                    float.Parse(input[9], CultureInfo.InvariantCulture),
                    float.Parse(input[10], CultureInfo.InvariantCulture)
                )
            )
        );
    }

    internal void WriteBinary(BinaryWriter bw)
    {
        bw.Write((uint)this.VertexId0);
        bw.Write((uint)this.VertexId1);
        bw.Write((uint)this.VertexId2);

        bw.BaseStream.Seek(64, SeekOrigin.Current);

        bw.Write(this.UV0.X);
        bw.Write(this.UV1.X);
        bw.Write(this.UV2.X);

        bw.Write(this.UV0.Y);
        bw.Write(this.UV1.Y);
        bw.Write(this.UV2.Y);
    }

    internal void WriteAscii(StreamWriter sw)
    {
        string uvs = string.Format(
            "{0} {1} {2} {3} {4} {5}",
            this.UV0.X.ToString(CultureInfo.InvariantCulture),
            this.UV1.X.ToString(CultureInfo.InvariantCulture),
            this.UV2.X.ToString(CultureInfo.InvariantCulture),
            this.UV0.Y.ToString(CultureInfo.InvariantCulture),
            this.UV1.Y.ToString(CultureInfo.InvariantCulture),
            this.UV2.Y.ToString(CultureInfo.InvariantCulture)
        );

        string line = $"{this.UV0.X.ToString(CultureInfo.InvariantCulture)}";

        sw.WriteLine(string.Format("3 {0} staticmesh {1}", $"{this.VertexId0} {this.VertexId1} {this.VertexId2}", uvs));
    }
}
