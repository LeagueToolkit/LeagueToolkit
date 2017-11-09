using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOSubmesh
    {
        public uint Unknown1 { get; set; }
        public string Name { get; set; }
        public uint StartIndex { get; set; }
        public uint IndexCount { get; set; }
        public uint StartVertex { get; set; }
        public uint VertexCount { get; set; }

        public MGEOSubmesh(BinaryReader br)
        {
            this.Unknown1 = br.ReadUInt32();
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.StartIndex = br.ReadUInt32();
            this.IndexCount = br.ReadUInt32();
            this.StartVertex = br.ReadUInt32();
            this.VertexCount = br.ReadUInt32();
        }
    }
}
