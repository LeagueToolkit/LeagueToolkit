using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueToolkit.Helpers.Structures.BucketGrid
{
    public class BucketGridBucket
    {
        public float MaxStickOutX { get; set; }
        public float MaxStickOutZ { get; set; }
        public uint StartIndex { get; set; }
        public uint BaseVertex { get; set; }
        public ushort InsideFaceCount { get; set; }
        public ushort StickingOutFaceCount { get; set; }

        public BucketGridBucket(BinaryReader br)
        {
            this.MaxStickOutX = br.ReadSingle();
            this.MaxStickOutZ = br.ReadSingle();
            this.StartIndex = br.ReadUInt32();
            this.BaseVertex = br.ReadUInt32();
            this.InsideFaceCount = br.ReadUInt16();
            this.StickingOutFaceCount = br.ReadUInt16();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.MaxStickOutX);
            bw.Write(this.MaxStickOutZ);
            bw.Write(this.StartIndex);
            bw.Write(this.BaseVertex);
            bw.Write(this.InsideFaceCount);
            bw.Write(this.StickingOutFaceCount);
        }
    }
}
