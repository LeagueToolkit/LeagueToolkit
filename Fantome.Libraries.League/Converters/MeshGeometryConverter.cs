using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.SimpleSkin;
using Fantome.Libraries.League.IO.WorldGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Fantome.Libraries.League.Converters
{
    public class MeshGeometryConverter
    {
        /// <summary>
        /// Converts <paramref name="obj"/> to a <see cref="MeshGeometry3D"/>
        /// </summary>
        /// <param name="obj">The <see cref="OBJFile"/> to convert to a <see cref="MeshGeometry3D"/></param>
        /// <returns>A <see cref="MeshGeometry3D"/> converted from <paramref name="obj"/></returns>
        /// <remarks>Normals do not get converted</remarks>
        public static MeshGeometry3D ConvertOBJ(OBJFile obj)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            Int32Collection indices = new Int32Collection();
            foreach (OBJFace face in obj.Faces)
            {
                indices.Add((int)face.VertexIndices[0]);
                indices.Add((int)face.VertexIndices[1]);
                indices.Add((int)face.VertexIndices[2]);
            }

            Point3DCollection vertices = new Point3DCollection();
            foreach (Vector3 vertex in obj.Vertices)
            {
                vertices.Add(new Point3D(vertex.X, vertex.Y, vertex.Z));
            }

            PointCollection uvs = new PointCollection();
            foreach (Vector2 uv in obj.UVs)
            {
                uvs.Add(new Point(uv.X, uv.Y));
            }

            mesh.TextureCoordinates = uvs;
            mesh.TriangleIndices = indices;
            mesh.Positions = vertices;

            return mesh;
        }

        /// <summary>
        /// Converts <paramref name="skn"/> to a list of <see cref="MeshGeometry3D"/>
        /// </summary>
        /// <param name="skn">The <see cref="SKNFile"/> which should get converted to a <c>Tuple{string, MeshGeometry3D}(submeshName, submeshData)</c></param>
        /// <returns>A collection of converted <see cref="SKNSubmesh"/></returns>
        /// <remarks>Normals do not get converted</remarks>
        public static IEnumerable<Tuple<string, MeshGeometry3D>> ConvertSKN(SKNFile skn)
        {
            foreach (SKNSubmesh submesh in skn.Submeshes)
            {
                MeshGeometry3D mesh = new MeshGeometry3D();

                Int32Collection indices = new Int32Collection(submesh.Indices.Select(x => (int)x));
                Point3DCollection vertices = new Point3DCollection();
                Vector3DCollection normals = new Vector3DCollection();
                PointCollection uvs = new PointCollection();
                for (int i = 0; i < submesh.Vertices.Count; i++)
                {
                    SKNVertex vertex = submesh.Vertices[i];
                    vertices.Add(new Point3D(vertex.Position.X, vertex.Position.Y, vertex.Position.Z));
                    normals.Add(new Vector3D(vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z));
                    uvs.Add(new Point(vertex.UV.X, vertex.UV.Y));
                }

                mesh.TextureCoordinates = uvs;
                mesh.Positions = vertices;
                mesh.Normals = normals;
                mesh.TriangleIndices = indices;

                yield return new Tuple<string, MeshGeometry3D>(submesh.Name, mesh);
            }
        }

        /// <summary>
        /// Converts <paramref name="wgeo"/> to a list of <see cref="MeshGeometry3D"/> 
        /// </summary>
        /// <param name="wgeo">The <see cref="WGEOFile"/> to convert to a <c>Tuple{string, string, MeshGeometry3D}(materialName, textureName, modelData)</c></param>
        /// <returns>A collection of converted <see cref="WGEOModel"/></returns>
        public static IEnumerable<Tuple<string, string, MeshGeometry3D>> ConvertWGEO(WGEOFile wgeo)
        {
            foreach (WGEOModel model in wgeo.Models)
            {
                MeshGeometry3D mesh = new MeshGeometry3D();

                Int32Collection indices = new Int32Collection(model.Indices.Select(x => (int)x));
                Point3DCollection vertices = new Point3DCollection();
                PointCollection uv = new PointCollection();
                foreach (WGEOVertex vertex in model.Vertices)
                {
                    vertices.Add(new Point3D(vertex.Position.X, vertex.Position.Y, vertex.Position.Z));
                    uv.Add(new Point(vertex.UV.X, vertex.UV.Y));
                }

                mesh.TextureCoordinates = uv;
                mesh.Positions = vertices;
                mesh.TriangleIndices = indices;

                yield return new Tuple<string, string, MeshGeometry3D>(model.Material, model.Texture, mesh);
            }
        }
    }
}
