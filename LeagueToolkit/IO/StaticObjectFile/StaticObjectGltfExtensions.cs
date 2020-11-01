using LeagueToolkit.Helpers.Structures;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using System.Collections.Generic;

namespace LeagueToolkit.IO.StaticObjectFile
{
    using VERTEX = VertexBuilder<VertexPosition, VertexTexture1, VertexEmpty>;

    public static class StaticObjectGltfExtensions
    {
        public static ModelRoot ToGltf(this StaticObject staticObject)
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("default");

            var mesh = VERTEX.CreateCompatibleMesh();

            foreach (StaticObjectSubmesh submesh in staticObject.Submeshes)
            {
                MaterialBuilder material = new MaterialBuilder(submesh.Name);
                var primitive = mesh.UsePrimitive(material);

                List<VERTEX> vertices = new List<VERTEX>();
                foreach (StaticObjectVertex vertex in submesh.Vertices)
                {
                    vertices.Add(new VERTEX()
                        .WithGeometry(vertex.Position)
                        .WithMaterial(vertex.UV));
                }

                for (int i = 0; i < submesh.Indices.Count; i += 3)
                {
                    VERTEX v1 = vertices[(int)submesh.Indices[i + 0]];
                    VERTEX v2 = vertices[(int)submesh.Indices[i + 1]];
                    VERTEX v3 = vertices[(int)submesh.Indices[i + 2]];

                    primitive.AddTriangle(v1, v2, v3);
                }
            }

            scene
                .CreateNode()
                .WithMesh(root.CreateMesh(mesh));

            return root;
        }
    }
}
