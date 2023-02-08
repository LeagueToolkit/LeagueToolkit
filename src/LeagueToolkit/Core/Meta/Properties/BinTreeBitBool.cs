namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeBitBool : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.BitBool;
    public byte Value { get; set; }

    public BinTreeBitBool(uint nameHash, byte value) : base(nameHash) => this.Value = value;

    internal BinTreeBitBool(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadByte();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeBitBool property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator byte(BinTreeBitBool property) => property.Value;
}
