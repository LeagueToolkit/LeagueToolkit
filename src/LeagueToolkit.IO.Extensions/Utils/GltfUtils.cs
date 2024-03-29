﻿using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.IO.MapGeometryFile;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.IO.Extensions.Utils;

internal static class GltfUtils
{
    private static readonly Dictionary<ElementName, MemoryAccessInfo> VERTEX_ELEMENT_MAP =
        new()
        {
            { ElementName.Position, new("POSITION", 0, 0, 0, DimensionType.VEC3, EncodingType.FLOAT) },
            { ElementName.BlendWeight, new("WEIGHTS_0", 0, 0, 0, DimensionType.VEC4, EncodingType.FLOAT) },
            { ElementName.Normal, new("NORMAL", 0, 0, 0, DimensionType.VEC3, EncodingType.FLOAT) },
            { ElementName.PrimaryColor, new("COLOR_0", 0, 0, 0, DimensionType.VEC4, EncodingType.UNSIGNED_BYTE, true) },
            {
                ElementName.SecondaryColor,
                new("COLOR_1", 0, 0, 0, DimensionType.VEC4, EncodingType.UNSIGNED_BYTE, true)
            },
            { ElementName.BlendIndex, new("JOINTS_0", 0, 0, 0, DimensionType.VEC4, EncodingType.UNSIGNED_BYTE) },
            { ElementName.Texcoord0, new("TEXCOORD_0", 0, 0, 0, DimensionType.VEC2, EncodingType.FLOAT) },
            { ElementName.Texcoord7, new("TEXCOORD_1", 0, 0, 0, DimensionType.VEC2, EncodingType.FLOAT) },
            { ElementName.Tangent, new("TANGENT", 0, 0, 0, DimensionType.VEC4, EncodingType.FLOAT) },
        };

    internal static MemoryAccessor CreateIndicesMemoryAccessor(IndexArray indices, int baseVertex)
    {
        MemoryAccessor memoryAccessor =
            new(
                new(new byte[indices.Buffer.Length]),
                new(
                    "INDEX",
                    0,
                    indices.Count,
                    0,
                    DimensionType.SCALAR,
                    indices.Format == IndexFormat.U16 ? EncodingType.UNSIGNED_SHORT : EncodingType.UNSIGNED_INT
                )
            );

        IntegerArray accessorArray = memoryAccessor.AsIntegerArray();
        for (int i = 0; i < indices.Count; i++)
            accessorArray[i] = (uint)(indices[i] - baseVertex);

        return memoryAccessor;
    }

    internal static IEnumerable<MemoryAccessor> CreateVertexMemoryAccessors(IVertexBufferView vertexBuffer)
    {
        foreach (VertexElement element in vertexBuffer.Description.Elements)
        {
            // Create access info
            if (
                TryGetElementMemoryAccessInfo(element, vertexBuffer.VertexCount, out MemoryAccessInfo accessInfo)
                is false
            )
                continue;

            // Create vertex memory accessor
            ArraySegment<byte> gltfAccessorBuffer = new(new byte[accessInfo.StepByteLength * vertexBuffer.VertexCount]);
            MemoryAccessor gltfMemoryAccessor = new(gltfAccessorBuffer, accessInfo);

            // Write data
            VertexElementAccessor elementAccessor = vertexBuffer.GetAccessor(element.Name);

            if (element.Name == ElementName.Position)
                WriteMemoryAccessorVector3(gltfMemoryAccessor, elementAccessor);
            else if (element.Name == ElementName.BlendWeight)
                WriteMemoryAccessorVector4(gltfMemoryAccessor, elementAccessor);
            else if (element.Name == ElementName.Normal)
                WriteMemoryAccessorVector3(gltfMemoryAccessor, elementAccessor);
            else if (element.Name == ElementName.PrimaryColor)
                WriteMemoryAccessorBgraU8(gltfMemoryAccessor, elementAccessor);
            else if (element.Name == ElementName.SecondaryColor)
                WriteMemoryAccessorBgraU8(gltfMemoryAccessor, elementAccessor);
            else if (element.Name == ElementName.BlendIndex)
                WriteMemoryAccessorXyzwU8(gltfMemoryAccessor, elementAccessor);
            else if (element.Name == ElementName.Texcoord0)
                WriteMemoryAccessorVector2(gltfMemoryAccessor, elementAccessor);
            else if (element.Name == ElementName.Texcoord7)
                WriteMemoryAccessorVector2(gltfMemoryAccessor, elementAccessor);
            else if (element.Name == ElementName.Tangent)
                WriteMemoryAccessorVector4(gltfMemoryAccessor, elementAccessor);
            else
                ThrowHelper.ThrowInvalidOperationException($"Cannot write element: {element.Name}");

            yield return gltfMemoryAccessor;
        }
    }

