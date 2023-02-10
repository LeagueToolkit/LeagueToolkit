using LeagueToolkit.Hashing;
using System.Text;

namespace LeagueToolkit.Core.Environment;

/// <summary>
/// Represents an environment mesh primitive
/// </summary>
public readonly struct EnvironmentAssetMeshPrimitive
{
    /// <summary>Represents the default material name for a <see cref="EnvironmentAssetMeshPrimitive"/></summary>
    public const string MISSING_MATERIAL = "-missing@environment-";

    /// <summary>Gets a <see cref="Fnv1a"/> hash of <see cref="Material"/></summary>
    /// <remarks>⚠️ This is always set to 0 because the game computes the hash by itself</remarks>
    public uint Hash { get; }

    /// <summary>
    /// Gets the name of the StaticMaterialDef to use
    /// </summary>
    /// <remarks>
    /// The StaticMaterialDef structure can be found in the respective ".materials.bin" file
    /// </remarks>
    public string Material { get; }

    /// <summary>Gets the start index</summary>
    public int StartIndex { get; }

    /// <summary>Gets the index count</summary>
    public int IndexCount { get; }

    /// <summary>Gets the vertex count</summary>
    public int VertexCount => this.MaxVertex - this.MinVertex + 1;

    /// <summary>Gets the min vertex</summary>
    public int MinVertex { get; }

    /// <summary>Gets the max vertex</summary>
    public int MaxVertex { get; }

    internal EnvironmentAssetMeshPrimitive(
        string material,
        int startIndex,
        int indexCount,
        int minVertex,
        int maxVertex
    )
    {
        this.Material = material ?? MISSING_MATERIAL;
        this.StartIndex = startIndex;
        this.IndexCount = indexCount;
        this.MinVertex = minVertex;
        this.MaxVertex = maxVertex;
    }

    internal EnvironmentAssetMeshPrimitive(BinaryReader br)
    {
        this.Hash = br.ReadUInt32();
        this.Material = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
        this.StartIndex = br.ReadInt32();
        this.IndexCount = br.ReadInt32();
        this.MinVertex = br.ReadInt32();
        this.MaxVertex = br.ReadInt32();
    }

    internal void Write(BinaryWriter bw)
    {
        bw.Write(this.Hash);
        bw.Write(this.Material.Length);
        bw.Write(Encoding.ASCII.GetBytes(this.Material));
        bw.Write(this.StartIndex);
        bw.Write(this.IndexCount);
        bw.Write(this.MinVertex);
        bw.Write(this.MaxVertex);
    }
}
