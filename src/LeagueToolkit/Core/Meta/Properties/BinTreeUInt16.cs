namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeUInt16 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.UInt16;
    public ushort Value { get; set; }

    public BinTreeUInt16(uint nameHash, ushort value) : base(nameHash) => this.Value = value;

    internal BinTreeUInt16(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt16();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 2;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeUInt16 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator ushort(BinTreeUInt16 property) => property.Value;
}
