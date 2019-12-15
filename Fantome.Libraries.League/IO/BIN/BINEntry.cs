using Fantome.Libraries.League.Helpers.BIN;
using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fantome.Libraries.League.IO.BIN
{
    /// <summary>
    /// Represents a <see cref="BINEntry"/> inside of a <see cref="BINFile"/>
    /// </summary>
    public class BINEntry : IBINValue, IEquatable<BINEntry>
    {
        public IBINValue Parent { get; private set; }
        /// <summary>
        /// Hash of the class of this <see cref="BINEntry"/>
        /// </summary>
        public uint Class { get; private set; }
        /// <summary>
        /// Hash of the name of this <see cref="BINEntry"/>
        /// </summary>
        public uint Property { get; private set; }
        /// <summary>
        /// A Collection of <see cref="BINValue"/>
        /// </summary>
        public List<BINValue> Values { get; private set; } = new List<BINValue>();
        public BINValue this[string path]
        {
            get
            {
                string[] properties = path.Split('/');
                string property = properties[0];

                //Build recursive property path
                string nextPath = string.Empty;
                if (properties.Length != 1)
                {
                    for (int i = 1; i < properties.Length; i++)
                    {
                        nextPath += properties[i];

                        if (i + 1 != properties.Length)
                        {
                            nextPath += '/';
                        }
                    }
                }

                //We can assume that the first value property is always going to be normal
                uint hash;
                if (!uint.TryParse(property, out hash))
                {
                    hash = Cryptography.FNV32Hash(property.ToLower());
                }

                //Recursively iterate over to the requested BINValue
                if (nextPath == string.Empty)
                {
                    return this[hash];
                }
                else
                {
                    return this[hash][nextPath];
                }
            }
        }
        public BINValue this[uint hash]
        {
            get
            {
                return this.Values.Find(x => x.Property == hash);
            }
        }

        public BINEntry(string type, string property, List<BINValue> values) 
            : this(Cryptography.FNV32Hash(type.ToLower()), Cryptography.FNV32Hash(property.ToLower()), values)
        {
            
        }

        public BINEntry(uint type, uint property, List<BINValue> values)
        {
            this.Class = type;
            this.Property = property;
            this.Values = values;
        }

        /// <summary>
        /// Initializes a new <see cref="BINEntry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public BINEntry(BinaryReader br)
        {
            this.Class = br.ReadUInt32();
        }

        /// <summary>
        /// Reads the data of this <see cref="BINEntry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public void ReadData(BinaryReader br)
        {
            uint length = br.ReadUInt32();
            this.Property = br.ReadUInt32();
            ushort valueCount = br.ReadUInt16();
            for (int i = 0; i < valueCount; i++)
            {
                this.Values.Add(new BINValue(br, this));
            }
        }

        /// <summary>
        /// Writes this <see cref="BINEntry"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(GetSize());
            bw.Write(this.Property);
            bw.Write((ushort)this.Values.Count);
            foreach (BINValue value in this.Values)
            {
                value.Write(bw, true);
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="BINEntry"/> in bytes
        /// </summary>
        public uint GetSize()
        {
            uint size = 0;
            foreach (BINValue value in this.Values)
            {
                size += value.GetSize();
            }

            return size + 4 + 2;
        }

        public string GetPath(bool excludeEntry = true)
        {
            return BINGlobal.GetEntry(this.Property);
        }

        public bool Equals(BINEntry other)
        {
            if(this.Class != other.Class || this.Property != other.Property || this.Values.Count != other.Values.Count)
            {
                return false;
            }

            foreach(BINValue binValue in this.Values)
            {
                if(!other.Values.Contains(binValue))
                {
                    return false;
                }
            }

            return true;
        }
    }
}