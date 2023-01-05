using System.IO;

namespace LeagueToolkit.Core.SceneGraph
{
    public struct BucketGridBucket
    {
        public float MaxStickOutX;
        public float MaxStickOutZ;
        public uint StartIndex;
        public uint BaseVertex;
        public ushort InsideFaceCount;
        public ushort StickingOutFaceCount;

        internal BucketGridBucket(BinaryReader br)
        {
            this.MaxStickOutX = br.ReadSingle();
            this.MaxStickOutZ = br.ReadSingle();
            this.StartIndex = br.ReadUInt32();
            this.BaseVertex = br.ReadUInt32();
            this.InsideFaceCount = br.ReadUInt16();
            this.StickingOutFaceCount = br.ReadUInt16();
        }

        internal void Write(BinaryWriter bw)
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
