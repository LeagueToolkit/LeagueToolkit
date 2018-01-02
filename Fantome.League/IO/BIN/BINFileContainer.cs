using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileContainer : IBINFileValue
    {
        public object Parent { get; private set; }
        public BINFileValueType EntryType { get; private set; }
        public List<BINFileValue> Entries { get; private set; } = new List<BINFileValue>();

        public BINFileContainer(BinaryReader br, object parent)
        {
            this.Parent = parent;

            this.EntryType = (BINFileValueType)br.ReadByte();
            uint size = br.ReadUInt32();
            uint entryCount = br.ReadUInt32();

            for (int i = 0; i < entryCount; i++)
            {
                this.Entries.Add(new BINFileValue(br, this, this.EntryType));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((byte)this.EntryType);
            bw.Write(GetContentSize());
            bw.Write(this.Entries.Count);

            foreach (BINFileValue value in this.Entries)
            {
                value.Write(bw, false);
            }
        }

        public uint GetContentSize()
        {
            uint size = 4;
            foreach (BINFileValue value in this.Entries)
            {
                size += value.GetSize();
            }
            return size;
        }

        public uint GetSize()
        {
            return 1 + 4 + GetContentSize();
        }
    }
}
