using System;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINOptional : IBINValue, IEquatable<BINOptional>
    {
        public IBINValue Parent { get; private set; }
        public BINValueType Type { get; private set; }
        public BINValue Value { get; private set; }
        public BINValue this[uint hash] => throw new NotImplementedException();
        public BINValue this[string path] => throw new NotImplementedException();

        public BINOptional(BinaryReader br, IBINValue parent)
        {
            this.Parent = parent;
            this.Type = BINValue.ByteToBINValueType(br.ReadByte());;
            byte valueCount = br.ReadByte(); //????

            if(valueCount > 1)
            {
                throw new Exception("Encountered an Optional value with Value Count: " + valueCount);
            }

            if(valueCount == 1)
            {
                this.Value = new BINValue(br, this, this.Type);
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((byte)this.Type);
            bw.Write(this.Value == null ? (byte)0 : (byte)1);
            this.Value?.Write(bw, false);
        }

        public uint GetSize()
        {
            uint size = 2;

            if(this.Value != null)
            {
                size += this.Value.GetSize();
            }

            return size;
        }

        public string GetPath(bool excludeEntry = true)
        {
            return this.Parent.GetPath(excludeEntry);
        }

        public bool Equals(BINOptional other)
        {
            if(this.Type != other.Type)
            {
                return false;
            }
            else
            {
                if(!this.Value.Equals(other.Value))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
