using System.Buffers;
using System.Numerics;
using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Memory;

namespace LeagueToolkit.Core.Environment.Builder;

/// <summary>
/// Provides an interface for building a <see cref="EnvironmentAssetMesh"/>
/// </summary>
public sealed class EnvironmentAssetMeshBuilder
{
    private uint _visibilityControllerPathHash;

    private Matrix4x4 _transform;

    private bool _disableBackfaceCulling;
    private EnvironmentQuality _environmentQualityMask;
    private EnvironmentVisibility _visibilityFlags;
    private EnvironmentAssetMeshRenderFlags _renderFlags;

    private EnvironmentAssetChannel _stationaryLight;
    private EnvironmentAssetChannel _bakedLight;
    private EnvironmentAssetMeshTextureOverride[] _textureOverrides = [];
    private Vector2 _bakedPaintScale;
    private Vector2 _bakedPaintBias;

    private EnvironmentAssetMeshPrimitive[] _ranges;

    // TODO:
    // Figure out a way to expose a writing interface to the user
    // without leaking writable references to Memory<T>
    // Solution 1:
    // Create a SpanBufferWriter<T> ref struct so the caller cannot store a reference
    // Solution 2:
    // Leave as is but warn the caller that it's their responsibility to not store a reference
    // Not complying can cause undefined behavior
    private IVertexBufferView _vertexBuffer;
    private IndexArray _indexBuffer;

    /// <summary>Creates a new <see cref="EnvironmentAssetBuilder"/> instance</summary>
    public EnvironmentAssetMeshBuilder() { }

    internal EnvironmentAssetMesh Build(int meshId)
    {
        // TODO: This should be simplified
        return new(
            meshId,
            this._vertexBuffer,
            this._indexBuffer,
            this._visibilityControllerPathHash,
            this._ranges,
            this._transform,
            this._disableBackfaceCulling,
            this._environmentQualityMask,
            this._visibilityFlags,
            this._renderFlags,
            this._stationaryLight,
            this._bakedLight,
            this._textureOverrides,
            this._bakedPaintScale,
            this._bakedPaintBias
        );
    }

    /// <summary>Sets the specified geometry data for the <see cref="EnvironmentAssetMesh"/></summary>
    /// <param name="primitives">The primitives of the <see cref="EnvironmentAssetMesh"/></param>
    /// <param name="vertexBuffer">The vertex buffer to use for the <see cref="EnvironmentAssetMesh"/></param>
    /// <param name="indexBuffer">The index buffer to use for the <see cref="EnvironmentAssetMesh"/></param>
    public EnvironmentAssetMeshBuilder WithGeometry(
        IEnumerable<MeshPrimitiveBuilder> primitives,
        IVertexBufferView vertexBuffer,
        IndexArray indexBuffer
    )
    {
        Guard.IsNotNull(primitives, nameof(primitives));
        Guard.IsNotNull(vertexBuffer, nameof(vertexBuffer));
        Guard.HasSizeGreaterThan(vertexBuffer.View.Span, 0, nameof(vertexBuffer));
        Guard.HasSizeGreaterThan(indexBuffer, 0, nameof(indexBuffer));

        this._vertexBuffer = vertexBuffer;
        this._indexBuffer = indexBuffer;
        this._ranges = CreateRanges(primitives, this._indexBuffer, this._vertexBuffer.VertexCount).ToArray();

        return this;
    }

    /// <summary>
    /// Sets the visibility controller path hash
    /// </summary>
    /// <param name="visibilityControllerPathHash">The visibility controller path hash to use</param>
    /// <returns></returns>
    public EnvironmentAssetMeshBuilder WithVisibilityController(uint visibilityControllerPathHash)
    {
        this._visibilityControllerPathHash = visibilityControllerPathHash;
        return this;
    }

    /// <summary>Sets the backface culling toggle for the <see cref="EnvironmentAssetMesh"/></summary>
    public EnvironmentAssetMeshBuilder WithDisableBackfaceCulling(bool disableBackfaceCulling)
    {
        this._disableBackfaceCulling = disableBackfaceCulling;
        return this;
    }

