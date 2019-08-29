using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINOptionalData : IBINValue, IEquatable<BINOptionalData>
    {
        public IBINValue Parent { get; private set; }
        public BINFileValueType EntryType { get; private set; }
        public List<BINValue> Values { get; private set; } = new List<BINValue>();
        public BINValue this[uint hash] => throw new NotImplementedException();
        public BINValue this[string path] => throw new NotImplementedException();

        public BINOptionalData(BinaryReader br, IBINValue parent)
        {
            this.Parent = parent;
            this.EntryType = (BINFileValueType)br.ReadByte();
            byte valueCount = br.ReadByte();

            for (int i = 0; i < valueCount; i++)
            {
                this.Values.Add(new BINValue(br, this, this.EntryType));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((byte)this.EntryType);
            bw.Write((byte)this.Values.Count);

            foreach (BINValue value in this.Values)
            {
                value.Write(bw, false);
            }
        }

        public uint GetSize()
        {
            uint size = 2;
            foreach (BINValue value in this.Values)
            {
                size += value.GetSize();
            }
            return size;
        }

        public string GetPath(bool excludeEntry = true)
        {
            return this.Parent.GetPath(excludeEntry);
        }

        public bool Equals(BINOptionalData other)
        {
            if(this.EntryType != other.EntryType || this.Values.Count != other.Values.Count)
            {
                return false;
            }
            else
            {
                foreach (BINValue binValue in this.Values)
                {
                    if (!other.Values.Contains(binValue))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
