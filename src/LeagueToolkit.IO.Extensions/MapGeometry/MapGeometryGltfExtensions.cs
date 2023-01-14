using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Memory;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometryFile
{
    public static class MapGeometryGltfExtensions
    {
        public static ModelRoot ToGltf(this MapGeometry mapGeometry)
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene(root.LogicalScenes.Count);
            Node mapNode = scene.CreateNode("map"); //.WithLocalScale(new(-1f, 1f, 1f));

            foreach (MapGeometryModel mesh in mapGeometry.Meshes)
            {
                // Create materials
                Dictionary<string, Material> materials = new();
                foreach (MapGeometrySubmesh range in mesh.Submeshes)
                    materials.TryAdd(
                        range.Material,
                        root.CreateMaterial(range.Material).WithUnlit().WithDoubleSide(mesh.DisableBackfaceCulling)
                    );

                // Create mesh
                Mesh gltfMesh = CreateGltfMesh(root, mesh, materials);

                // Create mesh node
                mapNode.CreateNode($"__{mesh.Name}__").WithMesh(gltfMesh); //.WithLocalTransform(mesh.Transform);
            }

            root.MergeBuffers();

            root.DefaultScene = scene;

            return root;
        }

        private static Mesh CreateGltfMesh(
            ModelRoot root,
            MapGeometryModel mesh,
            IReadOnlyDictionary<string, Material> materials
        )
        {
            Mesh gltfMesh = root.CreateMesh(mesh.Name);

            MemoryAccessor[] meshVertexMemoryAccessors = CreateGltfMeshVertexMemoryAccessors(mesh.VerticesView);

            MemoryAccessor.SanitizeVertexAttributes(meshVertexMemoryAccessors);
            SanitizeVertexMemoryAccessors(meshVertexMemoryAccessors);

            foreach (MapGeometrySubmesh range in mesh.Submeshes)
            {
                MemoryAccessor indicesMemoryAccessor = CreateGltfMeshPrimitiveIndicesMemoryAccessor(
                    range,
                    mesh.Indices
                );
                MemoryAccessor[] vertexMemoryAccessors = CreateGltfMeshPrimitiveVertexMemoryAccessors(
                    range,
                    meshVertexMemoryAccessors
                );

                MemoryAccessor.VerifyVertexIndices(indicesMemoryAccessor, (uint)range.VertexCount);

                gltfMesh
                    .CreatePrimitive()
                    .WithMaterial(materials[range.Material])
                    .WithIndicesAccessor(PrimitiveType.TRIANGLES, indicesMemoryAccessor)
                    .WithVertexAccessors(vertexMemoryAccessors);
            }

            return gltfMesh;
        }

        private static MemoryAccessor[] CreateGltfMeshVertexMemoryAccessors(InstancedVertexBufferView vertexBuffer)
        {
            List<MemoryAccessor> memoryAccessors = new();
            foreach (VertexElement element in vertexBuffer.Buffers.SelectMany(x => x.Description.Elements))
            {
                // Create access info
                MemoryAccessInfo accessInfo = GetElementMemoryAccessInfo(element, vertexBuffer.VertexCount);

                // Create vertex memory accessor
                ArraySegment<byte> gltfAccessorBuffer =
                    new(new byte[accessInfo.StepByteLength * vertexBuffer.VertexCount]);
                MemoryAccessor gltfMemoryAccessor = new(gltfAccessorBuffer, accessInfo);

                // Write data
                int elementSize = element.GetSize();
                VertexElementAccessor elementAccessor = vertexBuffer.GetAccessor(element.Name);
                Span<byte> gltfMemoryAccessorData = gltfMemoryAccessor.Data.AsSpan();
                for (int i = 0; i < vertexBuffer.VertexCount; i++)
                {
                    // First slice into start vertex and then slice the element
                    ReadOnlySpan<byte> elementData = elementAccessor.BufferView.Span.Slice(
                        i * elementAccessor.VertexStride + elementAccessor.ElementOffset,
                        elementSize
                    );

                    WriteAttributeData(
                        gltfMemoryAccessorData,
                        i * accessInfo.StepByteLength,
                        element.Format,
                        elementData
                    );
                }

                memoryAccessors.Add(gltfMemoryAccessor);
            }

            return memoryAccessors.ToArray();
        }

        private static MemoryAccessor[] CreateGltfMeshPrimitiveVertexMemoryAccessors(
            MapGeometrySubmesh range,
            MemoryAccessor[] meshMemoryAccessors
        ) =>
            meshMemoryAccessors
                .Select(x => new MemoryAccessor(x.Data, x.Attribute.Slice(range.MinVertex, range.VertexCount)))
                .ToArray();

        private static MemoryAccessor CreateGltfMeshPrimitiveIndicesMemoryAccessor(
            MapGeometrySubmesh range,
            ReadOnlyMemory<ushort> indexBuffer
        )
        {
            MemoryAccessor memoryAccessor =
                new(
                    new(new byte[2 * range.IndexCount]),
                    new("INDEX", 0, range.IndexCount, 0, DimensionType.SCALAR, EncodingType.UNSIGNED_SHORT)
                );

            Span<ushort> accessorBuffer = memoryAccessor.Data.AsSpan().Cast<byte, ushort>();
            for (int i = 0; i < range.IndexCount; i++)
                accessorBuffer[i] = (ushort)(indexBuffer.Span[i + range.StartIndex] - range.MinVertex);

            return memoryAccessor;
        }

        private static void WriteAttributeData(
            Span<byte> buffer,
            int offset,
            ElementFormat format,
            ReadOnlySpan<byte> data
        )
        {
            if (format is ElementFormat.BGRA_Packed8888)
                WriteAttributeBgraU8(buffer, offset, data);
            else if (format is ElementFormat.ZYXW_Packed8888)
                WriteAttributeZyxwU8(buffer, offset, data);
            else
                data.CopyTo(buffer[offset..]);
        }

        private static void WriteAttributeBgraU8(Span<byte> buffer, int offset, ReadOnlySpan<byte> data)
        {
            buffer[offset + 0] = data[2];
            buffer[offset + 1] = data[1];
            buffer[offset + 2] = data[0];
            buffer[offset + 3] = data[3];
        }

        private static void WriteAttributeZyxwU8(Span<byte> buffer, int offset, ReadOnlySpan<byte> data)
        {
            buffer[offset + 0] = data[2];
            buffer[offset + 1] = data[1];
            buffer[offset + 2] = data[0];
            buffer[offset + 3] = data[3];
        }

        private static void SanitizeVertexMemoryAccessors(MemoryAccessor[] memoryAccessors)
        {
            SanitizeVertexNormalMemoryAccessor(memoryAccessors);
        }

        private static void SanitizeVertexNormalMemoryAccessor(MemoryAccessor[] memoryAccessors)
        {
            MemoryAccessor normalAccessor = memoryAccessors.FirstOrDefault(x => x.Attribute.Name == "NORMAL");
            if (normalAccessor is not null)
            {
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
        }

        private static MemoryAccessInfo GetElementMemoryAccessInfo(VertexElement element, int vertexCount)
        {
            string attribute = GetGltfVertexAttributeFromElementName(element.Name);

            return element.Format switch
            {
                ElementFormat.X_Float32 => new(attribute, 0, vertexCount, 0, DimensionType.SCALAR, EncodingType.FLOAT),
                ElementFormat.XY_Float32 => new(attribute, 0, vertexCount, 0, DimensionType.VEC2, EncodingType.FLOAT),
                ElementFormat.XYZ_Float32 => new(attribute, 0, vertexCount, 0, DimensionType.VEC3, EncodingType.FLOAT),
                ElementFormat.XYZW_Float32 => new(attribute, 0, vertexCount, 0, DimensionType.VEC4, EncodingType.FLOAT),

                ElementFormat.BGRA_Packed8888
                or ElementFormat.ZYXW_Packed8888
                or ElementFormat.RGBA_Packed8888
                or ElementFormat.XYZW_Packed8888
                    => new(attribute, 0, vertexCount, 0, DimensionType.VEC4, EncodingType.UNSIGNED_BYTE, true),
            };
        }

        private static string GetGltfVertexAttributeFromElementName(ElementName element)
        {
            return element switch
            {
                ElementName.Position => "POSITION",
                ElementName.Normal => "NORMAL",
                ElementName.PrimaryColor => "COLOR_0",
                ElementName.SecondaryColor => "COLOR_1",
                ElementName.DiffuseUV => "TEXCOORD_0",
                ElementName.LightmapUV => "TEXCOORD_1",
                ElementName.Tangent => "TANGENT",
                _ => throw new NotImplementedException($"Cannot map element: {element} to a glTF vertex attribute")
            };
        }
    }
}
