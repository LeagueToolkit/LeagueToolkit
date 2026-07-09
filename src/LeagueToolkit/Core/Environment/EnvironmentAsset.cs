using System.Numerics;
using System.Text;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Core.SceneGraph;
using LeagueToolkit.Utils.Exceptions;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Environment;

/// <summary>Represents an environment asset</summary>
public sealed class EnvironmentAsset : IDisposable
{
    /// <summary>Gets the shader texture overrides used for this environment asset</summary>
    public IReadOnlyList<EnvironmentAssetShaderTextureOverride> ShaderTextureOverrides => this._shaderTextureOverrides;
    private readonly List<EnvironmentAssetShaderTextureOverride> _shaderTextureOverrides = new();

    /// <summary>Gets a read-only list of the meshes used by this environment asset</summary>
    public IReadOnlyList<EnvironmentAssetMesh> Meshes => this._meshes;
    private readonly List<EnvironmentAssetMesh> _meshes = new();

    /// <summary>Gets the <see cref="BucketedGeometry"/> scene graph for the environment asset</summary>
    public IReadOnlyList<BucketedGeometry> SceneGraphs => this._sceneGraphs;
    public readonly List<BucketedGeometry> _sceneGraphs = new();

    /// <summary>Gets a read-only list of the planar reflectors used by the environment asset</summary>
    public IReadOnlyList<PlanarReflector> PlanarReflectors => this._planarReflectors;
    private readonly List<PlanarReflector> _planarReflectors = new();

    private readonly VertexBuffer[] _vertexBuffers;
    private readonly IndexBuffer[] _indexBuffers;

    public bool IsDisposed { get; private set; }

    internal EnvironmentAsset(
        IEnumerable<EnvironmentAssetShaderTextureOverride> shaderTextureOverrides,
        IEnumerable<EnvironmentAssetMesh> meshes,
        IEnumerable<BucketedGeometry> sceneGraphs,
        IEnumerable<PlanarReflector> planarReflectors,
        IEnumerable<VertexBuffer> vertexBuffers,
        IEnumerable<IndexBuffer> indexBuffers
    )
    {
        Guard.IsNotNull(meshes, nameof(meshes));
        Guard.IsNotNull(sceneGraphs, nameof(sceneGraphs));
        Guard.IsNotNull(planarReflectors, nameof(planarReflectors));
        Guard.IsNotNull(vertexBuffers, nameof(vertexBuffers));
        Guard.IsNotNull(indexBuffers, nameof(indexBuffers));

        this._shaderTextureOverrides = new(shaderTextureOverrides);
        this._meshes = new(meshes);
        this._sceneGraphs = new(sceneGraphs);
        this._planarReflectors = new(planarReflectors);

        this._vertexBuffers = vertexBuffers.ToArray();
        this._indexBuffers = indexBuffers.ToArray();
    }

