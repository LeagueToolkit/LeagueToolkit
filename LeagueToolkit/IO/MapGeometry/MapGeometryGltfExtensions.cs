using LeagueToolkit.Helpers.Structures;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
using System;
using System.Collections.Generic;
using System.Numerics;

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
            var layerModelMap = new Dictionary<MapGeometryLayer, List<MapGeometryModel>>();
            foreach(MapGeometryModel model in mgeo.Models)
            {
                if(!layerModelMap.ContainsKey(model.Layer))
                {
                    layerModelMap.Add(model.Layer, new List<MapGeometryModel>());
                }

                layerModelMap[model.Layer].Add(model);
            }

            // Create node for each layer combination
            var layerNodeMap = new Dictionary<MapGeometryLayer, Node>();
            foreach (var layerModelPair in layerModelMap)
            {
                layerNodeMap.Add(layerModelPair.Key, rootNode.CreateNode(DeriveLayerCombinationName(layerModelPair.Key)));
            }

            foreach (MapGeometryModel model in mgeo.Models)
            {
                IMeshBuilder<MaterialBuilder> meshBuilder = BuildMapGeometryMeshStatic(model);

                layerNodeMap[model.Layer]
                    .CreateNode()
                    .WithMesh(root.CreateMesh(meshBuilder))
                    .WithLocalTransform(new AffineTransform(model.Transformation));
            }

            return root;
        }

        private static string DeriveLayerCombinationName(MapGeometryLayer layerCombination)
        {
            if(layerCombination == MapGeometryLayer.NoLayer)
            {
                return "NoLayer";
            }
            else if(layerCombination == MapGeometryLayer.AllLayers)
            {
                return "AllLayers";
            }
            else
            {
                string name = "Layer-";

                foreach (MapGeometryLayer layerFlag in Enum.GetValues(typeof(MapGeometryLayer)))
                {
                    if (layerCombination.HasFlag(layerFlag) && 
                        layerFlag != MapGeometryLayer.AllLayers && 
                        layerFlag != MapGeometryLayer.NoLayer)
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
            var meshBuilder = VERTEX.CreateCompatibleMesh();

            foreach (MapGeometrySubmesh submesh in model.Submeshes)
            {
                List<MapGeometryVertex> vertices = submesh.GetVertices();
                List<ushort> indices = submesh.GetIndices();

                MaterialBuilder material = new MaterialBuilder(submesh.Material).WithUnlitShader();
                var primitive = meshBuilder.UsePrimitive(material);

                List<VERTEX> gltfVertices = new List<VERTEX>();
                foreach (MapGeometryVertex vertex in vertices)
                {
                    gltfVertices.Add(CreateVertex(vertex));
                }

                for (int i = 0; i < indices.Count; i += 3)
                {
                    VERTEX v1 = gltfVertices[indices[i + 0]];
                    VERTEX v2 = gltfVertices[indices[i + 1]];
                    VERTEX v3 = gltfVertices[indices[i + 2]];

                    primitive.AddTriangle(v1, v2, v3);
                }
            }

            return meshBuilder;
        }
        private static VERTEX CreateVertex(MapGeometryVertex vertex)
        {
            VERTEX gltfVertex = new VERTEX();

            Vector3 position = vertex.Position.Value;
            Vector3 normal = vertex.Normal.HasValue ? vertex.Normal.Value : Vector3.Zero;
            Color color1 = vertex.SecondaryColor.HasValue ? vertex.SecondaryColor.Value : new Color(0, 0, 0, 1);
            Vector2 uv1 = vertex.DiffuseUV.HasValue ? vertex.DiffuseUV.Value : Vector2.Zero;
            Vector2 uv2 = vertex.LightmapUV.HasValue ? vertex.LightmapUV.Value : Vector2.Zero;

            return gltfVertex
                .WithGeometry(position, normal)
                .WithMaterial(color1, uv1, uv2);
        }
    }
}
