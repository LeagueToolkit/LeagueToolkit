using System.Buffers.Binary;
using System.Numerics;
using System.Text;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Mesh;

/// <summary>
/// Reads and writes the version 1 Riot render-mesh format shared by GMESH and TMESH.
/// Streams preserve their original bytes, including half-float attributes and unused layout slots.
/// </summary>
public sealed class RenderMesh
{
    private static readonly UTF8Encoding Encoding = new(false, true);
    public ReadOnlyMemory<byte> Magic { get; }
    public Box BoundingBox { get; }
    public int VertexCount { get; }
    public IReadOnlyList<RenderMeshVertexStream> VertexStreams { get; }
    public IReadOnlyList<ushort> Indices { get; }
    public IReadOnlyList<RenderMeshSubmesh> Submeshes { get; }

    private RenderMesh(byte[] magic, Box bounds, int vertexCount, RenderMeshVertexStream[] streams, ushort[] indices, RenderMeshSubmesh[] submeshes)
    {
        this.Magic = magic;
        this.BoundingBox = bounds;
        this.VertexCount = vertexCount;
        this.VertexStreams = Array.AsReadOnly(streams);
        this.Indices = Array.AsReadOnly(indices);
        this.Submeshes = Array.AsReadOnly(submeshes);
    }

    /// <summary>Reads an authored mesh; as in Riot's parser, magic is retained rather than validated.</summary>
    public static RenderMesh Read(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var reader = new BinaryReader(stream, Encoding, leaveOpen: true);
        byte[] magic = ReadBlock(reader, 4);
        if (reader.ReadUInt32() != 1) throw new InvalidDataException("Unsupported render mesh version.");
        int vertices = ReadCount(reader), indexCount = ReadCount(reader);
        Box bounds = reader.ReadBox();
        int streamCount = ReadCount(reader);
        // Each stream must introduce at least one unique name from Riot's 15-name enum.
        if (streamCount < 1 || streamCount > 15) throw new InvalidDataException("Invalid vertex stream count.");
        var descriptions = new (VertexBufferDescription Description, byte[] Bytes)[streamCount];
        var names = new HashSet<ElementName>();
        for (int s = 0; s < streamCount; s++)
        {
            byte[] bytes = ReadBlock(reader, 128);
            uint usage = BinaryPrimitives.ReadUInt32LittleEndian(bytes);
            uint count = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(4));
            if (usage > 2 || count < 1 || count > 15) throw new InvalidDataException("Invalid vertex buffer description.");
            var elements = new VertexElement[count];
            for (int e = 0; e < elements.Length; e++)
            {
                uint name = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(8 + e * 8));
                uint format = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(12 + e * 8));
                if (name > 14 || !names.Add((ElementName)name))
                    throw new InvalidDataException("Invalid or duplicate vertex element name.");
                elements[e] = new VertexElement((ElementName)name, MapFormat(format));
            }
            descriptions[s] = (new VertexBufferDescription((VertexBufferUsage)usage, elements), bytes);
        }
        var streams = new RenderMeshVertexStream[streamCount];
        for (int s = 0; s < streamCount; s++)
        {
            var (description, bytes) = descriptions[s];
            int size = ReadCount(reader);
            int stride = description.Elements.Sum(element => element.GetSize());
            if ((long)stride * vertices != size) throw new InvalidDataException("Vertex stream size does not match its description.");
            streams[s] = new RenderMeshVertexStream(description, bytes, ReadBlock(reader, size), vertices);
        }
        // Decode indices only after a complete, bounded read of their bytes.
        long indexBytes = (long)indexCount * 2;
        if (indexBytes > int.MaxValue) throw new InvalidDataException("Index buffer is too large.");
        byte[] rawIndices = ReadBlock(reader, (int)indexBytes);
        var indices = new ushort[indexCount];
        for (int i = 0; i < indexCount; i++)
        {
            indices[i] = BinaryPrimitives.ReadUInt16LittleEndian(rawIndices.AsSpan(i * 2));
            if (indices[i] >= vertices) throw new InvalidDataException("Index is outside the vertex streams.");
        }
        int submeshCount = ReadCount(reader);
        RequireRemaining(reader, (long)submeshCount * 20);
        // Avoid allocating a caller-controlled table before its rows have been read.
        var submeshes = new List<RenderMeshSubmesh>();
        for (int s = 0; s < submeshCount; s++)
        {
            string material = Encoding.GetString(ReadBlock(reader, ReadCount(reader)));
            uint start = reader.ReadUInt32(), count = reader.ReadUInt32();
            uint min = reader.ReadUInt32(), max = reader.ReadUInt32();
            if ((ulong)start + count > (ulong)indexCount) throw new InvalidDataException("Submesh exceeds the index buffer.");
            submeshes.Add(new RenderMeshSubmesh(material, start, count, min, max));
        }
        return new RenderMesh(magic, bounds, vertices, streams, indices, submeshes.ToArray());
    }

    /// <summary>Writes the mesh without changing layouts, attribute precision or authored bounds.</summary>
    public void Write(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var writer = new BinaryWriter(stream, Encoding, leaveOpen: true);
        writer.Write(this.Magic.Span);
        writer.Write(1u);
        writer.Write((uint)this.VertexCount);
        writer.Write((uint)this.Indices.Count);
        writer.WriteBox(this.BoundingBox);
        writer.Write((uint)this.VertexStreams.Count);
        foreach (RenderMeshVertexStream vertexStream in this.VertexStreams) writer.Write(vertexStream.SerializedDescription);
        foreach (RenderMeshVertexStream vertexStream in this.VertexStreams)
        {
            writer.Write((uint)vertexStream.Data.Length);
            writer.Write(vertexStream.Data.Span);
        }
        foreach (ushort index in this.Indices) writer.Write(index);
        writer.Write((uint)this.Submeshes.Count);
        foreach (RenderMeshSubmesh submesh in this.Submeshes)
        {
            byte[] name = Encoding.GetBytes(submesh.Material);
            writer.Write((uint)name.Length);
            writer.Write(name);
            writer.Write(submesh.StartIndex);
            writer.Write(submesh.IndexCount);
            writer.Write(submesh.MinVertex);
            writer.Write(submesh.MaxVertex);
        }
    }

    public bool TryGetAccessor(ElementName name, out VertexElementAccessor accessor)
    {
        foreach (RenderMeshVertexStream vertexStream in this.VertexStreams)
            if (vertexStream.TryGetAccessor(name, out accessor)) return true;
        accessor = default;
        return false;
    }

    public VertexElementAccessor GetAccessor(ElementName name) =>
        this.TryGetAccessor(name, out VertexElementAccessor accessor) ? accessor : throw new KeyNotFoundException($"Mesh has no vertex element: {name}.");

    /// <summary>Widens a float or half-float attribute to XYZ, ignoring an authored fourth component.</summary>
    public Vector3 ReadVector3(ElementName name, int vertex)
    {
        VertexElementAccessor accessor = this.GetAccessor(name);
        if ((uint)vertex >= (uint)this.VertexCount) throw new ArgumentOutOfRangeException(nameof(vertex));
        ReadOnlySpan<byte> value = accessor.DecodeAt(vertex);
        return accessor.Element.Format switch
        {
            ElementFormat.XYZ_Float32 or ElementFormat.XYZW_Float32 => new(ReadFloat(value), ReadFloat(value[4..]), ReadFloat(value[8..])),
            ElementFormat.XYZW_Packed16161616 => new(ReadHalf(value), ReadHalf(value[2..]), ReadHalf(value[4..])),
            _ => throw new InvalidOperationException("Element cannot be decoded as a Vector3.")
        };
    }

    public Vector2 ReadVector2(ElementName name, int vertex)
    {
        VertexElementAccessor accessor = this.GetAccessor(name);
        if ((uint)vertex >= (uint)this.VertexCount) throw new ArgumentOutOfRangeException(nameof(vertex));
        ReadOnlySpan<byte> value = accessor.DecodeAt(vertex);
        return accessor.Element.Format switch
        {
            ElementFormat.XY_Float32 or ElementFormat.XYZ_Float32 or ElementFormat.XYZW_Float32 => new(ReadFloat(value), ReadFloat(value[4..])),
            ElementFormat.XY_Packed1616 or ElementFormat.XYZW_Packed16161616 => new(ReadHalf(value), ReadHalf(value[2..])),
            _ => throw new InvalidOperationException("Element cannot be decoded as a Vector2.")
        };
    }

    private static float ReadFloat(ReadOnlySpan<byte> bytes) => BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(bytes));
    private static float ReadHalf(ReadOnlySpan<byte> bytes) => (float)BitConverter.UInt16BitsToHalf(BinaryPrimitives.ReadUInt16LittleEndian(bytes));

    private static ElementFormat MapFormat(uint format) => format switch
    {
        <= 4 => (ElementFormat)format,
        5 => ElementFormat.RGBA_Packed8888,
        6 => ElementFormat.XYZW_Packed8888,
        7 => ElementFormat.XY_Packed1616,
        8 => ElementFormat.XYZW_Packed16161616,
        _ => throw new InvalidDataException("Invalid render mesh element format.")
    };

    private static int ReadCount(BinaryReader reader)
    {
        uint count = reader.ReadUInt32();
        return count <= int.MaxValue ? (int)count : throw new InvalidDataException("Render mesh count is too large.");
    }

    private static void RequireRemaining(BinaryReader reader, long bytes)
    {
        if (reader.BaseStream.CanSeek && bytes > reader.BaseStream.Length - reader.BaseStream.Position)
            throw new EndOfStreamException("Truncated render mesh.");
    }

    private static byte[] ReadBlock(BinaryReader reader, int count)
    {
        RequireRemaining(reader, count);
        if (reader.BaseStream.CanSeek)
        {
            byte[] bytes = reader.ReadBytes(count);
            if (bytes.Length != count) throw new EndOfStreamException("Truncated render mesh.");
            return bytes;
        }
        // Grow only as data arrives on non-seekable streams.
        using var buffer = new MemoryStream();
        byte[] chunk = new byte[Math.Min(count, 4096)];
        while (count > 0)
        {
            int read = reader.Read(chunk, 0, Math.Min(count, chunk.Length));
            if (read == 0) throw new EndOfStreamException("Truncated render mesh.");
            buffer.Write(chunk, 0, read);
            count -= read;
        }
        return buffer.ToArray();
    }
}
