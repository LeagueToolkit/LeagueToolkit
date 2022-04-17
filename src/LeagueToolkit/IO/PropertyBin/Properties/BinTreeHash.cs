using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeHash : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Hash;

        public uint Value { get; set; }

        public BinTreeHash(IBinTreeParent parent, uint nameHash, uint value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeHash(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
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
            return other is BinTreeHash property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator uint(BinTreeHash property) => property.Value;
    }
}
