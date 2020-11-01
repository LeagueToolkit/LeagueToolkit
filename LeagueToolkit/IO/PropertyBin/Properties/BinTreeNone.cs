using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeNone : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.None;

        public BinTreeNone(IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {

        }
        internal BinTreeNone(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {

        }

        protected override void WriteContent(BinaryWriter bw) { }

        internal override int GetSize(bool includeHeader) => includeHeader ? 5 : 0;

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeNone property && property.NameHash == this.NameHash;
        }
    }
}
