using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.SCB;
using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.SKN;
using Fantome.Libraries.League.IO.WGT;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fantome.Libraries.League.Converters
{
    public static class SCOConverter
    {
        /// <summary>
        /// Converts <paramref name="scb"/> to an <see cref="SCOFile"/>
        /// </summary>
        /// <param name="scb">The <see cref="SCBFile"/> to convert to an <see cref="SCOFile"/></param>
        /// <returns>An <see cref="SCOFile"/> converted from <paramref name="scb"/></returns>
        public static SCOFile ConvertSCB(SCBFile scb)
        {
            List<ushort> indices = new List<ushort>();
            List<Vector2> uvs = new List<Vector2>();
            foreach (SCBFace face in scb.Faces)
            {
                indices.AddRange(face.Indices.AsEnumerable().Cast<ushort>());
                uvs.AddRange(face.UV);
            }
            return new SCOFile(indices, scb.Vertices, uvs);
        }

        /// <summary>
        /// Converts <paramref name="obj"/> to an <see cref="SCOFile"/>
        /// </summary>
        /// <param name="obj">The <see cref="OBJFile"/> to convert to an <see cref="SCOFile"/></param>
        /// <returns>An <see cref="SCOFile"/> converted from <paramref name="obj"/></returns>
        public static SCOFile ConvertOBJ(OBJFile obj)
        {
            List<UInt16> indices = new List<UInt16>();
            bool zeroPointIndex = false;
            foreach (OBJFace face in obj.Faces)
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
                foreach (OBJFace face in obj.Faces)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        face.VertexIndices[i] -= 1;
                        face.UVIndices[i] -= 1;
                    }
                }
            }
            foreach (OBJFace Face in obj.Faces)
            {
                indices.AddRange(Face.VertexIndices);
            }
            return new SCOFile(indices, obj.Vertices, obj.UVs);
        }

        /// <summary>
        /// Converts <paramref name="skn"/> to a <c>Tuple{SCOFile, WGTFile}</c>
        /// </summary>
        /// <param name="skn">The <see cref="SKNFile"/> to convert to a <c>Tuple{SCOFile, WGTFile}</c></param>
        /// <returns>A <c>Tuple{SCOFile, WGTFile}</c> converted from <paramref name="skn"/></returns>
        public static Tuple<SCOFile, WGTFile> ConvertToLegacy(SKNFile skn)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector4Byte> boneIndices = new List<Vector4Byte>();
            List<Vector4> weights = new List<Vector4>();

            foreach (SKNVertex vertex in skn.Vertices)
            {
                vertices.Add(vertex.Position);
                uvs.Add(vertex.UV);
                weights.Add(vertex.Weights);
                boneIndices.Add(vertex.BoneIndices);
            }
            return new Tuple<SCOFile, WGTFile>(new SCOFile(skn.Indices, vertices, uvs), new WGTFile(weights, boneIndices));
        }
    }
}
