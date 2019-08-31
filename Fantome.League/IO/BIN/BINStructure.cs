using Fantome.Libraries.League.Helpers.BIN;
using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINStructure : IBINValue, IEquatable<BINStructure>
    {
        public IBINValue Parent { get; private set; }
        public uint Property { get; private set; }
        public List<BINValue> Values { get; private set; } = new List<BINValue>();
        public BINValue this[string path] => throw new NotImplementedException();
        public BINValue this[uint hash]
        {
            get
            {
                return this.Values.Find(x => x.Property == hash);
            }
        }

        public BINStructure(BinaryReader br, BINValue parent)
        {
            this.Parent = parent;

            this.Property = br.ReadUInt32();
            if (this.Property == 0)
            {
                return;
            }
            
            uint size = br.ReadUInt32();
            ushort valueCount = br.ReadUInt16();

            for (int i = 0; i < valueCount; i++)
            {
                this.Values.Add(new BINValue(br, this));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Property);
            if (this.Property == 0)
            {
                return;
            }

            bw.Write(GetContentSize());
            bw.Write((ushort)this.Values.Count);

            foreach (BINValue value in this.Values)
            {
                value.Write(bw, true);
            }
        }

        public uint GetContentSize()
        {
            uint size = 2;
            foreach (BINValue value in this.Values)
            {
                size += value.GetSize();
            }
            return size;
        }

        public uint GetSize()
        {
            return (this.Property == 0) ? 4 : 4 + 4 + GetContentSize();
        }

        public string GetPath(bool excludeEntry = true)
        {
            string path = "";

            if(this.Parent.Parent is BINContainer)
            {
                BINContainer container = this.Parent.Parent as BINContainer;
                path = string.Format("{0}/{1}[{2}]", this.Parent.Parent.GetPath(excludeEntry), BINHelper.GetClass(this.Property), container.Values.IndexOf(this.Parent as BINValue));

            }
            else
            {
                path = string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), BINHelper.GetClass(this.Property));
            }

            return path;
        }

        public bool Equals(BINStructure other)
        {
            if(this.Property != other.Property || this.Values.Count != other.Values.Count)
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
