using Fantome.Libraries.League.Helpers.Exceptions;
using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.WGT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Fantome.Libraries.League.IO.SKN
{
    [DebuggerDisplay("[ Version: {Version} ]")]
    public class SKNFile
    {
        public UInt32 Version { get; private set; }
        public List<SKNSubmesh> Submeshes { get; private set; } = new List<SKNSubmesh>();
        public List<UInt16> Indices { get; private set; } = new List<UInt16>();
        public List<SKNVertex> Vertices { get; private set; } = new List<SKNVertex>();

        public SKNFile(WGTFile Weights, SCOFile Model)
        {
            this.Submeshes.Add(new SKNSubmesh(Model.Name, 0, (uint)Model.Vertices.Count, 0, (uint)Model.Faces.Count * 3));
            foreach (Vector3 Vertex in Model.Vertices)
            {
                this.Vertices.Add(new SKNVertex(Vertex));
            }
            for (int i = 0; i < this.Vertices.Count; i++)
            {
                this.Vertices[i].SetWeight(Weights.Weights[i].Indices, Weights.Weights[i].Weights);
            }
            for (int i = 0; i < Model.Faces.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this.Vertices[Model.Faces[i].Indices[j]].SetUV(Model.Faces[i].UV[j]);
                }
                for (int j = 0; j < 3; j++)
                {
                    this.Indices.Add(Model.Faces[i].Indices[j]);
                }
            }
            for (int i = 0; i < Indices.Count; i += 3)
            {
                Vector3 cp = Vector3.Cross(
                    Vertices[Indices[i + 1]].Position - Vertices[Indices[i]].Position,
                    Vertices[Indices[i + 2]].Position - Vertices[Indices[i]].Position);

                Vertices[Indices[i]].SetNormal(Vertices[Indices[i]].Normal + cp);
                Vertices[Indices[i + 1]].SetNormal(Vertices[Indices[i + 1]].Normal + cp);
                Vertices[Indices[i + 2]].SetNormal(Vertices[Indices[i + 2]].Normal + cp);
            }
            foreach (SKNVertex Vertex in Vertices)
            {
                float s = Vertex.Normal.X + Vertex.Normal.Y + Vertex.Normal.Z;
                Vertex.SetNormal(new Vector3(
                    Vertex.Normal.X / s,
                    Vertex.Normal.Y / s,
                    Vertex.Normal.Z / s
                    )
                    );
            }
        }

        public SKNFile(string fileLocation)
            : this(File.OpenRead(fileLocation))
        {

        }
        public SKNFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                UInt32 Magic = br.ReadUInt32();
                if (Magic != 0x00112233)
                    throw new InvalidFileMagicException();

                this.Version = br.ReadUInt16();
                if (this.Version != 2 && this.Version != 4)
                    throw new UnsupportedFileVersionException();

                UInt16 ObjectCount = br.ReadUInt16();
                UInt32 SubmeshCount = br.ReadUInt32();

                for (int i = 0; i < SubmeshCount; i++)
                {
                    this.Submeshes.Add(new SKNSubmesh(br));
                }
                if (this.Version == 4)
                    br.ReadUInt32();

                UInt32 IndexCount = br.ReadUInt32();
                UInt32 VertexCount = br.ReadUInt32();

                UInt32 VertexSize;
                bool IsTangent = false;
                Vector3 Min;
                Vector3 Max;
                Vector3 CentralPoint;
                float Radius;

                if (this.Version == 4)
                {
                    VertexSize = br.ReadUInt32();
                    IsTangent = br.ReadUInt32() == 1;
                    Min = new Vector3(br);
                    Max = new Vector3(br);
                    CentralPoint = new Vector3(br);
                    Radius = br.ReadSingle();
                }

                for (int i = 0; i < IndexCount; i++)
                {
                    this.Indices.Add(br.ReadUInt16());
                }
                for (int i = 0; i < VertexCount; i++)
                {
                    this.Vertices.Add(new SKNVertex(br, IsTangent));
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(0x00112233);
                bw.Write((UInt16)2);
                bw.Write((UInt16)1);
                bw.Write((UInt32)this.Submeshes.Count);

                UInt32 IndexCount = 0;
                UInt32 VertexCount = 0;
                foreach (SKNSubmesh Submesh in this.Submeshes)
                {
                    Submesh.Write(bw);
                    IndexCount += Submesh.IndexCount;
                    VertexCount += Submesh.VertexCount;
                }
                bw.Write(IndexCount);
                bw.Write(VertexCount);

                foreach (UInt16 Index in this.Indices)
                {
                    bw.Write(Index);
                }
                foreach (SKNVertex Vertex in this.Vertices)
                {
                    Vertex.Write(bw);
                }
                bw.Write(new byte[12]);
            }
        }
    }
}
