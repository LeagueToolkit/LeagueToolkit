using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Core.SceneGraph;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using System.Text;

namespace LeagueToolkit.Core.Environment;

/// <summary>Represents an environment asset</summary>
public sealed class EnvironmentAsset : IDisposable
{
    /// <summary>Gets the baked terrain samplers used for this environment asset</summary>
    public EnvironmentAssetBakedTerrainSamplers BakedTerrainSamplers { get; private set; }

    /// <summary>Gets a read-only list of the meshes used by this environment asset</summary>
    public IReadOnlyList<EnvironmentAssetMesh> Meshes => this._meshes;
    private readonly List<EnvironmentAssetMesh> _meshes = new();

    /// <summary>Gets the <see cref="BucketedGeometry"/> scene graph for the environment asset</summary>
    public BucketedGeometry SceneGraph { get; private set; }

    /// <summary>Gets a read-only list of the planar reflectors used by the environment asset</summary>
    public IReadOnlyList<PlanarReflector> PlanarReflectors => this._planarReflectors;
    private readonly List<PlanarReflector> _planarReflectors = new();

    private readonly VertexBuffer[] _vertexBuffers;
    private readonly IndexBuffer[] _indexBuffers;

    public bool IsDisposed { get; private set; }

    internal EnvironmentAsset(
        EnvironmentAssetBakedTerrainSamplers bakedTerrainSamplers,
        IEnumerable<EnvironmentAssetMesh> meshes,
        BucketedGeometry sceneGraph,
        IEnumerable<PlanarReflector> planarReflectors,
        IEnumerable<VertexBuffer> vertexBuffers,
        IEnumerable<IndexBuffer> indexBuffers
    )
    {
        Guard.IsNotNull(meshes, nameof(meshes));
        Guard.IsNotNull(sceneGraph, nameof(sceneGraph));
        Guard.IsNotNull(planarReflectors, nameof(planarReflectors));
        Guard.IsNotNull(vertexBuffers, nameof(vertexBuffers));
        Guard.IsNotNull(indexBuffers, nameof(indexBuffers));

        this.BakedTerrainSamplers = bakedTerrainSamplers;
        this._meshes = new(meshes);
        this.SceneGraph = sceneGraph;
        this._planarReflectors = new(planarReflectors);

        this._vertexBuffers = vertexBuffers.ToArray();
        this._indexBuffers = indexBuffers.ToArray();
    }

    /// <summary>
    /// Creates a new <see cref="EnvironmentAsset"/> instance by reading it from <paramref name="fileLocation"/>
    /// </summary>
    /// <param name="fileLocation">The file to read from</param>
    public EnvironmentAsset(string fileLocation) : this(File.OpenRead(fileLocation)) { }

