using LeagueToolkit.Helpers.Extensions;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeVector3 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Vector3;
    public Vector3 Value { get; set; }

    public BinTreeVector3(uint nameHash, Vector3 value) : base(nameHash) => this.Value = value;

    internal BinTreeVector3(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadVector3();

    protected override void WriteContent(BinaryWriter bw) => bw.WriteVector3(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 12;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeVector3 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Vector3(BinTreeVector3 property) => property.Value;
}
