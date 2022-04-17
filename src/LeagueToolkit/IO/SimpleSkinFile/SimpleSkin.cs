using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.IO.StaticObjectFile;
using LeagueToolkit.IO.WGT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.SimpleSkinFile
{
    public class SimpleSkin
    {
        public List<SimpleSkinSubmesh> Submeshes { get; private set; } = new List<SimpleSkinSubmesh>();

        public SimpleSkin(List<SimpleSkinSubmesh> submeshes)
        {
            this.Submeshes = submeshes;
        }
        public SimpleSkin(StaticObject staticObject, WGTFile weightFile)
        {
            List<uint> staticObjectIndices = staticObject.GetIndices();
            List<StaticObjectVertex> staticObjectVertices = staticObject.GetVertices();

            int currentVertexOffset = 0;
            foreach (StaticObjectSubmesh submesh in staticObject.Submeshes)
            {
                // Build vertices
                List<SimpleSkinVertex> vertices = new(staticObjectVertices.Count);
                for (int i = 0; i < submesh.Vertices.Count; i++)
                {
                    StaticObjectVertex vertex = submesh.Vertices[i];
                    WGTWeight weightData = weightFile.Weights[i + currentVertexOffset];

                    vertices.Add(new SimpleSkinVertex(vertex.Position, weightData.BoneIndices, weightData.Weights, Vector3.Zero, vertex.UV));
                }

                this.Submeshes.Add(new SimpleSkinSubmesh(submesh.Name, submesh.Indices.Select(x => (ushort)x).ToList(), vertices));

                currentVertexOffset += submesh.Vertices.Count;
            }
        }
        public SimpleSkin(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public SimpleSkin(Stream stream, bool leaveOpen = false)
        {
            using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, leaveOpen))
            {
                uint magic = br.ReadUInt32();
                if (magic != 0x00112233)
                {
                    throw new InvalidFileSignatureException();
                }

                ushort major = br.ReadUInt16();
                ushort minor = br.ReadUInt16();
                if (major != 0 && major != 2 && major != 4 && minor != 1)
                {
                    throw new UnsupportedFileVersionException();
                }

                uint indexCount = 0;
                uint vertexCount = 0;
                SimpleSkinVertexType vertexType = SimpleSkinVertexType.Basic;
                if (major == 0)
                {
                    indexCount = br.ReadUInt32();
                    vertexCount = br.ReadUInt32();
                }
                else
                {
                    uint submeshCount = br.ReadUInt32();

                    for (int i = 0; i < submeshCount; i++)
                    {
                        this.Submeshes.Add(new SimpleSkinSubmesh(br));
                    }
                    if (major == 4)
                    {
                        uint flags = br.ReadUInt32();
                    }

                    indexCount = br.ReadUInt32();
                    vertexCount = br.ReadUInt32();

                    uint vertexSize = major == 4 ? br.ReadUInt32() : 52;
                    vertexType = major == 4 ? (SimpleSkinVertexType)br.ReadUInt32() : SimpleSkinVertexType.Basic;
                    R3DBox boundingBox = major == 4 ? new R3DBox(br) : new R3DBox(Vector3.Zero, Vector3.Zero);
                    R3DSphere boundingSphere = major == 4 ? new R3DSphere(br) : R3DSphere.Infinite;
                }

                List<ushort> indices = new List<ushort>();
                List<SimpleSkinVertex> vertices = new List<SimpleSkinVertex>();
                for (int i = 0; i < indexCount; i++)
                {
                    indices.Add(br.ReadUInt16());
                }
                for (int i = 0; i < vertexCount; i++)
                {
                    vertices.Add(new SimpleSkinVertex(br, vertexType));
                }

                if(major == 0)
                {
                    this.Submeshes.Add(new SimpleSkinSubmesh("Base", indices, vertices));
                }
                else
                {
                    foreach (SimpleSkinSubmesh submesh in this.Submeshes)
                    {
                        List<ushort> submeshIndices = indices.GetRange((int)submesh._startIndex, (int)submesh._indexCount);
                        ushort minIndex = submeshIndices.Min();

                        submesh.Indices = submeshIndices.Select(x => x -= minIndex).ToList();
                        submesh.Vertices = vertices.GetRange((int)submesh._startVertex, (int)submesh._vertexCount);
                    }
                }
            }
        }

        public void Write(string fileLocation) => Write(File.Create(fileLocation));
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(0x00112233);
                bw.Write((ushort)4);
                bw.Write((ushort)1);
                bw.Write(this.Submeshes.Count);

                bool hasVertexColors = false;
                uint indexCount = 0;
                uint vertexCount = 0;
                foreach (SimpleSkinSubmesh submesh in this.Submeshes)
                {
                    if (!hasVertexColors)
                    {
                        foreach (SimpleSkinVertex vertex in submesh.Vertices)
                        {
                            if (vertex.Color != null)
                            {
                                hasVertexColors = true;
                                break;
                            }
                        }
                    }

                    submesh.Write(bw, vertexCount, indexCount);

                    indexCount += (uint)submesh.Indices.Count;
                    vertexCount += (uint)submesh.Vertices.Count;
                }

                bw.Write((uint)0); //Flags
                bw.Write(indexCount);
                bw.Write(vertexCount);
                if (hasVertexColors)
                {
                    bw.Write((uint)56);
                    bw.Write((uint)SimpleSkinVertexType.Color);
                }
                else
                {
                    bw.Write((uint)52);
                    bw.Write((uint)SimpleSkinVertexType.Basic);
                }

                R3DBox box = GetBoundingBox();
                box.Write(bw);
                box.GetBoundingSphere().Write(bw);

                ushort indexOffset = 0;
                foreach (SimpleSkinSubmesh submesh in this.Submeshes)
                {
                    foreach (ushort index in submesh.Indices.Select(x => x += indexOffset))
                    {
                        bw.Write(index);
                    }

                    indexOffset += submesh.Indices.Max();
                }

                foreach (SimpleSkinSubmesh submesh in this.Submeshes)
                {
                    foreach (SimpleSkinVertex vertex in submesh.Vertices)
                    {
                        vertex.Write(bw, hasVertexColors ? SimpleSkinVertexType.Color : SimpleSkinVertexType.Basic);
                    }
                }

                bw.Write(new byte[12]); //End tab
            }
        }

        public R3DBox GetBoundingBox()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (SimpleSkinSubmesh submesh in this.Submeshes)
            {
                foreach (SimpleSkinVertex vertex in submesh.Vertices)
                {
                    if (min.X > vertex.Position.X) min.X = vertex.Position.X;
                    if (min.Y > vertex.Position.Y) min.Y = vertex.Position.Y;
                    if (min.Z > vertex.Position.Z) min.Z = vertex.Position.Z;
                    if (max.X < vertex.Position.X) max.X = vertex.Position.X;
                    if (max.Y < vertex.Position.Y) max.Y = vertex.Position.Y;
                    if (max.Z < vertex.Position.Z) max.Z = vertex.Position.Z;
                }
            }

            return new R3DBox(min, max);
        }
    }
}
