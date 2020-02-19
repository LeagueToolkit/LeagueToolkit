using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.Libraries.League.IO.RMAN
{
    public class RMANDirectory
    {
        public string Name { get; private set; }
        public ulong DirectoryID { get; private set; }
        public ulong? ParentID { get; private set; } = null;

        public RMANDirectory(BinaryReader br)
        {
            int offsetTableOffset = br.ReadInt32();
            long position = br.BaseStream.Position;

            br.BaseStream.Seek(position - offsetTableOffset, SeekOrigin.Begin);

            ushort directoryIDOffset = br.ReadUInt16();
            ushort parentIDOffset = br.ReadUInt16();

            br.BaseStream.Seek(position, SeekOrigin.Begin);

            uint nameOffset = br.ReadUInt32();

            if (directoryIDOffset > 0)
            {
                this.DirectoryID = br.ReadUInt64();
            }
            if(parentIDOffset > 0)
            {
                this.ParentID = br.ReadUInt64();
            }

            br.BaseStream.Seek(position + nameOffset, SeekOrigin.Begin);
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
        }
    }
}
