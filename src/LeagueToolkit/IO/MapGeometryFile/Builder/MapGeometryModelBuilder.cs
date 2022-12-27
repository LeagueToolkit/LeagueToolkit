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

        private readonly List<MapGeometryModelBuilderRange> _submeshes = new();

        // TODO: Make a buffer wrapper for these
        private VertexBuffer _vertexBuffer;
        private MemoryOwner<ushort> _indices;

        public MapGeometryModelBuilder() { }

        internal MapGeometryModel Build(int meshId)
        {
            SanitizeMeshRanges();

            // TODO: This should be simplified
            return new(
                meshId,
                this._vertexBuffer,
                this._indices,
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

        private void SanitizeMeshRanges()
        {
            if (this._submeshes.Count == 0)
            {
                ThrowHelper.ThrowInvalidOperationException("Mesh must contain at least 1 submesh.");
            }

            // Get the index min/max for each submesh
            for (int i = 0; i < this._submeshes.Count; i++)
            {
                MapGeometryModelBuilderRange submesh = this._submeshes[i];

                // Index range must be within bounds
                if (submesh.StartIndex + submesh.IndexCount > this._indices.Length)
                {
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Submesh: {submesh.Material} index range goes out of bounds (IndexCount: {this._indices.Length})."
                    );
                }

                ReadOnlySpan<ushort> submeshIndices = this._indices.Span.Slice(submesh.StartIndex, submesh.IndexCount);

                ushort minVertex = submeshIndices.Min();
                ushort maxVertex = submeshIndices.Max();

                // Vertex interval must be within range
                if (minVertex + 1 > this._vertexBuffer.VertexCount || maxVertex - 1 > this._vertexBuffer.VertexCount)
                {
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Submesh: {submesh.Material} vertex interval: [{minVertex}, {maxVertex}] goes out of bounds"
                            + $" (VertexCount: {this._indices.Length})."
                    );
                }

                // Ranges are valid, assign them
                submesh.MinVertex = minVertex;
                submesh.MaxVertex = maxVertex;
                this._submeshes[i] = submesh;
            }
        }

        public void UseGeometry(
            int indexCount,
            int vertexCount,
            IEnumerable<VertexElement> vertexElements,
            Action<MemoryBufferWriter<ushort>, VertexBufferWriter> writeGeometryCallback
        )
        {
            Guard.IsGreaterThan(indexCount, 0, nameof(indexCount));
            Guard.IsGreaterThan(vertexCount, 0, nameof(vertexCount));
            Guard.IsNotNull(writeGeometryCallback, nameof(writeGeometryCallback));

            // Create buffers
            this._indices = MemoryOwner<ushort>.Allocate(indexCount);
            MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(vertexElements, vertexCount);

            // Create buffer writers
            MemoryBufferWriter<ushort> indexBufferWriter = new(this._indices.Memory);
            VertexBufferWriter vertexBufferWriter =
                new(VertexBufferUsage.Static, vertexElements, vertexBufferOwner.Memory);

            // Call the user-defined writing function
            writeGeometryCallback.Invoke(indexBufferWriter, vertexBufferWriter);

            this._vertexBuffer = VertexBuffer.Create(
                vertexBufferWriter.Usage,
                vertexBufferWriter.Elements.Values.Select(descriptor => descriptor.Element),
                vertexBufferOwner
            );

            // Verify that the data has been written
            if (indexBufferWriter.WrittenCount != indexCount)
                ThrowHelper.ThrowInvalidOperationException(
                    $"Only {indexBufferWriter.WrittenCount} indices were written out of the allocated {indexCount}"
                );
        }

        public MapGeometryModelBuilder UseSubmesh(MapGeometryModelBuilderRange submesh)
        {
            Guard.IsNotNullOrEmpty(submesh.Material, nameof(submesh.Material));
            Guard.IsGreaterThanOrEqualTo(submesh.StartIndex, 0, nameof(submesh.StartIndex));
            Guard.IsGreaterThan(submesh.IndexCount, 0, nameof(submesh.IndexCount));

            this._submeshes.Add(submesh);
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

    public struct MapGeometryModelBuilderRange
    {
        public string Material;

        public int StartIndex;
        public int IndexCount;

        // This is temporary hack, will be set by builder
        internal int MinVertex;
        internal int MaxVertex;

        public MapGeometryModelBuilderRange(string material, int startIndex, int indexCount)
        {
            this.Material = string.IsNullOrEmpty(material) ? MapGeometrySubmesh.MISSING_MATERIAL : material;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
        }

        internal MapGeometrySubmesh Build()
        {
            return new(this.Material, this.StartIndex, this.IndexCount, this.MinVertex, this.MaxVertex);
        }
    }
}
