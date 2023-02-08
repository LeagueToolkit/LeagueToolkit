namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeUInt64 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.UInt64;
    public ulong Value { get; set; }

    public BinTreeUInt64(uint nameHash, ulong value) : base(nameHash) => this.Value = value;

    internal BinTreeUInt64(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeUInt64 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator ulong(BinTreeUInt64 property) => property.Value;
}
