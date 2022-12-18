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

namespace LeagueToolkit.IO.MapGeometry
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

        private static IMeshBuilder<MaterialBuilder> BuildMapGeometryMeshStatic(MapGeometryModel model)
        {
            var meshBuilder = VERTEX.CreateCompatibleMesh(model.Name);

            foreach (MapGeometrySubmesh submesh in model.Submeshes)
            {
                ReadOnlySpan<ushort> indices = model.Indices.Slice(submesh.StartIndex, submesh.IndexCount);

                MaterialBuilder material = new MaterialBuilder(submesh.Material).WithUnlitShader();
                var primitive = meshBuilder.UsePrimitive(material);

                VERTEX[] gltfVertices = new VERTEX[submesh.VertexCount];
                for (int i = 0; i < submesh.VertexCount; i++)
                {
                    gltfVertices[i] = CreateVertex(model.Vertices[i + submesh.MinVertex]);
                }

                for (int i = 0; i < indices.Length; i += 3)
                {
                    VERTEX v1 = gltfVertices[indices[i + 0] - submesh.MinVertex];
                    VERTEX v2 = gltfVertices[indices[i + 1] - submesh.MinVertex];
                    VERTEX v3 = gltfVertices[indices[i + 2] - submesh.MinVertex];

                    primitive.AddTriangle(v1, v2, v3);
                }
            }

            return meshBuilder;
        }

        private static VERTEX CreateVertex(MapGeometryVertex vertex)
        {
            VERTEX gltfVertex = new();

            Vector3 position = vertex.Position.Value;
            Vector3 normal = vertex.Normal ?? Vector3.Zero;
            Color color1 = vertex.SecondaryColor ?? new Color(0, 0, 0, 1);
            Vector2 uv1 = vertex.DiffuseUV ?? Vector2.Zero;
            Vector2 uv2 = vertex.LightmapUV ?? Vector2.Zero;

            return gltfVertex.WithGeometry(position, normal).WithMaterial(color1, uv1, uv2);
        }
    }
}