    /// <summary>
    /// Creates a new <see cref="EnvironmentAsset"/> instance by reading it from <paramref name="stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from</param>
    /// <exception cref="InvalidFileSignatureException">The header magic signature is invalid</exception>
    /// <exception cref="InvalidFileVersionException">The version of the <see cref="EnvironmentAsset"/> file is not supported</exception>
    public EnvironmentAsset(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
        if (magic != "OEGM")
            throw new InvalidFileSignatureException();

        var version = br.ReadInt32();
        if (version is not (5 or 6 or 7 or 9 or 11 or 12 or 13 or 14 or 15 or 17 or 18))
            throw new InvalidFileVersionException();

        bool useSeparatePointLights = version < 7 && br.ReadBoolean();

        ReadSamplerDefs(br, version);

        // Read vertex declarations
        uint vertexDeclarationCount = br.ReadUInt32();
        VertexBufferDescription[] vertexDeclarations = new VertexBufferDescription[vertexDeclarationCount];
        for (int i = 0; i < vertexDeclarationCount; i++)
            vertexDeclarations[i] = VertexBufferDescription.ReadFromMapGeometry(br, version);

        // Reading of vertex buffers is deferred until we start reading meshes
        uint vertexBufferCount = br.ReadUInt32();
        this._vertexBuffers = new VertexBuffer[vertexBufferCount];
        long[] vertexBufferOffsets = new long[vertexBufferCount];
        for (int i = 0; i < vertexBufferCount; i++)
        {
            _ = version switch
            {
                >= 13 => (EnvironmentVisibility)br.ReadByte(),
                _ => EnvironmentVisibility.AllLayers
            };
            uint bufferSize = br.ReadUInt32();

            vertexBufferOffsets[i] = br.BaseStream.Position;
            br.BaseStream.Seek(bufferSize, SeekOrigin.Current);
        }

        uint indexBufferCount = br.ReadUInt32();
        this._indexBuffers = new IndexBuffer[indexBufferCount];
        for (int i = 0; i < indexBufferCount; i++)
        {
            _ = version switch
            {
                >= 13 => (EnvironmentVisibility)br.ReadByte(),
                _ => EnvironmentVisibility.AllLayers
            };
            int bufferSize = br.ReadInt32();

            MemoryOwner<byte> indexBufferOwner = MemoryOwner<byte>.Allocate(bufferSize);
            br.BaseStream.ReadExact(indexBufferOwner.Span);

            this._indexBuffers[i] = IndexBuffer.Create(IndexFormat.U16, indexBufferOwner);
        }

        // Read meshes
        uint meshCount = br.ReadUInt32();
        for (int i = 0; i < meshCount; i++)
            this._meshes.Add(
                new(i, this, br, vertexDeclarations, vertexBufferOffsets, useSeparatePointLights, version)
            );

        // Read bucketed geometry
        if (version >= 15)
        {
            int sceneGraphCount = br.ReadInt32();
            for (int i = 0; i < sceneGraphCount; i++)
            {
                this._sceneGraphs.Add(new(br, version));
            }
        }
        else
        {
            this._sceneGraphs = [new(br, version)];
        }

        if (version >= 13)
        {
            // Read reflection planes
            uint planarReflectorCount = br.ReadUInt32();
            for (int i = 0; i < planarReflectorCount; i++)
                this._planarReflectors.Add(PlanarReflector.ReadFromMapGeometry(br));
        }
    }

    internal void ReadSamplerDefs(BinaryReader br, int version)
    {
        if (version >= 17)
        {
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                this._shaderTextureOverrides.Add(EnvironmentAssetShaderTextureOverride.Read(br));

            return;
        }

        if (version >= 9)
            this._shaderTextureOverrides.Add(new(0, br.ReadSizedString()));

        if (version >= 11)
            this._shaderTextureOverrides.Add(new(1, br.ReadSizedString()));
    }

    internal IVertexBufferView ProvideVertexBuffer(
        int id,
        VertexBufferDescription description,
        int vertexCount,
        BinaryReader br,
        long offset
    )
    {
        // A buffer can be reused by multiple meshes
        if (this._vertexBuffers[id] is VertexBuffer existingBuffer)
            return existingBuffer;

        // If it hasn't been read yet:
        long returnPosition = br.BaseStream.Position;
        MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(description.Elements, vertexCount);

        // Seek to the buffer offset, read it and seek back
        br.BaseStream.Seek(offset, SeekOrigin.Begin);
        br.BaseStream.ReadExact(vertexBufferOwner.Span);
        br.BaseStream.Seek(returnPosition, SeekOrigin.Begin);

        // Create the buffer, store it and return a view into it
        VertexBuffer vertexBuffer = VertexBuffer.Create(description.Usage, description.Elements, vertexBufferOwner);
        this._vertexBuffers[id] = vertexBuffer;
        return vertexBuffer;
    }

    internal IndexArray ProvideIndexBuffer(int id) => this._indexBuffers[id].AsArray();

