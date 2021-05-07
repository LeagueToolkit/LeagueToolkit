using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeInt16 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Int16;
        public short Value { get; set; }

        public BinTreeInt16(IBinTreeParent parent, uint nameHash, short value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeInt16(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadInt16();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 2;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeInt16 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator short(BinTreeInt16 property) => property.Value;
    }
}
