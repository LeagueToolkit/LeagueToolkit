using LeagueToolkit.IO.OBJ;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometry
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
                            vertices[(int)face.VertexIndices[i]].Normal = obj.Normals[(int)face.NormalIndices[i]];
                        }
                    }

                    if (face.UVIndices != null)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            vertices[(int)face.VertexIndices[i]].DiffuseUV = obj.UVs[(int)face.UVIndices[i]];
                        }
                    }
                }
            }

            return (indices, vertices);
        }
    }
}
