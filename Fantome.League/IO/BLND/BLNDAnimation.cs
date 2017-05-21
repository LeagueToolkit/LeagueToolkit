using System;
using System.IO;
using static Utilities;

namespace Fantome.League.IO.BLND
{
    public class BLNDAnimation
    {
        public string Name   { get; private set; }
        public UInt32 Hash   { get; private set; }
        public UInt32 Offset { get; private set; }
        public BLNDAnimation(BinaryReader br)
        {
            Hash = br.ReadUInt32();
            Offset = (UInt32)br.BaseStream.Position;
            Offset += br.ReadUInt32();
            uint returnOffset = (uint)br.BaseStream.Position;
            br.Seek(Offset, SeekOrigin.Begin);
            Name = br.ReadString(4);
            br.Seek(returnOffset, SeekOrigin.Begin);
        }
    }
}
