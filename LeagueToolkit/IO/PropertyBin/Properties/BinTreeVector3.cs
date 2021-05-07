using LeagueToolkit.Helpers.Extensions;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeVector3 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Vector3;
        public Vector3 Value { get; set; }

        public BinTreeVector3(IBinTreeParent parent, uint nameHash, Vector3 value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeVector3(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadVector3();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteVector3(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 12;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeVector3 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Vector3(BinTreeVector3 property) => property.Value;
    }
}
