using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileStructure : IBINFileValue
    {
        public object Parent { get; private set; }
        public uint Property { get; private set; }
        public List<BINFileValue> Entries { get; private set; } = new List<BINFileValue>();

        public BINFileStructure(BinaryReader br, object parent)
        {
            this.Parent = parent;

            this.Property = br.ReadUInt32();
            if (this.Property == 0)
                return;

            uint size = br.ReadUInt32();
            ushort entryCount = br.ReadUInt16();

            for (int i = 0; i < entryCount; i++)
            {
                this.Entries.Add(new BINFileValue(br, this));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Property);
            if (this.Property == 0)
                return;

            bw.Write(GetContentSize());
            bw.Write((ushort)this.Entries.Count);

            foreach (BINFileValue value in this.Entries)
            {
                value.Write(bw, true);
            }
        }

        public uint GetContentSize()
        {
            uint size = 2;
            foreach (BINFileValue value in this.Entries)
            {
                size += value.GetSize();
            }
            return size;
        }

        public uint GetSize()
        {
            return (this.Property == 0) ? 4 : 4 + 4 + GetContentSize();
        }
    }
}
