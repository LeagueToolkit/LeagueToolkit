namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeInt64 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Int64;
    public long Value { get; set; }

    public BinTreeInt64(uint nameHash, long value) : base(nameHash) => this.Value = value;

    internal BinTreeInt64(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeInt64 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator long(BinTreeInt64 property) => property.Value;
}
