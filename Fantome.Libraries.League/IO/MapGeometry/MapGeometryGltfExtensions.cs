using Fantome.Libraries.League.Helpers.Structures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    using VERTEX = VertexBuilder<VertexPositionNormal, VertexColor1Texture2, VertexEmpty>;

    public static class MapGeometryGltfExtensions
    {
        public static ModelRoot ToGLTF(this MapGeometry mgeo)
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("Map");
            Node rootNode = scene.CreateNode();

            //Create a node for each layer
            Node[] layerNodes = new Node[8];
            for (int i = 0; i < 8; i++)
            {
                layerNodes[i] = rootNode.CreateNode("Layer" + (i + 1));
            }

            foreach (MapGeometryModel model in mgeo.Models)
            {
                IMeshBuilder<MaterialBuilder> meshBuilder = BuildMapGeometryMeshStatic(model);

                rootNode.CreateNode().WithMesh(root.CreateMesh(meshBuilder));
            }

            return root;
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

    public class MapGeometryGltfConversionOptions
    {

    }
}
