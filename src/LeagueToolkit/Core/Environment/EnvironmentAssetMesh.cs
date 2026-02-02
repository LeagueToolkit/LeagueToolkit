using System.Numerics;
using System.Text;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;

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
    private readonly List<EnvironmentAssetMeshPrimitive> _submeshes = [];

    /// <summary>
    /// Unknown Version 18 Int
    /// </summary>
    public uint UnknownVersion18Int { get; set; }

    /// <summary>
    /// Gets the path hash of the scene graph that this mesh belongs to
    /// </summary>
    public uint VisibilityControllerPathHash { get; private set; }

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

    public IReadOnlyList<EnvironmentAssetMeshTextureOverride> TextureOverrides => _textureOverrides;
    private readonly List<EnvironmentAssetMeshTextureOverride> _textureOverrides = [];

    public Vector2 BakedPaintScale { get; private set; }
    public Vector2 BakedPaintBias { get; private set; }

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
        uint visibilityControllerPathHash,
        IEnumerable<EnvironmentAssetMeshPrimitive> submeshes,
        Matrix4x4 transform,
        bool disableBackfaceCulling,
        EnvironmentQuality environmentQualityFilter,
        EnvironmentVisibility visibilityFlags,
        EnvironmentAssetMeshRenderFlags renderFlags,
        EnvironmentAssetChannel stationaryLight,
        EnvironmentAssetChannel bakedLight,
        IEnumerable<EnvironmentAssetMeshTextureOverride> bakedPaintChannelDefs,
        Vector2 bakedPaintScale,
        Vector2 bakedPaintBias
    )
    {
        this.Name = CreateName(id);

        this.VerticesView = new(vertexBufferView.VertexCount, new[] { vertexBufferView });
        this.Indices = indexBufferView;
        this.VisibilityControllerPathHash = visibilityControllerPathHash;
        this._submeshes = new(submeshes);

        this.Transform = transform;

        this.DisableBackfaceCulling = disableBackfaceCulling;
        this.EnvironmentQualityFilter = environmentQualityFilter;
        this.VisibilityFlags = visibilityFlags;
        this.RenderFlags = renderFlags;

        this.StationaryLight = stationaryLight;
        this.BakedLight = bakedLight;
        this._textureOverrides = new(bakedPaintChannelDefs);
        this.BakedPaintScale = bakedPaintScale;
        this.BakedPaintBias = bakedPaintBias;

        this.BoundingBox = Box.FromVertices(vertexBufferView.GetAccessor(ElementName.Position).AsVector3Array());
    }

    internal EnvironmentAssetMesh(
        int id,
        EnvironmentAsset environmentAsset,
        BinaryReader br,
        IReadOnlyList<VertexBufferDescription> vertexDeclarations,
        IReadOnlyList<long> vertexBufferOffsets,
        bool useSeparatePointLights,
        int version
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
            vertexBufferViews[i] = environmentAsset.ProvideVertexBuffer(
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
        this.Indices = environmentAsset.ProvideIndexBuffer(indexBufferId);

        if (version >= 13)
            this.VisibilityFlags = (EnvironmentVisibility)br.ReadByte();

        if (version >= 18)
        {
            this.UnknownVersion18Int = br.ReadUInt32();
        }

        if (version >= 15)
        {
            this.VisibilityControllerPathHash = br.ReadUInt32();
        }

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

        if (version >= 11 && version < 14)
        {
            this.RenderFlags = (EnvironmentAssetMeshRenderFlags)br.ReadByte();
            this.LayerTransitionBehavior = this.RenderFlags.HasFlag(EnvironmentAssetMeshRenderFlags.IsDecal)
                ? EnvironmentVisibilityTransitionBehavior.TurnVisibleDoesMatchNewLayerFilter
                : EnvironmentVisibilityTransitionBehavior.Unaffected;
        }
        else if (version >= 14)
        {
            this.LayerTransitionBehavior = (EnvironmentVisibilityTransitionBehavior)br.ReadByte();
            this.RenderFlags =
                version < 16
                    ? (EnvironmentAssetMeshRenderFlags)br.ReadByte()
                    : (EnvironmentAssetMeshRenderFlags)br.ReadUInt16();
        }

        if (useSeparatePointLights && version < 7)
            this.PointLight = br.ReadVector3();

        if (version < 9)
        {
            this._sphericalHarmonics = new Vector3[9];
            for (int i = 0; i < 9; i++)
                this._sphericalHarmonics[i] = br.ReadVector3();

            this.BakedLight = EnvironmentAssetChannel.Read(br);
            return;
        }

        this.BakedLight = EnvironmentAssetChannel.Read(br);
        this.StationaryLight = EnvironmentAssetChannel.Read(br);

        if (version >= 12 && version < 17)
        {
            var bakedPaint = EnvironmentAssetChannel.Read(br);
            this._textureOverrides.Add(new(0, bakedPaint.Texture));
        }

        if (version >= 17)
        {
            var textureOverrideCount = br.ReadInt32();
            for (int i = 0; i < textureOverrideCount; i++)
            {
                this._textureOverrides.Add(EnvironmentAssetMeshTextureOverride.Read(br));
            }

            this.BakedPaintScale = br.ReadVector2();
            this.BakedPaintBias = br.ReadVector2();
        }
    }

    internal void Write(BinaryWriter bw)
    {
        bw.Write(this.VerticesView.VertexCount);
        bw.Write(this._vertexBufferIds.Length);
        bw.Write(this._baseVertexBufferDescriptionId);
        foreach (var vertexBufferId in this._vertexBufferIds)
            bw.Write(vertexBufferId);

        bw.Write(this.Indices.Count);
        bw.Write(this._indexBufferId);

        bw.Write((byte)this.VisibilityFlags);
        bw.Write(this.VisibilityControllerPathHash);

        bw.Write(this._submeshes.Count);
        foreach (var submesh in this._submeshes)
            submesh.Write(bw);

        bw.Write(this.DisableBackfaceCulling);
        bw.WriteBox(this.BoundingBox);
        bw.WriteMatrix4x4RowMajor(this.Transform);

        bw.Write((byte)this.EnvironmentQualityFilter);
        bw.Write((byte)this.LayerTransitionBehavior);
        bw.Write((ushort)this.RenderFlags);

        this.BakedLight.Write(bw);
        this.StationaryLight.Write(bw);

        bw.Write(this._textureOverrides.Count);
        foreach (var bakedPaintChannelDef in this._textureOverrides)
            bakedPaintChannelDef.Write(bw);
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
public enum EnvironmentAssetMeshRenderFlags : ushort
{
    Default = 0,

    /// <summary>
    /// The renderer will treat the <see cref="EnvironmentAssetMesh"/> as a decal
    /// </summary>
    IsDecal = 1,

    /// <summary>
    /// Tells the renderer to render distortion effects of a mesh's material into a separate buffer
    /// </summary>
    HasEnvironmentDistortion = 2,

    /// <summary>
    /// Mesh will be rendered only if "Hide Eye Candy" option is unchecked
    /// </summary>
    /// <remarks>
    /// If no eye candy flag is set, the mesh will always be rendered
    /// </remarks>
    RenderOnlyIfEyeCandyOn = 4,

    /// <summary>
    /// Mesh will be rendered only if "Hide Eye Candy" option is checked
    /// </summary>
    /// <remarks>
    /// If no eye candy flag is set, the mesh will always be rendered
    /// </remarks>
    RenderOnlyIfEyeCandyOff = 8,

    CreateShadowBuffer = 16,
    CreateShadowMapMaterial = 32,
    UnkCreateDepthBuffer2 = 64,
    CreateDepthBuffer = 128,

    CreateShadowTransitionBuffer =
        CreateShadowBuffer | CreateShadowMapMaterial | UnkCreateDepthBuffer2 | CreateDepthBuffer
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
