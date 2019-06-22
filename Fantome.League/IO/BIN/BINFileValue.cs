using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileValue : IBINFileValue
    {
        public BINFileValueType? Type { get; private set; }
        public object Parent { get; private set; }
        public uint Property { get; set; }
        public object Value { get; private set; }

        private bool _typeRead;

        public BINFileValue(object parent, uint property, object value)
        {

        }

        public BINFileValue(BinaryReader br, object parent, BINFileValueType? type = null)
        {
            this.Parent = parent;
            this.Type = type;
            if (this.Type == null)
            {
                this.Property = br.ReadUInt32();
                this.Type = (BINFileValueType)br.ReadByte();
                this._typeRead = true;
            }

            if (this.Type == BINFileValueType.UInt16Vector3)
            {
                this.Value = new uint[] { br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16() };
            }
            else if (this.Type == BINFileValueType.Boolean)
            {
                this.Value = br.ReadBoolean();
            }
            else if (this.Type == BINFileValueType.SByte)
            {
                this.Value = br.ReadSByte();
            }
            else if (this.Type == BINFileValueType.Byte)
            {
                this.Value = br.ReadByte();
            }
            else if (this.Type == BINFileValueType.Int16)
            {
                this.Value = br.ReadInt16();
            }
            else if (this.Type == BINFileValueType.Int16)
            {
                this.Value = br.ReadUInt16();
            }
            else if (this.Type == BINFileValueType.UInt16)
            {
                this.Value = br.ReadUInt16();
            }
            else if (this.Type == BINFileValueType.Int32)
            {
                this.Value = br.ReadInt32();
            }
            else if (this.Type == BINFileValueType.UInt32)
            {
                this.Value = br.ReadUInt32();
            }
            else if (this.Type == BINFileValueType.Int64)
            {
                this.Value = br.ReadInt64();
            }
            else if (this.Type == BINFileValueType.UInt64)
            {
                this.Value = br.ReadUInt64();
            }
            else if (this.Type == BINFileValueType.Float)
            {
                this.Value = br.ReadSingle();
            }
            else if (this.Type == BINFileValueType.FloatVector2)
            {
                this.Value = new Vector2(br);
            }
            else if (this.Type == BINFileValueType.FloatVector3)
            {
                this.Value = new Vector3(br);
            }
            else if (this.Type == BINFileValueType.FloatVector4)
            {
                this.Value = new Vector4(br);
            }
            else if (this.Type == BINFileValueType.Matrix44)
            {
                this.Value = new R3DMatrix44(br);
            }
            else if (this.Type == BINFileValueType.Color)
            {
                this.Value = new ColorRGBAVector4Byte(br);
            }
            else if (this.Type == BINFileValueType.String)
            {
                this.Value = Encoding.ASCII.GetString(br.ReadBytes(br.ReadUInt16()));
            }
            else if (this.Type == BINFileValueType.StringHash)
            {
                this.Value = br.ReadUInt32();
            }
            else if (this.Type == BINFileValueType.Container)
            {
                this.Value = new BINFileContainer(br, this);
            }
            else if (this.Type == BINFileValueType.Structure || this.Type == BINFileValueType.Embedded)
            {
                this.Value = new BINFileStructure(br, this);
            }
            else if (this.Type == BINFileValueType.LinkOffset)
            {
                this.Value = br.ReadUInt32();
            }
            else if (this.Type == BINFileValueType.AdditionalOptionalData)
            {
                this.Value = new BINFileAdditionalData(br, this);
            }
            else if (this.Type == BINFileValueType.Map)
            {
                this.Value = new BINFileMap(br, this);
            }
            else if (this.Type == BINFileValueType.FlagsBoolean)
            {
                this.Value = br.ReadBoolean();
            }
            else
            {
                throw new Exception("An Unknown Value Type was encountered: " + (byte)this.Type);
            }
        }

        public void Write(BinaryWriter bw, bool writeType)
        {
            if (writeType)
            {
                bw.Write(this.Property);
                bw.Write((byte)this.Type);
            }

            if (this.Type == BINFileValueType.UInt16Vector3)
            {
                ushort[] vector = this.Value as ushort[];
                bw.Write(vector[0]);
                bw.Write(vector[1]);
                bw.Write(vector[2]);
            }
            else if (this.Type == BINFileValueType.Boolean)
            {
                bw.Write((bool)this.Value);
            }
            else if (this.Type == BINFileValueType.SByte)
            {
                bw.Write((sbyte)this.Value);
            }
            else if (this.Type == BINFileValueType.Byte)
            {
                bw.Write((byte)this.Value);
            }
            else if (this.Type == BINFileValueType.Int16)
            {
                bw.Write((short)this.Value);
            }
            else if (this.Type == BINFileValueType.UInt16)
            {
                bw.Write((ushort)this.Value);
            }
            else if (this.Type == BINFileValueType.Int32)
            {
                bw.Write((int)this.Value);
            }
            else if (this.Type == BINFileValueType.UInt32)
            {
                bw.Write((uint)this.Value);
            }
            else if (this.Type == BINFileValueType.Int64)
            {
                bw.Write((long)this.Value);
            }
            else if (this.Type == BINFileValueType.UInt64)
            {
                bw.Write((ulong)this.Value);
            }
            else if (this.Type == BINFileValueType.Float)
            {
                bw.Write((float)this.Value);
            }
            else if (this.Type == BINFileValueType.FloatVector2)
            {
                (this.Value as Vector2).Write(bw);
            }
            else if (this.Type == BINFileValueType.FloatVector3)
            {
                (this.Value as Vector3).Write(bw);
            }
            else if (this.Type == BINFileValueType.FloatVector4)
            {
                (this.Value as Vector4).Write(bw);
            }
            else if (this.Type == BINFileValueType.Matrix44)
            {
                (this.Value as R3DMatrix44).Write(bw);
            }
            else if (this.Type == BINFileValueType.Color)
            {
                (this.Value as ColorRGBAVector4Byte).Write(bw);
            }
            else if (this.Type == BINFileValueType.String)
            {
                string value = this.Value as string;
                bw.Write((ushort)value.Length);
                bw.Write(Encoding.ASCII.GetBytes(value));
            }
            else if (this.Type == BINFileValueType.StringHash)
            {
                bw.Write((uint)this.Value);
            }
            else if (this.Type == BINFileValueType.Container)
            {
                (this.Value as BINFileContainer).Write(bw);
            }
            else if (this.Type == BINFileValueType.Structure || this.Type == BINFileValueType.Embedded)
            {
                (this.Value as BINFileStructure).Write(bw);
            }
            else if (this.Type == BINFileValueType.LinkOffset)
            {
                bw.Write((uint)this.Value);
            }
            else if (this.Type == BINFileValueType.AdditionalOptionalData)
            {
                (this.Value as BINFileAdditionalData).Write(bw);
            }
            else if (this.Type == BINFileValueType.Map)
            {
                (this.Value as BINFileMap).Write(bw);
            }
            else if (this.Type == BINFileValueType.FlagsBoolean)
            {
                bw.Write((bool)this.Value);
            }
        }

        public uint GetSize()
        {
            uint size = this._typeRead ? (uint)5 : 0;

            switch (this.Type)
            {
                case BINFileValueType.UInt16Vector3:
                    size += 6;
                    break;

                case BINFileValueType.Boolean:
                case BINFileValueType.SByte:
                case BINFileValueType.Byte:
                case BINFileValueType.FlagsBoolean:
                    size += 1;
                    break;

                case BINFileValueType.Int16:
                case BINFileValueType.UInt16:
                    size += 2;
                    break;

                case BINFileValueType.Int32:
                case BINFileValueType.UInt32:
                case BINFileValueType.StringHash:
                case BINFileValueType.LinkOffset:
                case BINFileValueType.Float:
                case BINFileValueType.Color:
                    size += 4;
                    break;

                case BINFileValueType.Int64:
                case BINFileValueType.UInt64:
                case BINFileValueType.FloatVector2:
                    size += 8;
                    break;

                case BINFileValueType.FloatVector3:
                    size += 12;
                    break;
                case BINFileValueType.FloatVector4:
                    size += 16;
                    break;

                case BINFileValueType.Matrix44:
                    size += 64;
                    break;

                case BINFileValueType.String:
                    size += 2 + (uint)(this.Value as string).Length;
                    break;

                case BINFileValueType.Container:
                case BINFileValueType.Structure:
                case BINFileValueType.Embedded:
                case BINFileValueType.AdditionalOptionalData:
                case BINFileValueType.Map:
                    size += (this.Value as IBINFileValue).GetSize();
                    break;
            }

            return size;
        }
    }

    /// <summary>
    /// Type of a <see cref="BINFileValue"/>
    /// </summary>
    public enum BINFileValueType : byte
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
        /// Represents an <see cref="sbyte"/> value
        /// </summary>
        SByte = 2,
        /// <summary>
        /// Represents a <see cref="byte"/> value
        /// </summary>
        Byte = 3,
        /// <summary>
        /// Represents a <see cref="short"/> value
        /// </summary>
        Int16 = 4,
        /// <summary>
        /// Represents a <see cref="ushort"/> value
        /// </summary>
        UInt16 = 5,
        /// <summary>
        /// Represents an <see cref="int"/> value
        /// </summary>
        Int32 = 6,
        /// <summary>
        /// Represents a <see cref="uint"/> value
        /// </summary>
        UInt32 = 7,
        /// <summary>
        /// Reperesents a <see cref="long"/> value
        /// </summary>
        Int64 = 8,
        /// <summary>
        /// Represents a <see cref="ulong"/> value
        /// </summary>
        UInt64 = 9,
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
        /// Represents a <see cref="R3DMatrix44"/> value
        /// </summary>
        Matrix44 = 14,
        /// <summary>
        /// Represents a <see cref="ColorRGBAVector4Byte"/> value
        /// </summary>
        Color = 15,
        /// <summary>
        /// Represents a <see cref="string"/> value
        /// </summary>
        String = 16,
        /// <summary>
        /// Represents a <see cref="uint"/> value which is a hash
        /// </summary>
        StringHash = 17,
        /// <summary>
        /// Represents a Value Container
        /// </summary>
        Container = 18,
        /// <summary>
        /// Represents a Structure
        /// </summary>
        Structure = 19,
        /// <summary>
        /// Represents an Embedded Structure
        /// </summary>
        Embedded = 20,
        /// <summary>
        /// Represents a <see cref="uint"/> value which links to another entry
        /// </summary>
        LinkOffset = 21,
        /// <summary>
        /// Represents Additional Optional Data
        /// </summary>
        AdditionalOptionalData = 22,
        /// <summary>
        /// Represents a List which holds Key-Value values
        /// </summary>
        Map = 23,
        /// <summary>
        /// Represents a <see cref="bool"/>
        /// </summary>
        FlagsBoolean = 24
    }
}
