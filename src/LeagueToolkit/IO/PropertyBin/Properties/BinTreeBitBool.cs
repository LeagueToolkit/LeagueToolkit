using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeBitBool : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.BitBool;
        public byte Value { get; set; }

        public BinTreeBitBool(IBinTreeParent parent, uint nameHash, byte value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeBitBool(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadByte();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? HEADER_SIZE : 0;
            return size + 1;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeBitBool property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator byte(BinTreeBitBool property) => property.Value;
    }
}
