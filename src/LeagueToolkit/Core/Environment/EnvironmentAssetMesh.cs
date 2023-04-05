using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;
using SixLabors.ImageSharp.ColorSpaces;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Environment;

/// <summary>
/// Represents a mesh inside of an <see cref="EnvironmentAsset"/>
/// </summary>
public sealed class EnvironmentAssetMesh
{
    /// <summary>
    /// Gets the mesh instance name
    /// </summary>
    /// <remarks>
    /// This feature is supported only if <c>version &lt; 12</c>
    /// </remarks>
    public string Name { get; private set; }

    /// <summary>
    /// Gets a view into the vertex buffer
    /// </summary>
    public InstancedVertexBufferView VerticesView { get; private set; }

    /// <summary>
    /// Gets a view into the index buffer
    /// </summary>
    public IndexArray Indices { get; private set; }

    /// <summary>
    /// Gets a read-only collection of the mesh's primitives
    /// </summary>
    public IReadOnlyList<EnvironmentAssetMeshPrimitive> Submeshes => this._submeshes;
    private readonly List<EnvironmentAssetMeshPrimitive> _submeshes = new();

    /// <summary>
    /// Gets whether to disable backface culling for the mesh
    /// </summary>
    public bool DisableBackfaceCulling { get; private set; }

    /// <summary>
    /// Gets a <see cref="Box"/> which represents the mesh AABB
    /// </summary>
    public Box BoundingBox { get; private set; }

    /// <summary>
    /// Gets the mesh transform
    /// </summary>
    public Matrix4x4 Transform { get; private set; }

    /// <summary>
    /// Tells the game on which "Environment Quality" settings this mesh should be rendered
    /// </summary>
    public EnvironmentQuality EnvironmentQualityFilter { get; private set; } = EnvironmentQuality.AllQualities;

    /// <summary>
    /// Tells the game on which Visibility Flags this mesh should be rendered
    /// </summary>
    public EnvironmentVisibility VisibilityFlags { get; private set; } = EnvironmentVisibility.AllLayers;

    public EnvironmentVisibilityTransitionBehavior LayerTransitionBehavior { get; set; }

    /// <summary>Gets the render flags of the mesh</summary>
    public EnvironmentAssetMeshRenderFlags RenderFlags { get; private set; }

    /// <remarks>
    /// This feature is supported only if <c>version &lt; 7</c>
    /// </remarks>
    public Vector3? PointLight { get; private set; }

    /// <summary>
    /// For more information about generating light probes,
    /// see <see href="https://docs.unity3d.com/Manual/LightProbes-TechnicalInformation.html">Unity - Light Probes</see>
    /// </summary>
    /// <remarks>
    /// This feature is supported only if <c>version &lt; 9</c><br></br>
    /// Since version 9, terrain meshes use baked light instead
    /// </remarks>
    public IReadOnlyList<Vector3> SphericalHarmonics => this._sphericalHarmonics;
    private readonly Vector3[] _sphericalHarmonics;

    /// <summary>Gets the <c>"STATIONARY_LIGHT"</c> sampler data</summary>
    /// <remarks>Usually contains a diffuse texture</remarks>
    public EnvironmentAssetChannel StationaryLight { get; private set; }

    /// <summary>Gets the <c>"BAKED_LIGHT"</c> sampler data</summary>
    /// <remarks>Usually contains a lightmap texture (baked from scene point lights)</remarks>
    public EnvironmentAssetChannel BakedLight { get; private set; }

    /// <summary>Gets the <c>"BAKED_PAINT"</c> sampler data</summary>
    /// <remarks>Usually contains a texture with baked diffuse and lightmap data</remarks>
    public EnvironmentAssetChannel BakedPaint { get; private set; }

    internal int _baseVertexBufferDescriptionId;
    internal int[] _vertexBufferIds;
    internal int _indexBufferId;

    /// <summary>
    /// Represents the maximum primitive count of an <see cref="EnvironmentAssetMesh"/>
    /// </summary>
    public const int MAX_PRIMITIVE_COUNT = 64;

