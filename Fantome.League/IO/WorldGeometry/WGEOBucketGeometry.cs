using System;
using System.Collections.Generic;
using System.IO;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.WorldGeometry
{
    /// <summary>
    /// Represents a <see cref="WGEOBucketGeometry"/> inside of a <see cref="WGEOFile"/>
    /// </summary>
    public class WGEOBucketGeometry
    {
        public float MinX { get; set; }
        public float MinZ { get; set; }
        public float MaxX { get; set; }
        public float MaxZ { get; set; }
        public float CenterX { get; set; }
        public float CenterZ { get; set; }
        public float MinY { get; set; }
        public float MaxY { get; set; }
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<ushort> Indices { get; set; } = new List<ushort>();
        public WGEOBucket[,] Buckets { get; set; }

        /// <summary>
        /// Initializes a new <see cref="WGEOBucketGeometry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
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

            uint bucketsPerSide = br.ReadUInt32();
            uint VertexCount = br.ReadUInt32();
            uint IndexCount = br.ReadUInt32();

            this.Buckets = new WGEOBucket[bucketsPerSide, bucketsPerSide];

            for (int i = 0; i < VertexCount; i++)
            {
                this.Vertices.Add(new Vector3(br));
            }
            for (int i = 0; i < IndexCount; i++)
            {
                this.Indices.Add(br.ReadUInt16());
            }
            for (int i = 0; i < bucketsPerSide; i++)
            {
                for (int j = 0; j < bucketsPerSide; j++)
                {
                    this.Buckets[i, j] = new WGEOBucket(br);
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="BinaryWriter"/> to a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
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

            uint bucketsPerSide = (uint)Math.Sqrt(this.Buckets.Length);
            bw.Write(bucketsPerSide);
            bw.Write((uint)this.Vertices.Count);
            bw.Write((uint)this.Indices.Count);

            foreach (Vector3 Vertex in this.Vertices)
            {
                Vertex.Write(bw);
            }
            foreach (ushort Index in this.Indices)
            {
                bw.Write(Index);
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
