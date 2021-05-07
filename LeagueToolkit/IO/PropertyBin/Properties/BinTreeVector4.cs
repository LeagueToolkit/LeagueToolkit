using LeagueToolkit.Helpers.Extensions;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeVector4 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Vector4;
        public Vector4 Value { get; set; }

        public BinTreeVector4(IBinTreeParent parent, uint nameHash, Vector4 value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeVector4(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadVector4();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteVector4(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 16;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeVector4 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Vector4(BinTreeVector4 property) => property.Value;
    }
}
