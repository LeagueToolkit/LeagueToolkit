using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeColor : BinTreeProperty
    {
        public override BinPropertyType Type => BinPropertyType.Color;
        public Color Value { get; set; }

        public BinTreeColor(IBinTreeParent parent, uint nameHash, Color value) : base(parent, nameHash)
        {
            this.Value = value;
        }
        internal BinTreeColor(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadColor(ColorFormat.RgbaU8);
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteColor(this.Value, ColorFormat.RgbaU8);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeColor property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Color(BinTreeColor property) => property.Value;
    }
}
