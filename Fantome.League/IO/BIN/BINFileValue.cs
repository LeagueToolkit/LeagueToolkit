using System;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileValue
    {
        public UInt32 Property { get; private set; }
        public BINFileValueType Type { get; private set; }
        public Object Value { get; private set; }
        public Object Parent { get; private set; }
        public BINFileValue(BinaryReader br, Object parent, BINFileValueType? type = null)
        {
            this.Parent = parent;
            if(type == null)
            {
                this.Property = br.ReadUInt32();
                this.Type = (BINFileValueType)br.ReadByte();
            }
            else
            {
                this.Type = type.Value;
            }

            if(this.Type == BINFileValueType.String)
            {
                this.Value = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt16()));
            }
            else if(this.Type == BINFileValueType.SmallStaticTypeList)
            {
                this.Value = new BINFileValueList(br, this.Type);
            }
            else if (this.Type == BINFileValueType.LargeStaticTypeList)
            {
                this.Value = new BINFileValueList(br, this.Type);
            }
            else if (this.Type == BINFileValueType.List || this.Type == BINFileValueType.List2)
            {
                this.Value = new BINFileValueList(br, this.Type);
            }
            else if (this.Type == BINFileValueType.DoubleTypeList)
            {
                this.Value = new BINFileValueList(br, this.Type);
            }
            else if (this.Type == BINFileValueType.Float)
            {
                this.Value = br.ReadSingle();
            }
            else if (this.Type == BINFileValueType.UInt32 || this.Type == BINFileValueType.UInt32_2 || this.Type == BINFileValueType.UInt32_3)
            {
                this.Value = br.ReadUInt32();
            }
            else if(this.Type == BINFileValueType.UInt16)
            {
                this.Value = br.ReadUInt16();
            }
            else if(this.Type == BINFileValueType.ByteValue || this.Type == BINFileValueType.ByteValue2)
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
            else if(this.Type == BINFileValueType.UInt16Vector3)
            {
                this.Value = new ushort[] { br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16() };
            }
            else
            {
                throw new Exception();
            }
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
