using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOBucketGeometry
    {
        public float MinX { get; set; }
        public float MinZ { get; set; }
        public float MaxX { get; set; }
        public float MaxZ { get; set; }
        public float MaxOutStickX { get; set; }
        public float MaxOutStickZ { get; set; }
        public float BucketSizeX { get; set; }
        public float BucketSizeZ { get; set; }
        public List<Vector3> Vertices{ get; set; } = new List<Vector3>();
        public List<ushort> Indices { get; set; } = new List<ushort>();
        public MGEOBucket[,] Buckets { get; set; }

        public MGEOBucketGeometry(BinaryReader br)
        {
            this.MinX = br.ReadSingle();
            this.MinZ = br.ReadSingle();
            this.MaxX = br.ReadSingle();
            this.MaxZ = br.ReadSingle();
            this.MaxOutStickX = br.ReadSingle();
            this.MaxOutStickZ = br.ReadSingle();
            this.BucketSizeX = br.ReadSingle();
            this.BucketSizeZ = br.ReadSingle();

            uint bucketsPerSide = br.ReadUInt32();
            uint vertexCount = br.ReadUInt32();
            uint indexCount = br.ReadUInt32();

            for (int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(new Vector3(br));
            }

            for (int i = 0; i < indexCount; i++)
            {
                this.Indices.Add(br.ReadUInt16());
            }

            this.Buckets = new MGEOBucket[bucketsPerSide, bucketsPerSide];
            for (int i = 0; i < bucketsPerSide; i++)
            {
                for (int j = 0; j < bucketsPerSide; j++)
                {
                    this.Buckets[i, j] = new MGEOBucket(br);
                }
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.MinX);
            bw.Write(this.MinZ);

            bw.Write(this.MaxX);
            bw.Write(this.MaxZ);

            bw.Write(this.MaxOutStickX);
            bw.Write(this.MaxOutStickZ);

            bw.Write(this.BucketSizeX);
            bw.Write(this.BucketSizeZ);

            uint bucketsPerSide = (uint)Math.Sqrt(this.Buckets.Length);
            bw.Write(bucketsPerSide);
            bw.Write(this.Vertices.Count);
            bw.Write(this.Indices.Count);

            foreach (Vector3 vertex in this.Vertices)
            {
                vertex.Write(bw);
            }
            foreach (ushort index in this.Indices)
            {
                bw.Write(index);
            }
            for (int i = 0; i < bucketsPerSide; i++)
            {
                for (int j = 0; j < bucketsPerSide; j++)
                {
                    this.Buckets[i, j].Write(bw);
                }
            }
        }
    }
}