    /// <summary>Sets the specified transform for the <see cref="EnvironmentAssetMesh"/></summary>
    public EnvironmentAssetMeshBuilder WithTransform(Matrix4x4 transform)
    {
        this._transform = transform;
        return this;
    }

    /// <summary>Sets the specified environment quality filter for the <see cref="EnvironmentAssetMesh"/></summary>
    public EnvironmentAssetMeshBuilder WithEnvironmentQualityFilter(EnvironmentQuality environmentQualityFilter)
    {
        this._environmentQualityMask = environmentQualityFilter;
        return this;
    }

    /// <summary>Sets the specified visibility flags for the <see cref="EnvironmentAssetMesh"/></summary>
    public EnvironmentAssetMeshBuilder WithVisibilityFlags(EnvironmentVisibility visibilityFlags)
    {
        this._visibilityFlags = visibilityFlags;
        return this;
    }

    /// <summary>Sets the specified render flags for the <see cref="EnvironmentAssetMesh"/></summary>
    public EnvironmentAssetMeshBuilder WithRenderFlags(EnvironmentAssetMeshRenderFlags renderFlags)
    {
        this._renderFlags = renderFlags;
        return this;
    }

    /// <summary>Sets the specified Stationary Light sampler for the <see cref="EnvironmentAssetMesh"/></summary>
    public EnvironmentAssetMeshBuilder WithStationaryLightSampler(EnvironmentAssetChannel stationaryLight)
    {
        this._stationaryLight = stationaryLight;
        return this;
    }

    /// <summary>Sets the specified Baked Light sampler for the <see cref="EnvironmentAssetMesh"/></summary>
    public EnvironmentAssetMeshBuilder WithBakedLightSampler(EnvironmentAssetChannel bakedLight)
    {
        this._bakedLight = bakedLight;
        return this;
    }

    /// <summary>Sets the specified texture overrides for the <see cref="EnvironmentAssetMesh"/></summary>
    public EnvironmentAssetMeshBuilder WithTextureOverrides(
        IEnumerable<EnvironmentAssetMeshTextureOverride> textureOverrides,
        Vector2 scale,
        Vector2 bias
    )
    {
        this._textureOverrides = textureOverrides.ToArray();
        this._bakedPaintScale = scale;
        this._bakedPaintBias = bias;
        return this;
    }

    private static IEnumerable<EnvironmentAssetMeshPrimitive> CreateRanges(
        IEnumerable<MeshPrimitiveBuilder> primitives,
        IndexArray indexBuffer,
        int vertexCount
    )
    {
        // Get the index min/max for each range
        foreach (MeshPrimitiveBuilder primitive in primitives)
        {
            // Index range must be within bounds
            if (primitive.StartIndex + primitive.IndexCount > indexBuffer.Count)
                ThrowHelper.ThrowInvalidOperationException(
                    $"Primitive index range goes out of bounds ({nameof(indexBuffer.Count)}: {indexBuffer.Count})."
                );

            IndexArray rangeIndices = indexBuffer.Slice(primitive.StartIndex, primitive.IndexCount);

            uint minVertex = rangeIndices.Min();
            uint maxVertex = rangeIndices.Max();

            // Vertex interval must be within range
            if (minVertex + 1 > vertexCount || maxVertex - 1 > vertexCount)
                ThrowHelper.ThrowInvalidOperationException(
                    $"Primitive vertex range interval: [{minVertex}, {maxVertex}] goes out of bounds"
                        + $" ({nameof(vertexCount)}: {vertexCount})."
                );

            yield return new(
                primitive.Material,
                primitive.StartIndex,
                primitive.IndexCount,
                (int)minVertex,
                (int)maxVertex
            );
        }
    }
}

public readonly struct MeshPrimitiveBuilder
{
    public string Material { get; }

    public int StartIndex { get; }
    public int IndexCount { get; }

    public MeshPrimitiveBuilder(string material, int startIndex, int indexCount)
    {
        this.Material = material;
        this.StartIndex = startIndex;
        this.IndexCount = indexCount;
    }
}
