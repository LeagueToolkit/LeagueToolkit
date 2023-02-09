using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeNone : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.None;

    public BinTreeNone(uint nameHash) : base(nameHash) { }

    protected override void WriteContent(BinaryWriter bw) { }

    internal override int GetSize(bool includeHeader) => includeHeader ? 5 : 0;

    public override bool Equals(BinTreeProperty other) =>
        other is BinTreeNone property && property.NameHash == this.NameHash;
}
