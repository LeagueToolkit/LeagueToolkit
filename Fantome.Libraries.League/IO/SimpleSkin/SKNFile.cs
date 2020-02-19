using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.WGT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fantome.Libraries.League.IO.SimpleSkin
{
    /// <summary>
    /// Represents a Simple Skin (SKN) file
    /// </summary>
    public class SKNFile
    {
        /// <summary>
        /// Models of this <see cref="SKNFile"/>
        /// </summary>
        public List<SKNSubmesh> Submeshes { get; private set; } = new List<SKNSubmesh>();

        /// <summary>
        /// Initializes a new <see cref="SKNFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">Location to read from</param>
        public SKNFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initializes a new <see cref="SKNFile"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        public SKNFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
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
                    this.Submeshes.Add(new SKNSubmesh(this, br));
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

                List<ushort> indices = new List<ushort>();
                List<SKNVertex> vertices = new List<SKNVertex>();
                for (int i = 0; i < indexCount; i++)
                {
                    indices.Add(br.ReadUInt16());
                }
                for (int i = 0; i < vertexCount; i++)
                {
                    vertices.Add(new SKNVertex(br, isTangent));
                }

                foreach (SKNSubmesh submesh in this.Submeshes)
                {
                    submesh.Indices = indices.GetRange((int)submesh._startIndex, (int)submesh._indexCount);
                    submesh.Vertices = vertices.GetRange((int)submesh._startVertex, (int)submesh._vertexCount);
                }
            }
        }

        /// <summary>
        /// Calculates an AABB Bounding Box of this <see cref="SKNFile"/>
        /// </summary>
        public R3DBox CalculateBoundingBox()
        {
            List<SKNVertex> vertices = new List<SKNVertex>();
            foreach (SKNSubmesh submesh in this.Submeshes)
            {
                vertices.AddRange(submesh.Vertices);
            }

            if (vertices.Count == 0)
            {
                return new R3DBox(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            }
            else
            {
                Vector3 min = vertices[0].Position;
                Vector3 max = vertices[0].Position;

                foreach (SKNVertex vertex in vertices)
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
        }

        /// <summary>
        /// Calculates a Bounding Sphere of this <see cref="SKNFile"/>
        /// </summary>
        public R3DSphere CalculateBoundingSphere()
        {
            R3DBox box = CalculateBoundingBox();
            Vector3 centralPoint = CalculateCentralPoint();

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        /// <summary>
        /// Calculates a Bounding Sphere from a <see cref="R3DBox"/> of this <see cref="SKNFile"/>
        /// </summary>
        /// <param name="box"><see cref="R3DBox"/> of this <see cref="SKNFile"/></param>
        public R3DSphere CalculateBoundingSphere(R3DBox box)
        {
            Vector3 centralPoint = CalculateCentralPoint();

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        /// <summary>
        /// Calculates a Bounding Sphere from a <see cref="R3DBox"/> and a Central Point of this <see cref="SKNFile"/>
        /// </summary>
        /// <param name="box"><see cref="R3DBox"/> of this <see cref="SKNFile"/></param>
        /// <param name="centralPoint">Position of the <see cref="R3DSphere"/> of this <see cref="SKNFile"/></param>
        public R3DSphere CalculateBoundingSphere(R3DBox box, Vector3 centralPoint)
        {
            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        /// <summary>
        /// Calculates the Central Point of this <see cref="SKNFile"/>
        /// </summary>
        public Vector3 CalculateCentralPoint()
        {
            R3DBox box = CalculateBoundingBox();

            return new Vector3(0.5f * (box.Min.X + box.Max.X),
                0.5f * (box.Min.Y + box.Max.Y),
                0.5f * (box.Min.Z + box.Max.Z));
        }

        /// <summary>
        /// Calculates the Central Point of this <see cref="SKNFile"/> from a <see cref="R3DBox"/> of this <see cref="SKNFile"/>
        /// </summary>
        /// <param name="box"><see cref="R3DBox"/> of this <see cref="SKNFile"/></param>
        public Vector3 CalculateCentralPoint(R3DBox box)
        {
            return new Vector3(0.5f * (box.Min.X + box.Max.X),
                0.5f * (box.Min.Y + box.Max.Y),
                0.5f * (box.Min.Z + box.Max.Z));
        }

        /// <summary>
        /// Writes this <see cref="SKNFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">Location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="SKNFile"/> into the specified <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(0x00112233);
                bw.Write((ushort)4);
                bw.Write((ushort)1);
                bw.Write(this.Submeshes.Count);

                bool hasTangent = false;
                uint indexCount = 0;
                uint vertexCount = 0;
                foreach (SKNSubmesh submesh in this.Submeshes)
                {
                    if (!hasTangent)
                    {
                        foreach (SKNVertex vertex in submesh.Vertices)
                        {
                            if (vertex.Tangent != null)
                            {
                                hasTangent = true;
                            }
                        }
                    }
                    submesh.Write(bw);
                    indexCount += (uint)submesh.Indices.Count;
                    vertexCount += (uint)submesh.Vertices.Count;
                }
                bw.Write((uint)0);
                bw.Write(indexCount);
                bw.Write(vertexCount);

                if (hasTangent)
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

                foreach (SKNSubmesh submesh in this.Submeshes)
                {
                    foreach (ushort index in submesh.Indices)
                    {
                        bw.Write(index);
                    }
                }
                foreach (SKNSubmesh submesh in this.Submeshes)
                {
                    foreach (SKNVertex vertex in submesh.Vertices)
                    {
                        if (hasTangent && vertex.Tangent == null)
                        {
                            vertex.Tangent = new Vector4Byte(255, 255, 255, 255);
                        }
                        else if (!hasTangent && vertex.Tangent != null)
                        {
                            vertex.Tangent = null;
                        }

                        vertex.Write(bw);
                    }
                }
                bw.Write(new byte[12]);
            }
        }
    }
}
