using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileEntry
    {
        public UInt32 Type { get; private set; }
        public UInt32 Property { get; private set; }
        public List<BINFileValue> Values { get; private set; } = new List<BINFileValue>();
        public BINFileEntry(BinaryReader br)
        {
            this.Type = br.ReadUInt32();
        }

        public void ReadData(BinaryReader br)
        {
            uint length = br.ReadUInt32();
            this.Property = br.ReadUInt32();
            ushort valueCount = br.ReadUInt16();
            for (int i = 0; i < valueCount; i++)
            {
                this.Values.Add(new BINFileValue(br, this));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(GetSize());
            bw.Write(this.Property);
            bw.Write((ushort)this.Values.Count);
            foreach (BINFileValue value in this.Values)
            {
                value.Write(bw, true);
            }
        }

        public int GetSize()
        {
            int size = 0;
            foreach (BINFileValue value in this.Values)
            {
                size += value.GetSize();
            }

            return size + 4 + 2;
        }
    }
}
