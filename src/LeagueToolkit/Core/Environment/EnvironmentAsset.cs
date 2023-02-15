using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Environment.Builder;
using LeagueToolkit.Core.Environment.SimpleEnvironment;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Core.SceneGraph;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
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
    /// Creates a new <see cref="EnvironmentAsset"/> instance by reading it from <paramref name="stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from</param>
    /// <exception cref="InvalidFileSignatureException">The header magic signature is invalid</exception>
    /// <exception cref="UnsupportedFileVersionException">The version of the <see cref="EnvironmentAsset"/> file is not supported</exception>
    public EnvironmentAsset(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
        if (magic != "OEGM")
            throw new InvalidFileSignatureException();

        uint version = br.ReadUInt32();
        if (version is not (5 or 6 or 7 or 9 or 11 or 12 or 13))
            throw new UnsupportedFileVersionException();

        bool useSeparatePointLights = version < 7 && br.ReadBoolean();

        ReadBakedTerrainSamplers(br, version);

        // Read vertex declarations
        uint vertexDeclarationCount = br.ReadUInt32();
        VertexBufferDescription[] vertexDeclarations = new VertexBufferDescription[vertexDeclarationCount];
        for (int i = 0; i < vertexDeclarationCount; i++)
            vertexDeclarations[i] = VertexBufferDescription.ReadFromMapGeometry(br);

        // Reading of vertex buffers is deferred until we start reading meshes
        uint vertexBufferCount = br.ReadUInt32();
        this._vertexBuffers = new VertexBuffer[vertexBufferCount];
        long[] vertexBufferOffsets = new long[vertexBufferCount];
        for (int i = 0; i < vertexBufferCount; i++)
        {
            if (version >= 13)
            {
                EnvironmentVisibility _ = (EnvironmentVisibility)br.ReadByte();
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
                EnvironmentVisibility _ = (EnvironmentVisibility)br.ReadByte();
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

        // Read meshes
        uint meshCount = br.ReadUInt32();
        for (int i = 0; i < meshCount; i++)
            this._meshes.Add(
                new(i, this, br, vertexDeclarations, vertexBufferOffsets, useSeparatePointLights, version)
            );

        // Read bucketed geometry
        this.SceneGraph = new(br);

        if (version >= 13)
        {
            // Read reflection planes
            uint planarReflectorCount = br.ReadUInt32();
            for (int i = 0; i < planarReflectorCount; i++)
                this._planarReflectors.Add(PlanarReflector.ReadFromMapGeometry(br));
        }
    }

    internal void ReadBakedTerrainSamplers(BinaryReader br, uint version)
    {
        EnvironmentAssetBakedTerrainSamplers bakedTerrainSamplers = new();

        if (version >= 9)
            bakedTerrainSamplers.Primary = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

        if (version >= 11)
            bakedTerrainSamplers.Secondary = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

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
    /// Writes this <see cref="EnvironmentAsset"/> instance into <paramref name="stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to write to</param>
    /// <param name="version">The version of the written <see cref="EnvironmentAsset"/> file</param>
    /// <exception cref="ArgumentException"></exception>
    public void Write(Stream stream, uint version)
    {
        if (version is not (5 or 6 or 7 or 9 or 11 or 12 or 13))
            throw new ArgumentException($"Unsupported version: {version}", nameof(version));

        using BinaryWriter bw = new(stream, Encoding.UTF8, true);

        bw.Write(Encoding.ASCII.GetBytes("OEGM"));
        bw.Write(version);

        bool usesSeparatePointLights = false;
        if (version < 7)
        {
            usesSeparatePointLights = this._meshes.Any(mesh => mesh.PointLight is not null);
            bw.Write(usesSeparatePointLights);
        }

        WriteBakedTerrainSamplers(bw, version);

        List<VertexBufferDescription> vertexDeclarations = GenerateVertexBufferDescriptions();
        bw.Write(vertexDeclarations.Count);
        foreach (VertexBufferDescription vertexDeclaration in vertexDeclarations)
            vertexDeclaration.WriteToMapGeometry(bw);

        WriteVertexBuffers(bw, version);
        WriteIndexBuffers(bw, version);

        bw.Write(this._meshes.Count);
        foreach (EnvironmentAssetMesh model in this._meshes)
            model.Write(bw, usesSeparatePointLights, version);

        this.SceneGraph.Write(bw);

        if (version >= 13)
        {
            bw.Write(this.PlanarReflectors.Count);
            foreach (PlanarReflector planarReflector in this.PlanarReflectors)
                planarReflector.WriteToMapGeometry(bw);
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
        EnvironmentVisibility[] visibilityFlagsOfBuffers = new EnvironmentVisibility[this._vertexBuffers.Length];
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
        EnvironmentVisibility[] visibilityFlagsOfBuffers = new EnvironmentVisibility[this._indexBuffers.Length];
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

    #region World Geometry
    /// <summary>
    /// Reads a World Geometry (.wgeo) file from the specified stream
    /// </summary>
    /// <param name="stream">The stream to read from</param>
    /// <returns>The created <see cref="EnvironmentAsset"/> object</returns>
    public static EnvironmentAsset LoadWorldGeometry(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
        if (magic is not "WGEO")
            throw new InvalidFileSignatureException();

        // I'm pretty sure there was version 6 for a short while before the transition to mapgeo
        uint version = br.ReadUInt32();
        if (version is not (5 or 4))
            throw new UnsupportedFileVersionException();

        int modelCount = br.ReadInt32();
        uint faceCount = br.ReadUInt32();

        List<VertexBuffer> vertexBuffers = new(modelCount);
        List<IndexBuffer> indexBuffers = new(modelCount);
        EnvironmentAssetMesh[] meshes = new EnvironmentAssetMesh[modelCount];

        for (int i = 0; i < modelCount; i++)
            meshes[i] = ReadWorldGeometryMesh(br, i, vertexBuffers, indexBuffers);

        BucketedGeometry sceneGraph = version switch
        {
            5 => new(br),
            _ => new()
        };

        return new(new(), meshes, sceneGraph, Array.Empty<PlanarReflector>(), vertexBuffers, indexBuffers);
    }

    private static EnvironmentAssetMesh ReadWorldGeometryMesh(
        BinaryReader br,
        int id,
        List<VertexBuffer> vertexBuffers,
        List<IndexBuffer> indexBuffers
    )
    {
        string texture = br.ReadPaddedString(260);
        string material = br.ReadPaddedString(64);
        Sphere sphere = br.ReadSphere();
        Box aabb = br.ReadBox();

        int vertexCount = br.ReadInt32();
        int indexCount = br.ReadInt32();

        // Create vertex buffer memory
        VertexBufferDescription vertexDeclaration =
            new(VertexBufferUsage.Static, BakedEnvironmentVertexDescription.BASIC);
        int vertexSize = vertexDeclaration.GetVertexSize();
        MemoryOwner<byte> vertexBufferOwner = MemoryOwner<byte>.Allocate(vertexCount * vertexSize);

        // Create index buffer memory
        IndexFormat indexFormat = indexCount <= ushort.MaxValue + 1 ? IndexFormat.U16 : IndexFormat.U32;
        int indexFormatSize = IndexBuffer.GetFormatSize(indexFormat);
        MemoryOwner<byte> indexBufferOwner = MemoryOwner<byte>.Allocate(indexCount * indexFormatSize);

        // Read buffers
        br.Read(vertexBufferOwner.Span);
        br.Read(indexBufferOwner.Span);

        // Create buffers
        IndexBuffer indexBuffer = IndexBuffer.Create(indexFormat, indexBufferOwner);
        VertexBuffer vertexBuffer = VertexBuffer.Create(
            VertexBufferUsage.Static,
            vertexDeclaration.Elements,
            vertexBufferOwner
        );

        indexBuffers.Add(indexBuffer);
        vertexBuffers.Add(vertexBuffer);

        // Create primitive
        EnvironmentAssetMeshPrimitive[] primitives = new[]
        {
            new EnvironmentAssetMeshPrimitive(material, 0, indexCount, 0, vertexCount - 1)
        };

        return new(
            id,
            vertexBuffer,
            indexBuffer.AsArray(),
            primitives,
            Matrix4x4.Identity,
            disableBackfaceCulling: false,
            EnvironmentQuality.AllQualities,
            EnvironmentVisibility.AllLayers,
            EnvironmentAssetMeshRenderFlags.Default,
            new(texture, Vector2.One, Vector2.Zero),
            new(),
            new()
        );
    }
    #endregion

    #region Simple Environment
    public static EnvironmentAsset LoadSimpleEnvironment(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.UTF8.GetString(br.ReadBytes(4));
        if (magic is not "NVR\0")
            throw new InvalidFileSignatureException();

        ushort major = br.ReadUInt16();
        ushort minor = br.ReadUInt16();

        int materialsCount = br.ReadInt32();
        int vertexBufferCount = br.ReadInt32();
        int indexBufferCount = br.ReadInt32();
        int meshCount = br.ReadInt32();
        int nodesCount = br.ReadInt32();

        SimpleEnvironmentMaterial[] materials = new SimpleEnvironmentMaterial[materialsCount];
        long[] vertexBufferOffsets = new long[vertexBufferCount];
        IndexBuffer[] nvrIndexBuffers = new IndexBuffer[indexBufferCount];
        SimpleEnvironmentMesh[] nvrMeshes = new SimpleEnvironmentMesh[meshCount];

        // Read materials
        for (int i = 0; i < materialsCount; i++)
        {
            materials[i] = (major, minor) switch
            {
                (8, 1) => SimpleEnvironmentMaterial.ReadOld(br),
                _ => SimpleEnvironmentMaterial.Read(br)
            };
        }

        // Store vertex buffer offsets
        for (int i = 0; i < vertexBufferCount; i++)
        {
            int vertexBufferSize = br.ReadInt32();
            vertexBufferOffsets[i] = br.BaseStream.Position;

            br.BaseStream.Seek(vertexBufferSize, SeekOrigin.Current);
        }

        // Read index buffers
        for (int i = 0; i < indexBufferCount; i++)
        {
            int indexBufferSize = br.ReadInt32();
            int indexFormat = br.ReadInt32();
            MemoryOwner<byte> indexBufferOwner = MemoryOwner<byte>.Allocate(indexBufferSize);

            br.Read(indexBufferOwner.Span);

            nvrIndexBuffers[i] = IndexBuffer.Create(
                indexFormat is 0x65 ? IndexFormat.U16 : IndexFormat.U32,
                indexBufferOwner
            );
        }

        // Read meshes
        for (int i = 0; i < meshCount; i++)
        {
            nvrMeshes[i] = (major, minor) switch
            {
                (8, 1) => SimpleEnvironmentMesh.ReadOld(br),
                _ => SimpleEnvironmentMesh.Read(br)
            };
        }

        EnvironmentAssetMesh[] meshes = new EnvironmentAssetMesh[meshCount];
        VertexBuffer[] meshVertexBuffers = new VertexBuffer[meshCount];
        IndexBuffer[] meshIndexBuffers = new IndexBuffer[meshCount];
        for (int meshId = 0; meshId < nvrMeshes.Length; meshId++)
        {
            SimpleEnvironmentMesh nvrMesh = nvrMeshes[meshId];
            SimpleEnvironmentMaterial nvrMeshMaterial = materials[nvrMesh.MaterialId];
            SimpleEnvironmentMeshPrimitive nvrMeshPrimitive = nvrMesh.Primitives[0];
            SimpleEnvironmentMeshPrimitive nvrComplexPrimitive = nvrMesh.Primitives[1];

            EnvironmentAssetMeshBuilder meshBuilder = new();

            meshBuilder.WithVisibilityFlags(EnvironmentVisibility.AllLayers);

            if (nvrMeshMaterial.Type is SimpleEnvironmentMaterialType.Decal)
                meshBuilder.WithRenderFlags(EnvironmentAssetMeshRenderFlags.Decal);

            VertexBufferDescription vertexDeclaration = nvrMeshMaterial.GetVertexDeclaration();
            MemoryOwner<byte> vertexBufferOwner = MemoryOwner<byte>.Allocate(
                nvrMeshPrimitive.VertexCount * vertexDeclaration.GetVertexSize()
            );

            MemoryOwner<byte> indexBufferOwner = MemoryOwner<byte>.Allocate(
                nvrMeshPrimitive.IndexCount * sizeof(ushort)
            );

            // Seek to vertex buffer + offset to first vertex
            br.BaseStream.Seek(
                vertexBufferOffsets[nvrMeshPrimitive.VertexBufferId]
                    + (nvrMeshPrimitive.StartVertex * vertexDeclaration.GetVertexSize()),
                SeekOrigin.Begin
            );
            br.Read(vertexBufferOwner.Span);

            // Copy and normalize indices
            IndexArray nvrMeshIndexArray = nvrIndexBuffers[nvrMeshPrimitive.IndexBufferId]
                .AsArray()
                .Slice(nvrMeshPrimitive.StartIndex, nvrMeshPrimitive.IndexCount);

            uint minVertex = nvrMeshIndexArray.Min();
            for (int i = 0; i < nvrMeshPrimitive.IndexCount; i++)
            {
                ushort normalizedIndex = (ushort)(nvrMeshIndexArray[i] - minVertex);
                MemoryMarshal.Write(indexBufferOwner.Span[(i * sizeof(ushort))..], ref normalizedIndex);
            }

            VertexBuffer vertexBuffer = VertexBuffer.Create(
                vertexDeclaration.Usage,
                vertexDeclaration.Elements,
                vertexBufferOwner
            );
            IndexBuffer indexBuffer = IndexBuffer.Create(IndexFormat.U16, indexBufferOwner);

            meshVertexBuffers[meshId] = vertexBuffer;
            meshIndexBuffers[meshId] = indexBuffer;
            meshes[meshId] = new(
                meshId,
                vertexBuffer,
                indexBuffer.AsArray(),
                new[]
                {
                    new EnvironmentAssetMeshPrimitive(
                        nvrMeshMaterial.GetFormattedName(),
                        0,
                        nvrMeshPrimitive.IndexCount,
                        0,
                        nvrMeshPrimitive.VertexCount - 1
                    )
                },
                Matrix4x4.Identity,
                false,
                EnvironmentQuality.AllQualities,
                EnvironmentVisibility.AllLayers,
                nvrMeshMaterial.Type is SimpleEnvironmentMaterialType.Decal
                    ? EnvironmentAssetMeshRenderFlags.Decal
                    : EnvironmentAssetMeshRenderFlags.Default,
                new(nvrMeshMaterial.Channels[0].Texture, Vector2.One, Vector2.Zero),
                new(),
                new()
            );
        }

        return new(new(), meshes, new(), Array.Empty<PlanarReflector>(), meshVertexBuffers, meshIndexBuffers);
    }
    #endregion

    #region IDisposable
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
    #endregion
}

internal static class BakedEnvironmentVertexDescription
{
    public static readonly VertexElement[] BASIC = new[] { VertexElement.POSITION, VertexElement.TEXCOORD_0 };
}

internal static class SimpleEnvironmentVertexDescription
{
    public static readonly VertexElement[] DEFAULT = new[]
    {
        VertexElement.POSITION,
        VertexElement.NORMAL,
        VertexElement.TEXCOORD_0,
        VertexElement.PRIMARY_COLOR
    };
    public static readonly VertexElement[] FOUR_BLEND = new[]
    {
        VertexElement.POSITION,
        VertexElement.NORMAL,
        VertexElement.TEXCOORD_0,
        VertexElement.TEXCOORD_7,
        VertexElement.PRIMARY_COLOR
    };
    public static readonly VertexElement[] DUAL_VERTEX_COLOR = new[]
    {
        VertexElement.POSITION,
        VertexElement.NORMAL,
        VertexElement.TEXCOORD_0,
        VertexElement.PRIMARY_COLOR,
        VertexElement.SECONDARY_COLOR
    };
}
