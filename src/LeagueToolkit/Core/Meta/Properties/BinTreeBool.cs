using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeBool : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Bool;

    public bool Value { get; set; }

    public BinTreeBool(uint nameHash, bool value) : base(nameHash) => this.Value = value;

    internal BinTreeBool(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadBoolean();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeBool property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator bool(BinTreeBool property) => property.Value;
}
