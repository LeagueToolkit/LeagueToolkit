using LeagueToolkit.Helpers.Extensions;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeMatrix44 : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Matrix44;
        public Matrix4x4 Value { get; set; }

        public BinTreeMatrix44(IBinTreeParent parent, uint nameHash, Matrix4x4 value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeMatrix44(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadMatrix4x4RowMajor();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteMatrix4x4RowMajor(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 64;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeMatrix44 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Matrix4x4(BinTreeMatrix44 property) => property.Value;
    }
}
