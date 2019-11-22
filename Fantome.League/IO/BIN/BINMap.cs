using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINMap : IBINValue, IEquatable<BINMap>
    {
        public IBINValue Parent { get; private set; }
        public BINValueType KeyType { get; private set; }
        public BINValueType ValueType { get; private set; }
        public Dictionary<BINValue, BINValue> Values { get; private set; } = new Dictionary<BINValue, BINValue>();
        public BINValue this[uint hash] => throw new NotImplementedException();
        public BINValue this[string path] => throw new NotImplementedException();
        public BINValue this[BINValue value]
        {
            get
            {
                return this.Values[value];
            }
        }

        public BINMap(IBINValue parent, BINValueType keyType, BINValueType valueType, Dictionary<BINValue, BINValue> values)
        {
            this.Parent = parent;
            this.KeyType = keyType;
            this.ValueType = valueType;
            this.Values = values;
        }

        public BINMap(BinaryReader br, IBINValue parent)
        {
            this.Parent = parent;

            this.KeyType = BINValue.ByteToBINValueType(br.ReadByte());
            this.ValueType = BINValue.ByteToBINValueType(br.ReadByte());
            uint size = br.ReadUInt32();
            uint valueCount = br.ReadUInt32();

            for (int i = 0; i < valueCount; i++)
            {
                this.Values.Add(new BINValue(br, this, this.KeyType), new BINValue(br, this, this.ValueType));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((byte)this.KeyType);
            bw.Write((byte)this.ValueType);
            bw.Write(GetContentSize());
            bw.Write(this.Values.Count);

            foreach (KeyValuePair<BINValue, BINValue> pair in this.Values)
            {
                pair.Key.Write(bw, false);
                pair.Value.Write(bw, false);
            }
        }

        public uint GetContentSize()
        {
            uint size = 4;
            foreach (KeyValuePair<BINValue, BINValue> pair in this.Values)
            {
                size += pair.Key.GetSize() + pair.Value.GetSize();
            }
            return size;
        }

        public uint GetSize()
        {
            return 1 + 1 + 4 + GetContentSize();
        }

        public string GetPath(bool excludeEntry = true)
        {
            return this.Parent.GetPath(excludeEntry);
        }

        public bool Equals(BINMap other)
        {
            if (this.KeyType != other.KeyType || this.ValueType != other.ValueType || this.Values.Count != other.Values.Count)
            {
                return false;
            }
            else
            {
                foreach (KeyValuePair<BINValue, BINValue> pair in this.Values)
                {
                    if (!other.Values.ContainsKey(pair.Key) || !other.Values.ContainsValue(pair.Value))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
