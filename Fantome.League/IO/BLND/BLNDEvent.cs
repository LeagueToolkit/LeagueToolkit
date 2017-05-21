using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Utilities;

namespace Fantome.League.IO.BLND
{
    public class BLNDEvent
    {
        public byte[]    Data     { get; private set; }
        public UInt32    Offset   { get; private set; }
        public UInt32    Size     { get; private set; }
        public EventType Flag     { get; private set; }
        public UInt32    Index    { get; private set; }
        public UInt32    Length1  { get; private set; }
        public UInt32    Length2  { get; private set; }
        public string    Name     { get; private set; }
        public BLNDEvent(UInt32 Offset, UInt32 returnOffset, int entryIndex, BinaryReader br)
        {
            this.Offset = (UInt32)(Offset - entryIndex * 4);
            br.Seek(this.Offset, SeekOrigin.Begin);
            Size = br.ReadUInt32();
            Flag = (EventType)br.ReadUInt32();
            Index = br.ReadUInt32();
            Length1 = br.ReadUInt32();
            Length2 = br.ReadUInt32();
            Name = br.ReadString(4);
            Data = br.ReadBytes((int)(Size - 12 - (Length2 - Length1)));
            br.Seek(returnOffset + 4, SeekOrigin.Begin);
        }
        public class ParticleData
        {
            public UInt32       Flag1    { get; private set; }           //2
            public UInt32       Flag2    { get; private set; }           //1
            public UInt32       Unknown3 { get; private set; }           //0
            public UInt32       Length1  { get; private set; }           //36
            public UInt32       Length2  { get; private set; }           //48
            public UInt32       Length3  { get; private set; }           //80
            public UInt32       Length4  { get; private set; }           //92
            public UInt32       Unknown8 { get; private set; }           //-1
            public List<string> Strings  { get; private set; } = new List<string>();
            public ParticleData(BinaryReader br) //lengths of strings get computed as such: Length2 - Length1, Length3 - Length2, Length4 - Length3
            {
                Flag1 = br.ReadUInt32();
                Flag2 = br.ReadUInt32();
                Unknown3 = br.ReadUInt32();
                Length1 = br.ReadUInt32();
                Length2 = br.ReadUInt32();
                Length3 = br.ReadUInt32();
                Length4 = br.ReadUInt32();
                Unknown8 = br.ReadUInt32();
            }
        }

        public enum EventType : UInt32
        {

        }
    }
}