    /// <summary>
    /// Creates a new <see cref="EnvironmentAsset"/> instance by reading it from <paramref name="stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from</param>
    /// <exception cref="InvalidFileSignatureException">The header magic signature is invalid</exception>
    /// <exception cref="UnsupportedFileVersionException">The version of the <see cref="EnvironmentAsset"/> file is not supported</exception>
    public EnvironmentAsset(Stream stream)
    {
        using BinaryReader br = new(stream);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
        if (magic != "OEGM")
        {
            throw new InvalidFileSignatureException();
        }

        uint version = br.ReadUInt32();
        if (version is not (5 or 6 or 7 or 9 or 11 or 12 or 13))
        {
            throw new UnsupportedFileVersionException();
        }

        bool useSeparatePointLights = false;
        if (version < 7)
        {
            useSeparatePointLights = br.ReadBoolean();
        }

        ReadBakedTerrainSamplers(br, version);

        uint vertexBufferDescriptionCount = br.ReadUInt32();
        VertexBufferDescription[] vertexBufferDescriptions = new VertexBufferDescription[vertexBufferDescriptionCount];
        for (int i = 0; i < vertexBufferDescriptionCount; i++)
        {
            vertexBufferDescriptions[i] = VertexBufferDescription.ReadFromMapGeometry(br);
        }

        // Reading of vertex buffers is deferred until we start reading meshes
        uint vertexBufferCount = br.ReadUInt32();
        this._vertexBuffers = new VertexBuffer[vertexBufferCount];
        long[] vertexBufferOffsets = new long[vertexBufferCount];
        for (int i = 0; i < vertexBufferCount; i++)
        {
            if (version >= 13)
            {
                EnvironmentVisibilityFlags _ = (EnvironmentVisibilityFlags)br.ReadByte();
            }

            uint bufferSize = br.ReadUInt32();

            vertexBufferOffsets[i] = br.BaseStream.Position;
            br.BaseStream.Seek(bufferSize, SeekOrigin.Current);
        }

        uint indexBufferCount = br.ReadUInt32();
        this._indexBuffers = new IndexBuffer[indexBufferCount];
        for (int i = 0; i < indexBufferCount; i++)
        {
            if (version >= 13)
            {
                EnvironmentVisibilityFlags _ = (EnvironmentVisibilityFlags)br.ReadByte();
            }

            int bufferSize = br.ReadInt32();
            MemoryOwner<byte> indexBufferOwner = MemoryOwner<byte>.Allocate(bufferSize);

            int bytesRead = br.Read(indexBufferOwner.Span);
            if (bytesRead != bufferSize)
                throw new IOException(
                    $"Failed to read index buffer: {i} {nameof(bufferSize)}: {bufferSize} {nameof(bytesRead)}: {bytesRead}"
                );

            this._indexBuffers[i] = IndexBuffer.Create(IndexFormat.U16, indexBufferOwner);
        }

        uint modelCount = br.ReadUInt32();
        for (int i = 0; i < modelCount; i++)
        {
            this._meshes.Add(
                new(i, this, br, vertexBufferDescriptions, vertexBufferOffsets, useSeparatePointLights, version)
            );
        }

        this.SceneGraph = new(br);

        if (version >= 13)
        {
            uint planarReflectorCount = br.ReadUInt32();
            for (int i = 0; i < planarReflectorCount; i++)
            {
                this._planarReflectors.Add(PlanarReflector.ReadFromMapGeometry(br));
            }
        }
    }

    internal void ReadBakedTerrainSamplers(BinaryReader br, uint version)
    {
        EnvironmentAssetBakedTerrainSamplers bakedTerrainSamplers = new();
        if (version >= 9)
        {
            bakedTerrainSamplers.Primary = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

            if (version >= 11)
            {
                bakedTerrainSamplers.Secondary = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            }
        }

        this.BakedTerrainSamplers = bakedTerrainSamplers;
    }

    internal IVertexBufferView ReflectVertexBuffer(
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
        int bytesRead = br.Read(vertexBufferOwner.Span);
        if (bytesRead != vertexBufferOwner.Length)
            throw new IOException(
                $"Failed to read vertex buffer: {id} size: {vertexBufferOwner.Length} {nameof(bytesRead)}: {bytesRead}"
            );
        br.BaseStream.Seek(returnPosition, SeekOrigin.Begin);

        // Create the buffer, store it and return a view into it
        VertexBuffer vertexBuffer = VertexBuffer.Create(description.Usage, description.Elements, vertexBufferOwner);
        this._vertexBuffers[id] = vertexBuffer;
        return vertexBuffer;
    }

    internal IndexArray ReflectIndexBuffer(int id) => this._indexBuffers[id].AsArray();

    /// <summary>
    /// Writes this <see cref="EnvironmentAsset"/> instance into <paramref name="fileLocation"/> with the requested <paramref name="version"/>
    /// </summary>
    /// <param name="fileLocation">The file to write into</param>
    /// <param name="version">The version of the written <see cref="EnvironmentAsset"/> file</param>
    public void Write(string fileLocation, uint version) => Write(File.Create(fileLocation), version);

