using LeagueToolkit.Helpers.Extensions;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeVector4 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Vector4;
    public Vector4 Value { get; set; }

    public BinTreeVector4(uint nameHash, Vector4 value) : base(nameHash) => this.Value = value;

    internal BinTreeVector4(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadVector4();

    protected override void WriteContent(BinaryWriter bw) => bw.WriteVector4(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 16;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeVector4 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Vector4(BinTreeVector4 property) => property.Value;
}
