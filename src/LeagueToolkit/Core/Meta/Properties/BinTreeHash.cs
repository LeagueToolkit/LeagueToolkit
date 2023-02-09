using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{_debuggerDisplay, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeHash : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Hash;

    public uint Value { get; set; }

    private string _debuggerDisplay => string.Format("{0:x}", this.Value);

    public BinTreeHash(uint nameHash, uint value) : base(nameHash) => this.Value = value;

    internal BinTreeHash(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt32();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeHash property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator uint(BinTreeHash property) => property.Value;
}