    /// <summary>
    /// Writes this <see cref="EnvironmentAsset"/> instance into <paramref name="stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to write to</param>
    /// <param name="version">The version of the written <see cref="EnvironmentAsset"/> file</param>
    /// <param name="leaveOpen">Whether the internal reader should leave <paramref name="stream"/> opened</param>
    /// <exception cref="ArgumentException"></exception>
    public void Write(Stream stream, uint version, bool leaveOpen = false)
    {
        if (version is not (5 or 6 or 7 or 9 or 11 or 12 or 13))
        {
            throw new ArgumentException($"Unsupported version: {version}", nameof(version));
        }

        using BinaryWriter bw = new(stream, Encoding.UTF8, leaveOpen);

        bw.Write(Encoding.ASCII.GetBytes("OEGM"));
        bw.Write(version);

        bool usesSeparatePointLights = false;
        if (version < 7)
        {
            usesSeparatePointLights = this._meshes.Any(mesh => mesh.PointLight is not null);
            bw.Write(usesSeparatePointLights);
        }

        WriteBakedTerrainSamplers(bw, version);

        List<VertexBufferDescription> vertexElementGroups = GenerateVertexBufferDescriptions();
        bw.Write(vertexElementGroups.Count);
        foreach (VertexBufferDescription vertexElementGroup in vertexElementGroups)
        {
            vertexElementGroup.WriteToMapGeometry(bw);
        }

        WriteVertexBuffers(bw, version);
        WriteIndexBuffers(bw, version);

        bw.Write(this._meshes.Count);
        foreach (EnvironmentAssetMesh model in this._meshes)
        {
            model.Write(bw, usesSeparatePointLights, version);
        }

        this.SceneGraph.Write(bw);

        if (version >= 13)
        {
            bw.Write(this.PlanarReflectors.Count);
            foreach (PlanarReflector planarReflector in this.PlanarReflectors)
            {
                planarReflector.WriteToMapGeometry(bw);
            }
        }
    }

