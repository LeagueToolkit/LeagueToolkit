using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOBucket
    {
        public float MaxStickOutX { get; set; }
        public float MaxStickOutY { get; set; }
        public uint StartIndex { get; set; }
        public uint Vertex { get; set; }
        public ushort InsideFaceCount { get; set; }
        public ushort StickingOutFaceCount { get; set; }

        public MGEOBucket(BinaryReader br)
        {
            this.MaxStickOutX = br.ReadSingle();
            this.MaxStickOutY = br.ReadSingle();
            this.StartIndex = br.ReadUInt32();
            this.Vertex = br.ReadUInt32();
            this.InsideFaceCount = br.ReadUInt16();
            this.StickingOutFaceCount = br.ReadUInt16();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.MaxStickOutX);
            bw.Write(this.MaxStickOutY);
            bw.Write(this.StartIndex);
            bw.Write(this.Vertex);
            bw.Write(this.InsideFaceCount);
            bw.Write(this.StickingOutFaceCount);
        }
    }
}
