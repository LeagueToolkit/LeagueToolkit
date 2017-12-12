using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.LeagueSoundbank
{
    public class LeagueSoundbankFile
    {
        public List<LeagueSoundbankEntry> Entries { get; private set; } = new List<LeagueSoundbankEntry>();

        public LeagueSoundbankFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public LeagueSoundbankFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "r3d2")
                {
                    throw new Exception("This is not a valid WPK file");
                }

                uint version = br.ReadUInt32();
                if (version != 1)
                {
                    throw new Exception("Version: " + version + " of WPK files is not supported");
                }

                uint entryCount = br.ReadUInt32();
                for(int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new LeagueSoundbankEntry(br));
                }
            }
        }


        public void Write(string fileLocation)
        {
            Write(File.OpenWrite(fileLocation));
        }

        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(Encoding.ASCII.GetBytes("r3d2"));
                bw.Write(1);
                bw.Write(this.Entries.Count);

                for (int i = 0; i < this.Entries.Count; i++)
                {
                    bw.Write(16 + this.Entries.Count * 4 + i * 40);
                }

                long metaOffset = bw.BaseStream.Position;
                bw.BaseStream.Seek(40 * this.Entries.Count, SeekOrigin.Current);

                foreach (LeagueSoundbankEntry entry in this.Entries)
                {
                    entry._dataOffset = (uint)bw.BaseStream.Position;
                    bw.Write(entry.Data);
                }

                bw.BaseStream.Seek(metaOffset, SeekOrigin.Begin);
                foreach (LeagueSoundbankEntry entry in this.Entries)
                {
                    entry.Write(bw);
                }
            }
        }
    }
}
