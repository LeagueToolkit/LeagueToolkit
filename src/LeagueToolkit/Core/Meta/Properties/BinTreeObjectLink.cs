namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeObjectLink : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.ObjectLink;
    public uint Value { get; set; }

    public BinTreeObjectLink(uint nameHash, uint value) : base(nameHash) => this.Value = value;

    internal BinTreeObjectLink(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt32();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeObjectLink property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator uint(BinTreeObjectLink property) => property.Value;
}