    /// <summary>
    /// Writes this <see cref="EnvironmentAsset"/> instance into <paramref name="stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to write to</param>
    /// <exception cref="ArgumentException"></exception>
    public void Write(Stream stream)
    {
        using BinaryWriter bw = new(stream, Encoding.UTF8, true);

        bw.Write(Encoding.ASCII.GetBytes("OEGM"));
        bw.Write(17);

        WriteSamplers(bw);

        var vertexDeclarations = GenerateVertexBufferDescriptions();
        bw.Write(vertexDeclarations.Count);
        foreach (VertexBufferDescription vertexDeclaration in vertexDeclarations)
            vertexDeclaration.WriteToMapGeometry(bw);

        // 1. Deduplicate Vertex Buffers
        List<VertexBuffer> uniqueVertexBuffers = new();
        int[] vertexBufferMap = new int[this._vertexBuffers.Length];
        for (int i = 0; i < this._vertexBuffers.Length; i++)
        {
            var buffer = this._vertexBuffers[i];
            int uniqueIndex = uniqueVertexBuffers.FindIndex(b => b.View.Span.SequenceEqual(buffer.View.Span));
            if (uniqueIndex == -1)
            {
                uniqueIndex = uniqueVertexBuffers.Count;
                uniqueVertexBuffers.Add(buffer);
            }
            vertexBufferMap[i] = uniqueIndex;
        }

        // 2. Deduplicate Index Buffers
        List<IndexBuffer> uniqueIndexBuffers = new();
        int[] indexBufferMap = new int[this._indexBuffers.Length];
        for (int i = 0; i < this._indexBuffers.Length; i++)
        {
            var buffer = this._indexBuffers[i];
            int uniqueIndex = uniqueIndexBuffers.FindIndex(b => b.Buffer.Span.SequenceEqual(buffer.Buffer.Span));
            if (uniqueIndex == -1)
            {
                uniqueIndex = uniqueIndexBuffers.Count;
                uniqueIndexBuffers.Add(buffer);
            }
            indexBufferMap[i] = uniqueIndex;
        }

        WriteVertexBuffers(bw, uniqueVertexBuffers, vertexBufferMap);
        WriteIndexBuffers(bw, uniqueIndexBuffers, indexBufferMap);

        bw.Write(this._meshes.Count);
        foreach (EnvironmentAssetMesh model in this._meshes)
            model.Write(bw);

        bw.Write(this._sceneGraphs.Count);
        foreach (var sceneGraph in this._sceneGraphs)
            sceneGraph.Write(bw);

        bw.Write(this.PlanarReflectors.Count);
        foreach (PlanarReflector planarReflector in this.PlanarReflectors)
            planarReflector.WriteToMapGeometry(bw);
    }

    // TODO: Instanced Vertex Buffers
    private List<VertexBufferDescription> GenerateVertexBufferDescriptions()
    {
        List<VertexBufferDescription> descriptions = new();

        foreach (EnvironmentAssetMesh mesh in this.Meshes)
        {
            // Find base descriptor index, if it doesn't exist, create it
            IEnumerable<VertexBufferDescription> meshDescriptions = mesh.VerticesView.Buffers.Select(buffer =>
                buffer.Description
            );
            int baseDescriptionId = descriptions.IndexOf(meshDescriptions);
            if (baseDescriptionId == -1)
            {
                baseDescriptionId = descriptions.Count;
                descriptions.AddRange(meshDescriptions);
            }

            mesh._baseVertexBufferDescriptionId = baseDescriptionId;
        }

        return descriptions;
    }

    private void WriteSamplers(BinaryWriter bw)
    {
        bw.Write(this._shaderTextureOverrides.Count);
        foreach (var sampler in this._shaderTextureOverrides)
        {
            sampler.Write(bw);
        }
    }

