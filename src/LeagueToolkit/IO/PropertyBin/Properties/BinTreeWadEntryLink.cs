using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeWadEntryLink : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.WadEntryLink;

        public ulong Value { get; set; }

        public BinTreeWadEntryLink(IBinTreeParent parent, uint nameHash, ulong value) : base(parent, nameHash) 
        {
            this.Value = value;
        }
        internal BinTreeWadEntryLink(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
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
            return other is BinTreeWadEntryLink property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }
    }
}
