using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapGeometryFile.Builder
{
    public sealed class MapGeometryModelBuilder
    {
        private Matrix4x4 _transform;

        private bool _flipNormals;
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
        private VertexBuffer _vertexBuffer;
        private MemoryOwner<ushort> _indexBuffer;

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
                this._flipNormals,
                this._environmentQualityMask,
                this._visibilityFlags,
                this._renderFlags,
                this._stationaryLight,
                this._bakedLight,
                this._bakedPaint
            );
        }

        private IEnumerable<MapGeometrySubmesh> CreateRanges(
            IEnumerable<MeshPrimitiveBuilder> primitives,
            MemoryOwner<ushort> indexBuffer,
            VertexBuffer vertexBuffer
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
                if (minVertex + 1 > vertexBuffer.VertexCount || maxVertex - 1 > vertexBuffer.VertexCount)
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Primitive vertex range interval: [{minVertex}, {maxVertex}] goes out of bounds"
                            + $" ({nameof(vertexBuffer.VertexCount)}: {vertexBuffer.VertexCount})."
                    );

                yield return new(primitive.Material, primitive.StartIndex, primitive.IndexCount, minVertex, maxVertex);
            }
        }

        public MapGeometryModelBuilder UseGeometry(
            IEnumerable<MeshPrimitiveBuilder> primitives,
            IEnumerable<VertexElement> vertexElements,
            int vertexCount,
            Action<MemoryBufferWriter<ushort>, VertexBufferWriter> writeGeometryCallback
        )
        {
            Guard.IsNotNull(primitives, nameof(primitives));
            Guard.IsNotNull(vertexElements, nameof(vertexElements));
            Guard.IsNotNull(writeGeometryCallback, nameof(writeGeometryCallback));
            Guard.IsGreaterThan(vertexCount, 0, nameof(vertexCount));

            // Create index buffer
            int indexCount = primitives.Sum(primitive => primitive.IndexCount);
            this._indexBuffer = MemoryOwner<ushort>.Allocate(indexCount);

            // Create vertex buffer
            MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(vertexElements, vertexCount);
            this._vertexBuffer = VertexBuffer.Create(VertexBufferUsage.Static, vertexElements, vertexBufferOwner);

            writeGeometryCallback.Invoke(new(this._indexBuffer.Memory), new(vertexElements, vertexBufferOwner.Memory));

            // TODO:
            // If we had a separate callback for writing indices, we would be able to
            // figure out vertexCount from max index of said indices
            this._ranges = CreateRanges(primitives, this._indexBuffer, this._vertexBuffer).ToArray();
            return this;
        }

        public MapGeometryModelBuilder UseFlipNormalsToggle(bool flipNormals)
        {
            this._flipNormals = flipNormals;
            return this;
        }

        public MapGeometryModelBuilder UseTransform(Matrix4x4 transform)
        {
            this._transform = transform;
            return this;
        }

        public MapGeometryModelBuilder UseEnvironmentQualityFilter(
            MapGeometryEnvironmentQualityFilter environmentQualityFilter
        )
        {
            this._environmentQualityMask = environmentQualityFilter;
            return this;
        }

        public MapGeometryModelBuilder UseVisibilityFlags(MapGeometryVisibilityFlags visibilityFlags)
        {
            this._visibilityFlags = visibilityFlags;
            return this;
        }

        public MapGeometryModelBuilder UseRenderFlags(MapGeometryMeshRenderFlags renderFlags)
        {
            this._renderFlags = renderFlags;
            return this;
        }

        public MapGeometryModelBuilder UseStationaryLightSampler(MapGeometrySamplerData stationaryLight)
        {
            this._stationaryLight = stationaryLight;
            return this;
        }

        public MapGeometryModelBuilder UseBakedLightSampler(MapGeometrySamplerData bakedLight)
        {
            this._bakedLight = bakedLight;
            return this;
        }

        public MapGeometryModelBuilder UseBakedPaintSampler(MapGeometrySamplerData bakedPaint)
        {
            this._bakedPaint = bakedPaint;
            return this;
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
