using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeOptional : BinTreeProperty, IBinTreeParent
    {
        public override BinPropertyType Type => BinPropertyType.Optional;
        public BinPropertyType ValueType { get; private set; }
        public BinTreeProperty Value { get; private set; }

        public BinTreeOptional(IBinTreeParent parent, uint nameHash, BinPropertyType type, BinTreeProperty value) : base(parent, nameHash)
        {
            this.ValueType = type;
            this.Value = value;

            if (value is not null)
            {
                value.Parent = this;
            }
        }
        internal BinTreeOptional(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.ValueType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
            bool isSome = br.ReadBoolean();

            if (isSome)
            {
                this.Value = BinTreeProperty.Read(br, this, this.ValueType);
            }
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write((byte)BinUtilities.PackType(this.ValueType));
            bw.Write(this.Value is not null);

            if (this.Value is not null)
            {
                this.Value.Write(bw, false);
            }
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = 2 + (includeHeader ? 5 : 0);

            if (this.Value is not null)
            {
                size += this.Value.GetSize(false);
            }

            return size;
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other?.NameHash) return false;

            if (other is BinTreeOptional otherProperty)
            {
                if (this.ValueType != otherProperty.ValueType) return false;
                return this.Value is BinTreeProperty value && value.Equals(otherProperty.Value);
            }
            else return false;
        }
    }
}
