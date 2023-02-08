using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeColor : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Color;
    public Color Value { get; set; }

    public BinTreeColor(uint nameHash, Color value) : base(nameHash) => this.Value = value;

    internal BinTreeColor(BinaryReader br, uint nameHash) : base(nameHash) =>
        this.Value = br.ReadColor(ColorFormat.RgbaU8);

    protected override void WriteContent(BinaryWriter bw) => bw.WriteColor(this.Value, ColorFormat.RgbaU8);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeColor property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Color(BinTreeColor property) => property.Value;

    public static implicit operator Vector4(BinTreeColor property) => property.Value;
}
