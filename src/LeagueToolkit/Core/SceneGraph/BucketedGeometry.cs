using System.Numerics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.SceneGraph;

/// <summary>
/// Represents a scene graph with bucketed geometry data
/// </summary>
public class BucketedGeometry
{
    /// <summary>
    /// Gets the path hash
    /// </summary>
    public uint VisibilityControllerPathHash { get; }

    /// <summary>Gets the Min X bound</summary>
    public float MinX { get; }

    /// <summary>Gets the Min Z bound</summary>
    public float MinZ { get; }

    /// <summary>Gets the Max X bound</summary>
    public float MaxX { get; }

    /// <summary>Gets the Max Z bound</summary>
    public float MaxZ { get; }

    /// <summary>Gets the Max Stuck Out X bound</summary>
    public float MaxStickOutX { get; }

    /// <summary>Gets the Max Stick Out Z bound</summary>
    public float MaxStickOutZ { get; }

    /// <summary>Gets the size of a bucket accross the X axis</summary>
    public float BucketSizeX { get; }

    /// <summary>Gets the size of a bucket accross the Y axis</summary>
    public float BucketSizeZ { get; }

    /// <summary>Gets a value indicating whether the bucket grid is disabled</summary>
    public bool IsDisabled { get; }

    /// <summary>Gets a read-only view into the buckets</summary>
    public ReadOnlyMemory2D<GeometryBucket> Buckets => this._buckets;

    /// <summary>Gets a read-only view into the vertices</summary>
    public IReadOnlyList<Vector3> Vertices => this._vertices;

    /// <summary>Gets a read-only view into the indices</summary>
    public IReadOnlyList<ushort> Indices => this._indices;

    /// <summary>Gets a read-only view into the face visibility flags</summary>
    public IReadOnlyList<EnvironmentVisibility> FaceVisibilityFlags => this._faceVisibilityFlags;

    private readonly GeometryBucket[,] _buckets;

    private readonly Vector3[] _vertices;
    private readonly ushort[] _indices;

    private readonly EnvironmentVisibility[] _faceVisibilityFlags;

    // TODO: This is temporary hack
    internal BucketedGeometry()
    {
        this.IsDisabled = true;
        this._buckets = new GeometryBucket[0, 0];
        this._vertices = Array.Empty<Vector3>();
        this._indices = Array.Empty<ushort>();
    }

    internal BucketedGeometry(BinaryReader br, int version)
    {
        if (version >= 15)
        {
            this.VisibilityControllerPathHash = br.ReadUInt32();
        }
        if(version >= 18)
        {
            float unkFloat = br.ReadSingle();
        }

        this.MinX = br.ReadSingle();
        this.MinZ = br.ReadSingle();
        this.MaxX = br.ReadSingle();
        this.MaxZ = br.ReadSingle();
        this.MaxStickOutX = br.ReadSingle();
        this.MaxStickOutZ = br.ReadSingle();
        this.BucketSizeX = br.ReadSingle();
        this.BucketSizeZ = br.ReadSingle();

        ushort bucketsPerSide = br.ReadUInt16();
        this.IsDisabled = br.ReadBoolean();
        BucketedGeometryFlags flags = (BucketedGeometryFlags)br.ReadByte();

        uint vertexCount = br.ReadUInt32();
        uint indexCount = br.ReadUInt32();

        // If disabled, do not read data
        if (this.IsDisabled)
            return;

        this._vertices = new Vector3[vertexCount];
        for (int i = 0; i < vertexCount; i++)
            this._vertices[i] = br.ReadVector3();

        this._indices = new ushort[indexCount];
        for (int i = 0; i < indexCount; i++)
            this._indices[i] = br.ReadUInt16();

        this._buckets = new GeometryBucket[bucketsPerSide, bucketsPerSide];
        for (int i = 0; i < bucketsPerSide; i++)
        for (int j = 0; j < bucketsPerSide; j++)
            this._buckets[i, j] = new(br);

        if (flags.HasFlag(BucketedGeometryFlags.HasFaceVisibilityFlags))
        {
            uint faceCount = indexCount / 3;
            this._faceVisibilityFlags = new EnvironmentVisibility[faceCount];

            for (int i = 0; i < faceCount; i++)
                this._faceVisibilityFlags[i] = (EnvironmentVisibility)br.ReadByte();
        }
    }

    internal void Write(BinaryWriter bw)
    {
        bw.Write(this.VisibilityControllerPathHash);

        bw.Write(this.MinX);
        bw.Write(this.MinZ);

        bw.Write(this.MaxX);
        bw.Write(this.MaxZ);

        bw.Write(this.MaxStickOutX);
        bw.Write(this.MaxStickOutZ);

        bw.Write(this.BucketSizeX);
        bw.Write(this.BucketSizeZ);

        ushort bucketsPerSide = (ushort)Math.Sqrt(this._buckets.Length);
        bw.Write(bucketsPerSide);
        bw.Write(this.IsDisabled);

        BucketedGeometryFlags flags = ComposeFlags();
        bw.Write((byte)flags);

        bw.Write(this.Vertices.Count);
        bw.Write(this.Indices.Count);

        // If bucket grid is disabled, do not write any data
        if (this.IsDisabled)
            return;

        foreach (Vector3 vertex in this.Vertices)
            bw.WriteVector3(vertex);

        foreach (ushort index in this.Indices)
            bw.Write(index);

        for (int i = 0; i < bucketsPerSide; i++)
        for (int j = 0; j < bucketsPerSide; j++)
            this._buckets[i, j].Write(bw);

        if (flags.HasFlag(BucketedGeometryFlags.HasFaceVisibilityFlags) && this.FaceVisibilityFlags is not null)
        {
            if (this.FaceVisibilityFlags.Count != this.Indices.Count / 3)
                throw new InvalidOperationException(
                    $"{nameof(this.FaceVisibilityFlags)}.Count is invalid, must be {nameof(this.Indices)}.Count / 3"
                );

            foreach (EnvironmentVisibility faceVisibilityFlag in this.FaceVisibilityFlags)
                bw.Write((byte)faceVisibilityFlag);
        }
    }

    private BucketedGeometryFlags ComposeFlags()
    {
        BucketedGeometryFlags flags = 0;

        if (this.FaceVisibilityFlags is not null)
            flags |= BucketedGeometryFlags.HasFaceVisibilityFlags;

        return flags;
    }

    /// <summary>
    /// Gets the AABB of a bucket at the specified coordinates
    /// </summary>
    /// <param name="x">The X coordinate/index of the bucket</param>
    /// <param name="z">The Z coordinate/index of the bucket</param>
    /// <returns>The <see cref="Box"/> for the specified bucket in world space</returns>
    public Box GetBucketBox(int x, int z)
    {
        float minX = this.BucketSizeX * x;
        float minZ = this.BucketSizeZ * z;

        return new(
            new(minX, float.MinValue, minZ),
            new(minX + this.BucketSizeX, float.MaxValue, minZ + this.BucketSizeZ)
        );
    }
}

[Flags]
public enum BucketedGeometryFlags : byte
{
    HasFaceVisibilityFlags = 1 << 0
}
