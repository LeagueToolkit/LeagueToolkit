using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileMap : IBINFileValue
    {
        public object Parent { get; private set; }
        public BINFileValueType KeyType { get; private set; }
        public BINFileValueType ValueType { get; private set; }
        public List<BINFileValuePair> Entries { get; private set; } = new List<BINFileValuePair>();

        public BINFileMap(object parent, BINFileValueType keyType, BINFileValueType valueType, List<BINFileValuePair> entries)
        {
            this.Parent = parent;
            this.KeyType = keyType;
            this.ValueType = valueType;

            foreach(BINFileValuePair valuePair in entries)
            {
                //valuePair.Pair.Key.SetParent(this);
                //valuePair.Pair.Value.SetParent(this);
                if(valuePair.KeyType != this.KeyType || valuePair.ValueType != this.ValueType)
                {
                    throw new Exception("keyType or valueType do not match with types in entries");
                }
            }
        }

        public BINFileMap(BinaryReader br, object parent)
        {
            this.Parent = parent;

            this.KeyType = (BINFileValueType)br.ReadByte();
            this.ValueType = (BINFileValueType)br.ReadByte();
            uint size = br.ReadUInt32();
            uint entryCount = br.ReadUInt32();

            for (int i = 0; i < entryCount; i++)
            {
                this.Entries.Add(new BINFileValuePair(br, this, this.KeyType, this.ValueType));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((byte)this.KeyType);
            bw.Write((byte)this.ValueType);
            bw.Write(GetContentSize());
            bw.Write(this.Entries.Count);

            foreach(BINFileValuePair pair in this.Entries)
            {
                pair.Write(bw);
            }
        }

        public uint GetContentSize()
        {
            uint size = 4;
            foreach (BINFileValuePair pair in this.Entries)
            {
                size += pair.GetSize();
            }
            return size;
        }

        public uint GetSize()
        {
            return 1 + 1 + 4 + GetContentSize();
        }
    }
}
