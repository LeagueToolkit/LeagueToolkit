﻿using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.SimpleSkin;
using Fantome.Libraries.League.IO.WorldGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using Fantome.Libraries.League.IO.MapGeometry;

namespace Fantome.Libraries.League.Converters
{
    public static class OBJConverter
    {
        /// <summary>
        /// Converts the models of <paramref name="wgeo"/> into the <see cref="OBJFile"/> format
        /// </summary>
        /// <param name="wgeo">The <see cref="WGEOFile"/> to convert models from</param>
        /// <returns>Converted <see cref="WGEOModel"/> models in the <see cref="OBJFile"/> format</returns>
        public static IEnumerable<OBJFile> ConvertWGEOModels(WGEOFile wgeo)
        {
            foreach (WGEOModel model in wgeo.Models)
            {
                List<Vector3> vertices = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();
                foreach (WGEOVertex vertex in model.Vertices)
                {
                    vertices.Add(vertex.Position);
                    uvs.Add(vertex.UV);
                }
                yield return new OBJFile(vertices, model.Indices, uvs);
            }
        }

        public static IEnumerable<Tuple<string, OBJFile>> ConvertMGEOModels(MGEOFile mgeo)
        {
            foreach (MGEOModel model in mgeo.Models)
            {
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();

                foreach (MGEOVertex vertex in model.Vertices)
                {
                    vertices.Add(model.Transformation.ApplyTransformation(vertex.Position));
                    normals.Add(vertex.Normal);
                    if (vertex.DiffuseUV != null)
                    {
                        uvs.Add(vertex.DiffuseUV);
                    }
                }

                yield return new Tuple<string, OBJFile>(model.Name, new OBJFile(vertices, model.Indices.Select(x => (uint)x).ToList(), uvs, normals));
            }
        }

        //public static OBJFile VisualiseNVRNodes(NVRFile Nvr)
        //{
        //    List<UInt16> Indices = new List<UInt16>();
        //    List<UInt16> BaseIndices = new List<UInt16>()
        //    {
        //        0, 1, 2,
        //        0, 3, 2,

        //        4, 5, 6,
        //        4, 7, 6,

        //        6, 5, 1,
        //        6, 2, 1,

        //        0, 4, 7,
        //        0, 3, 7,

        //        2, 6, 7,
        //        2, 3, 7,

        //        0, 1, 5,
        //        0, 4, 5
        //    };
        //    List<Vector3> Vertices = new List<Vector3>();
        //    List<NVRNode> Nodes = Nvr.GetNodes();
        //    foreach (NVRNode Node in Nodes)
        //    {
        //        /* 0 Vector3 minLeftUp = Node.BoundingBox.Min;
        //           1 Vector3 minRightUp = new Vector3(Node.BoundingBox.Min.X, Node.BoundingBox.Min.Y, Node.BoundingBox.Max.Z);
        //           2 Vector3 minLeftDown = new Vector3(Node.BoundingBox.Max.X, Node.BoundingBox.Min.Y, Node.BoundingBox.Min.Z);
        //           3 Vector3 minRightDown = new Vector3(Node.BoundingBox.Max.X, Node.BoundingBox.Min.Y, Node.BoundingBox.Max.Z);

        //           6 Vector3 maxRightDown = Node.BoundingBox.Max;
        //           7 Vector3 maxLeftDown = new Vector3(Node.BoundingBox.Max.X, Node.BoundingBox.Max.Y, Node.BoundingBox.Min.Z);
        //           4 Vector3 maaxLeftUp = new Vector3(Node.BoundingBox.Min.X, Node.BoundingBox.Max.Y, Node.BoundingBox.Min.Z);
        //           5 Vector3 maxRightUp = new Vector3(Node.BoundingBox.Max.X, Node.BoundingBox.Min.Y, Node.BoundingBox.Max.Z);*/

        //        Vertices.AddRange(new Vector3[]
        //        {
        //            Node.BoundingBox.Min,
        //            new Vector3(Node.BoundingBox.Min.X, Node.BoundingBox.Min.Y, Node.BoundingBox.Max.Z),
        //            new Vector3(Node.BoundingBox.Max.X, Node.BoundingBox.Min.Y, Node.BoundingBox.Min.Z),
        //            new Vector3(Node.BoundingBox.Max.X, Node.BoundingBox.Min.Y, Node.BoundingBox.Max.Z),
        //            new Vector3(Node.BoundingBox.Min.X, Node.BoundingBox.Max.Y, Node.BoundingBox.Min.Z),
        //            new Vector3(Node.BoundingBox.Max.X, Node.BoundingBox.Min.Y, Node.BoundingBox.Max.Z),
        //            Node.BoundingBox.Max,
        //            new Vector3(Node.BoundingBox.Max.X, Node.BoundingBox.Max.Y, Node.BoundingBox.Min.Z),
        //    });
        //    }
        //    for (int i = 0; i * 8 < Vertices.Count; i++)
        //    {
        //        Indices.AddRange(BaseIndices);
        //        for (int j = 0; j < BaseIndices.Count; j++)
        //        {
        //            BaseIndices[j] += 8;
        //        }
        //    }
        //    return new OBJFile(Vertices, Indices);
        //}

        /// <summary>
        /// Converts <paramref name="model"/> to an <see cref="OBJFile"/>
        /// </summary>
        /// <param name="model">The <see cref="SKNFile"/> to convert to a <see cref="OBJFile"/></param>
        /// <returns>An <see cref="OBJFile"/> converted from <paramref name="model"/></returns>
        public static OBJFile ConvertSKN(SKNFile model)
        {
            List<uint> indices = new List<uint>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            foreach(SKNSubmesh submesh in model.Submeshes)
            {
                indices.AddRange(submesh.Indices.Cast<uint>());
                foreach (SKNVertex vertex in submesh.Vertices)
                {
                    vertices.Add(vertex.Position);
                    uv.Add(vertex.UV);
                    normals.Add(vertex.Normal);
                }
            }


            return new OBJFile(vertices, indices, uv, normals);
        }

        /// <summary>
        /// Converts the Submeshes of the specified <see cref="SKNFile"/> into a List of <see cref="OBJFile"/>
        /// </summary>
        /// <param name="model"><see cref="SKNFile"/> to convert</param>
        public static IEnumerable<Tuple<string, OBJFile>> ConvertSKNModels(SKNFile model)
        {
            foreach (SKNSubmesh submesh in model.Submeshes)
            {
                List<uint> indices = new List<uint>();
                List<Vector3> vertices = new List<Vector3>();
                List<Vector2> uv = new List<Vector2>();
                List<Vector3> normals = new List<Vector3>();
                indices.AddRange(submesh.Indices.Select(i => (uint)i));
                foreach (SKNVertex vertex in submesh.Vertices)
                {
                    vertices.Add(vertex.Position);
                    uv.Add(vertex.UV);
                    normals.Add(vertex.Normal);
                }

                yield return new Tuple<string, OBJFile>(submesh.Name, new OBJFile(vertices, indices, uv, normals));
            }
        }
    }
}
