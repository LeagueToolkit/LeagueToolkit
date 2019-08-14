using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOSubmesh
    {
        public MGEOObject Parent { get; internal set; }
        public uint Unknown { get; internal set; } = 0;
        public string Name { get; set; }
        public int StartIndex { get; internal set; }
        public uint IndexCount { get; internal set; }
        public uint StartVertex { get; internal set; }
        public uint EndVertex { get; internal set; }

        public MGEOSubmesh(BinaryReader br, MGEOObject parent)
        {
            this.Parent = parent;
            this.Unknown = br.ReadUInt32();
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.StartIndex = br.ReadInt32();
            this.IndexCount = br.ReadUInt32();
            this.StartVertex = br.ReadUInt32();
            this.EndVertex = br.ReadUInt32() + 1;

            if (this.StartVertex != 0)
            {
                this.StartVertex--;
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Unknown);
            bw.Write(this.Name.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Name));
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
            bw.Write((this.StartVertex == 0) ? this.StartVertex : this.StartVertex + 1); //edit later
            bw.Write(this.EndVertex - 1); //edit later
        }
    }
}
