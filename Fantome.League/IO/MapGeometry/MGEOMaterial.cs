using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOMaterial
    {
        public uint Unknown1 { get; set; }
        public string Name { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
        public uint Unknown4 { get; set; }
        public uint Unknown5 { get; set; }

        public MGEOMaterial(BinaryReader br)
        {
            this.Unknown1 = br.ReadUInt32();
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.Unknown2 = br.ReadUInt32();
            this.Unknown3 = br.ReadUInt32();
            this.Unknown4 = br.ReadUInt32();
            this.Unknown5 = br.ReadUInt32();
        }
    }
}
