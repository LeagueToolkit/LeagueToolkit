using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Exceptions;
using LeagueToolkit.Utils.Extensions;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Mesh;

public class StaticMesh
{
    public string Name { get; }

    public bool HasVertexColors { get; }

    public IReadOnlyList<Vector3> Vertices => this._vertices;
    private readonly Vector3[] _vertices;

    public IReadOnlyList<Color> VertexColors => this._vertexColors;
    private readonly Color[] _vertexColors;

    public IReadOnlyList<StaticMeshFace> Faces => this._faces;
    private readonly StaticMeshFace[] _faces;

    public StaticMesh(string name, IEnumerable<StaticMeshFace> faces, IEnumerable<Vector3> vertices)
    {
        Guard.IsNotNull(name, nameof(name));
        Guard.IsNotNull(faces, nameof(faces));
        Guard.IsNotNull(vertices, nameof(vertices));

        this.Name = name;

        this._vertices = vertices.ToArray();
        this._faces = faces.ToArray();

        this.HasVertexColors = false;
    }

    public StaticMesh(
        string name,
        IEnumerable<StaticMeshFace> faces,
        IEnumerable<Vector3> vertices,
        IEnumerable<Color> vertexColors
    )
    {
        Guard.IsNotNull(name, nameof(name));
        Guard.IsNotNull(faces, nameof(faces));
        Guard.IsNotNull(vertices, nameof(vertices));
        Guard.IsNotNull(vertexColors, nameof(vertexColors));

        this.Name = name;

        this.HasVertexColors = true;

        this._vertices = vertices.ToArray();
        this._vertexColors = vertexColors.ToArray();
        this._faces = faces.ToArray();

        if (this._vertices.Length != this._vertexColors.Length)
            ThrowHelper.ThrowArgumentException("Vertex colors count must match vertices count");
    }

    public static StaticMesh ReadBinary(Stream stream)
    {
        using BinaryReader br = new(stream);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
        if (magic is not "r3d2Mesh")
            throw new InvalidFileSignatureException();

        ushort major = br.ReadUInt16();
        ushort minor = br.ReadUInt16();
        if (major is not (2 or 3) && minor is not 1) //There are versions [2][1] and [1][1] aswell
            throw new InvalidFileVersionException();

        string name = br.ReadPaddedString(128);

        int vertexCount = br.ReadInt32();
        int faceCount = br.ReadInt32();

        StaticMeshFlags flags = (StaticMeshFlags)br.ReadUInt32();
        Box boundingBox = br.ReadBox();

        bool hasVertexColors = (major, minor) switch
        {
            (>= 3, >= 2) => br.ReadUInt32() is 1,
            _ => false
        };

        // Read vertices
        Vector3[] vertices = new Vector3[vertexCount];
        br.Read(vertices.AsSpan().Cast<Vector3, byte>());

        // Read vertex colors
        Color[] vertexColors = Array.Empty<Color>();
        if (hasVertexColors)
        {
            vertexColors = new Color[vertexCount];

            for (int i = 0; i < vertexCount; i++)
                vertexColors[i] = br.ReadColor(ColorFormat.BgraU8);
        }

        Vector3 centralPoint = br.ReadVector3();

        // Read faces
        StaticMeshFace[] faces = new StaticMeshFace[faceCount];
        for (int i = 0; i < faceCount; i++)
            faces[i] = StaticMeshFace.ReadBinary(br);

        // Read face vertex colors ??
        if (flags.HasFlag(StaticMeshFlags.HasVcp))
            for (int i = 0; i < faceCount; i++)
                faces[i] = faces[i] with
                {
                    Color0 = br.ReadColor(ColorFormat.RgbU8),
                    Color1 = br.ReadColor(ColorFormat.RgbU8),
                    Color2 = br.ReadColor(ColorFormat.RgbU8)
                };

        return hasVertexColors switch
        {
            true => new(name, faces, vertices, vertexColors),
            false => new(name, faces, vertices)
        };
    }

    public static StaticMesh ReadAscii(Stream stream)
    {
        using StreamReader sr = new(stream);

        char[] splittingArray = new char[] { ' ' };
        string[] input = null;

        if (sr.ReadLine() != "[ObjectBegin]")
            throw new InvalidFileSignatureException();

        input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);
        string name = input.Length != 1 ? input[1] : string.Empty;

