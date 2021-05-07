using System.IO;
using System.Text;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeString : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.String;

        public string Value { get; set; }

        public BinTreeString(IBinTreeParent parent, uint nameHash, string value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeString(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = Encoding.ASCII.GetString(br.ReadBytes(br.ReadUInt16()));
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write((ushort)this.Value.Length);
            bw.Write(Encoding.UTF8.GetBytes(this.Value));
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 2 + this.Value.Length;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeString property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator string(BinTreeString property) => property.Value;
    }
}
