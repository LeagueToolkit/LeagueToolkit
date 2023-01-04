using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometryFile.Builder
{
    /// <summary>
    /// Provides an interface for building a <see cref="MapGeometryModel"/>
    /// </summary>
    public sealed class MapGeometryModelBuilder
    {
        private Matrix4x4 _transform;

        private bool _disableBackfaceCulling;
        private MapGeometryEnvironmentQualityFilter _environmentQualityMask;
        private MapGeometryVisibilityFlags _visibilityFlags;
        private MapGeometryMeshRenderFlags _renderFlags;

        private MapGeometrySamplerData _stationaryLight;
        private MapGeometrySamplerData _bakedLight;
        private MapGeometrySamplerData _bakedPaint;

        private MapGeometrySubmesh[] _ranges;

        // TODO:
        // Figure out a way to expose a writing interface to the user
        // without leaking writable references to Memory<T>
        // Solution 1:
        // Create a SpanBufferWriter<T> ref struct so the caller cannot store a reference
        // Solution 2:
        // Leave as is but warn the caller that it's their responsibility to not store a reference
        // Not complying can cause undefined behavior
        private IVertexBufferView _vertexBuffer;
        private ReadOnlyMemory<ushort> _indexBuffer;

        /// <summary>Creates a new <see cref="MapGeometryBuilder"/> instance</summary>
        public MapGeometryModelBuilder() { }

        internal MapGeometryModel Build(int meshId)
        {
            // TODO: This should be simplified
            return new(
                meshId,
                this._vertexBuffer,
                this._indexBuffer,
                this._ranges,
                this._transform,
                this._disableBackfaceCulling,
                this._environmentQualityMask,
                this._visibilityFlags,
                this._renderFlags,
                this._stationaryLight,
                this._bakedLight,
                this._bakedPaint
            );
        }

        /// <summary>Sets the specified geometry data for the <see cref="MapGeometryModel"/></summary>
        /// <param name="primitives">The primitives of the <see cref="MapGeometryModel"/></param>
        /// <param name="vertexBuffer">The vertex buffer to use for the <see cref="MapGeometryModel"/></param>
        /// <param name="indexBuffer">The index buffer to use for the <see cref="MapGeometryModel"/></param>
        public MapGeometryModelBuilder WithGeometry(
            IEnumerable<MeshPrimitiveBuilder> primitives,
            IVertexBufferView vertexBuffer,
            ReadOnlyMemory<ushort> indexBuffer
        )
        {
            ArgumentNullException.ThrowIfNull(primitives, nameof(primitives));
            ArgumentNullException.ThrowIfNull(vertexBuffer, nameof(vertexBuffer));
            Guard.HasSizeGreaterThan(vertexBuffer.View.Span, 0, nameof(vertexBuffer));
            Guard.HasSizeGreaterThan(indexBuffer.Span, 0, nameof(indexBuffer));

            this._vertexBuffer = vertexBuffer;
            this._indexBuffer = indexBuffer;
            this._ranges = CreateRanges(primitives, this._indexBuffer, this._vertexBuffer.VertexCount).ToArray();

            return this;
        }

        /// <summary>Sets the backface culling toggle for the <see cref="MapGeometryModel"/></summary>
        public MapGeometryModelBuilder WithDisableBackfaceCulling(bool disableBackfaceCulling)
        {
            this._disableBackfaceCulling = disableBackfaceCulling;
            return this;
        }

        /// <summary>Sets the specified transform for the <see cref="MapGeometryModel"/></summary>
        public MapGeometryModelBuilder WithTransform(Matrix4x4 transform)
        {
            this._transform = transform;
            return this;
        }

        /// <summary>Sets the specified environment quality filter for the <see cref="MapGeometryModel"/></summary>
        public MapGeometryModelBuilder WithEnvironmentQualityFilter(
            MapGeometryEnvironmentQualityFilter environmentQualityFilter
        )
        {
            this._environmentQualityMask = environmentQualityFilter;
            return this;
        }

        /// <summary>Sets the specified visibility flags for the <see cref="MapGeometryModel"/></summary>
        public MapGeometryModelBuilder WithVisibilityFlags(MapGeometryVisibilityFlags visibilityFlags)
        {
            this._visibilityFlags = visibilityFlags;
            return this;
        }

        /// <summary>Sets the specified render flags for the <see cref="MapGeometryModel"/></summary>
        public MapGeometryModelBuilder WithRenderFlags(MapGeometryMeshRenderFlags renderFlags)
        {
            this._renderFlags = renderFlags;
            return this;
        }

        /// <summary>Sets the specified Stationary Light sampler for the <see cref="MapGeometryModel"/></summary>
        public MapGeometryModelBuilder WithStationaryLightSampler(MapGeometrySamplerData stationaryLight)
        {
            this._stationaryLight = stationaryLight;
            return this;
        }

        /// <summary>Sets the specified Baked Light sampler for the <see cref="MapGeometryModel"/></summary>
        public MapGeometryModelBuilder WithBakedLightSampler(MapGeometrySamplerData bakedLight)
        {
            this._bakedLight = bakedLight;
            return this;
        }

        /// <summary>Sets the specified Baked Paint sampler for the <see cref="MapGeometryModel"/></summary>
        public MapGeometryModelBuilder WithBakedPaintSampler(MapGeometrySamplerData bakedPaint)
        {
            this._bakedPaint = bakedPaint;
            return this;
        }

        private static IEnumerable<MapGeometrySubmesh> CreateRanges(
            IEnumerable<MeshPrimitiveBuilder> primitives,
            ReadOnlyMemory<ushort> indexBuffer,
            int vertexCount
        )
        {
            // Get the index min/max for each range
            foreach (MeshPrimitiveBuilder primitive in primitives)
            {
                // Index range must be within bounds
                if (primitive.StartIndex + primitive.IndexCount > indexBuffer.Length)
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Primitive index range goes out of bounds ({nameof(indexBuffer.Length)}: {indexBuffer.Length})."
                    );

                ReadOnlySpan<ushort> rangeIndices = indexBuffer.Span.Slice(primitive.StartIndex, primitive.IndexCount);

                ushort minVertex = rangeIndices.Min();
                ushort maxVertex = rangeIndices.Max();

                // Vertex interval must be within range
                if (minVertex + 1 > vertexCount || maxVertex - 1 > vertexCount)
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Primitive vertex range interval: [{minVertex}, {maxVertex}] goes out of bounds"
                            + $" ({nameof(vertexCount)}: {vertexCount})."
                    );

                yield return new(primitive.Material, primitive.StartIndex, primitive.IndexCount, minVertex, maxVertex);
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
}
