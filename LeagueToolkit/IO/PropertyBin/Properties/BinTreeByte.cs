using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeByte : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Byte;
        public byte Value { get; set; }

        public BinTreeByte(IBinTreeParent parent, uint nameHash, byte value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeByte(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadByte();
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
            return other is BinTreeByte property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator byte(BinTreeByte property) => property.Value;
    }
}
