using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeUInt64 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.UInt64;
        public ulong Value { get; set; }

        public BinTreeUInt64(IBinTreeParent parent, uint nameHash, ulong value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeUInt64(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt64();
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
            return other is BinTreeUInt64 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator ulong(BinTreeUInt64 property) => property.Value;
    }
}
