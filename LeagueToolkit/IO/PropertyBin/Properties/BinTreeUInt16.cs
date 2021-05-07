using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeUInt16 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.UInt16;
        public ushort Value { get; set; }

        public BinTreeUInt16(IBinTreeParent parent, uint nameHash, ushort value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeUInt16(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt16();
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
            return other is BinTreeUInt16 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator ushort(BinTreeUInt16 property) => property.Value;
    }
}
