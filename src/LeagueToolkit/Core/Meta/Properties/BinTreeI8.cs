using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeI8 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.I8;
    public sbyte Value { get; set; }

    public BinTreeI8(uint nameHash, sbyte value) : base(nameHash) => this.Value = value;

    internal BinTreeI8(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadSByte();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeI8 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator sbyte(BinTreeI8 property) => property.Value;
}
