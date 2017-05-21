using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.League.IO.BLND
{
    public class BLNDCategory
    {
        public UInt32 Size  { get; private set; }
        public float  Value { get; private set; }
        public UInt32 Flag  { get; private set; }
        public UInt32 Index { get; private set; }
        public string Name  { get; private set; }
        public BLNDCategory(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Value = br.ReadSingle();
            Flag = br.ReadUInt32();
            Index = br.ReadUInt32();
            Name = Encoding.ASCII.GetString(br.ReadBytes(32)).Replace("\0", "");
        }
    }
}
