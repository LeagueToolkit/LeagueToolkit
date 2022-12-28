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

        private MapGeometryModelBuilderRange[] _submeshes;

        // TODO:
        // Figure out a way to expose a writing interface to the user
        // without leaking writable references to Memory<T>
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
                this._submeshes.Select(submesh => submesh.Build()),
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

        private IEnumerable<MapGeometryModelBuilderRange> SanitizeMeshRanges(
            IEnumerable<MapGeometryModelBuilderRange> ranges,
            MemoryOwner<ushort> indexBuffer,
            VertexBuffer vertexBuffer
        )
        {
            // Get the index min/max for each submesh
            foreach (MapGeometryModelBuilderRange range in ranges)
            {
                // Index range must be within bounds
                if (range.StartIndex + range.IndexCount > indexBuffer.Length)
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Submesh: {range.Material} index range goes out of bounds ({nameof(indexBuffer.Length)}: {indexBuffer.Length})."
                    );

                ReadOnlySpan<ushort> rangeIndices = indexBuffer.Span.Slice(range.StartIndex, range.IndexCount);

                ushort minVertex = rangeIndices.Min();
                ushort maxVertex = rangeIndices.Max();

                // Vertex interval must be within range
                if (minVertex + 1 > vertexBuffer.VertexCount || maxVertex - 1 > vertexBuffer.VertexCount)
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Submesh: {range.Material} vertex interval: [{minVertex}, {maxVertex}] goes out of bounds"
                            + $" ({nameof(vertexBuffer.VertexCount)}: {vertexBuffer.VertexCount})."
                    );

                yield return new(range.Material, range.StartIndex, range.IndexCount, minVertex, maxVertex);
            }
        }

        public MapGeometryModelBuilder UseGeometry(
            IEnumerable<MapGeometryModelBuilderRange> ranges,
            IEnumerable<VertexElement> vertexElements,
            int vertexCount,
            Action<MemoryBufferWriter<ushort>, VertexBufferWriter> writeGeometryCallback
        )
        {
            Guard.IsNotNull(ranges, nameof(ranges));

            // Create index buffer
            int indexCount = ranges.Sum(submesh => submesh.IndexCount);
            this._indexBuffer = MemoryOwner<ushort>.Allocate(indexCount);

            // Create vertex buffer
            MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(vertexElements, vertexCount);
            this._vertexBuffer = VertexBuffer.Create(VertexBufferUsage.Static, vertexElements, vertexBufferOwner);

            writeGeometryCallback.Invoke(new(this._indexBuffer.Memory), new(vertexElements, vertexBufferOwner.Memory));

            // TODO:
            // If we had a separate callback for writing indices, we would be able to
            // figure out vertexCount from max index of said indices
            this._submeshes = SanitizeMeshRanges(ranges, this._indexBuffer, this._vertexBuffer).ToArray();
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

    public readonly struct MapGeometryModelBuilderRange
    {
        public string Material { get; }

        public int StartIndex { get; }
        public int IndexCount { get; }

        public int MinVertex { get; }
        public int MaxVertex { get; }

        public MapGeometryModelBuilderRange(string material, int startIndex, int indexCount)
        {
            this.Material = string.IsNullOrEmpty(material) ? MapGeometrySubmesh.MISSING_MATERIAL : material;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
        }

        internal MapGeometryModelBuilderRange(
            string material,
            int startIndex,
            int indexCount,
            int minVertex,
            int maxVertex
        ) : this(material, startIndex, indexCount)
        {
            this.MinVertex = minVertex;
            this.MaxVertex = maxVertex;
        }

        internal MapGeometrySubmesh Build()
        {
            return new(this.Material, this.StartIndex, this.IndexCount, this.MinVertex, this.MaxVertex);
        }
    }
}
