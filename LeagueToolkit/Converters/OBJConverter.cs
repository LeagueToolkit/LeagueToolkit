using LeagueToolkit.IO.OBJ;
using LeagueToolkit.IO.SimpleSkinFile;
using LeagueToolkit.IO.WorldGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using LeagueToolkit.IO.MapGeometry;
using System.Numerics;
using LeagueToolkit.IO.NVR;

namespace LeagueToolkit.Converters
{
    public static class OBJConverter
    {
        /// <summary>
        /// Converts the models of <paramref name="wgeo"/> into the <see cref="OBJFile"/> format
        /// </summary>
        /// <param name="wgeo">The <see cref="WorldGeometry"/> to convert models from</param>
        /// <returns>Converted <see cref="WorldGeometryModel"/> models in the <see cref="OBJFile"/> format</returns>
        public static IEnumerable<OBJFile> ConvertWGEOModels(WorldGeometry wgeo)
        {
            foreach (WorldGeometryModel model in wgeo.Models)
            {
                List<Vector3> vertices = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();
                foreach (WorldGeometryVertex vertex in model.Vertices)
                {
                    vertices.Add(vertex.Position);
                    uvs.Add(vertex.UV);
                }
                yield return new OBJFile(vertices, model.Indices, uvs);
            }
        }

        public static IEnumerable<Tuple<string, OBJFile>> ConvertMGEOModels(MapGeometry mgeo)
        {
            foreach (MapGeometryModel model in mgeo.Models)
            {
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();

                foreach (MapGeometryVertex vertex in model.Vertices)
                {
                    vertices.Add(model.Transformation.ApplyTransformation(vertex.Position.Value));
                    normals.Add(vertex.Normal.Value);
                    if (vertex.DiffuseUV != null)
                    {
                        uvs.Add(vertex.DiffuseUV.Value);
                    }
                }

                yield return new Tuple<string, OBJFile>(model.Name, new OBJFile(vertices, model.Indices.Select(x => (uint)x).ToList(), uvs, normals));
            }
        }

        /// <summary>
        /// Converts the meshes of <paramref name="nvr"/> into the <see cref="OBJFile"/> format.
        /// </summary>
        /// <param name="nvr">The <see cref="NVRFile"/> to convert meshes from</param>
        /// <param name="simple">Converts simple primitives which do not contain UVs or Normals.</param>
        /// <param name="doSet">Converted <see cref="OBJFile"/>(s) are grouped into a single <see cref="OBJFile"/> by material.</param>
        /// <returns>Converted <see cref="NVRMesh"/> meshes in the <see cref="OBJFile"/> format</returns>
        public static IEnumerable<OBJFile> ConvertNVRMeshes(NVRFile nvr, bool simple = false, bool doSet = false)
        {
            foreach (NVRMaterial material in nvr.Materials)
            {
                int materialIndex = nvr.Materials.IndexOf(material);
                List<OBJFile> objSet = new List<OBJFile>();

                foreach (NVRMesh mesh in nvr.Meshes)
                {
                    int meshMaterialIndex = nvr.Materials.IndexOf(mesh.Material);

                    if (meshMaterialIndex == materialIndex)
                    {
                        continue;
                    }

                    List<Vector3> vertices = new List<Vector3>();
                    List<uint> indices = new List<uint>();
                    List<Vector2> uvs = new List<Vector2>();
                    List<Vector3> normals = new List<Vector3>();

                    NVRDrawIndexedPrimitive primitive = simple ? mesh.IndexedPrimitives[1] : mesh.IndexedPrimitives[0];

                    indices.AddRange(primitive.Indices.Select(i => (uint)i));

                    foreach (var vertex in primitive.Vertices)
                    {
                        vertices.Add(vertex.Position);

                        if (primitive.VertexType == NVRVertexType.NVRVERTEX_4)
                        {
                            NVRVertex4 vertex4 = vertex as NVRVertex4;
                            uvs.Add(vertex4.UV);
                            normals.Add(vertex4.Normal);
                        }
                        else if (primitive.VertexType == NVRVertexType.NVRVERTEX_8)
                        {
                            NVRVertex8 vertex8 = vertex as NVRVertex8;
                            uvs.Add(vertex8.UV);
                            normals.Add(vertex8.Normal);
                        }
                        else if (primitive.VertexType == NVRVertexType.NVRVERTEX_12)
                        {
                            NVRVertex12 vertex12 = vertex as NVRVertex12;
                            uvs.Add(vertex12.UV);
                            normals.Add(vertex12.Normal);
                        }
                    }

                    var obj = new OBJFile(vertices, indices);
                    if (simple)
                    {
                        if (!doSet)
                        {
                            yield return obj;
                        }

                        objSet.Add(obj);
                        continue;
                    }

                    obj = new OBJFile(vertices, indices, uvs, normals);

                    if (!doSet)
                    {
                        yield return obj;
                    }

                    objSet.Add(obj);
                }

                yield return new OBJFile(objSet);
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
        /// <param name="model">The <see cref="SimpleSkin"/> to convert to a <see cref="OBJFile"/></param>
        /// <returns>An <see cref="OBJFile"/> converted from <paramref name="model"/></returns>
        public static OBJFile ConvertSKN(SimpleSkin model)
        {
            List<uint> indices = new List<uint>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            uint indexOffset = 0;
            foreach(SimpleSkinSubmesh submesh in model.Submeshes)
            {
                indices.AddRange(submesh.Indices.Select(x => x + indexOffset));
                foreach (SimpleSkinVertex vertex in submesh.Vertices)
                {
                    vertices.Add(vertex.Position);
                    uv.Add(vertex.UV);
                    normals.Add(vertex.Normal);
                }

                indexOffset += submesh.Indices.Min();
            }


            return new OBJFile(vertices, indices, uv, normals);
        }

        /// <summary>
        /// Converts the Submeshes of the specified <see cref="SimpleSkin"/> into a List of <see cref="OBJFile"/>
        /// </summary>
        /// <param name="model"><see cref="SimpleSkin"/> to convert</param>
        public static IEnumerable<Tuple<string, OBJFile>> ConvertSKNModels(SimpleSkin model)
        {
            foreach (SimpleSkinSubmesh submesh in model.Submeshes)
            {
                List<uint> indices = new List<uint>();
                List<Vector3> vertices = new List<Vector3>();
                List<Vector2> uv = new List<Vector2>();
                List<Vector3> normals = new List<Vector3>();
                indices.AddRange(submesh.Indices.Select(i => (uint)i));
                foreach (SimpleSkinVertex vertex in submesh.Vertices)
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
