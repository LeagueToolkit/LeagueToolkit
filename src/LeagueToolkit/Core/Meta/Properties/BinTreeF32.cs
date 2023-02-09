using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeF32 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.F32;
    public float Value { get; set; }

    public BinTreeF32(uint nameHash, float value) : base(nameHash) => this.Value = value;

    internal BinTreeF32(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadSingle();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeF32 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator float(BinTreeF32 property) => property.Value;
}
