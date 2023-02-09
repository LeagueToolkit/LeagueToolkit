using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeI64 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.I64;
    public long Value { get; set; }

    public BinTreeI64(uint nameHash, long value) : base(nameHash) => this.Value = value;

    internal BinTreeI64(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeI64 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator long(BinTreeI64 property) => property.Value;
}
