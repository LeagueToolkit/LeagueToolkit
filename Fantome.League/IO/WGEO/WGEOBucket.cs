using System;
using System.IO;

namespace Fantome.Libraries.League.IO.WGEO
{
    public class WGEOBucket
    {
        public float MaxStickOutX { get; private set; }
        public float MaxStickOutY { get; private set; }
        public UInt32 StartIndex { get; private set; }
        public UInt32 Vertex { get; private set; }
        public UInt16 InsideFaceCount { get; private set; }
        public UInt16 StickingOutFaceCount { get; private set; }

        public WGEOBucket(BinaryReader br)
        {
            MaxStickOutX = br.ReadSingle();
            MaxStickOutY = br.ReadSingle();
            StartIndex = br.ReadUInt32();
            Vertex = br.ReadUInt32();
            InsideFaceCount = br.ReadUInt16();
            StickingOutFaceCount = br.ReadUInt16();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(MaxStickOutX);
            bw.Write(MaxStickOutY);
            bw.Write(StartIndex);
            bw.Write(Vertex);
            bw.Write(InsideFaceCount);
            bw.Write(StickingOutFaceCount);
        }
    }
}
