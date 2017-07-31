using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFile
    {
        public List<String> LinkedFiles { get; private set; } = new List<String>();
        public List<BINFileEntry> Entries { get; private set; } = new List<BINFileEntry>();
        public BINFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "PROP")
                {
                    throw new Exception("Not a valid BIN file");
                }

                uint version = br.ReadUInt32();
                if (version >= 2)
                {
                    uint linkedFileCount = br.ReadUInt32();
                    for (int i = 0; i < linkedFileCount; i++)
                    {
                        this.LinkedFiles.Add(Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())));
                    }
                }

                uint entryCount = br.ReadUInt32();
                for (int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileEntry(br));
                }
                foreach (BINFileEntry entry in this.Entries)
                {
                    entry.ReadData(br);
                }
            }
        }

        public void Write(string fileLocation)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(fileLocation, FileMode.Create)))
            {
                bw.Write(Encoding.ASCII.GetBytes("PROP"));
                bw.Write((uint)2);
                bw.Write((uint)this.LinkedFiles.Count);
                foreach (string linkedFile in this.LinkedFiles)
                {
                    bw.Write((ushort)linkedFile.Length);
                    bw.Write(Encoding.ASCII.GetBytes(linkedFile));
                }

                bw.Write((uint)this.Entries.Count);
                foreach (BINFileEntry entry in this.Entries)
                {
                    bw.Write(entry.Type);
                }
                foreach (BINFileEntry entry in this.Entries)
                {
                    entry.Write(bw);
                }
            }
        }
    }
}
