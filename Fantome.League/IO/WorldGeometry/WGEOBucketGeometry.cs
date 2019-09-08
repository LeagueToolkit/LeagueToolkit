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
        /// <summary>
        /// Min X Bound of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public float MinX { get; set; }
        /// <summary>
        /// Min Y Bound of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public float MinZ { get; set; }
        /// <summary>
        /// Max X bound of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public float MaxX { get; set; }
        /// <summary>
        /// Max Z Bound of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public float MaxZ { get; set; }
        /// <summary>
        /// X Center of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public float CenterX { get; set; }
        /// <summary>
        /// Z Center of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public float CenterZ { get; set; }
        /// <summary>
        /// Min Y Bound of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public float MinY { get; set; }
        /// <summary>
        /// Max X bound of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public float MaxY { get; set; }
        /// <summary>
        /// Vertices of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        /// <summary>
        /// Indices of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
        public List<ushort> Indices { get; set; } = new List<ushort>();
        /// <summary>
        /// Buckets of this <see cref="WGEOBucketGeometry"/>
        /// </summary>
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
            uint vertexCount = br.ReadUInt32();
            uint indexCount = br.ReadUInt32();

            this.Buckets = new WGEOBucket[bucketsPerSide, bucketsPerSide];

            for (int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(new Vector3(br));
            }
            for (int i = 0; i < indexCount; i++)
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
