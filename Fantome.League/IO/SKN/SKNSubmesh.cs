using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.League.IO.SKN
{
    public class SKNSubmesh
    {
        public string Name { get; private set; }
        public UInt32 StartVertex { get; private set; }
        public UInt32 VertexCount { get; private set; }
        public UInt32 StartIndex { get; private set; }
        public UInt32 IndexCount { get; private set; }
        public SKNSubmesh(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(64)).Replace("\0", "");
            this.StartVertex = br.ReadUInt32();
            this.VertexCount = br.ReadUInt32();
            this.StartIndex = br.ReadUInt32();
            this.IndexCount = br.ReadUInt32();
        }
        public void Write(BinaryWriter bw, List<SKNSubmesh> Submeshes)
        {
            bw.Write(this.Name.PadRight(64, '\u0000').ToCharArray());
            bw.Write(StartVertex);
            bw.Write(VertexCount);
            bw.Write(StartIndex);
            bw.Write(IndexCount);
        }
    }
}
