using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Exceptions;
using LeagueToolkit.Utils.Extensions;
using System.Text;

namespace LeagueToolkit.Core.Mesh;

/// <summary>Represents a skinned mesh</summary>
/// <remarks>
/// The <see cref="SkinnedMesh"/> class can instantiated by reading a Simple Skin (.skn) file
/// </remarks>
public sealed class SkinnedMesh : IDisposable
{
    /// <summary>Gets the mesh's AABB</summary>
    public Box AABB { get; private set; }

    /// <summary>Gets the mesh's Bounding Sphere</summary>
    public Sphere BoundingSphere { get; private set; }

    /// <summary>Gets a read-only list of the mesh's primitive ranges</summary>
    public IReadOnlyList<SkinnedMeshRange> Ranges => this._ranges;
    private readonly SkinnedMeshRange[] _ranges;

    /// <summary>Gets a view into the mesh's vertex buffer</summary>
    public IVertexBufferView VerticesView => this._vertexBuffer;

    /// <summary>Gets a read-only view into the mesh's index buffer</summary>
    public IndexArray Indices => this._indexBuffer.AsArray();

    private readonly VertexBuffer _vertexBuffer;
    private readonly IndexBuffer _indexBuffer;

    /// <summary>Gets a value indicating whether the <see cref="SkinnedMesh"/> has been disposed of</summary>
    public bool IsDisposed { get; private set; }

    /// <summary>Creates a new <see cref="SkinnedMesh"/> object with the specified parameters</summary>
    /// <param name="ranges">The ranges of the <see cref="SkinnedMesh"/></param>
    /// <param name="vertexBuffer">The vertex buffer of the <see cref="SkinnedMesh"/></param>
    /// <param name="indexBuffer">The index buffer of the <see cref="SkinnedMesh"/></param>
    public SkinnedMesh(IEnumerable<SkinnedMeshRange> ranges, VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
    {
        this._ranges = ranges.ToArray();
        this._vertexBuffer = vertexBuffer;
        this._indexBuffer = indexBuffer;

        this.AABB = Box.FromVertices(vertexBuffer.GetAccessor(ElementName.Position).AsVector3Array());
        this.BoundingSphere = this.AABB.GetBoundingSphere();
    }

    /// <summary>
    /// Reads a <see cref="SkinnedMesh"/> object from the specified <paramref name="file"/>
    /// </summary>
    /// <param name="file">The file to read from</param>
    /// <returns>The read <see cref="SkinnedMesh"/> object</returns>
    public static SkinnedMesh ReadFromSimpleSkin(string file) => ReadFromSimpleSkin(File.OpenRead(file));

    /// <summary>
    /// Reads a <see cref="SkinnedMesh"/> object from the specified <paramref name="stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from</param>
    /// <param name="leaveOpen">Whether to leave <paramref name="stream"/> opened</param>
    /// <returns>The read <see cref="SkinnedMesh"/> object</returns>
    public static SkinnedMesh ReadFromSimpleSkin(Stream stream, bool leaveOpen = false)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, leaveOpen);

        uint magic = br.ReadUInt32();
        if (magic != 0x00112233)
            throw new InvalidFileSignatureException();

        ushort major = br.ReadUInt16();
        ushort minor = br.ReadUInt16();
        if (major is not (0 or 2 or 4) && minor is not 1)
            throw new InvalidFileVersionException();

        int indexCount = 0;
        int vertexCount = 0;
        VertexBufferDescription vertexDeclaration = SkinnedMeshVertex.BASIC;
        Box boundingBox = new();
        Sphere boundingSphere = Sphere.INFINITE;
        SkinnedMeshRange[] ranges;
        if (major is 0)
        {
            indexCount = br.ReadInt32();
            vertexCount = br.ReadInt32();

            ranges = new SkinnedMeshRange[] { new("Base", 0, vertexCount, 0, indexCount) };
        }
        else
        {
            uint rangeCount = br.ReadUInt32();
            ranges = new SkinnedMeshRange[rangeCount];
            for (int i = 0; i < rangeCount; i++)
            {
                ranges[i] = SkinnedMeshRange.ReadFromSimpleSkin(br);
            }

            if (major is 4)
            {
                uint flags = br.ReadUInt32();
            }

            indexCount = br.ReadInt32();
            vertexCount = br.ReadInt32();

            if (major is 4)
            {
                uint vertexSize = br.ReadUInt32();
                SkinnedMeshVertexType vertexType = (SkinnedMeshVertexType)br.ReadUInt32();
                vertexDeclaration = (vertexSize, vertexType) switch
                {
                    (52, SkinnedMeshVertexType.Basic) => SkinnedMeshVertex.BASIC,
                    (56, SkinnedMeshVertexType.Color) => SkinnedMeshVertex.COLOR,
                    (72, SkinnedMeshVertexType.Tangent) => SkinnedMeshVertex.TANGENT,
                    _
                        => throw new InvalidOperationException(
                            $"Vertex size: {vertexSize} is not correct for {vertexType}, "
                                + $"expected: {GetDescriptionForVertexType(vertexType).GetVertexSize()}"
                        )
                };

                boundingBox = br.ReadBox();
                boundingSphere = br.ReadSphere();
            }
        }

        // Allocate buffer memory
        var indexBufferOwner = MemoryOwner<byte>.Allocate(indexCount * IndexBuffer.GetFormatSize(IndexFormat.U16));
        var vertexBufferOwner = MemoryOwner<byte>.Allocate(vertexDeclaration.GetVertexSize() * vertexCount);

