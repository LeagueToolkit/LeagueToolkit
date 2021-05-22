using LeagueToolkit.IO.OBJ;
using System.Collections.Generic;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometry
{
    public static class MapGeometryOBJExtensions
    {
        public static (List<ushort>, List<MapGeometryVertex>) GetMGEOData(this OBJFile obj)
        {
            List<ushort> indices = new List<ushort>();
            List<MapGeometryVertex> vertices = new List<MapGeometryVertex>();

            foreach (Vector3 vertex in obj.Vertices)
            {
                vertices.Add(new MapGeometryVertex() { Position = vertex });
            }

            foreach (OBJFace face in obj.Faces)
            {
                for (int i = 0; i < 3; i++)
                {
                    indices.Add((ushort)face.VertexIndices[i]);
                }

                if (face.NormalIndices != null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        vertices[(int)face.VertexIndices[i]].Normal = obj.Normals[(int)face.NormalIndices[i]];
                    }
                }

                if (face.UVIndices!= null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        vertices[(int)face.VertexIndices[i]].DiffuseUV = obj.UVs[(int)face.UVIndices[i]];
                    }
                }
            }

            return (indices, vertices);
        }
    }
}
