using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Core.SceneGraph;
using LeagueToolkit.Utils.Exceptions;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Environment;

public static class WorldGeometry
{
    public static EnvironmentAsset Load(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
        if (magic is not "WGEO")
            throw new InvalidFileSignatureException();

        // I'm pretty sure there was version 6 for a short while before the transition to mapgeo
        uint version = br.ReadUInt32();
        if (version is not (5 or 4))
            throw new InvalidFileVersionException();

        int modelCount = br.ReadInt32();
        uint faceCount = br.ReadUInt32();

        List<VertexBuffer> vertexBuffers = new(modelCount);
        List<IndexBuffer> indexBuffers = new(modelCount);
        EnvironmentAssetMesh[] meshes = new EnvironmentAssetMesh[modelCount];

        for (int i = 0; i < modelCount; i++)
            meshes[i] = ReadWorldGeometryMesh(br, i, vertexBuffers, indexBuffers);

        BucketedGeometry[] sceneGraphs =
        {
            version switch
            {
                5 => new(br, 0),
                _ => new()
            }
        };

        return new([], meshes, sceneGraphs, [], vertexBuffers, indexBuffers);
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
            0,
            primitives,
            Matrix4x4.Identity,
            disableBackfaceCulling: false,
            EnvironmentQuality.AllQualities,
            EnvironmentVisibility.AllLayers,
            EnvironmentAssetMeshRenderFlags.Default,
            new(texture, Vector2.One, Vector2.Zero),
            new(),
            [],
            Vector2.Zero,
            Vector2.Zero
        );
    }

    internal static class BakedEnvironmentVertexDescription
    {
        public static readonly VertexElement[] BASIC = new[] { VertexElement.POSITION, VertexElement.TEXCOORD_0 };
    }
}
