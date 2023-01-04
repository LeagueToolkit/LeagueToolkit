﻿using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Helpers.Structures.BucketGrid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueToolkit.IO.MapGeometryFile.Builder
{
    public sealed class MapGeometryBuilder
    {
        private MapGeometryBakedTerrainSamplers _bakedTerrainSamplers;
        private readonly List<MapGeometryModelBuilder> _meshes = new();
        private BucketGrid _bucketGrid;
        private readonly List<MapGeometryPlanarReflector> _planarReflectors = new();

        private readonly List<VertexBuffer> _vertexBuffers = new();
        private readonly List<MemoryOwner<ushort>> _indexBuffers = new();

        public MapGeometryBuilder() { }

        public MapGeometry Build()
        {
            return new(
                this._bakedTerrainSamplers,
                this._meshes.Select((mesh, id) => mesh.Build(id)),
                this._bucketGrid,
                this._planarReflectors,
                this._vertexBuffers,
                this._indexBuffers
            );
        }

        public MapGeometryBuilder WithBakedTerrainSamplers(MapGeometryBakedTerrainSamplers bakedTerrainSamplers)
        {
            this._bakedTerrainSamplers = bakedTerrainSamplers;
            return this;
        }

        public MapGeometryBuilder WithMesh(MapGeometryModelBuilder mesh)
        {
            ArgumentNullException.ThrowIfNull(mesh, nameof(mesh));

            this._meshes.Add(mesh);
            return this;
        }

        public MapGeometryBuilder WithBucketGrid(BucketGrid bucketGrid)
        {
            ArgumentNullException.ThrowIfNull(bucketGrid, nameof(bucketGrid));

            this._bucketGrid = bucketGrid;
            return this;
        }

        public MapGeometryBuilder WithPlanarReflector(MapGeometryPlanarReflector planarReflector)
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
        /// ⚠️ You should not use the returned writer interface after building the <see cref="MapGeometry"/>,
        /// doing so is considered undefined behavior
        /// </remarks>
        public (IVertexBufferView view, VertexBufferWriter writer) UseVertexBuffer(
            VertexBufferUsage usage,
            IEnumerable<VertexElement> vertexElements,
            int vertexCount
        )
        {
            ArgumentNullException.ThrowIfNull(vertexElements, nameof(vertexElements));
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
        /// ⚠️ You should not use the returned writer interface after building the <see cref="MapGeometry"/>,
        /// doing so is considered undefined behavior
        /// </remarks>
        public (ReadOnlyMemory<ushort> view, MemoryBufferWriter<ushort> writer) UseIndexBuffer(int indexCount)
        {
            Guard.IsGreaterThan(indexCount, 0, nameof(indexCount));
            if (indexCount % 3 != 0)
                ThrowHelper.ThrowArgumentException(nameof(indexCount), $"{nameof(indexCount)} must be a multiple of 3");

            MemoryOwner<ushort> bufferOwner = MemoryOwner<ushort>.Allocate(indexCount);

            this._indexBuffers.Add(bufferOwner);

            // TODO: This should probably return a ref struct for the writer
            return (bufferOwner.Memory, new(bufferOwner.Memory));
        }
    }
}
