using Fantome.League.Helpers.Exceptions;
using Fantome.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.League.IO.SCB
{
    public class SCBFile
    {
        public string Name { get; private set; }
        public R3DBoundingBox BoundingBox { get; private set; }
        public Vector3 CentralPoint { get; private set; }
        public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
        public List<Vector4Byte> Tangents { get; private set; } = new List<Vector4Byte>();
        public List<SCBFace> Faces { get; private set; } = new List<SCBFace>();
        public List<Vector3Byte> VertexColors { get; private set; } = new List<Vector3Byte>();
        public SCBFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                if (Magic != "r3d2Mesh")
                    throw new InvalidFileMagicException();

                UInt16 Major = br.ReadUInt16();
                UInt16 Minor = br.ReadUInt16();
                if (Major != 3 && Major != 2 && Minor != 1) //There are versions [2][1] and [1][1] aswell 
                    throw new UnsupportedFileVersionException();

                this.Name = Encoding.ASCII.GetString(br.ReadBytes(128)).Replace("\0", "");
                UInt32 VertexCount = br.ReadUInt32();
                UInt32 FaceCount = br.ReadUInt32();
                SCBFlags Flags = (SCBFlags)br.ReadUInt32();
                this.BoundingBox = new R3DBoundingBox(br);

                bool HasTangents = false;
                if(Major == 3 && Minor == 2)
                {
                    HasTangents = br.ReadUInt32() == 1;
                }

                for(int i = 0; i < VertexCount; i++)
                {
                    this.Vertices.Add(new Vector3(br));
                }

                if(Major == 3 && Minor == 2 && Flags.HasFlag(SCBFlags.Tangents) && HasTangents)
                {
                    for(int i = 0; i < VertexCount; i++)
                    {
                        this.Tangents.Add(new Vector4Byte(br));
                    }
                }

                this.CentralPoint = new Vector3(br);

                for (int i = 0; i < FaceCount; i++)
                {
                    this.Faces.Add(new SCBFace(br));
                }

                if(Flags.HasFlag(SCBFlags.VertexColors))
                {
                    this.VertexColors.Add(new Vector3Byte(br));
                }
            }
        }
        public void Write(string Location)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(Location)))
            {
                bw.Write("r3d2Mesh".ToCharArray());
                bw.Write((UInt16)3);
                bw.Write((UInt16)2);
                bw.Write(this.Name.PadRight(128, '\u0000').ToCharArray());
                bw.Write((UInt32)this.Vertices.Count);
                bw.Write((UInt32)this.Faces.Count);

                UInt32 Flags = 0;
                UInt32 HasTangent = 0;
                if (this.Tangents.Count != 0)
                {
                    Flags |= (UInt32)SCBFlags.Tangents;
                    HasTangent = 1;
                }
                if (this.VertexColors.Count != 0)
                    Flags |= (UInt32)SCBFlags.VertexColors;
                bw.Write(Flags);
                CalculateBoundingBox().Write(bw);
                bw.Write(HasTangent);

                foreach(Vector3 Vertex in this.Vertices)
                {
                    Vertex.Write(bw);
                }
                foreach(Vector4Byte Tangent in this.Tangents)
                {
                    Tangent.Write(bw);
                }
                this.CentralPoint.Write(bw);
                foreach(SCBFace Face in this.Faces)
                {
                    Face.Write(bw);
                }
                foreach(Vector3Byte Color in this.VertexColors)
                {
                    Color.Write(bw);
                }
            }
        }
        public R3DBoundingBox CalculateBoundingBox()
        {
            Vector3 Min = this.Vertices[0];
            Vector3 Max = this.Vertices[0];

            foreach(Vector3 Vertex in this.Vertices)
            {
                if (Min.X > Vertex.X) Min.X = Vertex.X;
                if (Min.Y > Vertex.Y) Min.Y = Vertex.Y;
                if (Min.Z > Vertex.Z) Min.Z = Vertex.Z;
                if (Max.X < Vertex.X) Max.X = Vertex.X;
                if (Max.Y < Vertex.Y) Max.Y = Vertex.Y;
                if (Max.Z < Vertex.Z) Max.Z = Vertex.Z;
            }

            return new R3DBoundingBox(Min, new Vector3(Math.Abs(Max.X - Min.X), Math.Abs(Max.Y - Min.Y), Math.Abs(Max.Z - Min.Z)));
        }
    }

    public enum SCBFlags : UInt32
    {
        VertexColors = 1,
        Tangents = 2
    }
}
