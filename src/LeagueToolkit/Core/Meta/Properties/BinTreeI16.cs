namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeI16 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.I16;
    public short Value { get; set; }

    public BinTreeI16(uint nameHash, short value) : base(nameHash) => this.Value = value;

    internal BinTreeI16(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadInt16();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 2;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeI16 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator short(BinTreeI16 property) => property.Value;
}
