using LeagueToolkit.Helpers.Structures;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
using System;
using System.Collections.Generic;
using System.Numerics;
using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Core.Memory;
using CommunityToolkit.Diagnostics;

namespace LeagueToolkit.IO.MapGeometryFile
{
    using VERTEX = VertexBuilder<VertexPositionNormal, VertexColor1Texture2, VertexEmpty>;

    public static class MapGeometryGltfExtensions
    {
        public static ModelRoot ToGLTF(this MapGeometry mgeo)
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("Map");
            Node rootNode = scene.CreateNode("Map");

            // Find all layer combinations used in the Map
            // so we can group the meshes
            var layerModelMap = new Dictionary<MapGeometryVisibilityFlags, List<MapGeometryModel>>();
            foreach (MapGeometryModel mesh in mgeo.Meshes)
            {
                if (!layerModelMap.ContainsKey(mesh.VisibilityFlags))
                {
                    layerModelMap.Add(mesh.VisibilityFlags, new List<MapGeometryModel>());
                }

                layerModelMap[mesh.VisibilityFlags].Add(mesh);
            }

            // Create node for each layer combination
            var layerNodeMap = new Dictionary<MapGeometryVisibilityFlags, Node>();
            foreach (var layerModelPair in layerModelMap)
            {
                layerNodeMap.Add(
                    layerModelPair.Key,
                    rootNode.CreateNode(DeriveLayerCombinationName(layerModelPair.Key))
                );
            }

            foreach (MapGeometryModel mesh in mgeo.Meshes)
            {
                IMeshBuilder<MaterialBuilder> meshBuilder = BuildMapGeometryMeshStatic(mesh);

                layerNodeMap[mesh.VisibilityFlags]
                    .CreateNode()
                    .WithMesh(root.CreateMesh(meshBuilder))
                    .WithLocalTransform(new AffineTransform(mesh.Transform));
            }

            return root;
        }

        private static string DeriveLayerCombinationName(MapGeometryVisibilityFlags layerCombination)
        {
            if (layerCombination == MapGeometryVisibilityFlags.NoLayer)
            {
                return "NoLayer";
            }
            else if (layerCombination == MapGeometryVisibilityFlags.AllLayers)
            {
                return "AllLayers";
            }
            else
            {
                string name = "Layer-";

                foreach (MapGeometryVisibilityFlags layerFlag in Enum.GetValues(typeof(MapGeometryVisibilityFlags)))
                {
                    if (
                        layerCombination.HasFlag(layerFlag)
                        && layerFlag != MapGeometryVisibilityFlags.AllLayers
                        && layerFlag != MapGeometryVisibilityFlags.NoLayer
                    )
                    {
                        byte layerIndex = byte.Parse(layerFlag.ToString().Replace("Layer", ""));
                        name += layerIndex + "-";
                    }
                }

                return name.Remove(name.Length - 1);
            }
        }

        private static VERTEX[] BuildMeshVertices(MapGeometryModel mesh)
        {
            bool hasPositions = mesh.VerticesView.TryGetAccessor(ElementName.Position, out var positionAccessor);
            bool hasNormals = mesh.VerticesView.TryGetAccessor(ElementName.Normal, out var normalAccessor);
            bool hasBaseColor = mesh.VerticesView.TryGetAccessor(ElementName.PrimaryColor, out var baseColorAccessor);
            bool hasDiffuseUvs = mesh.VerticesView.TryGetAccessor(ElementName.DiffuseUV, out var diffuseUvAccessor);
            bool hasLightmapUvs = mesh.VerticesView.TryGetAccessor(ElementName.LightmapUV, out var lightmapUvAccessor);

            if (hasPositions is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh: {mesh.Name} does not have vertex positions");

            VertexElementArray<Vector3> positionsArray = positionAccessor.AsVector3Array();
            VertexElementArray<Vector3> normalsArray = hasNormals ? normalAccessor.AsVector3Array() : new();
            var baseColorArray = hasBaseColor ? baseColorAccessor.AsBgraU8Array() : new();
            VertexElementArray<Vector2> diffuseUvsArray = hasDiffuseUvs ? diffuseUvAccessor.AsVector2Array() : new();
            VertexElementArray<Vector2> lightmapUvsArray = hasLightmapUvs ? lightmapUvAccessor.AsVector2Array() : new();

            VERTEX[] gltfVertices = new VERTEX[mesh.VerticesView.VertexCount];
            for (int i = 0; i < mesh.VerticesView.VertexCount; i++)
            {
                gltfVertices[i] = new VERTEX()
                    .WithGeometry(positionsArray[i], hasNormals ? normalsArray[i] : Vector3.Zero)
                    .WithMaterial(
                        hasBaseColor
                            ? new(
                                baseColorArray[i].r * 255,
                                baseColorArray[i].g * 255,
                                baseColorArray[i].b * 255,
                                baseColorArray[i].a * 255
                            )
                            : Vector4.UnitW,
                        hasDiffuseUvs ? diffuseUvsArray[i] : Vector2.Zero,
                        hasLightmapUvs ? lightmapUvsArray[i] : Vector2.Zero
                    );
            }

            return gltfVertices;
        }

        private static IMeshBuilder<MaterialBuilder> BuildMapGeometryMeshStatic(MapGeometryModel mesh)
        {
            VERTEX[] vertices = BuildMeshVertices(mesh);
            var meshBuilder = VERTEX.CreateCompatibleMesh(mesh.Name);

            foreach (MapGeometrySubmesh submesh in mesh.Submeshes)
            {
                ReadOnlySpan<ushort> indices = mesh.Indices.Span.Slice(submesh.StartIndex, submesh.IndexCount);

                MaterialBuilder material = new MaterialBuilder(submesh.Material).WithUnlitShader();
                var primitive = meshBuilder.UsePrimitive(material);

                for (int i = 0; i < indices.Length; i += 3)
                {
                    VERTEX v1 = vertices[indices[i + 0]];
                    VERTEX v2 = vertices[indices[i + 1]];
                    VERTEX v3 = vertices[indices[i + 2]];

                    primitive.AddTriangle(v1, v2, v3);
                }
            }

            return meshBuilder;
        }
    }
}
