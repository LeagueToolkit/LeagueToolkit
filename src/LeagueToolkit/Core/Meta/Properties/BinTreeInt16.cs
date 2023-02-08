namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeInt16 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.I16;
    public short Value { get; set; }

    public BinTreeInt16(uint nameHash, short value) : base(nameHash) => this.Value = value;

    internal BinTreeInt16(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadInt16();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 2;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeInt16 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator short(BinTreeInt16 property) => property.Value;
}
