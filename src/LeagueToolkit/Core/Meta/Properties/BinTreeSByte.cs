namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeSByte : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.SByte;
    public sbyte Value { get; set; }

    public BinTreeSByte(uint nameHash, sbyte value) : base(nameHash) => this.Value = value;

    internal BinTreeSByte(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadSByte();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeSByte property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator sbyte(BinTreeSByte property) => property.Value;
}
