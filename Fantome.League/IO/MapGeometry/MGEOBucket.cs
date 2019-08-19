using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOBucket
    {
        public float MaxStickOutX { get; set; }
        public float MaxStickOutZ { get; set; }
        public uint StartIndex { get; set; }
        public uint BaseVertex { get; set; }
        public ushort InsideFaceCount { get; set; }
        public ushort StickingOutFaceCount { get; set; }

        public MGEOBucket(BinaryReader br)
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