    internal EnvironmentAssetMesh(
        int id,
        IVertexBufferView vertexBufferView,
        IndexArray indexBufferView,
        IEnumerable<EnvironmentAssetMeshPrimitive> submeshes,
        Matrix4x4 transform,
        bool disableBackfaceCulling,
        EnvironmentQuality environmentQualityFilter,
        EnvironmentVisibility visibilityFlags,
        EnvironmentAssetMeshRenderFlags renderFlags,
        EnvironmentAssetChannel stationaryLight,
        EnvironmentAssetChannel bakedLight,
        EnvironmentAssetChannel bakedPaint
    )
    {
        this.Name = CreateName(id);

        this.VerticesView = new(vertexBufferView.VertexCount, new[] { vertexBufferView });
        this.Indices = indexBufferView;
        this._submeshes = new(submeshes);

        this.Transform = transform;

        this.DisableBackfaceCulling = disableBackfaceCulling;
        this.EnvironmentQualityFilter = environmentQualityFilter;
        this.VisibilityFlags = visibilityFlags;
        this.RenderFlags = renderFlags;

        this.StationaryLight = stationaryLight;
        this.BakedLight = bakedLight;
        this.BakedPaint = bakedPaint;

        this.BoundingBox = Box.FromVertices(vertexBufferView.GetAccessor(ElementName.Position).AsVector3Array());
    }

    internal EnvironmentAssetMesh(
        int id,
        EnvironmentAsset environmentAsset,
        BinaryReader br,
        IReadOnlyList<VertexBufferDescription> vertexDeclarations,
        IReadOnlyList<long> vertexBufferOffsets,
        bool useSeparatePointLights,
        uint version
    )
    {
        this.Name = version <= 11 ? Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())) : CreateName(id);

        int vertexCount = br.ReadInt32();
        uint vertexDeclarationCount = br.ReadUInt32();

        // This ID is always that of the "instanced" vertex buffer which
        // means that the data of the first vertex buffer is instanced (unique to this mesh instance).
        // (Assuming that this mesh uses at least 2 vertex buffers)
        int vertexDeclarationId = br.ReadInt32();

        IVertexBufferView[] vertexBufferViews = new IVertexBufferView[vertexDeclarationCount];
        for (int i = 0; i < vertexDeclarationCount; i++)
        {
            int vertexBufferId = br.ReadInt32();
            vertexBufferViews[i] = environmentAsset.ReflectVertexBuffer(
                vertexBufferId,
                vertexDeclarations[vertexDeclarationId + i],
                vertexCount,
                br,
                vertexBufferOffsets[vertexBufferId]
            );
        }

        this.VerticesView = new(vertexCount, vertexBufferViews);

        uint indexCount = br.ReadUInt32();
        int indexBufferId = br.ReadInt32();
        this.Indices = environmentAsset.ReflectIndexBuffer(indexBufferId);

        if (version >= 13)
            this.VisibilityFlags = (EnvironmentVisibility)br.ReadByte();

        uint submeshCount = br.ReadUInt32();
        for (int i = 0; i < submeshCount; i++)
            this._submeshes.Add(new(br));

        if (version != 5)
            this.DisableBackfaceCulling = br.ReadBoolean();

        this.BoundingBox = br.ReadBox();
        this.Transform = br.ReadMatrix4x4RowMajor();
        this.EnvironmentQualityFilter = (EnvironmentQuality)br.ReadByte();

        if (version >= 7 && version <= 12)
            this.VisibilityFlags = (EnvironmentVisibility)br.ReadByte();

        if (version >= 14)
            this.LayerTransitionBehavior = (EnvironmentVisibilityTransitionBehavior)br.ReadByte();

        if (version >= 11)
            this.RenderFlags = (EnvironmentAssetMeshRenderFlags)br.ReadByte();

        if (useSeparatePointLights && version < 7)
            this.PointLight = br.ReadVector3();

        if (version < 9)
        {
            this._sphericalHarmonics = new Vector3[9];
            for (int i = 0; i < 9; i++)
                this._sphericalHarmonics[i] = br.ReadVector3();

            this.BakedLight = EnvironmentAssetChannel.Read(br);
        }
        else
        {
            this.BakedLight = EnvironmentAssetChannel.Read(br);
            this.StationaryLight = EnvironmentAssetChannel.Read(br);

            if (version >= 12)
                this.BakedPaint = EnvironmentAssetChannel.Read(br);
        }
    }

    internal void Write(BinaryWriter bw, bool useSeparatePointLights, uint version)
    {
        if (version <= 11)
        {
            bw.Write(this.Name.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Name));
        }

        bw.Write(this.VerticesView.VertexCount);
        bw.Write(this._vertexBufferIds.Length);
        bw.Write(this._baseVertexBufferDescriptionId);

        foreach (int vertexBufferId in this._vertexBufferIds)
            bw.Write(vertexBufferId);

        bw.Write(this.Indices.Count);
        bw.Write(this._indexBufferId);

        if (version >= 13)
            bw.Write((byte)this.VisibilityFlags);

        bw.Write(this._submeshes.Count);
        foreach (EnvironmentAssetMeshPrimitive submesh in this._submeshes)
            submesh.Write(bw);

        if (version != 5)
            bw.Write(this.DisableBackfaceCulling);

        bw.WriteBox(this.BoundingBox);
        bw.WriteMatrix4x4RowMajor(this.Transform);
        bw.Write((byte)this.EnvironmentQualityFilter);

        if (version >= 7 && version <= 12)
            bw.Write((byte)this.VisibilityFlags);

        if (version >= 14)
            bw.Write((byte)this.LayerTransitionBehavior);

        if (version >= 11)
            bw.Write((byte)this.RenderFlags);

        if (version < 9)
        {
            if (useSeparatePointLights)
                bw.WriteVector3(this.PointLight ?? Vector3.Zero);

            foreach (Vector3 sphericalHarmonic in this.SphericalHarmonics)
                bw.WriteVector3(sphericalHarmonic);

            for (int i = 0; i < 9 - this.SphericalHarmonics.Count; i++)
                bw.WriteVector3(Vector3.Zero);

            this.BakedLight.Write(bw);
        }
        else
        {
            this.BakedLight.Write(bw);
            this.StationaryLight.Write(bw);

            if (version >= 12)
                this.BakedPaint.Write(bw);
        }
    }

    /// <summary>
    /// Creates a name for a <see cref="EnvironmentAssetMesh"/> with the specified <paramref name="id"/>
    /// </summary>
    /// <param name="id">The ID of the <see cref="EnvironmentAssetMesh"/></param>
    /// <returns>The created name</returns>
    public static string CreateName(int id)
    {
        // League assigns this name to the meshes automatically during reading
        return $"MapGeo_Instance_{id}";
    }
}

