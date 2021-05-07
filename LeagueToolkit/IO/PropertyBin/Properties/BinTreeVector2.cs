using LeagueToolkit.Helpers.Extensions;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeVector2 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Vector2;
        public Vector2 Value { get; set; }

        public BinTreeVector2(IBinTreeParent parent, uint nameHash, Vector2 value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeVector2(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadVector2();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteVector2(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 8;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeVector2 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Vector2(BinTreeVector2 property) => property.Value;
    }
}
