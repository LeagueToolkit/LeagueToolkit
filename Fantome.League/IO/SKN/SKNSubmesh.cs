using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.SKN
{
    [DebuggerDisplay("[ {Name} ]")]
    public class SKNSubmesh
    {
        public string Name { get; private set; }
        public UInt32 StartVertex { get; private set; }
        public UInt32 VertexCount { get; private set; }
        public UInt32 StartIndex { get; private set; }
        public UInt32 IndexCount { get; private set; }

        public SKNSubmesh(string Name, UInt32 StartVertex, UInt32 VertexCount, UInt32 StartIndex, UInt32 IndexCount)
        {
            this.Name = Name;
            this.StartVertex = StartVertex;
            this.VertexCount = VertexCount;
            this.StartIndex = StartIndex;
            this.IndexCount = IndexCount;
        }

        public SKNSubmesh(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(64)).Replace("\0", "");
            this.StartVertex = br.ReadUInt32();
            this.VertexCount = br.ReadUInt32();
            this.StartIndex = br.ReadUInt32();
            this.IndexCount = br.ReadUInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Name.PadRight(64, '\u0000').ToCharArray());
            bw.Write(this.StartVertex);
            bw.Write(this.VertexCount);
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
        }
    }
}