    internal static MemoryAccessor[] SliceVertexMemoryAccessors(
        int baseVertex,
        int vertexCount,
        MemoryAccessor[] meshMemoryAccessors
    ) =>
        meshMemoryAccessors
            .Select(x => new MemoryAccessor(x.Data, x.Attribute.Slice(baseVertex, vertexCount)))
            .ToArray();

    private static void WriteMemoryAccessorVector2(MemoryAccessor gltfAccessor, VertexElementAccessor accessor)
    {
        VertexElementArray<Vector2> accessorArray = accessor.AsVector2Array();
        Vector2Array gltfArray = gltfAccessor.AsVector2Array();

        gltfArray.Fill(accessorArray);
    }

    private static void WriteMemoryAccessorVector3(MemoryAccessor gltfAccessor, VertexElementAccessor accessor)
    {
        VertexElementArray<Vector3> accessorArray = accessor.AsVector3Array();
        Vector3Array gltfArray = gltfAccessor.AsVector3Array();

        gltfArray.Fill(accessorArray);
    }

    private static void WriteMemoryAccessorVector4(MemoryAccessor gltfAccessor, VertexElementAccessor accessor)
    {
        VertexElementArray<Vector4> accessorArray = accessor.AsVector4Array();
        Vector4Array gltfArray = gltfAccessor.AsVector4Array();

        gltfArray.Fill(accessorArray);
    }

    private static void WriteMemoryAccessorBgraU8(MemoryAccessor gltfAccessor, VertexElementAccessor accessor)
    {
        VertexElementArray<(byte b, byte g, byte r, byte a)> accessorArray = accessor.AsBgraU8Array();
        Vector4Array gltfArray = gltfAccessor.AsVector4Array();

        gltfArray.Fill(accessorArray.Select(x => new Vector4(x.r, x.g, x.b, x.a)));
    }

    private static void WriteMemoryAccessorXyzwU8(MemoryAccessor gltfAccessor, VertexElementAccessor accessor)
    {
        VertexElementArray<(byte x, byte y, byte z, byte w)> accessorArray = accessor.AsXyzwU8Array();
        Vector4Array gltfArray = gltfAccessor.AsVector4Array();

        gltfArray.Fill(accessorArray.Select(x => new Vector4(x.x, x.y, x.z, x.w)));
    }

    internal static void SanitizeVertexMemoryAccessors(MemoryAccessor[] memoryAccessors)
    {
        SanitizeVertexNormalMemoryAccessor(memoryAccessors);
    }

    private static void SanitizeVertexNormalMemoryAccessor(MemoryAccessor[] memoryAccessors)
    {
        MemoryAccessor normalAccessor = memoryAccessors.FirstOrDefault(x => x.Attribute.Name == "NORMAL");
        if (normalAccessor is null)
            return;

        MemoryAccessor positionAccessor = memoryAccessors.First(x => x.Attribute.Name == "POSITION");

        Vector3Array positionsArray = positionAccessor.AsVector3Array();
        Vector3Array normalsArray = normalAccessor.AsVector3Array();

        for (int i = 0; i < positionsArray.Count; i++)
        {
            Vector3 normal = normalsArray[i];

            if (normal == Vector3.Zero)
                normal = positionsArray[i];

            float length = normal.Length();
            if (length < 0.99f || length > 0.01f)
                normal = Vector3.Normalize(normal);

            normalsArray[i] = normal;
        }
    }

    internal static bool TryGetElementMemoryAccessInfo(
        VertexElement element,
        int vertexCount,
        out MemoryAccessInfo memoryAccessInfo
    )
    {
        memoryAccessInfo = default;
        if (VERTEX_ELEMENT_MAP.TryGetValue(element.Name, out MemoryAccessInfo elementMemoryAccessInfo))
        {
            memoryAccessInfo = elementMemoryAccessInfo with { ItemsCount = vertexCount };
            return true;
        }

        return false;
    }

    internal static Node FindRootNode(Node node)
    {
        Guard.IsNotNull(node, nameof(node));

        while (true)
        {
            if (node.VisualParent is null)
                return node;

            node = node.VisualParent;
        }
    }
}
