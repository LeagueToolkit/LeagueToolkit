using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeInt64 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Int64;
        public long Value { get; set; }

        public BinTreeInt64(IBinTreeParent parent, uint nameHash, long value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeInt64(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadInt64();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 8;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeInt64 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator long(BinTreeInt64 property) => property.Value;
    }
}
