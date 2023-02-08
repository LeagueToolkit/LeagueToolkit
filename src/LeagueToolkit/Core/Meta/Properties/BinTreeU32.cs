namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeU32 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.U32;
    public uint Value { get; set; }

    public BinTreeU32(uint nameHash, uint value) : base(nameHash) => this.Value = value;

    internal BinTreeU32(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt32();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeU32 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator uint(BinTreeU32 property) => property.Value;
}
