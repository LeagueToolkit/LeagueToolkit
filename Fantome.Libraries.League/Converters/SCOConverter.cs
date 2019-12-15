using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.SCB;
using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.SimpleSkin;
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
            List<uint> indices = new List<uint>();
            List<Vector2> uvs = new List<Vector2>();

            foreach (KeyValuePair<string, List<SCBFace>> material in scb.Materials)
            {
                foreach (SCBFace face in material.Value)
                {
                    indices.AddRange(face.Indices);
                    uvs.AddRange(face.UVs);
                }
            }
            return new SCOFile(scb.Vertices, indices, uvs);
        }

        /// <summary>
        /// Converts <paramref name="obj"/> to an <see cref="SCOFile"/>
        /// </summary>
        /// <param name="obj">The <see cref="OBJFile"/> to convert to an <see cref="SCOFile"/></param>
        /// <returns>An <see cref="SCOFile"/> converted from <paramref name="obj"/></returns>
        public static SCOFile ConvertOBJ(OBJFile obj)
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
                indices.AddRange(face.VertexIndices);
            }
            return new SCOFile(obj.Vertices, indices, obj.UVs);
        }

        /// <summary>
        /// Converts <paramref name="skn"/> to a <c>Tuple{SCOFile, WGTFile}</c>
        /// </summary>
        /// <param name="skn">The <see cref="SKNFile"/> to convert to a <c>Tuple{SCOFile, WGTFile}</c></param>
        /// <returns>A <c>Tuple{SCOFile, WGTFile}</c> converted from <paramref name="skn"/></returns>
        public static Tuple<SCOFile, WGTFile> ConvertToLegacy(SKNFile skn)
        {
            List<Vector3> vertices = new List<Vector3>();
            Dictionary<string, List<SCOFace>> materials = new Dictionary<string, List<SCOFace>>();
            List<Vector4Byte> boneIndices = new List<Vector4Byte>();
            List<Vector4> weights = new List<Vector4>();

            foreach (SKNSubmesh submesh in skn.Submeshes)
            {
                List<SCOFace> faces = new List<SCOFace>();
                for (int i = 0; i < submesh.Indices.Count; i += 3)
                {
                    faces.Add(new SCOFace(new uint[] { submesh.Indices[i], submesh.Indices[i + 1], submesh.Indices[i + 2], }, submesh.Name,
                        new Vector2[]
                        {
                            submesh.Vertices[submesh.Indices[i]].UV,
                            submesh.Vertices[submesh.Indices[i + 1]].UV,
                            submesh.Vertices[submesh.Indices[i + 2]].UV,
                        }));
                }
                materials.Add(submesh.Name, faces);

                foreach (SKNVertex vertex in submesh.Vertices)
                {
                    vertices.Add(vertex.Position);
                    weights.Add(vertex.Weights);
                    boneIndices.Add(vertex.BoneIndices);
                }
            }
            return new Tuple<SCOFile, WGTFile>(new SCOFile(vertices, materials), new WGTFile(weights, boneIndices));
        }
    }
}
