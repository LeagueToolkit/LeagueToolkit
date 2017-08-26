using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.WGT;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.SKN
{
    public class SKNFile
    {
        public List<SKNSubmesh> Submeshes { get; private set; } = new List<SKNSubmesh>();
        public List<uint> Indices { get; private set; } = new List<uint>();
        public List<SKNVertex> Vertices { get; private set; } = new List<SKNVertex>();

        public SKNFile(WGTFile weightsFile, SCOFile modelFile)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector4Byte> boneIndices = new List<Vector4Byte>();
            List<Vector4> weights = new List<Vector4>();
            List<Vector2> uvs = new List<Vector2>();
            List<uint> indices = new List<uint>();

            foreach (WGTWeight weight in weightsFile.Weights)
            {
                boneIndices.Add(weight.Indices);
                weights.Add(weight.Weights);
            }

            foreach (KeyValuePair<string, List<SCOFace>> material in modelFile.Materials)
            {
                uint currentIndexValue = 0;

                this.Submeshes.Add(new SKNSubmesh(
                    material.Key,
                    currentIndexValue,
                    (uint)material.Value.Count,
                    currentIndexValue,
                    (uint)(material.Value.Count * 3)));

                currentIndexValue += (uint)material.Value.Count;

                foreach (SCOFace face in material.Value)
                {
                    indices.AddRange(face.Indices);
                    for (int i = 0; i < 3; i++)
                    {
                        vertices.Add(modelFile.Vertices[(int)face.Indices[i]]);
                        uvs.Add(face.UVs[i]);
                    }
                }
            }

            //Calculates smooth normals for the mesh
            List<Vector3> normals = new List<Vector3>(vertices.Count);
            for (int i = 0; i < indices.Count; i++)
            {
                uint a = indices[i];
                uint b = indices[i + 1];
                uint c = indices[i + 2];

                Vector3 edgeA = vertices[(int)a] - vertices[(int)b];
                Vector3 edgeB = vertices[(int)c] - vertices[(int)b];
                Vector3 normal = Vector3.Cross(edgeA, edgeB);

                normals[(int)a] = normal;
                normals[(int)b] = normal;
                normals[(int)c] = normal;

                //Normalizes normals
                for (int j = 0; j < normals.Count; j++)
                {
                    Vector3 normalNormalize = normals[i];
                    float sum = normalNormalize.X + normalNormalize.Y + normalNormalize.Z;
                    normals[j] = new Vector3(normalNormalize.X / sum, normalNormalize.Y / sum, normalNormalize.Z / sum);
                }
            }

            List<SKNVertex> sknVertices = new List<SKNVertex>();
            for (int i = 0; i < vertices.Count; i++)
            {
                sknVertices.Add(new SKNVertex(vertices[i], boneIndices[i], weights[i], normals[i], uvs[i]));
            }

            this.Indices = indices;
            this.Vertices = sknVertices;
        }

        public SKNFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                uint magic = br.ReadUInt32();
                if (magic != 0x00112233)
                {
                    throw new Exception("Not a valid SKN file");
                }

                ushort major = br.ReadUInt16();
                ushort minor = br.ReadUInt16();
                if (major != 2 && major != 4 && minor != 1)
                {
                    throw new Exception("This SKN version is not supported");
                }

                uint submeshCount = br.ReadUInt32();

                for (int i = 0; i < submeshCount; i++)
                {
                    this.Submeshes.Add(new SKNSubmesh(br));
                }
                if (major == 4)
                {
                    uint unknown = br.ReadUInt32();
                }

                uint indexCount = br.ReadUInt32();
                uint vertexCount = br.ReadUInt32();

                uint vertexSize;
                bool isTangent = false;
                R3DBox boundingBox;
                R3DSphere boundingSphere;

                if (major == 4)
                {
                    vertexSize = br.ReadUInt32();
                    isTangent = br.ReadUInt32() == 1;
                    boundingBox = new R3DBox(br);
                    boundingSphere = new R3DSphere(br);
                }

                for (int i = 0; i < indexCount; i++)
                {
                    this.Indices.Add(br.ReadUInt16());
                }
                for (int i = 0; i < vertexCount; i++)
                {
                    this.Vertices.Add(new SKNVertex(br, isTangent));
                }
            }
        }

        public void Write(string fileLocation)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileLocation)))
            {
                bw.Write(0x00112233);
                bw.Write((ushort)4);
                bw.Write((ushort)1);
                bw.Write(this.Submeshes.Count);

                uint indexCount = 0;
                uint vertexCount = 0;
                foreach (SKNSubmesh submesh in this.Submeshes)
                {
                    submesh.Write(bw);
                    indexCount += submesh.IndexCount;
                    vertexCount += submesh.VertexCount;
                }
                bw.Write((uint)0);
                bw.Write(indexCount);
                bw.Write(vertexCount);

                if (this.Vertices[0].Tangent != null)
                {
                    bw.Write((uint)56);
                    bw.Write((uint)1);
                }
                else
                {
                    bw.Write((uint)52);
                    bw.Write((uint)0);
                }

                R3DBox box = CalculateBoundingBox();
                box.Write(bw);
                CalculateBoundingSphere(box).Write(bw);

                foreach (ushort index in this.Indices)
                {
                    bw.Write(index);
                }
                foreach (SKNVertex vertex in this.Vertices)
                {
                    vertex.Write(bw);
                }
                bw.Write(new byte[12]);
            }
        }

        public R3DBox CalculateBoundingBox()
        {
            Vector3 min = this.Vertices[0].Position;
            Vector3 max = this.Vertices[0].Position;

            foreach (SKNVertex vertex in this.Vertices)
            {
                if (min.X > vertex.Position.X) min.X = vertex.Position.X;
                if (min.Y > vertex.Position.Y) min.Y = vertex.Position.Y;
                if (min.Z > vertex.Position.Z) min.Z = vertex.Position.Z;
                if (max.X < vertex.Position.X) max.X = vertex.Position.X;
                if (max.Y < vertex.Position.Y) max.Y = vertex.Position.Y;
                if (max.Z < vertex.Position.Z) max.Z = vertex.Position.Z;
            }

            return new R3DBox(min, max);
        }

        public R3DSphere CalculateBoundingSphere()
        {
            R3DBox box = CalculateBoundingBox();
            Vector3 centralPoint = CalculateCentralPoint();

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        public R3DSphere CalculateBoundingSphere(R3DBox box)
        {
            Vector3 centralPoint = CalculateCentralPoint();

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        public R3DSphere CalculateBoundingSphere(R3DBox box, Vector3 centralPoint)
        {
            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        public Vector3 CalculateCentralPoint()
        {
            R3DBox box = CalculateBoundingBox();

            return new Vector3(
                0.5f * (box.Min.X + box.Max.X),
                0.5f * (box.Min.Y + box.Max.Y),
                0.5f * (box.Min.Z + box.Max.Z)
                );
        }

        public Vector3 CalculateCentralPoint(R3DBox box)
        {
            return new Vector3(
                0.5f * (box.Min.X + box.Max.X),
                0.5f * (box.Min.Y + box.Max.Y),
                0.5f * (box.Min.Z + box.Max.Z)
                );
        }
    }
}
