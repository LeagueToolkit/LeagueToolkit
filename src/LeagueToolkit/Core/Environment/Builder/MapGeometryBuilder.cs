using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Core.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Environment.Builder
{
    /// <summary>Exposes an API for building a <see cref="EnvironmentAsset"/> object</summary>
    public sealed class MapGeometryBuilder
    {
        private EnvironmentAssetBakedTerrainSamplers _bakedTerrainSamplers;
        private readonly List<MapGeometryModelBuilder> _meshes = new();
        private BucketedGeometry _sceneGraph;
        private readonly List<PlanarReflector> _planarReflectors = new();

        private readonly List<VertexBuffer> _vertexBuffers = new();
        private readonly List<IndexBuffer> _indexBuffers = new();

        /// <summary>Creates a new <see cref="MapGeometryBuilder"/> object</summary>
        public MapGeometryBuilder() { }

        /// <summary>Builds a new <see cref="EnvironmentAsset"/> object from this <see cref="MapGeometryBuilder"/></summary>
        /// <returns>The built <see cref="EnvironmentAsset"/> object</returns>
        /// <remarks>
        /// Each <see cref="MapGeometryBuilder"/> instance should only be built from once,
        /// building multiple <see cref="EnvironmentAsset"/> objects from a single <see cref="MapGeometryBuilder"/> instance is undefined behavior
        /// </remarks>
        public EnvironmentAsset Build()
        {
            return new(
                this._bakedTerrainSamplers,
                this._meshes.Select((mesh, id) => mesh.Build(id)),
                this._sceneGraph,
                this._planarReflectors,
                this._vertexBuffers,
                this._indexBuffers
            );
        }

        /// <summary>
        /// Sets the specified <see cref="EnvironmentAssetBakedTerrainSamplers"/> to be used by the environment asset
        /// </summary>
        /// <param name="bakedTerrainSamplers">The <see cref="EnvironmentAssetBakedTerrainSamplers"/> to use</param>
        public MapGeometryBuilder WithBakedTerrainSamplers(EnvironmentAssetBakedTerrainSamplers bakedTerrainSamplers)
        {
            this._bakedTerrainSamplers = bakedTerrainSamplers;
            return this;
        }

        /// <summary>
        /// Adds the specified <see cref="MapGeometryModelBuilder"/> into the environment asset
        /// </summary>
        /// <param name="mesh">The <see cref="MapGeometryModelBuilder"/> to add</param>
        public MapGeometryBuilder WithMesh(MapGeometryModelBuilder mesh)
        {
            Guard.IsNotNull(mesh, nameof(mesh));

            this._meshes.Add(mesh);
            return this;
        }

        /// <summary>
        /// Sets the specified <see cref="BucketedGeometry"/> scene graph to be used by the environment asset
        /// </summary>
        /// <param name="sceneGraph">The <see cref="BucketedGeometry"/> scene graph to add</param>
        public MapGeometryBuilder WithSceneGraph(BucketedGeometry sceneGraph)
        {
            Guard.IsNotNull(sceneGraph, nameof(sceneGraph));

            this._sceneGraph = sceneGraph;
            return this;
        }

        /// <summary>
        /// Adds the specified <see cref="PlanarReflector"/> into the environment asset
        /// </summary>
        /// <param name="planarReflector">The <see cref="PlanarReflector"/> to add</param>
        public MapGeometryBuilder WithPlanarReflector(PlanarReflector planarReflector)
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
}
