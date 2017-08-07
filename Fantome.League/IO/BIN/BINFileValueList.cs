using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileValueList
    {
        public uint Property { get; private set; }
        public BINFileValueType Type { get; private set; }
        public BINFileValueType[] EntryTypes { get; private set; }
        public List<BINFileValue> Entries { get; private set; } = new List<BINFileValue>();
        public BINFileValueList(BinaryReader br, BINFileValueType type)
        {
            this.Type = type;
            if (this.Type == BINFileValueType.LargeStaticTypeList)
            {
                this.EntryTypes = new BINFileValueType[] { (BINFileValueType)br.ReadByte() };
                uint entrySize = br.ReadUInt32();
                uint entryCount = br.ReadUInt32();
                for (int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileValue(br, this, this.EntryTypes[0]));
                }
            }
            else if (this.Type == BINFileValueType.SmallStaticTypeList)
            {
                this.EntryTypes = new BINFileValueType[] { (BINFileValueType)br.ReadByte() };
                byte entryCount = br.ReadByte();
                for (int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileValue(br, this, this.EntryTypes[0]));
                }
            }
            else if (this.Type == BINFileValueType.List || this.Type == BINFileValueType.List2)
            {
                this.Property = br.ReadUInt32();
                uint entrySize = br.ReadUInt32();
                ushort entryCount = br.ReadUInt16();
                for (int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileValue(br, this));
                }
            }
            else if (this.Type == BINFileValueType.DoubleTypeList)
            {
                this.EntryTypes = new BINFileValueType[] { (BINFileValueType)br.ReadByte(), (BINFileValueType)br.ReadByte() };
                uint entrySize = br.ReadUInt32();
                uint entryCount = br.ReadUInt32();
                for (int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileValue(br, this, this.EntryTypes[0]));
                    this.Entries.Add(new BINFileValue(br, this, this.EntryTypes[1]));
                }
            }
        }

        public void Write(BinaryWriter bw)
        {
            if (this.Type == BINFileValueType.LargeStaticTypeList)
            {
                bw.Write((byte)this.EntryTypes[0]);
                bw.Write(GetEntriesSize());
                bw.Write(this.Entries.Count);
                foreach (BINFileValue value in this.Entries)
                {
                    value.Write(bw, false);
                }
            }
            else if (this.Type == BINFileValueType.SmallStaticTypeList)
            {
                bw.Write((byte)this.EntryTypes[0]);
                bw.Write((byte)this.Entries.Count);
                foreach (BINFileValue value in this.Entries)
                {
                    value.Write(bw, false);
                }
            }
            else if (this.Type == BINFileValueType.List || this.Type == BINFileValueType.List2)
            {
                bw.Write(this.Property);
                bw.Write(GetEntriesSize());
                bw.Write((ushort)this.Entries.Count);
                foreach (BINFileValue value in this.Entries)
                {
                    value.Write(bw, true);
                }
            }
            else if (this.Type == BINFileValueType.DoubleTypeList)
            {
                bw.Write((byte)this.EntryTypes[0]);
                bw.Write((byte)this.EntryTypes[1]);
                bw.Write(GetEntriesSize());
                bw.Write(this.Entries.Count);
                foreach (BINFileValue value in this.Entries)
                {
                    value.Write(bw, false);
                }
            }
        }

        private int GetEntriesSize()
        {
            int size = 0;

            if (this.Type == BINFileValueType.LargeStaticTypeList)
            {
                size += 4;
            }
            else if (this.Type == BINFileValueType.List || this.Type == BINFileValueType.List2)
            {
                size += 2;
            }
            else if (this.Type == BINFileValueType.DoubleTypeList)
            {
                size += 4;
            }

            foreach (BINFileValue value in this.Entries)
            {
                size += value.GetSize();
            }

            return size;
        }

        public int GetSize()
        {
            int size = this.GetEntriesSize();

            if (this.Type == BINFileValueType.LargeStaticTypeList)
            {
                size += 1 + 4;
            }
            else if (this.Type == BINFileValueType.SmallStaticTypeList)
            {
                size += 1 + 1;
            }
            else if (this.Type == BINFileValueType.List || this.Type == BINFileValueType.List2)
            {
                size += 4 + 4;
            }
            else if (this.Type == BINFileValueType.DoubleTypeList)
            {
                size += 1 + 1 + 4;
            }

            return size;
        }
    }
}