        input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);
        Vector3 centralPoint =
            new(
                float.Parse(input[1], CultureInfo.InvariantCulture),
                float.Parse(input[2], CultureInfo.InvariantCulture),
                float.Parse(input[3], CultureInfo.InvariantCulture)
            );
        Vector3 pivotPoint = centralPoint;

        bool hasVertexColors = false;

        input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);
        if (input[0] == "PivotPoint=")
        {
            pivotPoint = new Vector3(
                float.Parse(input[1], CultureInfo.InvariantCulture),
                float.Parse(input[2], CultureInfo.InvariantCulture),
                float.Parse(input[3], CultureInfo.InvariantCulture)
            );
        }
        else if (input[0] == "VertexColors=")
        {
            hasVertexColors = uint.Parse(input[1]) != 0;
        }

        int vertexCount = 0;
        if (input[0] == "Verts=")
        {
            vertexCount = int.Parse(input[1]);
        }
        else
        {
            vertexCount = int.Parse(sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries)[1]);
        }

        Vector3[] vertices = new Vector3[vertexCount];
        Color[] vertexColors = Array.Empty<Color>();
        for (int i = 0; i < vertexCount; i++)
        {
            input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);

            vertices[i] = new Vector3(
                float.Parse(input[0], CultureInfo.InvariantCulture),
                float.Parse(input[1], CultureInfo.InvariantCulture),
                float.Parse(input[2], CultureInfo.InvariantCulture)
            );
        }

        if (hasVertexColors)
        {
            vertexColors = new Color[vertexCount];

            for (int i = 0; i < vertexCount; i++)
            {
                string[] colorComponents = sr.ReadLine()
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (colorComponents.Length != 4)
                {
                    throw new Exception("Invalid number of vertex color components: " + colorComponents.Length);
                }

                byte r = byte.Parse(colorComponents[0]);
                byte g = byte.Parse(colorComponents[1]);
                byte b = byte.Parse(colorComponents[2]);
                byte a = byte.Parse(colorComponents[3]);

                vertexColors[i] = new(r, g, b, a);
            }
        }

        int faceCount = int.Parse(sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries)[1]);
        StaticMeshFace[] faces = new StaticMeshFace[faceCount];
        for (int i = 0; i < faceCount; i++)
            faces[i] = StaticMeshFace.ReadAscii(sr);

        return hasVertexColors switch
        {
            true => new(name, faces, vertices, vertexColors),
            false => new(name, faces, vertices)
        };
    }

    public void WriteBinary(Stream stream)
    {
        using BinaryWriter bw = new(stream, Encoding.UTF8, true);

        StaticMeshFlags flags = StaticMeshFlags.HasLocalOriginLocatorAndPivot;
        Box aabb = Box.FromVertices(this.Vertices);

        // Figure out if we need to write face vertex colors
        foreach (StaticMeshFace face in this.Faces)
            if (face.Color0 != Color.One || face.Color1 != Color.One || face.Color2 != Color.One)
            {
                flags |= StaticMeshFlags.HasVcp;
                break;
            }

        bw.Write("r3d2Mesh"u8);
        bw.Write((ushort)3);
        bw.Write((ushort)2);
        bw.WritePaddedString(this.Name, 128);

        bw.Write(this.Vertices.Count);
        bw.Write(this.Faces.Count);

        bw.Write((uint)flags);
        bw.WriteBox(aabb);
        bw.Write(this.HasVertexColors ? 1 : 0);

        // Write vertices
        foreach (Vector3 vertex in this.Vertices)
            bw.WriteVector3(vertex);

        // Write vertex colors
        if (this.HasVertexColors)
            foreach (Color vertexColor in this.VertexColors)
                bw.WriteColor(vertexColor, ColorFormat.RgbaU8);

        bw.WriteVector3(aabb.GetCentralPoint());

        // Write faces
        foreach (StaticMeshFace face in this.Faces)
            face.WriteBinary(bw);

        // TODO: Write face vertex colors
        if (flags.HasFlag(StaticMeshFlags.HasVcp))
            foreach (StaticMeshFace face in this.Faces)
            {
                bw.WriteColor(face.Color0, ColorFormat.RgbU8);
                bw.WriteColor(face.Color1, ColorFormat.RgbU8);
                bw.WriteColor(face.Color2, ColorFormat.RgbU8);
            }
    }

    public void WriteAscii(Stream stream)
    {
        using StreamWriter sw = new(stream, Encoding.UTF8, leaveOpen: true);

        Vector3 centralPoint = Box.FromVertices(this.Vertices).GetCentralPoint();

        sw.WriteLine("[ObjectBegin]");
        sw.WriteLine("Name= " + this.Name);
        sw.WriteLine("CentralPoint= " + centralPoint.ToString());
        sw.WriteLine("PivotPoint= " + centralPoint.ToString());

        if (this.HasVertexColors)
            sw.WriteLine("VertexColors= 1");

        sw.WriteLine("Verts= " + this.Vertices.Count);
        foreach (Vector3 vertex in this.Vertices)
            sw.WriteLine("{0} {1} {2}", vertex.X, vertex.Y, vertex.Z);

        if (this.HasVertexColors)
        {
            foreach (Color vertexColor in this.VertexColors)
            {
                sw.WriteColor(vertexColor, ColorFormat.RgbaU8);
                sw.Write('\n');
            }
        }

        sw.WriteLine("Faces= " + this.Faces.Count);
        foreach (StaticMeshFace face in this.Faces)
            face.WriteAscii(sw);

        sw.WriteLine("[ObjectEnd]");
    }
}

[Flags]
internal enum StaticMeshFlags : uint
{
    HasVcp = 1,
    HasLocalOriginLocatorAndPivot = 2
}
