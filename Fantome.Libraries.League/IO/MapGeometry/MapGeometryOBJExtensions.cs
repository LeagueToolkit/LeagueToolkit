﻿using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using System.Collections.Generic;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public static class MapGeometryOBJExtensions
    {
        public static (List<ushort>, List<MGEOVertex>) GetMGEOData(this OBJFile obj)
        {
            List<ushort> indices = new List<ushort>();
            List<MGEOVertex> vertices = new List<MGEOVertex>();

            foreach (Vector3 vertex in obj.Vertices)
            {
                vertices.Add(new MGEOVertex() { Position = vertex });
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
