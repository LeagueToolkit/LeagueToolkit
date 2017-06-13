using Fantome.League.Helpers.Structures;
using Fantome.League.IO.OBJ;
using Fantome.League.IO.SCB;
using Fantome.League.IO.SKN;
using Fantome.League.IO.WGEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Fantome.League.Converters
{
    public class MeshGeometryConverter
    {
        public static MeshGeometry3D ConvertOBJ(OBJFile obj)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            Int32Collection indices = new Int32Collection();
            foreach(OBJFace Face in obj.Faces)
            {
                indices.Add(Face.VertexIndices[0]);
                indices.Add(Face.VertexIndices[1]);
                indices.Add(Face.VertexIndices[2]);
            }

            Point3DCollection vertices = new Point3DCollection();
            foreach(Vector3 Vertex in obj.Vertices)
            {
                vertices.Add(new Point3D(Vertex.X, Vertex.Y, Vertex.Z));
            }

            PointCollection uv = new PointCollection();
            foreach(Vector2 UV in obj.UVs)
            {
                uv.Add(new Point(UV.X, UV.Y));
            }

            mesh.TextureCoordinates = uv;
            mesh.TriangleIndices = indices;
            mesh.Positions = vertices;

            return mesh;
        }
        
        public static IEnumerable<Tuple<string, MeshGeometry3D>> ConvertSKN(SKNFile skn)
        {
            foreach(SKNSubmesh Submesh in skn.Submeshes)
            {
                MeshGeometry3D mesh = new MeshGeometry3D();

                Int32Collection indices = new Int32Collection();
                for(int i = 0; i < Submesh.IndexCount; i++)
                {
                    indices.Add(skn.Indices[i + (int)Submesh.StartIndex] - (int)Submesh.StartIndex);
                }

                Point3DCollection vertices = new Point3DCollection();
                Vector3DCollection normals = new Vector3DCollection();
                PointCollection uv = new PointCollection();
                for(int i = 0; i < Submesh.VertexCount; i++)
                {
                    SKNVertex vertex = skn.Vertices[i + (int)Submesh.StartVertex];
                    vertices.Add(new Point3D(vertex.Position.X, vertex.Position.Y, vertex.Position.Z));
                    normals.Add(new Vector3D(vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z));
                    uv.Add(new Point(vertex.UV.X, vertex.UV.Y));
                }

                mesh.TextureCoordinates = uv;
                mesh.Positions = vertices;
                mesh.Normals = normals;
                mesh.TriangleIndices = indices;

                yield return new Tuple<string, MeshGeometry3D>(Submesh.Name, mesh);
            }
        }

        public static IEnumerable<Tuple<string, string, MeshGeometry3D>> ConvertWGEO(WGEOFile wgeo)
        {
            foreach(WGEOModel Model in wgeo.Models)
            {
                MeshGeometry3D mesh = new MeshGeometry3D();

                Int32Collection indices = new Int32Collection(Model.Indices.Cast<int>());
                Point3DCollection vertices = new Point3DCollection();
                PointCollection uv = new PointCollection();
                foreach(WGEOVertex Vertex in Model.Vertices)
                {
                    vertices.Add(new Point3D(Vertex.Position.X, Vertex.Position.Y, Vertex.Position.Z));
                    uv.Add(new Point(Vertex.UV.X, Vertex.UV.Y));
                }

                mesh.TextureCoordinates = uv;
                mesh.Positions = vertices;
                mesh.TriangleIndices = indices;

                yield return new Tuple<string, string, MeshGeometry3D>(Model.Material, Model.Texture, mesh);
            }
        }
    }
}
