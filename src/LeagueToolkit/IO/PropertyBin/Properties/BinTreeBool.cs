using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeBool : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Bool;

        public bool Value { get; set; }

        public BinTreeBool(IBinTreeParent parent, uint nameHash, bool value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeBool(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadBoolean();
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
            return other is BinTreeBool property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator bool(BinTreeBool property) => property.Value;
    }
}
