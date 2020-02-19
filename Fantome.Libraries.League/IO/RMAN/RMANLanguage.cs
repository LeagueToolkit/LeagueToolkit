using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.Libraries.League.IO.RMAN
{
    public class RMANLanguage
    {
        public uint ID { get; private set; }
        public string Name { get; private set; }

        public RMANLanguage(BinaryReader br)
        {
            uint trash1 = br.ReadUInt32();
            this.ID = br.ReadUInt32();

            uint nameOffset = br.ReadUInt32();
            long returnOffset = br.BaseStream.Position;

            br.BaseStream.Seek(returnOffset + nameOffset - 4, SeekOrigin.Begin);
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            br.BaseStream.Seek(returnOffset, SeekOrigin.Begin);
        }
    }
}
