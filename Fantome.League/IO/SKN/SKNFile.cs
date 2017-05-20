using Fantome.League.Helpers.Exceptions;
using Fantome.League.Helpers.Structures;
using Fantome.League.IO.SCO;
using Fantome.League.IO.WGT;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.League.IO.SKN
{
    public class SKNFile
    {
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
        }

        public SKNFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                UInt32 Magic = br.ReadUInt32();
                if (Magic != 0x00112233)
                    throw new InvalidFileMagicException();

                UInt16 Version = br.ReadUInt16();
                if (Version != 2 && Version != 4)
                    throw new UnsupportedFileVersionException();

                UInt16 ObjectCount = br.ReadUInt16();
                UInt32 SubmeshCount = br.ReadUInt32();

                for (int i = 0; i < SubmeshCount; i++)
                {
                    this.Submeshes.Add(new SKNSubmesh(br));
                }
                if (Version == 4)
                    br.ReadUInt32();

                UInt32 IndexCount = br.ReadUInt32();
                UInt32 VertexCount = br.ReadUInt32();

                UInt32 VertexSize;
                bool IsTangent = false;
                Vector3 Min;
                Vector3 Max;
                Vector3 CentralPoint;
                float Radius;

                if (Version == 4)
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

        public void Write(string Location)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(Location)))
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
