namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeInt32 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.I32;
    public int Value { get; set; }

    public BinTreeInt32(uint nameHash, int value) : base(nameHash) => this.Value = value;

    internal BinTreeInt32(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadInt32();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeInt32 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator int(BinTreeInt32 property) => property.Value;
}
