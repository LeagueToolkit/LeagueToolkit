using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeInt32 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Int32;
        public int Value { get; set; }

        public BinTreeInt32(IBinTreeParent parent, uint nameHash, int value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeInt32(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadInt32();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeInt32 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator int(BinTreeInt32 property) => property.Value;
    }
}