    // TODO: Instanced Vertex Buffers
    private List<VertexBufferDescription> GenerateVertexBufferDescriptions()
    {
        List<VertexBufferDescription> descriptions = new();

        foreach (EnvironmentAssetMesh mesh in this.Meshes)
        {
            // Find base descriptor index, if it doesn't exist, create it
            IEnumerable<VertexBufferDescription> meshDescriptions = mesh.VerticesView.Buffers.Select(
                buffer => buffer.Description
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

    private void WriteBakedTerrainSamplers(BinaryWriter bw, uint version)
    {
        if (version >= 9)
        {
            WriteSampler(this.BakedTerrainSamplers.Primary);

            if (version >= 11)
            {
                WriteSampler(this.BakedTerrainSamplers.Secondary);
            }
        }

        void WriteSampler(string sampler)
        {
            if (string.IsNullOrEmpty(sampler))
            {
                bw.Write(0); // Length
            }
            else
            {
                bw.Write(sampler.Length);
                bw.Write(Encoding.ASCII.GetBytes(sampler));
            }
        }
    }

    // TODO: Vertex Buffer instancing
    private void WriteVertexBuffers(BinaryWriter bw, uint version)
    {
        // Get vertex buffer IDs for each mesh
        List<int[]> bufferIdsOfMeshes = new(this._meshes.Select(GetMeshVertexBufferIds));

        // Set the vertex buffer IDs for each mesh and collect visibility flags for each vertex buffer
        EnvironmentVisibilityFlags[] visibilityFlagsOfBuffers = new EnvironmentVisibilityFlags[
            this._vertexBuffers.Length
        ];
        for (int meshId = 0; meshId < bufferIdsOfMeshes.Count; meshId++)
        {
            EnvironmentAssetMesh mesh = this._meshes[meshId];

            // It would be better to pass this into the mesh writing function
            int[] currentMeshBufferIds = bufferIdsOfMeshes[meshId];
            mesh._vertexBufferIds = currentMeshBufferIds;

            // Merge flags
            for (int i = 0; i < currentMeshBufferIds.Length; i++)
                visibilityFlagsOfBuffers[currentMeshBufferIds[i]] |= mesh.VisibilityFlags;
        }

        // Write count of buffers
        bw.Write(this._vertexBuffers.Length);

        // Write buffer data
        for (int i = 0; i < this._vertexBuffers.Length; i++)
        {
            VertexBuffer vertexBuffer = this._vertexBuffers[i];

            if (version >= 13)
                bw.Write((byte)visibilityFlagsOfBuffers[i]);
            bw.Write(vertexBuffer.View.Length);
            bw.Write(vertexBuffer.View.Span);
        }
    }

    private void WriteIndexBuffers(BinaryWriter bw, uint version)
    {
        // Get index buffer id for each mesh
        List<int> bufferIdOfMeshes = new(this._meshes.Select(GetMeshIndexBufferId));

        // Set the index buffer id for each mesh and collect visibility flags for each buffer
        EnvironmentVisibilityFlags[] visibilityFlagsOfBuffers = new EnvironmentVisibilityFlags[
            this._indexBuffers.Length
        ];
        for (int meshId = 0; meshId < bufferIdOfMeshes.Count; meshId++)
        {
            EnvironmentAssetMesh mesh = this._meshes[meshId];

            // It would be better to pass this into the mesh writing function
            int meshIndexBufferId = bufferIdOfMeshes[meshId];
            mesh._indexBufferId = meshIndexBufferId;

            // Merge flags
            visibilityFlagsOfBuffers[meshIndexBufferId] |= mesh.VisibilityFlags;
        }

        // Write count of buffers
        bw.Write(this._indexBuffers.Length);

        // Write buffer data
        for (int i = 0; i < this._indexBuffers.Length; i++)
        {
            ReadOnlyMemory<byte> indexBuffer = this._indexBuffers[i].Buffer;

            if (version >= 13)
                bw.Write((byte)visibilityFlagsOfBuffers[i]);
            bw.Write(indexBuffer.Length);
            bw.Write(indexBuffer.Span);
        }
    }

    private int[] GetMeshVertexBufferIds(EnvironmentAssetMesh mesh)
    {
        int[] bufferIds = new int[mesh.VerticesView.Buffers.Count];
        for (int i = 0; i < bufferIds.Length; i++)
        {
            int bufferId = Array.FindIndex(
                this._vertexBuffers,
                buffer => buffer.View.Equals(mesh.VerticesView.Buffers[i].View)
            );
            if (bufferId == -1)
                ThrowHelper.ThrowInvalidOperationException($"Failed to find vertex buffer {i} for mesh: {mesh.Name}");

            bufferIds[i] = bufferId;
        }

        return bufferIds;
    }

    private int GetMeshIndexBufferId(EnvironmentAssetMesh mesh)
    {
        return Array.FindIndex(this._indexBuffers, buffer => buffer.Buffer.Span == mesh.Indices.Buffer.Span) switch
        {
            -1 => throw new InvalidOperationException($"Failed to find index buffer for mesh: {mesh.Name}"),
            int bufferId => bufferId
        };
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.IsDisposed)
            return;

        if (disposing)
        {
            if (this._vertexBuffers is not null)
                foreach (VertexBuffer vertexBuffer in this._vertexBuffers)
                {
                    vertexBuffer?.Dispose();
                }

            if (this._indexBuffers is not null)
                foreach (IndexBuffer indexBuffer in this._indexBuffers)
                {
                    indexBuffer?.Dispose();
                }
        }

        this.IsDisposed = true;
    }
}

internal static class BakedEnvironmentVertexDescription
{
    public static readonly VertexElement[] BASIC = new[] { VertexElement.POSITION, VertexElement.DIFFUSE_UV };
}
