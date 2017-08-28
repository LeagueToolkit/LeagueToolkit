using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.Inibin
{
    public class InibinFile
    {
        public Dictionary<InibinFlags, InibinSet> Sets { get; private set; } = new Dictionary<InibinFlags, InibinSet>();

        public InibinFile() { }

        public InibinFile(Dictionary<InibinFlags, InibinSet> sets)
        {
            this.Sets = sets;
        }

        public InibinFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public InibinFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                uint version = br.ReadByte();

                uint stringDataLength = 0;
                InibinFlags flags = 0;
                if (version == 1)
                {
                    br.ReadBytes(3);
                    uint valueCount = br.ReadUInt32();
                    stringDataLength = br.ReadUInt32();

                    this.Sets.Add(InibinFlags.StringList, new InibinSet(br, InibinFlags.StringList, (uint)br.BaseStream.Length - stringDataLength, valueCount));
                }
                else if (version == 2)
                {
                    stringDataLength = br.ReadUInt16();
                    flags = (InibinFlags)br.ReadUInt16();

                    if (flags.HasFlag(InibinFlags.Int32List))
                    {
                        this.Sets.Add(InibinFlags.Int32List, new InibinSet(br, InibinFlags.Int32List));
                    }
                    if (flags.HasFlag(InibinFlags.Float32List))
                    {
                        this.Sets.Add(InibinFlags.Float32List, new InibinSet(br, InibinFlags.Float32List));
                    }
                    if (flags.HasFlag(InibinFlags.FixedPointFloatList))
                    {
                        this.Sets.Add(InibinFlags.FixedPointFloatList, new InibinSet(br, InibinFlags.FixedPointFloatList));
                    }
                    if (flags.HasFlag(InibinFlags.Int16List))
                    {
                        this.Sets.Add(InibinFlags.Int16List, new InibinSet(br, InibinFlags.Int16List));
                    }
                    if (flags.HasFlag(InibinFlags.Int8List))
                    {
                        this.Sets.Add(InibinFlags.Int8List, new InibinSet(br, InibinFlags.Int8List));
                    }
                    if (flags.HasFlag(InibinFlags.BitList))
                    {
                        this.Sets.Add(InibinFlags.BitList, new InibinSet(br, InibinFlags.BitList));
                    }
                    if (flags.HasFlag(InibinFlags.FixedPointFloatListVec3))
                    {
                        this.Sets.Add(InibinFlags.FixedPointFloatListVec3, new InibinSet(br, InibinFlags.FixedPointFloatListVec3));
                    }
                    if (flags.HasFlag(InibinFlags.Float32ListVec3))
                    {
                        this.Sets.Add(InibinFlags.Float32ListVec3, new InibinSet(br, InibinFlags.Float32ListVec3));
                    }
                    if (flags.HasFlag(InibinFlags.FixedPointFloatListVec2))
                    {
                        this.Sets.Add(InibinFlags.FixedPointFloatListVec2, new InibinSet(br, InibinFlags.FixedPointFloatListVec2));
                    }
                    if (flags.HasFlag(InibinFlags.Float32ListVec2))
                    {
                        this.Sets.Add(InibinFlags.Float32ListVec2, new InibinSet(br, InibinFlags.Float32ListVec2));
                    }
                    if (flags.HasFlag(InibinFlags.FixedPointFloatListVec4))
                    {
                        this.Sets.Add(InibinFlags.FixedPointFloatListVec4, new InibinSet(br, InibinFlags.FixedPointFloatListVec4));
                    }
                    if (flags.HasFlag(InibinFlags.Float32ListVec4))
                    {
                        this.Sets.Add(InibinFlags.Float32ListVec4, new InibinSet(br, InibinFlags.Float32ListVec4));
                    }
                    if (flags.HasFlag(InibinFlags.StringList))
                    {
                        this.Sets.Add(InibinFlags.StringList, new InibinSet(br, InibinFlags.StringList, (uint)br.BaseStream.Length - stringDataLength));
                    }
                }
                else
                {
                    throw new Exception("This version is not supported");
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                ushort stringDataLength = 0;
                InibinFlags flags = 0;

                if (this.Sets.ContainsKey(InibinFlags.BitList))
                {
                    flags |= InibinFlags.BitList;
                }
                if (this.Sets.ContainsKey(InibinFlags.FixedPointFloatList))
                {
                    flags |= InibinFlags.FixedPointFloatList;
                }
                if (this.Sets.ContainsKey(InibinFlags.FixedPointFloatListVec2))
                {
                    flags |= InibinFlags.FixedPointFloatListVec2;
                }
                if (this.Sets.ContainsKey(InibinFlags.FixedPointFloatListVec3))
                {
                    flags |= InibinFlags.FixedPointFloatListVec3;
                }
                if (this.Sets.ContainsKey(InibinFlags.FixedPointFloatListVec4))
                {
                    flags |= InibinFlags.FixedPointFloatListVec4;
                }
                if (this.Sets.ContainsKey(InibinFlags.Float32List))
                {
                    flags |= InibinFlags.Float32List;
                }
                if (this.Sets.ContainsKey(InibinFlags.Float32ListVec2))
                {
                    flags |= InibinFlags.Float32ListVec2;
                }
                if (this.Sets.ContainsKey(InibinFlags.Float32ListVec3))
                {
                    flags |= InibinFlags.Float32ListVec3;
                }
                if (this.Sets.ContainsKey(InibinFlags.Float32ListVec4))
                {
                    flags |= InibinFlags.Float32ListVec4;
                }
                if (this.Sets.ContainsKey(InibinFlags.Int16List))
                {
                    flags |= InibinFlags.Int16List;
                }
                if (this.Sets.ContainsKey(InibinFlags.Int32List))
                {
                    flags |= InibinFlags.Int32List;
                }
                if (this.Sets.ContainsKey(InibinFlags.Int8List))
                {
                    flags |= InibinFlags.Int8List;
                }
                if (this.Sets.ContainsKey(InibinFlags.StringList))
                {
                    flags |= InibinFlags.StringList;

                    foreach (string dataString in this.Sets[InibinFlags.StringList].Properties.Values)
                    {
                        stringDataLength += (ushort)(dataString.Length + 1);
                    }
                }


                bw.Write((byte)2);
                bw.Write(stringDataLength);
                bw.Write((ushort)flags);

                foreach (KeyValuePair<InibinFlags, InibinSet> set in this.Sets)
                {
                    set.Value.Write(bw);
                }
            }
        }

        public void AddValue(string section, string property, object value)
        {
            AddValue(Cryptography.SectionHash(section, property), value);
        }

        public void AddValue(uint hash, object value)
        {
            InibinFlags valueType;
            Type valueObjectType = value.GetType();

            if (valueObjectType == typeof(int))
            {
                valueType = InibinFlags.Int32List;
            }
            else if (valueObjectType == typeof(float))
            {
                valueType = InibinFlags.Float32List;
            }
            else if (valueObjectType == typeof(double))
            {
                double doubleValue = (double)value;
                if (doubleValue <= 25.5 && doubleValue >= 0.0)
                {
                    valueType = InibinFlags.FixedPointFloatList;
                }
                else
                {
                    throw new FixedPointFloatOverflowException();
                }
            }
            else if (valueObjectType == typeof(short))
            {
                valueType = InibinFlags.Int16List;
            }
            else if (valueObjectType == typeof(byte))
            {
                valueType = InibinFlags.Int8List;
            }
            else if (valueObjectType == typeof(bool))
            {
                valueType = InibinFlags.BitList;
            }
            else if (valueObjectType == typeof(byte[]))
            {
                byte[] vector = (byte[])value;
                if (vector.Length == 2)
                {
                    valueType = InibinFlags.FixedPointFloatListVec2;
                }
                else if (vector.Length == 3)
                {
                    valueType = InibinFlags.FixedPointFloatListVec3;
                }
                else if (vector.Length == 4)
                {
                    valueType = InibinFlags.FixedPointFloatListVec4;
                }
                else
                {
                    throw new VectorOverflowException();
                }
            }
            else if (valueObjectType == typeof(float[]))
            {
                float[] vector = (float[])value;
                if (vector.Length == 2)
                {
                    valueType = InibinFlags.Float32ListVec2;
                }
                else if (vector.Length == 3)
                {
                    valueType = InibinFlags.Float32ListVec3;
                }
                else if (vector.Length == 4)
                {
                    valueType = InibinFlags.Float32ListVec4;
                }
                else
                {
                    throw new VectorOverflowException();
                }
            }
            else
            {
                throw new UnsupportedValueTypeException("The type of: " + valueObjectType + "is not supported");
            }

            if (!this.Sets.ContainsKey(valueType))
            {
                this.Sets.Add(valueType, new InibinSet(valueType));
            }

            try
            {
                this.Sets[valueType].Properties.Add(hash, value);
            }
            catch (ArgumentException)
            {
                throw new Exception("The property you are trying to add already exists");
            }
        }
    }

    public class UnsupportedValueTypeException : Exception
    {
        public UnsupportedValueTypeException(string message) : base(message) { }
    }

    public class FixedPointFloatOverflowException : Exception
    {
        public FixedPointFloatOverflowException() : base("The Fixed Point Float value has to be between 0.0 and 25.5") { }
    }

    public class VectorOverflowException : Exception
    {
        public VectorOverflowException() : base("A Vector can only have the size of 2, 3, and 4") { }
    }

    [Flags]
    public enum InibinFlags : ushort
    {
        Int32List = 1,
        Float32List = 1 << 1,
        FixedPointFloatList = 1 << 2,
        Int16List = 1 << 3,
        Int8List = 1 << 4,
        BitList = 1 << 5,
        FixedPointFloatListVec3 = 1 << 6,
        Float32ListVec3 = 1 << 7,
        FixedPointFloatListVec2 = 1 << 8,
        Float32ListVec2 = 1 << 9,
        FixedPointFloatListVec4 = 1 << 10,
        Float32ListVec4 = 1 << 11,
        StringList = 1 << 12
    }
}
