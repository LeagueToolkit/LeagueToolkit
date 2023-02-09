using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeU16 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.U16;
    public ushort Value { get; set; }

    public BinTreeU16(uint nameHash, ushort value) : base(nameHash) => this.Value = value;

    internal BinTreeU16(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt16();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 2;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeU16 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator ushort(BinTreeU16 property) => property.Value;
}
