using LeagueToolkit.IO.OBJ;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometryFile
{
    public static class MapGeometryOBJExtensions
    {
        public static (ushort[], List<MapGeometryVertex>) GetMGEOData(this OBJFile obj)
        {
            ushort[] indices = new ushort[obj.Groups.Sum(group => group.Faces.Count * 3)];
            List<MapGeometryVertex> vertices = new();

            foreach (Vector3 vertex in obj.Vertices)
            {
                vertices.Add(new() { Position = vertex });
            }

            int currentIndex = 0;
            foreach (OBJGroup group in obj.Groups)
            {
                foreach (OBJFace face in group.Faces)
                {
                    indices[currentIndex++] = (ushort)face.VertexIndices[0];
                    indices[currentIndex++] = (ushort)face.VertexIndices[1];
                    indices[currentIndex++] = (ushort)face.VertexIndices[2];

                    if (face.NormalIndices != null)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            MapGeometryVertex vertex = vertices[(int)face.VertexIndices[i]];

                            vertex.Normal = obj.Normals[(int)face.NormalIndices[i]];

                            vertices[(int)face.VertexIndices[i]] = vertex;
                        }
                    }

                    if (face.UVIndices != null)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            MapGeometryVertex vertex = vertices[(int)face.VertexIndices[i]];

                            vertex.DiffuseUV = obj.UVs[(int)face.UVIndices[i]];

                            vertices[(int)face.VertexIndices[i]] = vertex;
                        }
                    }
                }
            }

            return (indices, vertices);
        }
    }
}
