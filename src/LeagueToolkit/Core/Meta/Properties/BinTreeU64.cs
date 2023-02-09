using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeU64 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.U64;
    public ulong Value { get; set; }

    public BinTreeU64(uint nameHash, ulong value) : base(nameHash) => this.Value = value;

    internal BinTreeU64(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeU64 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator ulong(BinTreeU64 property) => property.Value;
}
