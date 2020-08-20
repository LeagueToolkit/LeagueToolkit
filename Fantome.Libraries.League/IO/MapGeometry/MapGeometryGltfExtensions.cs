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
using System.Text;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public static class MapGeometryGltfExtensions
    {
        public static ModelRoot ToGLTF(this MapGeometry mgeo)
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("Map");
            Node rootNode = scene.CreateNode();

            //Create a node for each layer
            Node[] layerNodes = new Node[8];
            for(int i = 0; i < 8; i++)
            {
                layerNodes[i] = rootNode.CreateNode("Layer" + (i + 1));
            }
            
            foreach(MapGeometryModel model in mgeo.Models)
            {
                IMeshBuilder<MaterialBuilder> meshBuilder = BuildMapGeometryMesh(model);

                rootNode.WithMesh(root.CreateMesh(meshBuilder));
            }

            return root;
        }

        // WARNING: SCARY CODE BELOW
        private static IMeshBuilder<MaterialBuilder> BuildMapGeometryMesh(MapGeometryModel model)
        {
            Type vertexBuilderType = GetModelVertexType(model);
            
            // Create MeshBuilder from the VertexBuilder type we got
            MethodInfo createCompatibleMeshMethod = vertexBuilderType.GetMethod("CreateCompatibleMesh", 0, new[] { typeof(string) });
            object meshBuilder = createCompatibleMeshMethod.Invoke(null, new object[] { null });
            
            MethodInfo usePrimitiveMethod = meshBuilder.GetType().GetMethod("UsePrimitive");

            foreach(MapGeometrySubmesh submesh in model.Submeshes)
            {
                List<ushort> indices = submesh.GetIndices();
                List<MapGeometryVertex> vertices = submesh.GetVertices();

                MaterialBuilder material = new MaterialBuilder().WithUnlitShader();

                object primitive = usePrimitiveMethod.Invoke(meshBuilder, new object[] { material, 3 });

                if (model.BakedPaintColor != Color.Zero)
                {
                    material.WithBaseColor(model.BakedPaintColor);
                }

                // Build vertices
                IList gltfVertices = CreateGenericVertexList(vertexBuilderType) as IList;
                foreach(MapGeometryVertex vertex in vertices)
                {
                    gltfVertices.Add(CreateGenericVertex(vertexBuilderType, vertex));
                }

                // Create geometry
                MethodInfo addTriangleMethod = primitive
                    .GetType()
                    .GetMethod("AddTriangle", new[] { typeof(IVertexBuilder), typeof(IVertexBuilder), typeof(IVertexBuilder) });
                for(int i = 0; i < indices.Count; i += 3)
                {
                    IVertexBuilder v1 = gltfVertices[indices[i + 0]] as IVertexBuilder;
                    IVertexBuilder v2 = gltfVertices[indices[i + 1]] as IVertexBuilder;
                    IVertexBuilder v3 = gltfVertices[indices[i + 2]] as IVertexBuilder;

                    addTriangleMethod.Invoke(primitive, new[] { v1, v2, v3 });
                }

            }



            return meshBuilder as IMeshBuilder<MaterialBuilder>;
        }

        // WARNING: SCARY CODE BELOW
        private static Type GetModelVertexType(MapGeometryModel model)
        {
            // This is a total mess but oh well
            MapGeometryVertex vertex = model.Vertices.First();

            // Get the type of the VertexBuilder so we can dynamically set its generic vertex components
            Type vertexBuilderType = typeof(VertexBuilder<,,>);

            // These are the generic arguments we will pass to construct the VertexBuilder
            Type[] vertexBuilderComponents = new Type[3];

            // ------ DETERMINE VERTEX COMPONENT TYPES ------- \\
            // Vertices must have a position component
            if (!vertex.Position.HasValue)
            {
                throw new ArgumentException("the model vertices have no position, this shouldn't happen", nameof(model));
            }

            vertexBuilderComponents[0] = GetGeometryComponent();
            vertexBuilderComponents[1] = GetMaterialComponent();
            vertexBuilderComponents[2] = typeof(VertexEmpty);

            Type vertexBuilderActualType = vertexBuilderType.MakeGenericType(vertexBuilderComponents);

            return vertexBuilderActualType;

            Type GetGeometryComponent()
            {
                // [X] Position, [ ] Normal
                if (vertex.Position.HasValue && !vertex.Normal.HasValue)
                {
                    return typeof(VertexPosition);
                }
                // [X] Position, [X] Normal
                else if (vertex.Position.HasValue && vertex.Normal.HasValue)
                {
                    return typeof(VertexPositionNormal);
                }
                else
                {
                    throw new Exception("Vertex does not have a Position component");
                }
            }

            Type GetMaterialComponent()
            {
                // [X] Diffuse UV, [ ] Lightmap UV, [ ] Secondary Color
                if (vertex.DiffuseUV.HasValue && !vertex.LightmapUV.HasValue && !vertex.SecondaryColor.HasValue)
                {
                    return typeof(VertexTexture1);
                }
                // [X] Diffuse UV, [X] Lightmap UV, [ ] Secondary Color
                else if (vertex.DiffuseUV.HasValue && vertex.LightmapUV.HasValue && !vertex.SecondaryColor.HasValue)
                {
                    return typeof(VertexTexture2);
                }
                // [X] Diffuse UV, [X] Lightmap UV, [X] Secondary Color
                else if (vertex.DiffuseUV.HasValue && vertex.LightmapUV.HasValue && vertex.SecondaryColor.HasValue)
                {
                    return typeof(VertexColor1Texture2);
                }
                // [ ] Diffuse UV, [X] Lightmap UV, [ ] Secondary Color
                else if (!vertex.DiffuseUV.HasValue && vertex.LightmapUV.HasValue && !vertex.SecondaryColor.HasValue)
                {
                    return typeof(VertexTexture2);
                }
                // [ ] Diffuse UV, [X] Lightmap UV, [X] Secondary Color
                else if (!vertex.DiffuseUV.HasValue && vertex.LightmapUV.HasValue && vertex.SecondaryColor.HasValue)
                {
                    return typeof(VertexTexture2);
                }
                // [ ] Diffuse UV, [ ] Lightmap UV, [X] Secondary Color
                else if (!vertex.DiffuseUV.HasValue && !vertex.LightmapUV.HasValue && vertex.SecondaryColor.HasValue)
                {
                    return typeof(VertexColor1);
                }

                else
                {
                    return typeof(VertexEmpty);
                }
            }
        }

        private static object CreateGenericVertexList(Type vertexBuilderType)
        {
            Type listType = typeof(List<>);
            Type vertexListType = listType.MakeGenericType(vertexBuilderType);

            return Activator.CreateInstance(vertexListType);
        }
        private static object CreateGenericVertex(Type vertexBuilderType, MapGeometryVertex vertex)
        {
            IVertexBuilder gltfVertex = Activator.CreateInstance(vertexBuilderType) as IVertexBuilder;

            gltfVertex.SetGeometry(new VertexPositionNormal(vertex.Position.Value, Vector3.Zero));

            return gltfVertex;
        }
    }

    public class MapGeometryGltfConversionOptions
    {

    }
}
