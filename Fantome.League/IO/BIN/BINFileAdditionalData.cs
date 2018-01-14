using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileAdditionalData : IBINFileValue
    {
        public object Parent { get; private set; }
        public BINFileValueType EntryType { get; private set; }
        public List<BINFileValue> Entries { get; private set; } = new List<BINFileValue>();

        public BINFileAdditionalData(BinaryReader br, object parent)
        {
            this.EntryType = (BINFileValueType)br.ReadByte();
            byte entryCount = br.ReadByte();

            for (int i = 0; i < entryCount; i++)
            {
                this.Entries.Add(new BINFileValue(br, this, this.EntryType));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((byte)this.EntryType);
            bw.Write((byte)this.Entries.Count);

            foreach (BINFileValue value in this.Entries)
            {
                value.Write(bw, false);
            }
        }

        public uint GetSize()
        {
            uint size = 2;
            foreach (BINFileValue value in this.Entries)
            {
                size += value.GetSize();
            }
            return size;
        }
    }
}
