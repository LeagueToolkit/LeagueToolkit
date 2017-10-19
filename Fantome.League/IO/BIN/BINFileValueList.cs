using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    /// <summary>
    /// Represents a list of <see cref="BINFileValue"/>
    /// </summary>
    public class BINFileValueList
    {
        /// <summary>
        /// The hash of the name of this <see cref="BINFileValueList"/>
        /// </summary>
        /// <remarks>Will be <see cref="null"/> if <see cref="Type"/> is anything else than <c>BINFileValueType.List</c> or <c>BINFileValueType.List2</c></remarks>
        public uint Property { get; private set; }
        /// <summary>
        /// Type of this <see cref="BINFileValueList"/>
        /// </summary>
        public BINFileValueType Type { get; private set; }
        /// <summary>
        /// Types of the entries this <see cref="BINFileValueList"/> contains
        /// </summary>
        public BINFileValueType[] EntryTypes { get; private set; }
        /// <summary>
        /// A Collection of <see cref="BINFileValue"/>
        /// </summary>
        public List<object> Entries { get; private set; } = new List<object>();

        /// <summary>
        /// Initializes a new <see cref="BINFileValueList"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="type">What type this <see cref="BINFileValueList"/> should be</param>
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
            else if (this.Type == BINFileValueType.PairList)
            {
                this.EntryTypes = new BINFileValueType[] { (BINFileValueType)br.ReadByte(), (BINFileValueType)br.ReadByte() };
                uint entrySize = br.ReadUInt32();
                uint entryCount = br.ReadUInt32();
                for (int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINFileValuePair(br, this.EntryTypes[0], this.EntryTypes[1]));
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="BINFileValueList"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            if (this.Type == BINFileValueType.LargeStaticTypeList)
            {
                bw.Write((byte)this.EntryTypes[0]);
                bw.Write(GetContentSize());
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
                bw.Write(GetContentSize());
                bw.Write((ushort)this.Entries.Count);
                foreach (BINFileValue value in this.Entries)
                {
                    value.Write(bw, true);
                }
            }
            else if (this.Type == BINFileValueType.PairList)
            {
                bw.Write((byte)this.EntryTypes[0]);
                bw.Write((byte)this.EntryTypes[1]);
                bw.Write(GetContentSize());
                bw.Write(this.Entries.Count * 2);
                foreach (BINFileValuePair valuePair in this.Entries)
                {
                    valuePair.Write(bw, false);
                }
            }
        }


        private int GetContentSize()
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
            else if (this.Type == BINFileValueType.PairList)
            {
                size += 4;
            }

            foreach (object value in this.Entries)
            {
                if(value is BINFileValue)
                {
                    size += (value as BINFileValue).GetSize();
                }
                else
                {
                    size += (value as BINFileValuePair).GetSize();
                }
            }

            return size;
        }

        /// <summary>
        /// Gets the size of this <see cref="BINFileValueList"/> in bytes
        /// </summary>
        public int GetSize()
        {
            int size = this.GetContentSize();

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
            else if (this.Type == BINFileValueType.PairList)
            {
                size += 1 + 1 + 4;
            }

            return size;
        }
    }
}
