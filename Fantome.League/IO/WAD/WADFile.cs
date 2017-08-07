using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.WAD
{
    public class WADFile
    {
        public byte[] ECDSA { get; private set; }
        public List<WADEntry> Entries { get; private set; } = new List<WADEntry>();

        public WADFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(2));
                if (magic != "RW")
                {
                    throw new Exception("This is not a valid WAD file");
                }

                byte major = br.ReadByte();
                byte minor = br.ReadByte();
                if (major > 2 || minor > 0)
                {
                    throw new Exception("This version is not supported");
                }
                if (major == 2 && minor == 0)
                {
                    byte ecdsaLength = br.ReadByte();
                    this.ECDSA = br.ReadBytes(ecdsaLength);
                    br.ReadBytes(83 - ecdsaLength);
                }

                ulong unknown = br.ReadUInt64();

                ushort tocStartOffset = br.ReadUInt16();
                ushort tocFileEntrySize = br.ReadUInt16();
                uint fileCount = br.ReadUInt32();

                br.BaseStream.Seek(tocStartOffset, SeekOrigin.Begin);

                for (int i = 0; i < fileCount; i++)
                {
                    Entries.Add(new WADEntry(br, major, minor));
                }
                foreach (WADEntry entry in Entries)
                {
                    entry.ReadData(br);
                }
            }
        }
    }
}
