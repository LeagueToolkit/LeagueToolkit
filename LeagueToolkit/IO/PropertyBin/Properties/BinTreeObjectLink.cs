using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeObjectLink : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.ObjectLink;
        public uint Value { get; set; }

        public BinTreeObjectLink(IBinTreeParent parent, uint nameHash, uint value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeObjectLink(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt32();
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
            return other is BinTreeObjectLink property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator uint(BinTreeObjectLink property) => property.Value;
    }
}
