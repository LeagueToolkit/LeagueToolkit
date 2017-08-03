using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.WGEO
{
    public class WGEOBucketGeometry
    {
        public float MinX { get; private set; }
        public float MinZ { get; private set; }
        public float MaxX { get; private set; }
        public float MaxZ { get; private set; }
        public float CenterX { get; private set; }
        public float CenterZ { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }
        public UInt32 BucketsPerSide { get; private set; }
        public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
        public List<UInt16> Indices { get; private set; } = new List<UInt16>();
        public WGEOBucket[,] Buckets { get; private set; }

        public WGEOBucketGeometry(BinaryReader br)
        {
            this.MinX = br.ReadSingle();
            this.MinZ = br.ReadSingle();

            this.MaxX = br.ReadSingle();
            this.MaxZ = br.ReadSingle();

            this.CenterX = br.ReadSingle();
            this.CenterZ = br.ReadSingle();

            this.MinY = br.ReadSingle();
            this.MaxY = br.ReadSingle();

            this.BucketsPerSide = br.ReadUInt32();
            UInt32 VertexCount = br.ReadUInt32();
            UInt32 IndexCount = br.ReadUInt32();

            this.Buckets = new WGEOBucket[this.BucketsPerSide, this.BucketsPerSide];

            for (int i = 0; i < VertexCount; i++)
            {
                this.Vertices.Add(new Vector3(br));
            }
            for (int i = 0; i < IndexCount; i++)
            {
                this.Indices.Add(br.ReadUInt16());
            }
            for (int i = 0; i < this.BucketsPerSide; i++)
            {
                for (int j = 0; j < this.BucketsPerSide; j++)
                {
                    this.Buckets[i, j] = new WGEOBucket(br);
                }
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.MinX);
            bw.Write(this.MinZ);

            bw.Write(this.MaxX);
            bw.Write(this.MaxZ);

            bw.Write(this.CenterX);
            bw.Write(this.CenterZ);

            bw.Write(this.MinY);
            bw.Write(this.MaxY);

            bw.Write(this.BucketsPerSide);
            bw.Write((UInt32)this.Vertices.Count);
            bw.Write((UInt32)this.Indices.Count);

            foreach (Vector3 Vertex in Vertices)
            {
                Vertex.Write(bw);
            }
            foreach (UInt16 Index in Indices)
            {
                bw.Write(Index);
            }
            for (int i = 0; i < this.BucketsPerSide; i++)
            {
                for (int j = 0; j < this.BucketsPerSide; j++)
                {
                    this.Buckets[i, j].Write(bw);
                }
            }
        }
    }
}
