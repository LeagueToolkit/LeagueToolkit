using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileValue
    {
        public UInt32 Property { get; private set; }
        public BINFileValueType? Type { get; private set; }
        public Object Value { get; private set; }
        public Object Parent { get; private set; }
        private bool _typeRead = false;

        public BINFileValue(BinaryReader br, Object parent, BINFileValueType? type = null)
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
            else if (this.Type == BINFileValueType.ByteValue || this.Type == BINFileValueType.ByteValue2)
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
                this.Value = new Byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() };
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

        public void Write(BinaryWriter bw, bool writeType)
        {
            if (writeType)
            {
                bw.Write(this.Property);
                bw.Write((byte)this.Type);
            }

            if (this.Type == BINFileValueType.String)
            {
                string value = this.Value as String;
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
            else if (this.Type == BINFileValueType.ByteValue || this.Type == BINFileValueType.ByteValue2)
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
                size += 2 + (this.Value as String).Length;
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
            else if (this.Type == BINFileValueType.Boolean || this.Type == BINFileValueType.ByteValue || this.Type == BINFileValueType.ByteValue2)
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

    public enum BINFileValueType : Byte
    {
        UInt16Vector3 = 0,
        Boolean = 1,
        ByteValue = 3,
        UInt16 = 5,
        UInt32_3 = 6,
        UInt32_2 = 7,
        UInt32Vector2 = 9,
        Float = 10,
        FloatVector2 = 11,
        FloatVector3 = 12,
        FloatVector4 = 13,
        ByteVector4 = 15,
        String = 16,
        UInt32 = 17,
        LargeStaticTypeList = 18,
        List2 = 19,
        List = 20,
        ByteVector4_2 = 21,
        SmallStaticTypeList = 22,
        DoubleTypeList = 23,
        ByteValue2 = 24
    }
}
