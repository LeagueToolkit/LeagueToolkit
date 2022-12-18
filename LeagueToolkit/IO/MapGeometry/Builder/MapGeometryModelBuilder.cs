using CommunityToolkit.Diagnostics;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapGeometry.Builder
{
    public sealed class MapGeometryModelBuilder
    {
        private Matrix4x4 _transform;

        private bool _flipNormals;
        private MapGeometryQualityFilter _qualityMask;
        private MapGeometryLayer _layerMask;
        private MapGeometryMeshRenderFlags _renderFlags;

        private MapGeometrySamplerData _stationaryLight;
        private MapGeometrySamplerData _bakedLight;
        private MapGeometrySamplerData _bakedPaint;

        private readonly List<MapGeometryModelBuilderRange> _submeshes = new();

        // TODO: Make a buffer wrapper for these
        private Memory<MapGeometryVertex> _vertices;
        private Memory<ushort> _indices;

        public MapGeometryModelBuilder() { }

        internal MapGeometryModel Build(int meshId)
        {
            SanitizeMeshRanges();
            ValidateVertexElements();

            // TODO: This should be simplified
            return new(
                meshId,
                this._vertices,
                this._indices,
                this._submeshes.Select(submesh => submesh.Build()),
                this._transform,
                this._flipNormals,
                this._qualityMask,
                this._layerMask,
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

                ReadOnlyMemory<ushort> submeshIndices = this._indices.Slice(submesh.StartIndex, submesh.IndexCount);

                ushort minVertex = submeshIndices.Span.Min();
                ushort maxVertex = submeshIndices.Span.Max();

                // Vertex interval must be within range
                if (minVertex + 1 > this._vertices.Length || maxVertex - 1 > this._vertices.Length)
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

        private void ValidateVertexElements()
        {
            ReadOnlySpan<MapGeometryVertex> vertices = this._vertices.Span;

            // All vertices need to have the same elements because we do not support multi-buffer meshes
            VertexElementGroupDescriptionFlags descriptionFlags = vertices[0].GetDescriptionFlags();
            for (int i = 1; i < vertices.Length; i++)
            {
                if (vertices[i].GetDescriptionFlags() != descriptionFlags)
                {
                    ThrowHelper.ThrowInvalidOperationException(
                        "Vertex element description inconsistency."
                            + " Meshes with multiple vertex buffers are not supported."
                    );
                }
            }
        }

        public MapGeometryModelBuilder UseGeometry(Memory<MapGeometryVertex> vertices, Memory<ushort> indices)
        {
            Guard.HasSizeGreaterThan(vertices, 0, nameof(vertices));
            Guard.HasSizeGreaterThan(indices, 0, nameof(indices));

            this._vertices = vertices;
            this._indices = indices;
            return this;
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

        public MapGeometryModelBuilder UseQualityMask(MapGeometryQualityFilter qualityMask)
        {
            this._qualityMask = qualityMask;
            return this;
        }

        public MapGeometryModelBuilder UseLayerMask(MapGeometryLayer layerMask)
        {
            this._layerMask = layerMask;
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
