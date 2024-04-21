using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Environment.Builder;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Utils.Exceptions;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Environment;

public static class SimpleEnvironment
{
    public static EnvironmentAsset Load(Stream stream)
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
                meshBuilder.WithRenderFlags(EnvironmentAssetMeshRenderFlags.IsDecal);

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
                0,
                [
                    new EnvironmentAssetMeshPrimitive(
                        nvrMeshMaterial.GetFormattedName(),
                        0,
                        nvrMeshPrimitive.IndexCount,
                        0,
                        nvrMeshPrimitive.VertexCount - 1
                    )
                ],
                Matrix4x4.Identity,
                false,
                EnvironmentQuality.AllQualities,
                EnvironmentVisibility.AllLayers,
                nvrMeshMaterial.Type is SimpleEnvironmentMaterialType.Decal
                    ? EnvironmentAssetMeshRenderFlags.IsDecal
                    : EnvironmentAssetMeshRenderFlags.Default,
                new(nvrMeshMaterial.Channels[0].Texture, Vector2.One, Vector2.Zero),
                new(),
                [],
                Vector2.Zero,
                Vector2.Zero
            );
        }

        return new([], meshes, [], [], meshVertexBuffers, meshIndexBuffers);
    }

    internal static class SimpleEnvironmentVertexDescription
    {
        public static readonly VertexElement[] DEFAULT =
        [
            VertexElement.POSITION,
            VertexElement.NORMAL,
            VertexElement.TEXCOORD_0,
            VertexElement.PRIMARY_COLOR
        ];
        public static readonly VertexElement[] FOUR_BLEND =
        [
            VertexElement.POSITION,
            VertexElement.NORMAL,
            VertexElement.TEXCOORD_0,
            VertexElement.TEXCOORD_7,
            VertexElement.PRIMARY_COLOR
        ];
        public static readonly VertexElement[] DUAL_VERTEX_COLOR =
        [
            VertexElement.POSITION,
            VertexElement.NORMAL,
            VertexElement.TEXCOORD_0,
            VertexElement.PRIMARY_COLOR,
            VertexElement.SECONDARY_COLOR
        ];
    }
}
