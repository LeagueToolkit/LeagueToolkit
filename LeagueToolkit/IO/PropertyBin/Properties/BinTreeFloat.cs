using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeFloat : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Float;
        public float Value { get; set; }

        public BinTreeFloat(IBinTreeParent parent, uint nameHash, float value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeFloat(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadSingle();
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
            return other is BinTreeFloat property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator float(BinTreeFloat property) => property.Value;
    }
}
