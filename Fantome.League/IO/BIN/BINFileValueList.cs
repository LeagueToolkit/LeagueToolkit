using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileValueList
    {
        public UInt32 Property { get; private set; }
        public BINFileValueType Type { get; private set; }
        public List<BINFileValue> Entries { get; private set; } = new List<BINFileValue>();
        public BINFileValueList(BinaryReader br, BINFileValueType type)
        {
            this.Type = type;
            if(this.Type == BINFileValueType.LargeStaticTypeList)
            {
                BINFileValueType valueType = (BINFileValueType)br.ReadByte();
                uint entrySize = br.ReadUInt32();
                uint entryCount = br.ReadUInt32();
                for(int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileValue(br, this, valueType));
                }
            }
            else if (this.Type == BINFileValueType.SmallStaticTypeList)
            {
                BINFileValueType valueType = (BINFileValueType)br.ReadByte();
                Byte entryCount = br.ReadByte();
                for (int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileValue(br, this, valueType));
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
            else if(this.Type == BINFileValueType.DoubleTypeList)
            {
                BINFileValueType[] entryTypes = new BINFileValueType[] { (BINFileValueType)br.ReadByte(), (BINFileValueType)br.ReadByte() };
                uint entrySize = br.ReadUInt32();
                uint entryCount = br.ReadUInt32();
                for(int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileValue(br, this, entryTypes[0]));
                    this.Entries.Add(new BINFileValue(br, this, entryTypes[1]));
                }
            }
        }
    }
}
