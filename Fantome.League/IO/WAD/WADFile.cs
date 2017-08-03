using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Fantome.Libraries.League.Helpers.Exceptions;

namespace Fantome.Libraries.League.IO.WAD
{
    public class WADFile
    {
        public byte[] ECDSA { get; private set; }
        public List<WADEntry> Files { get; private set; } = new List<WADEntry>();

        public WADFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(2));
                if (Magic != "RW")
                    throw new InvalidFileMagicException();

                byte Major = br.ReadByte();
                byte Minor = br.ReadByte();
                if (Major > 2 || Minor > 0)
                    throw new UnsupportedFileVersionException();
                if (Major == 2 && Minor == 00)
                {
                    byte EcdsaLength = br.ReadByte();
                    this.ECDSA = br.ReadBytes(EcdsaLength);
                    br.ReadBytes(83 - EcdsaLength);
                }

                UInt64 Checksum = br.ReadUInt64();

                UInt16 TOCStartOffset = br.ReadUInt16();
                UInt16 TOCFileEntrySize = br.ReadUInt16();
                UInt32 FileCount = br.ReadUInt32();

                br.BaseStream.Seek(TOCStartOffset, SeekOrigin.Begin);

                for (int i = 0; i < FileCount; i++)
                {
                    Files.Add(new WADEntry(br, Major, Minor));
                }
                foreach (WADEntry Entry in Files)
                {
                    Entry.ReadData(br);
                }
            }
        }
    }
}
