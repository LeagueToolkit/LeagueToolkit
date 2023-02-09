using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeU8 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.U8;
    public byte Value { get; set; }

    public BinTreeU8(uint nameHash, byte value) : base(nameHash) => this.Value = value;

    internal BinTreeU8(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadByte();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeU8 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator byte(BinTreeU8 property) => property.Value;
}
