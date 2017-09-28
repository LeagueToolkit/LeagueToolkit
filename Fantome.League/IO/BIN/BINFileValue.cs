﻿using System;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.BIN
{
    /// <summary>
    /// Represents a value inside of a <see cref="BINFileEntry"/> or <see cref="BINFileValueList"/>
    /// </summary>
    public class BINFileValue
    {
        /// <summary>
        /// The hash of the name of this <see cref="BINFileValue"/>
        /// </summary>
        /// <remarks>Can be <see cref="null"/> if <see cref="Parent"/> assigns it a type</remarks>
        public uint Property { get; private set; }
        /// <summary>
        /// Type of this <see cref="BINFileValue"/>
        /// </summary>
        /// <remarks>Can be <see cref="null"/> if <see cref="Parent"/> assigns it a type</remarks>
        public BINFileValueType? Type { get; private set; }
        /// <summary>
        /// The value of this <see cref="BINFileValue"/>
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// Object that is the parent of this <see cref="BINFileValue"/>
        /// </summary>
        /// <remarks>Can be either <see cref="BINFileEntry"/> or <see cref="BINFileValueList"/></remarks>
        public object Parent { get; private set; }
        /// <summary>
        /// Whether <see cref="Type"/> and <see cref="Property"/> were read
        /// </summary>
        private bool _typeRead = false;

        /// <summary>
        /// Initializes a new <see cref="BINFileValue"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="parent">The <see cref="Parent"/> of this value</param>
        /// <param name="type">The type that should be given to this entry</param>
        /// <remarks>If you give this <see cref="BINFileValue"/> a custom type then <see cref="Property"/> and <see cref="Type"/> won't be read from the <see cref="BinaryReader"/></remarks>
        public BINFileValue(BinaryReader br, object parent, BINFileValueType? type = null)
        {
            this.Parent = parent;
            this.Type = type;
            if (type == null)
            {
                this.Property = br.ReadUInt32();
                this.Type = (BINFileValueType)br.ReadByte();
                this._typeRead = true;
            }

            if (this.Type == BINFileValueType.String)
            {
                this.Value = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt16()));
            }
            else if (this.Type == BINFileValueType.SmallStaticTypeList)
            {
                this.Value = new BINFileValueList(br, this.Type.Value);
            }
            else if (this.Type == BINFileValueType.LargeStaticTypeList)
            {
                this.Value = new BINFileValueList(br, this.Type.Value);
            }
            else if (this.Type == BINFileValueType.List || this.Type == BINFileValueType.List2)
            {
                this.Value = new BINFileValueList(br, this.Type.Value);
            }
            else if (this.Type == BINFileValueType.DoubleTypeList)
            {
                this.Value = new BINFileValueList(br, this.Type.Value);
            }
            else if (this.Type == BINFileValueType.Float)
            {
                this.Value = br.ReadSingle();
            }
            else if (this.Type == BINFileValueType.UInt32 || this.Type == BINFileValueType.UInt32_2 || this.Type == BINFileValueType.UInt32_3)
            {
                this.Value = br.ReadUInt32();
            }
            else if (this.Type == BINFileValueType.UInt16)
            {
                this.Value = br.ReadUInt16();
            }
            else if (this.Type == BINFileValueType.ByteValue || this.Type == BINFileValueType.ByteValue2 || this.Type == BINFileValueType.ByteValue3)
            {
                this.Value = br.ReadByte();
            }
            else if (this.Type == BINFileValueType.Boolean)
            {
                this.Value = br.ReadBoolean();
            }
            else if (this.Type == BINFileValueType.UInt32Vector2)
            {
                this.Value = new uint[] { br.ReadUInt32(), br.ReadUInt32() };
            }
            else if (this.Type == BINFileValueType.ByteVector4 || this.Type == BINFileValueType.ByteVector4_2)
            {
                this.Value = new byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() };
            }
            else if (this.Type == BINFileValueType.FloatVector2)
            {
                this.Value = new float[] { br.ReadSingle(), br.ReadSingle() };
            }
            else if (this.Type == BINFileValueType.FloatVector3)
            {
                this.Value = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            }
            else if (this.Type == BINFileValueType.FloatVector4)
            {
                this.Value = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            }
            else if (this.Type == BINFileValueType.UInt16Vector3)
            {
                this.Value = new ushort[] { br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16() };
            }
            else
            {
                throw new Exception("Unknown value type: " + this.Type);
            }
        }

        /// <summary>
        /// Writes this <see cref="BINFileValue"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        /// <param name="writeType">Whether to write the <see cref="Property"/> and <see cref="Type"/> of this <see cref="BINFileValue"/></param>
        public void Write(BinaryWriter bw, bool writeType)
        {
            if (writeType)
            {
                bw.Write(this.Property);
                bw.Write((byte)this.Type);
            }

            if (this.Type == BINFileValueType.String)
            {
                string value = this.Value as string;
                bw.Write((ushort)value.Length);
                bw.Write(Encoding.ASCII.GetBytes(value));
            }
            else if (this.Type == BINFileValueType.SmallStaticTypeList ||
                this.Type == BINFileValueType.LargeStaticTypeList ||
                this.Type == BINFileValueType.List ||
                this.Type == BINFileValueType.List2 ||
                this.Type == BINFileValueType.DoubleTypeList)
            {
                (this.Value as BINFileValueList).Write(bw);
            }
            else if (this.Type == BINFileValueType.Float)
            {
                bw.Write((float)this.Value);
            }
            else if (this.Type == BINFileValueType.UInt32 || this.Type == BINFileValueType.UInt32_2 || this.Type == BINFileValueType.UInt32_3)
            {
                bw.Write((uint)this.Value);
            }
            else if (this.Type == BINFileValueType.UInt16)
            {
                bw.Write((ushort)this.Value);
            }
            else if (this.Type == BINFileValueType.ByteValue || this.Type == BINFileValueType.ByteValue2 || this.Type == BINFileValueType.ByteValue3)
            {
                bw.Write((byte)this.Value);
            }
            else if (this.Type == BINFileValueType.Boolean)
            {
                bw.Write((bool)this.Value);
            }
            else if (this.Type == BINFileValueType.UInt32Vector2)
            {
                uint[] value = this.Value as uint[];
                bw.Write(value[0]);
                bw.Write(value[1]);
            }
            else if (this.Type == BINFileValueType.ByteVector4 || this.Type == BINFileValueType.ByteVector4_2)
            {
                byte[] value = this.Value as byte[];
                bw.Write(value[0]);
                bw.Write(value[1]);
                bw.Write(value[2]);
                bw.Write(value[3]);
            }
            else if (this.Type == BINFileValueType.FloatVector2)
            {
                float[] value = this.Value as float[];
                bw.Write(value[0]);
                bw.Write(value[1]);
            }
            else if (this.Type == BINFileValueType.FloatVector3)
            {
                float[] value = this.Value as float[];
                bw.Write(value[0]);
                bw.Write(value[1]);
                bw.Write(value[2]);
            }
            else if (this.Type == BINFileValueType.FloatVector4)
            {
                float[] value = this.Value as float[];
                bw.Write(value[0]);
                bw.Write(value[1]);
                bw.Write(value[2]);
                bw.Write(value[3]);
            }
            else if (this.Type == BINFileValueType.UInt16Vector3)
            {
                ushort[] value = this.Value as ushort[];
                bw.Write(value[0]);
                bw.Write(value[1]);
                bw.Write(value[2]);
            }
        }

        /// <summary>
        /// Gets the size of this entry in bytes
        /// </summary>
        public int GetSize()
        {
            int size = this._typeRead ? 5 : 0;

            if (this.Type == BINFileValueType.SmallStaticTypeList ||
                this.Type == BINFileValueType.LargeStaticTypeList ||
                this.Type == BINFileValueType.DoubleTypeList ||
                this.Type == BINFileValueType.List ||
                this.Type == BINFileValueType.List2)
            {
                size += (this.Value as BINFileValueList).GetSize();
            }
            else if (this.Type == BINFileValueType.String)
            {
                size += 2 + (this.Value as string).Length;
            }
            else if (this.Type == BINFileValueType.Float ||
                this.Type == BINFileValueType.UInt32 ||
                this.Type == BINFileValueType.UInt32_2 ||
                this.Type == BINFileValueType.UInt32_3 ||
                this.Type == BINFileValueType.ByteVector4 ||
                this.Type == BINFileValueType.ByteVector4_2)
            {
                size += 4;
            }
            else if (this.Type == BINFileValueType.UInt16)
            {
                size += 2;
            }
            else if (this.Type == BINFileValueType.Boolean || this.Type == BINFileValueType.ByteValue || this.Type == BINFileValueType.ByteValue2 || this.Type == BINFileValueType.ByteValue3)
            {
                size += 1;
            }
            else if (this.Type == BINFileValueType.UInt32Vector2 || this.Type == BINFileValueType.FloatVector2)
            {
                size += 8;
            }
            else if (this.Type == BINFileValueType.FloatVector3)
            {
                size += 12;
            }
            else if (this.Type == BINFileValueType.FloatVector4)
            {
                size += 16;
            }
            else if (this.Type == BINFileValueType.UInt16Vector3)
            {
                size += 6;
            }

            return size;
        }
    }

    /// <summary>
    /// Type of a <see cref="BINFileValue"/>
    /// </summary>
    public enum BINFileValueType : Byte
    {
        /// <summary>
        /// Represents a <see cref="ushort"/> Vector3 value
        /// </summary>
        UInt16Vector3 = 0,
        /// <summary>
        /// Represents a <see cref="bool"/> value
        /// </summary>
        Boolean = 1,
        /// <summary>
        /// Represents a <see cref="byte"/> value
        /// </summary>
        ByteValue = 2,
        /// <summary>
        /// Represents a <see cref="byte"/> value
        /// </summary>
        ByteValue2 = 3,
        /// <summary>
        /// Represents a <see cref="ushort"/> value
        /// </summary>
        UInt16 = 5,
        /// <summary>
        /// Represents a <see cref="uint"/> value
        /// </summary>
        UInt32 = 6,
        /// <summary>
        /// Represents a <see cref="uint"/> value
        /// </summary>
        UInt32_2 = 7,
        /// <summary>
        /// Represents a <see cref="uint"/> Vector2 value
        /// </summary>
        UInt32Vector2 = 9,
        /// <summary>
        /// Represents a <see cref="float"/> value
        /// </summary>
        Float = 10,
        /// <summary>
        /// Represents a <see cref="float"/> Vector2 value
        /// </summary>
        FloatVector2 = 11,
        /// <summary>
        /// Represents a <see cref="float"/> Vector3 value
        /// </summary>
        FloatVector3 = 12,
        /// <summary>
        /// Represents a <see cref="float"/> Vector4 value
        /// </summary>
        FloatVector4 = 13,
        /// <summary>
        /// Represents a <see cref="byte"/> Vector4 value
        /// </summary>
        ByteVector4 = 15,
        /// <summary>
        /// Represents a <see cref="string"/> value
        /// </summary>
        String = 16,
        /// <summary>
        /// Represents a <see cref="uint"/> value
        /// </summary>
        UInt32_3 = 17,
        /// <summary>
        /// Represents a List with a <see cref="uint"/> Size and Value Count
        /// </summary>
        LargeStaticTypeList = 18,
        /// <summary>
        /// Represents a List which holds values of different types
        /// </summary>
        List2 = 19,
        /// <summary>
        /// Represents a List which holds values of different types
        /// </summary>
        List = 20,
        /// <summary>
        /// Represents a <see cref="byte"/> Vector4 value
        /// </summary>
        ByteVector4_2 = 21,
        /// <summary>
        /// Represents a List with <see cref="byte"/> Value Count
        /// </summary>
        SmallStaticTypeList = 22,
        /// <summary>
        /// Represents a List which holds values of 2 types
        /// </summary>
        DoubleTypeList = 23,
        /// <summary>
        /// Represents a <see cref="byte"/> value
        /// </summary>
        ByteValue3 = 24
    }
}
