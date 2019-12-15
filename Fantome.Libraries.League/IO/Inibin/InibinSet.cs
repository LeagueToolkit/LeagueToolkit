using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fantome.Libraries.League.IO.Inibin
{
    /// <summary>
    /// Represents a set of values inside of a <see cref="InibinFile"/>
    /// </summary>
    public class InibinSet
    {
        /// <summary>
        /// Type of the values of this <see cref="InibinSet"/>
        /// </summary>
        public InibinFlags Type { get; private set; }

        /// <summary>
        /// Values of this <see cref="InibinSet"/>
        /// </summary>
        public Dictionary<uint, object> Properties { get; private set; } = new Dictionary<uint, object>();

        /// <summary>
        /// Initializes a blank <see cref="InibinSet"/>
        /// </summary>
        public InibinSet() { }

        /// <summary>
        /// Initializes a new <see cref="InibinSet"/> with the specified type
        /// </summary>
        /// <param name="type">Type of this <see cref="InibinSet"/></param>
        public InibinSet(InibinFlags type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new <see cref="InibinSet"/>
        /// </summary>
        /// <param name="type">Type of this <see cref="InibinSet"/></param>
        /// <param name="properties">Values of this <see cref="InibinSet"/></param>
        public InibinSet(InibinFlags type, Dictionary<uint, object> properties)
        {
            this.Type = type;
            this.Properties = properties;
        }

        /// <summary>
        /// Initializes a new <see cref="InibinSet"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="type">The type of this <see cref="InibinSet"/></param>
        public InibinSet(BinaryReader br, InibinFlags type)
        {
            this.Type = type;
            ushort valueCount = br.ReadUInt16();
            List<uint> hashes = new List<uint>();

            for (int i = 0; i < valueCount; i++)
            {
                uint hash = br.ReadUInt32();
                this.Properties.Add(hash, null);
                hashes.Add(hash);
            }

            byte boolean = 0;
            for (int i = 0; i < valueCount; i++)
            {
                if (this.Type == InibinFlags.Int32List)
                {
                    this.Properties[hashes[i]] = br.ReadInt32();
                }
                else if (this.Type == InibinFlags.Float32List)
                {
                    this.Properties[hashes[i]] = br.ReadSingle();
                }
                else if (this.Type == InibinFlags.FixedPointFloatList)
                {
                    this.Properties[hashes[i]] = br.ReadByte() * 0.1;
                }
                else if (this.Type == InibinFlags.Int16List)
                {
                    this.Properties[hashes[i]] = br.ReadInt16();
                }
                else if (this.Type == InibinFlags.Int8List)
                {
                    this.Properties[hashes[i]] = br.ReadByte();
                }
                else if (this.Type == InibinFlags.BitList)
                {
                    boolean = (i % 8) == 0 ? br.ReadByte() : (byte)(boolean >> 1);
                    this.Properties[hashes[i]] = Convert.ToBoolean(0x1 & boolean);
                }
                else if (this.Type == InibinFlags.FixedPointFloatListVec3)
                {
                    this.Properties[hashes[i]] = new byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte() };
                }
                else if (this.Type == InibinFlags.Float32ListVec3)
                {
                    this.Properties[hashes[i]] = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
                }
                else if (this.Type == InibinFlags.FixedPointFloatListVec2)
                {
                    this.Properties[hashes[i]] = new byte[] { br.ReadByte(), br.ReadByte() };
                }
                else if (this.Type == InibinFlags.Float32ListVec2)
                {
                    this.Properties[hashes[i]] = new float[] { br.ReadSingle(), br.ReadSingle() };
                }
                else if (this.Type == InibinFlags.FixedPointFloatListVec4)
                {
                    this.Properties[hashes[i]] = new byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() };
                }
                else if (this.Type == InibinFlags.Float32ListVec4)
                {
                    this.Properties[hashes[i]] = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
                }
            }
        }

        /// <summary>
        /// Initializes a new legacy <see cref="InibinSet"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="type">The type of this <see cref="InibinSet"/></param>
        /// <param name="stringOffset">Offset to the string data</param>
        /// <param name="valueCount">Amount of values in this <see cref="InibinSet"/></param>
        public InibinSet(BinaryReader br, InibinFlags type, uint stringOffset, uint? valueCount = null)
        {
            this.Type = type;
            List<uint> hashes = new List<uint>();
            if (valueCount == null)
            {
                valueCount = br.ReadUInt16();

                for (int i = 0; i < valueCount; i++)
                {
                    uint hash = br.ReadUInt32();
                    this.Properties.Add(hash, null);
                    hashes.Add(hash);
                }
            }

            for (int i = 0; i < valueCount; i++)
            {
                uint offset = br.ReadUInt16();
                long oldPos = br.BaseStream.Position;
                br.BaseStream.Seek(offset + stringOffset, SeekOrigin.Begin);

                string s = "";
                char c;
                while ((c = br.ReadChar()) != '\u0000')
                {
                    s += c;
                }

                this.Properties[hashes[i]] = s;
                br.BaseStream.Seek(oldPos, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Writes this <see cref="InibinSet"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write((ushort)this.Properties.Count);

            foreach (uint hash in this.Properties.Keys)
            {
                bw.Write(hash);
            }

            if (this.Type == InibinFlags.BitList)
            {
                List<bool> booleans = this.Properties.Values.Cast<bool>().ToList();
                byte value = 0;

                while ((booleans.Count % 8) != 0)
                {
                    booleans.Add(false);
                }

                for (int i = 0, j = 0; i < this.Properties.Count; i++)
                {
                    if (booleans[i])
                    {
                        value |= (byte)(1 << j);
                    }
                    j++;

                    if (j == 8 || i == this.Properties.Count - 1)
                    {
                        bw.Write(value);
                        j = value = 0;
                    }
                }
            }
            else if (this.Type == InibinFlags.StringList)
            {
                ushort offset = 0;
                foreach (string value in this.Properties.Values)
                {
                    bw.Write(offset);
                    offset += (ushort)(value.Length + 1);
                }
                foreach (string value in this.Properties.Values)
                {
                    bw.Write(Encoding.ASCII.GetBytes(value));
                    bw.Write((byte)0);
                }
            }
            else
            {
                foreach (object value in this.Properties.Values)
                {
                    if (this.Type == InibinFlags.Int32List)
                    {
                        bw.Write((int)value);
                    }
                    else if (this.Type == InibinFlags.Float32List)
                    {
                        bw.Write((float)value);
                    }
                    else if (this.Type == InibinFlags.FixedPointFloatList)
                    {
                        bw.Write(Convert.ToByte((double)value * 10));
                    }
                    else if (this.Type == InibinFlags.Int16List)
                    {
                        bw.Write((short)value);
                    }
                    else if (this.Type == InibinFlags.Int8List)
                    {
                        bw.Write((byte)value);
                    }
                    else if (this.Type == InibinFlags.FixedPointFloatListVec3)
                    {
                        byte[] valueList = (byte[])value;

                        bw.Write(valueList[0]);
                        bw.Write(valueList[1]);
                        bw.Write(valueList[2]);
                    }
                    else if (this.Type == InibinFlags.Float32ListVec3)
                    {
                        float[] valueList = (float[])value;

                        bw.Write(valueList[0]);
                        bw.Write(valueList[1]);
                        bw.Write(valueList[2]);
                    }
                    else if (this.Type == InibinFlags.FixedPointFloatListVec2)
                    {
                        byte[] valueList = (byte[])value;

                        bw.Write(valueList[0]);
                        bw.Write(valueList[1]);
                    }
                    else if (this.Type == InibinFlags.Float32ListVec2)
                    {
                        float[] valueList = (float[])value;

                        bw.Write(valueList[0]);
                        bw.Write(valueList[1]);
                    }
                    else if (this.Type == InibinFlags.FixedPointFloatListVec4)
                    {
                        byte[] valueList = (byte[])value;

                        bw.Write(valueList[0]);
                        bw.Write(valueList[1]);
                        bw.Write(valueList[2]);
                        bw.Write(valueList[3]);
                    }
                    else if (this.Type == InibinFlags.Float32ListVec4)
                    {
                        float[] valueList = (float[])value;

                        bw.Write(valueList[0]);
                        bw.Write(valueList[1]);
                        bw.Write(valueList[2]);
                        bw.Write(valueList[3]);
                    }
                }
            }
        }
    }
}
