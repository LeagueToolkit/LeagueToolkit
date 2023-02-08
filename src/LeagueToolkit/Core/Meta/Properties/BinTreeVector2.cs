using LeagueToolkit.Helpers.Extensions;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeVector2 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Vector2;
    public Vector2 Value { get; set; }

    public BinTreeVector2(uint nameHash, Vector2 value) : base(nameHash) => this.Value = value;

    internal BinTreeVector2(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadVector2();

    protected override void WriteContent(BinaryWriter bw) => bw.WriteVector2(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeVector2 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Vector2(BinTreeVector2 property) => property.Value;
}