    private void WriteVertexBuffers(BinaryWriter bw, List<VertexBuffer> uniqueVertexBuffers, int[] vertexBufferMap)
    {
        // Get vertex buffer IDs for each mesh (mapped to unique vertex buffers)
        List<int[]> bufferIdsOfMeshes = new();
        foreach (var mesh in this._meshes)
        {
            int[] bufferIds = new int[mesh.VerticesView.Buffers.Count];
            for (int i = 0; i < bufferIds.Length; i++)
            {
                int originalId = Array.FindIndex(
                    this._vertexBuffers,
                    buffer => buffer.View.Equals(mesh.VerticesView.Buffers[i].View)
                );
                if (originalId == -1)
                    ThrowHelper.ThrowInvalidOperationException($"Failed to find vertex buffer {i} for mesh: {mesh.Name}");

                bufferIds[i] = vertexBufferMap[originalId];
            }
            bufferIdsOfMeshes.Add(bufferIds);
        }

        // Set the vertex buffer IDs for each mesh and collect visibility flags for each vertex buffer
        var visibilityFlagsOfBuffers = new EnvironmentVisibility[uniqueVertexBuffers.Count];
        for (int meshId = 0; meshId < bufferIdsOfMeshes.Count; meshId++)
        {
            var mesh = this._meshes[meshId];
            var currentMeshBufferIds = bufferIdsOfMeshes[meshId];
            mesh._vertexBufferIds = currentMeshBufferIds;

            // Merge flags
            for (int i = 0; i < currentMeshBufferIds.Length; i++)
            {
                visibilityFlagsOfBuffers[currentMeshBufferIds[i]] |= mesh.VisibilityFlags;
            }
        }

        // Write count of buffers
        bw.Write(uniqueVertexBuffers.Count);

        // Write buffer data
        for (int i = 0; i < uniqueVertexBuffers.Count; i++)
        {
            var vertexBuffer = uniqueVertexBuffers[i];

            bw.Write((byte)visibilityFlagsOfBuffers[i]);
            bw.Write(vertexBuffer.View.Length);
            bw.Write(vertexBuffer.View.Span);
        }
    }

    private void WriteIndexBuffers(BinaryWriter bw, List<IndexBuffer> uniqueIndexBuffers, int[] indexBufferMap)
    {
        // Get index buffer id for each mesh (mapped to unique index buffers)
        List<int> bufferIdOfMeshes = new();
        foreach (var mesh in this._meshes)
        {
            int originalId = Array.FindIndex(this._indexBuffers, buffer => buffer.Buffer.Span == mesh.Indices.Buffer.Span);
            if (originalId == -1)
                ThrowHelper.ThrowInvalidOperationException($"Failed to find index buffer for mesh: {mesh.Name}");

            bufferIdOfMeshes.Add(indexBufferMap[originalId]);
        }

        // Set the index buffer id for each mesh and collect visibility flags for each buffer
        var visibilityFlagsOfBuffers = new EnvironmentVisibility[uniqueIndexBuffers.Count];
        for (int meshId = 0; meshId < bufferIdOfMeshes.Count; meshId++)
        {
            var mesh = this._meshes[meshId];
            var meshIndexBufferId = bufferIdOfMeshes[meshId];
            mesh._indexBufferId = meshIndexBufferId;

            // Merge flags
            visibilityFlagsOfBuffers[meshIndexBufferId] |= mesh.VisibilityFlags;
        }

        // Write count of buffers
        bw.Write(uniqueIndexBuffers.Count);

        // Write buffer data
        for (int i = 0; i < uniqueIndexBuffers.Count; i++)
        {
            var indexBuffer = uniqueIndexBuffers[i].Buffer;

            bw.Write((byte)visibilityFlagsOfBuffers[i]);
            bw.Write(indexBuffer.Length);
            bw.Write(indexBuffer.Span);
        }
    }

    #region IDisposable
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            if (this._vertexBuffers is not null)
            {
                foreach (VertexBuffer vertexBuffer in this._vertexBuffers)
                {
                    vertexBuffer?.Dispose();
                }
            }

            if (this._indexBuffers is not null)
            {
                foreach (IndexBuffer indexBuffer in this._indexBuffers)
                {
                    indexBuffer?.Dispose();
                }
            }
        }

        this.IsDisposed = true;
    }
    #endregion
}
