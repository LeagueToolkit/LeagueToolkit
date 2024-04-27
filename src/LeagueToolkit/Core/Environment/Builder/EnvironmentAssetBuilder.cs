using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Core.SceneGraph;

namespace LeagueToolkit.Core.Environment.Builder;

/// <summary>Exposes an API for building a <see cref="EnvironmentAsset"/> object</summary>
public sealed class EnvironmentAssetBuilder
{
    private readonly List<EnvironmentAssetShaderTextureOverride> _samplerDefs = new();
    private readonly List<EnvironmentAssetMeshBuilder> _meshes = new();
    private readonly List<BucketedGeometry> _sceneGraphs;
    private readonly List<PlanarReflector> _planarReflectors = new();

    private readonly List<VertexBuffer> _vertexBuffers = new();
    private readonly List<IndexBuffer> _indexBuffers = new();

    /// <summary>Creates a new <see cref="EnvironmentAssetBuilder"/> object</summary>
    public EnvironmentAssetBuilder() { }

    /// <summary>Builds a new <see cref="EnvironmentAsset"/> object from this <see cref="EnvironmentAssetBuilder"/></summary>
    /// <returns>The built <see cref="EnvironmentAsset"/> object</returns>
    /// <remarks>
    /// Each <see cref="EnvironmentAssetBuilder"/> instance should only be built from once,
    /// building multiple <see cref="EnvironmentAsset"/> objects from a single <see cref="EnvironmentAssetBuilder"/> instance is undefined behavior
    /// </remarks>
    public EnvironmentAsset Build()
    {
        return new(
            this._samplerDefs,
            this._meshes.Select((mesh, id) => mesh.Build(id)),
            this._sceneGraphs,
            this._planarReflectors,
            this._vertexBuffers,
            this._indexBuffers
        );
    }

    /// <summary>
    /// Adds the specified <see cref="EnvironmentAssetShaderTextureOverride"/> into the environment asset
    /// </summary>
    /// <param name="samplerDef">The <see cref="EnvironmentAssetShaderTextureOverride"/> to add</param>
    public EnvironmentAssetBuilder WithSamplerDef(EnvironmentAssetShaderTextureOverride samplerDef)
    {
        this._samplerDefs.Add(samplerDef);
        return this;
    }

    /// <summary>
    /// Adds the specified <see cref="EnvironmentAssetMeshBuilder"/> into the environment asset
    /// </summary>
    /// <param name="mesh">The <see cref="EnvironmentAssetMeshBuilder"/> to add</param>
    public EnvironmentAssetBuilder WithMesh(EnvironmentAssetMeshBuilder mesh)
    {
        Guard.IsNotNull(mesh, nameof(mesh));

        this._meshes.Add(mesh);
        return this;
    }

    /// <summary>
    /// Adds the specified <see cref="BucketedGeometry"/> scene graph to be used by the environment asset
    /// </summary>
    /// <param name="sceneGraph">The <see cref="BucketedGeometry"/> scene graph to add</param>
    public EnvironmentAssetBuilder WithSceneGraph(BucketedGeometry sceneGraph)
    {
        Guard.IsNotNull(sceneGraph, nameof(sceneGraph));

        this._sceneGraphs.Add(sceneGraph);
        return this;
    }

    /// <summary>
    /// Adds the specified <see cref="PlanarReflector"/> into the environment asset
    /// </summary>
    /// <param name="planarReflector">The <see cref="PlanarReflector"/> to add</param>
    public EnvironmentAssetBuilder WithPlanarReflector(PlanarReflector planarReflector)
    {
        this._planarReflectors.Add(planarReflector);
        return this;
    }

    /// <summary>Creates a new vertex buffer and adds it to the environment asset</summary>
    /// <param name="usage">The usage of the created buffer</param>
    /// <param name="vertexElements">The vertex elements of the created buffer</param>
    /// <param name="vertexCount">The vertex count of the created buffer</param>
    /// <returns>A <see cref="VertexBufferWriter"/> for the created vertex buffer and a view into it</returns>
    /// <remarks>
    /// ⚠️ It is recommended to order <paramref name="vertexElements"/> by their <see cref="ElementName"/> in ascending order<br></br>
    /// ⚠️ You should not use the returned writer interface after building the <see cref="EnvironmentAsset"/>,
    /// doing so is considered undefined behavior
    /// </remarks>
    public (IVertexBufferView view, VertexBufferWriter writer) UseVertexBuffer(
        VertexBufferUsage usage,
        IEnumerable<VertexElement> vertexElements,
        int vertexCount
    )
    {
        Guard.IsNotNull(vertexElements, nameof(vertexElements));
        Guard.IsGreaterThan(vertexCount, 0, nameof(vertexCount));

        MemoryOwner<byte> bufferOwner = VertexBuffer.AllocateForElements(vertexElements, vertexCount);
        VertexBuffer buffer = VertexBuffer.Create(usage, vertexElements, bufferOwner);

        this._vertexBuffers.Add(buffer);

        // TODO: This should probably return a ref struct for the writer
        return (buffer, new(vertexElements, bufferOwner.Memory));
    }

    /// <summary>Creates a new index buffer and adds it to the environment asset</summary>
    /// <param name="indexCount">The index count of the created buffer</param>
    /// <returns>A <see cref="MemoryBufferWriter{T}"/> for the created index buffer and a read-only view into it</returns>
    /// <remarks>
    /// ⚠️ You should not use the returned writer interface after building the <see cref="EnvironmentAsset"/>,
    /// doing so is considered undefined behavior
    /// </remarks>
    public (IndexArray view, MemoryBufferWriter<ushort> writer) UseIndexBuffer(int indexCount)
    {
        Guard.IsGreaterThan(indexCount, 0, nameof(indexCount));
        if (indexCount % 3 != 0)
            ThrowHelper.ThrowArgumentException(nameof(indexCount), $"{nameof(indexCount)} must be a multiple of 3");

        int indexSize = IndexBuffer.GetFormatSize(IndexFormat.U16);
        MemoryOwner<byte> bufferOwner = MemoryOwner<byte>.Allocate(indexCount * indexSize);
        IndexBuffer buffer = IndexBuffer.Create(IndexFormat.U16, bufferOwner);

        this._indexBuffers.Add(buffer);

        // TODO: This should probably return a ref struct for the writer
        return (buffer.AsArray(), new(bufferOwner.Memory.Cast<byte, ushort>()));
    }
}
