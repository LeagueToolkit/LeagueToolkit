namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeByte : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Byte;
    public byte Value { get; set; }

    public BinTreeByte(uint nameHash, byte value) : base(nameHash) => this.Value = value;

    internal BinTreeByte(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadByte();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeByte property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator byte(BinTreeByte property) => property.Value;
}
