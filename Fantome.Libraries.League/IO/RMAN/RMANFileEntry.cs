using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.Libraries.League.IO.RMAN
{
    public class RMANFileEntry
    {
        public string Name { get; private set; }
        public string Link { get; private set; }
        public ulong ID { get; private set; }
        public ulong DirectoryID { get; private set; }
        public uint FileSize { get; private set; }
        public List<int> LanguageIDs { get; private set; } = new List<int>();
        public List<ulong> ChunkIDs { get; private set; } = new List<ulong>();

        public RMANFileEntry(BinaryReader br)
        {
            uint trash1 = br.ReadUInt32();
            long position = br.BaseStream.Position;
            uint flags = br.ReadUInt32();
            uint nameOffset = 0;

            if (flags == 0x00010200 || (flags >> 24) != 0)
            {
                nameOffset = br.ReadUInt32();
            }
            else
            {
                nameOffset = flags - 4;
            }

            uint structureSize = br.ReadUInt32();
            uint linkOffset = br.ReadUInt32();
            this.ID = br.ReadUInt64();

            if (structureSize > 28)
            {
                this.DirectoryID = br.ReadUInt64();
            }

            this.FileSize = br.ReadUInt32();
            uint permissions = br.ReadUInt32();

            if (structureSize > 36)
            {
                ulong languageMask = br.ReadUInt64();

                for (int i = 0; i < 64; i++)
                {
                    if ((languageMask & (1UL << i)) != 0)
                    {
                        this.LanguageIDs.Add(i);
                    }
                }
            }

            uint unknown1 = br.ReadUInt32();
            if (unknown1 != 0) throw new Exception("Unknown1: " + unknown1);

            uint chunkCount = br.ReadUInt32();
            for (int i = 0; i < chunkCount; i++)
            {
                this.ChunkIDs.Add(br.ReadUInt64());
            }

            br.BaseStream.Seek(nameOffset + 4 + position, SeekOrigin.Begin);
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

            br.BaseStream.Seek(linkOffset + 12 + position, SeekOrigin.Begin);
            this.Link = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
        }
    }

    public enum RMANFileEntryType
    {
        WAD = 2
    }
}