        // Read buffers
        br.BaseStream.ReadExact(indexBufferOwner.Span);
        br.BaseStream.ReadExact(vertexBufferOwner.Span);

        // Create buffers
        var indexBuffer = IndexBuffer.Create(IndexFormat.U16, indexBufferOwner);
        var vertexBuffer = VertexBuffer.Create(vertexDeclaration.Usage, vertexDeclaration.Elements, vertexBufferOwner);

        return new(ranges, vertexBuffer, indexBuffer);
    }

    /// <summary>
    /// Writes the <see cref="SkinnedMesh"/> into the specified <paramref name="file"/>
    /// </summary>
    /// <param name="file">The file to write into</param>
    public void WriteSimpleSkin(string file) => WriteSimpleSkin(File.Create(file));

    /// <summary>
    /// Writes the <see cref="SkinnedMesh"/> into the specified <paramref name="stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to write into</param>
    /// <param name="leaveOpen">Whether to leave <paramref name="stream"/> opened</param>
    public void WriteSimpleSkin(Stream stream, bool leaveOpen = false)
    {
        using BinaryWriter bw = new(stream, Encoding.UTF8, leaveOpen);

        bw.Write(0x00112233);
        bw.Write((ushort)4);
        bw.Write((ushort)1);
        bw.Write(this.Ranges.Count);

        foreach (SkinnedMeshRange range in this.Ranges)
            range.WriteToSimpleSkin(bw);

        bw.Write((uint)0); // Flags
        bw.Write(this._indexBuffer.Count);
        bw.Write(this._vertexBuffer.VertexCount);
        bw.Write(this._vertexBuffer.VertexStride);
        bw.Write((int)GetVertexTypeForDescription(this._vertexBuffer.Description));

        bw.WriteBox(this.AABB);
        bw.WriteSphere(this.BoundingSphere);

        bw.Write(this._indexBuffer.Buffer.Span);
        bw.Write(this._vertexBuffer.View.Span);

        Span<byte> endTab = stackalloc byte[12];
        endTab.Clear();
        bw.Write(endTab);
    }

    private static VertexBufferDescription GetDescriptionForVertexType(SkinnedMeshVertexType vertexType) =>
        vertexType switch
        {
            SkinnedMeshVertexType.Basic => SkinnedMeshVertex.BASIC,
            SkinnedMeshVertexType.Color => SkinnedMeshVertex.COLOR,
            SkinnedMeshVertexType.Tangent => SkinnedMeshVertex.TANGENT,
            _ => throw new NotImplementedException($"{vertexType} is not a valid {nameof(SkinnedMeshVertexType)}")
        };

    private static SkinnedMeshVertexType GetVertexTypeForDescription(VertexBufferDescription description)
    {
        return description switch
        {
            _ when description == SkinnedMeshVertex.BASIC => SkinnedMeshVertexType.Basic,
            _ when description == SkinnedMeshVertex.COLOR => SkinnedMeshVertexType.Color,
            _ when description == SkinnedMeshVertex.TANGENT => SkinnedMeshVertexType.Tangent,
            _
                => throw new NotImplementedException(
                    $"The specified {nameof(VertexBufferDescription)} is not a valid skinned mesh vertex description"
                )
        };
    }

    private void Dispose(bool disposing)
    {
        if (!this.IsDisposed)
        {
            if (disposing)
            {
                this._vertexBuffer?.Dispose();
                this._indexBuffer?.Dispose();
            }

            this.IsDisposed = true;
        }
    }

    /// <summary>
    /// Disposes the <see cref="SkinnedMesh"/> object and its buffers
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Contains <see cref="SkinnedMesh"/> vertex descriptions
/// </summary>
public static class SkinnedMeshVertex
{
    /// <summary>
    /// Represents a basic vertex
    /// </summary>
    public static readonly VertexBufferDescription BASIC =
        new(
            VertexBufferUsage.Static,
            new[]
            {
                VertexElement.POSITION,
                VertexElement.BLEND_INDEX,
                VertexElement.BLEND_WEIGHT,
                VertexElement.NORMAL,
                VertexElement.TEXCOORD_0
            }
        );

    /// <summary>
    /// Represents a vertex with a <see cref="ElementName.PrimaryColor"/>
    /// </summary>
    public static readonly VertexBufferDescription COLOR =
        new(
            VertexBufferUsage.Static,
            new[]
            {
                VertexElement.POSITION,
                VertexElement.BLEND_INDEX,
                VertexElement.BLEND_WEIGHT,
                VertexElement.NORMAL,
                VertexElement.TEXCOORD_0,
                VertexElement.PRIMARY_COLOR
            }
        );

    /// <summary>
    /// Represents a vertex with a <see cref="ElementName.PrimaryColor"/> and a <see cref="ElementName.Tangent"/>
    /// </summary>
    public static readonly VertexBufferDescription TANGENT =
        new(
            VertexBufferUsage.Static,
            new[]
            {
                VertexElement.POSITION,
                VertexElement.BLEND_INDEX,
                VertexElement.BLEND_WEIGHT,
                VertexElement.NORMAL,
                VertexElement.TEXCOORD_0,
                VertexElement.PRIMARY_COLOR,
                VertexElement.TANGENT
            }
        );
}

internal enum SkinnedMeshVertexType : int
{
    Basic,
    Color,
    Tangent
}
