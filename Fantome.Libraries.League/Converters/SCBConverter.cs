using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.SCB;
using Fantome.Libraries.League.IO.SCO;
using System.Collections.Generic;
using System.Linq;

namespace Fantome.Libraries.League.Converters
{
    public static class SCBConverter
    {
        /// <summary>
        /// Converts <paramref name="sco"/> to an <see cref="SCBFile"/>
        /// </summary>
        /// <param name="sco">The <see cref="SCOFile"/> to convert to an <see cref="SCBFile"/></param>
        /// <returns>An <see cref="SCBFile"/> converted from <paramref name="sco"/></returns>
        public static SCBFile ConvertSCO(SCOFile sco)
        {
            List<uint> indices = new List<uint>();
            List<Vector2> uvs = new List<Vector2>();

            foreach (KeyValuePair<string, List<SCOFace>> material in sco.Materials)
            {
                foreach (SCOFace face in material.Value)
                {
                    indices.AddRange(face.Indices);
                    uvs.AddRange(face.UVs);
                }
            }
            return new SCBFile(sco.Vertices, indices, uvs);
        }

        /// <summary>
        /// Converts <paramref name="obj"/> to an <see cref="SCBFile"/>
        /// </summary>
        /// <param name="obj">The <see cref="OBJFile"/> to convert to an <see cref="SCBFile"/></param>
        /// <returns>An <see cref="SCBFile"/> converted from <paramref name="obj"/></returns>
        public static SCBFile ConvertOBJ(OBJFile obj)
        {
            List<Vector3> vertices = obj.Vertices;
            List<OBJFace> faces = obj.Faces;
            List<uint> indices = new List<uint>();
            bool zeroPointIndex = false;

            foreach (OBJFace face in faces)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (face.VertexIndices[i] == 0)
                    {
                        zeroPointIndex = true;
                        break;
                    }
                }
                if (zeroPointIndex)
                {
                    break;
                }
            }
            if (!zeroPointIndex)
            {
                foreach (OBJFace face in faces)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        face.VertexIndices[i] -= 1;
                        face.UVIndices[i] -= 1;
                    }
                }
            }
            foreach (OBJFace face in faces)
            {
                indices.AddRange(face.VertexIndices.Cast<uint>());
            }
            return new SCBFile(obj.Vertices, indices, obj.UVs);
        }
    }
}
