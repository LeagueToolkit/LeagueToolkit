using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.League.IO.BLND
{
    public class BLNDBlend
    {
        public dynamic FromBlend   { get; private set; }
        public dynamic ToBlend     { get; private set; }
        public UInt32  Flag        { get; private set; }
        public float   TimeToBlend { get; private set; }
        public BLNDBlend(BinaryReader br)
        {
            FromBlend = br.ReadUInt32();
            ToBlend = br.ReadUInt32();
            Flag = br.ReadUInt32();
            TimeToBlend = br.ReadSingle();
        }
        public void AssignEntries(BLNDEvent FromBlend, BLNDEvent ToBlend)
        {
            this.FromBlend = FromBlend;
            this.ToBlend = ToBlend;
        }
    }
}
