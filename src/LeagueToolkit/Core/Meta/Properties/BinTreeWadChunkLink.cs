using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{_debuggerDisplay, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeWadChunkLink : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.WadChunkLink;

    public ulong Value { get; set; }

    private string _debuggerDisplay => string.Format("{0:x}", this.Value);

    public BinTreeWadChunkLink(uint nameHash, ulong value) : base(nameHash) => this.Value = value;

    internal BinTreeWadChunkLink(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeWadChunkLink property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };
}
