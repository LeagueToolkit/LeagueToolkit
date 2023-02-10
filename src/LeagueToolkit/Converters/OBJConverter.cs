﻿using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.IO.NVR;
using LeagueToolkit.IO.OBJ;
using System.Numerics;

namespace LeagueToolkit.Converters
{
    public static class OBJConverter
    {
        public static IEnumerable<Tuple<string, OBJFile>> ConvertMGEOModels(EnvironmentAsset mgeo)
        {
            foreach (EnvironmentAssetMesh mesh in mgeo.Meshes)
            {
                List<Vector3> vertices = new();
                List<Vector3> normals = new();
                List<Vector2> uvs = new();

                bool hasPositions = mesh.VerticesView.TryGetAccessor(ElementName.Position, out var positionAccessor);
                bool hasNormals = mesh.VerticesView.TryGetAccessor(ElementName.Normal, out var normalAccessor);
                bool hasDiffuseUvs = mesh.VerticesView.TryGetAccessor(ElementName.DiffuseUV, out var diffuseUvAccessor);

                if (hasPositions is false)
                    ThrowHelper.ThrowInvalidOperationException($"Mesh: {mesh.Name} does not have vertex positions");

                VertexElementArray<Vector3> positionsArray = positionAccessor.AsVector3Array();
                VertexElementArray<Vector3> normalsArray = hasNormals ? normalAccessor.AsVector3Array() : default;
                VertexElementArray<Vector2> diffuseUvsArray = hasDiffuseUvs
                    ? diffuseUvAccessor.AsVector2Array()
                    : default;

                for (int i = 0; i < mesh.VerticesView.VertexCount; i++)
                {
                    vertices.Add(Vector3.Transform(positionsArray[i], mesh.Transform));

                    if (hasNormals)
                        normals.Add(normalsArray[i]);
                    if (hasDiffuseUvs)
                        uvs.Add(diffuseUvsArray[i]);
                }

                // TODO: Rework OBJ API
                yield return new Tuple<string, OBJFile>(
                    mesh.Name,
                    new OBJFile(vertices, mesh.Indices.ToList(), uvs, normals)
                );
            }
        }

        /// <summary>
        /// Converts the meshes of <paramref name="nvr"/> into the <see cref="OBJFile"/> format.
        /// </summary>
        /// <param name="nvr">The <see cref="NVRFile"/> to convert meshes from</param>
        /// <param name="materialFile">The name of the material file that the <see cref="OBJFile"/> will use for materials.</param>
        /// <param name="simple">Converts simple primitives which do not contain UVs or Normals.</param>
        /// <returns>Converted <see cref="NVRMesh"/> meshes in the <see cref="OBJFile"/> format</returns>
        public static List<OBJFile> ConvertNVRMeshes(NVRFile nvr, string materialFile = "", bool simple = false)
        {
            List<OBJFile> returnList = new();

            foreach (NVRMaterial material in nvr.Materials)
            {
                var materialIndex = nvr.Materials.IndexOf(material);
                List<OBJFile> objSet = new();

                for (int meshIndex = 0; meshIndex < nvr.Meshes.Count; meshIndex++)
                {
                    NVRMesh mesh = nvr.Meshes[meshIndex];

                    var meshMaterialIndex = nvr.Materials.IndexOf(mesh.Material);

                    if (meshMaterialIndex != materialIndex)
                    {
                        continue;
                    }

                    NVRDrawIndexedPrimitive primitive = simple ? mesh.IndexedPrimitives[1] : mesh.IndexedPrimitives[0];

                    List<Vector3> vertices = new();
                    List<uint> indices = primitive.Indices.ConvertAll(i => (uint)i);
                    List<Vector2> uvs = new();
                    List<Vector3> normals = new();

                    foreach (var vertex in primitive.Vertices)
                    {
                        vertices.Add(vertex.Position);

                        if (primitive.VertexType == NVRVertexType.NVRVERTEX_4)
                        {
                            NVRVertex4 vertex4 = (NVRVertex4)vertex;
                            uvs.Add(vertex4.UV);
                            normals.Add(vertex4.Normal);
                        }
                        else if (primitive.VertexType == NVRVertexType.NVRVERTEX_8)
                        {
                            NVRVertex8 vertex8 = (NVRVertex8)vertex;
                            uvs.Add(vertex8.UV);
                            normals.Add(vertex8.Normal);
                        }
                        else if (primitive.VertexType == NVRVertexType.NVRVERTEX_12)
                        {
                            NVRVertex12 vertex12 = (NVRVertex12)vertex;
                            uvs.Add(vertex12.UV);
                            normals.Add(vertex12.Normal);
                        }
                    }

                    if (simple)
                    {
                        objSet.Add(new OBJFile(vertices, indices));
                        continue;
                    }

                    var groups = new List<OBJGroup>();
                    groups.Add(new OBJGroup(material.Name + "_" + meshIndex, material.Name, new List<uint>(indices)));

                    objSet.Add(new OBJFile(vertices, groups, uvs, normals, materialFile));
                }

                returnList.Add(new OBJFile(objSet));
            }

            return returnList;
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
    }
}
