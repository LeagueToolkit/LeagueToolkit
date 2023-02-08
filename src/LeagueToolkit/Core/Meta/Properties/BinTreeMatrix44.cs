using LeagueToolkit.Helpers.Extensions;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeMatrix44 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Matrix44;
    public Matrix4x4 Value { get; set; }

    public BinTreeMatrix44(uint nameHash, Matrix4x4 value) : base(nameHash) => this.Value = value;

    internal BinTreeMatrix44(BinaryReader br, uint nameHash) : base(nameHash) =>
        this.Value = br.ReadMatrix4x4RowMajor();

    protected override void WriteContent(BinaryWriter bw) => bw.WriteMatrix4x4RowMajor(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 64;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeMatrix44 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Matrix4x4(BinTreeMatrix44 property) => property.Value;
}
