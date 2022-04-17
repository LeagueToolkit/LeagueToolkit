using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeSByte : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.SByte;
        public sbyte Value { get; set; }

        public BinTreeSByte(IBinTreeParent parent, uint nameHash, sbyte value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeSByte(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadSByte();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 1;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeSByte property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator sbyte(BinTreeSByte property) => property.Value;
    }
}
