using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.League.IO.BLND
{
    public class BLNDUnknown
    {
        public UInt32 Size     { get; private set; }
        public UInt32 Hash     { get; private set; }
        public UInt32 Unknown1 { get; private set; }
        public UInt32 Unknown2 { get; private set; }
        public UInt32 Index    { get; private set; }
        public UInt32 Unknown3 { get; private set; }
        public UInt32 Unknown4 { get; private set; }
        public UInt32 Unknown5 { get; private set; }
        public string Name     { get; private set; }
        public byte[] Data     { get; private set; }
        public BLNDUnknown(BinaryReader br, UInt32 offset, UInt32 returnOffset)
        {
            br.Seek(returnOffset + offset, SeekOrigin.Begin);
            Size = br.ReadUInt32();
            Hash = br.ReadUInt32();
            Unknown1 = br.ReadUInt32();
            Unknown2 = br.ReadUInt32();
            Index = br.ReadUInt32();
            Unknown3 = br.ReadUInt32();
            Unknown4 = br.ReadUInt32();
            Unknown5 = br.ReadUInt32();
            Data = br.ReadBytes((int)Size);
            br.Seek(returnOffset + 4, SeekOrigin.Begin);
        }
    }
}
