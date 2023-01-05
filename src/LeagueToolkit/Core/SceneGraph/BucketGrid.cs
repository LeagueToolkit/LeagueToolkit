using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.Core.SceneGraph
{
    public class BucketGrid
    {
        public float MinX { get; set; }
        public float MinZ { get; set; }
        public float MaxX { get; set; }
        public float MaxZ { get; set; }
        public float MaxOutStickX { get; set; }
        public float MaxOutStickZ { get; set; }
        public float BucketSizeX { get; set; }
        public float BucketSizeZ { get; set; }

        public bool DisableBucketGrid { get; set; }

        public List<Vector3> Vertices { get; set; } = new();
        public List<ushort> Indices { get; set; } = new();
        public BucketGridBucket[,] Buckets { get; set; }
        public List<BucketGridFaceFlag> FaceFlags = null;

        internal BucketGrid(BinaryReader br)
        {
            this.MinX = br.ReadSingle();
            this.MinZ = br.ReadSingle();
            this.MaxX = br.ReadSingle();
            this.MaxZ = br.ReadSingle();
            this.MaxOutStickX = br.ReadSingle();
            this.MaxOutStickZ = br.ReadSingle();
            this.BucketSizeX = br.ReadSingle();
            this.BucketSizeZ = br.ReadSingle();

            ushort bucketsPerSide = br.ReadUInt16();
            this.DisableBucketGrid = br.ReadBoolean();
            BucketGridFlags flags = (BucketGridFlags)br.ReadByte();

            uint vertexCount = br.ReadUInt32();
            uint indexCount = br.ReadUInt32();

            // If diable bucket grid flag is set, do not read data
            if (this.DisableBucketGrid)
            {
                return;
            }

            for (int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(br.ReadVector3());
            }

            for (int i = 0; i < indexCount; i++)
            {
                this.Indices.Add(br.ReadUInt16());
            }

            this.Buckets = new BucketGridBucket[bucketsPerSide, bucketsPerSide];
            for (int i = 0; i < bucketsPerSide; i++)
            {
                for (int j = 0; j < bucketsPerSide; j++)
                {
                    this.Buckets[i, j] = new(br);
                }
            }

            if (flags.HasFlag(BucketGridFlags.HasFaceFlags))
            {
                uint faceCount = indexCount / 3;
                this.FaceFlags = new();

                for (int i = 0; i < faceCount; i++)
                {
                    this.FaceFlags.Add((BucketGridFaceFlag)br.ReadByte());
                }
            }
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write(this.MinX);
            bw.Write(this.MinZ);

            bw.Write(this.MaxX);
            bw.Write(this.MaxZ);

            bw.Write(this.MaxOutStickX);
            bw.Write(this.MaxOutStickZ);

            bw.Write(this.BucketSizeX);
            bw.Write(this.BucketSizeZ);

            ushort bucketsPerSide = (ushort)Math.Sqrt(this.Buckets.Length);
            bw.Write(bucketsPerSide);
            bw.Write(this.DisableBucketGrid);

            BucketGridFlags flags = ComposeFlags();
            bw.Write((byte)flags);

            bw.Write(this.Vertices.Count);
            bw.Write(this.Indices.Count);

            // If bucket grid is disabled, do not write data
            if (this.DisableBucketGrid)
            {
                return;
            }

            foreach (Vector3 vertex in this.Vertices)
            {
                bw.WriteVector3(vertex);
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

            if (flags.HasFlag(BucketGridFlags.HasFaceFlags) && this.FaceFlags is not null)
            {
                if (this.FaceFlags.Count != this.Indices.Count / 3)
                {
                    throw new InvalidOperationException(
                        $"{nameof(this.FaceFlags)}.Count is invalid, must be {nameof(this.Indices)}.Count / 3"
                    );
                }

                foreach (BucketGridFaceFlag faceFlag in this.FaceFlags)
                {
                    bw.Write((byte)faceFlag);
                }
            }
        }

        private BucketGridFlags ComposeFlags()
        {
            BucketGridFlags flags = 0;

            if (this.FaceFlags is not null)
            {
                flags |= BucketGridFlags.HasFaceFlags;
            }

            return flags;
        }
    }

    [Flags]
    public enum BucketGridFlags : byte
    {
        HasFaceFlags = 1
    }

    [Flags]
    public enum BucketGridFaceFlag : byte { }
}
