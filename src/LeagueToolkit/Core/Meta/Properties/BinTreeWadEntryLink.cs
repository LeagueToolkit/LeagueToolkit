namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeWadEntryLink : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.WadEntryLink;

    public ulong Value { get; set; }

    public BinTreeWadEntryLink(uint nameHash, ulong value) : base(nameHash) => this.Value = value;

    internal BinTreeWadEntryLink(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeWadEntryLink property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };
}