/// <summary>
/// Used for limiting the visibility of an environment mesh for specific environment quality settings
/// </summary>
[Flags]
public enum EnvironmentQuality : byte
{
    VeryLow = 1 << 0,
    Low = 1 << 1,
    Medium = 1 << 2,
    High = 1 << 3,
    VeryHigh = 1 << 4,

    /// <summary>
    /// Toggles visibility for all qualities
    /// </summary>
    AllQualities = VeryLow | Low | Medium | High | VeryHigh
}

/// <summary>
/// General render flags
/// </summary>
[Flags]
public enum EnvironmentAssetMeshRenderFlags : byte
{
    Default = 0,

    /// <summary>
    /// The renderer will treat the <see cref="EnvironmentAssetMesh"/> as a decal
    /// </summary>
    Decal = 1 << 0,

    /// <summary>
    /// ⚠️ SPECULATIVE ⚠️<br></br>
    /// Tells the renderer to render distortion effects of a mesh's material into a separate buffer
    /// </summary>
    UseEnvironmentDistortionEffectBuffer = 1 << 1,

    /// <summary>
    /// Mesh will be rendered only if "Hide Eye Candy" option is unchecked
    /// </summary>
    /// <remarks>
    /// If no eye candy flag is set, the mesh will always be rendered
    /// </remarks>
    RenderOnlyIfEyeCandyOn = 1 << 2, // (meshTypeFlags & 4) == 0 || envSettingsFlags)

    /// <summary>
    /// Mesh will be rendered only if "Hide Eye Candy" option is checked
    /// </summary>
    /// <remarks>
    /// If no eye candy flag is set, the mesh will always be rendered
    /// </remarks>
    RenderOnlyIfEyeCandyOff = 1 << 3 // ((meshTypeFlags & 8) == 0 || envSettingsFlags != 1)
}

public enum EnvironmentVisibilityTransitionBehavior
{
    /// <summary>
    /// Default - Only if mesh layer mask matches both CURRENT and NEW layer masks
    /// </summary>
    Unaffected = 0,

    /// <summary>
    /// Only if unfilteredMeshNewLayerMaskMatch == 0
    /// </summary>
    TurnInvisibleDoesNotMatchNewLayerFilter = 1,

    /// <summary>
    /// Only if unfilteredMeshNewLayerMaskMatch != 0
    /// </summary>
    TurnVisibleDoesMatchNewLayerFilter = 2
}
