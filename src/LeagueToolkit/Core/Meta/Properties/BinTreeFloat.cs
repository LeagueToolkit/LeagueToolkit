namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeFloat : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.F32;
    public float Value { get; set; }

    public BinTreeFloat(uint nameHash, float value) : base(nameHash) => this.Value = value;

    internal BinTreeFloat(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadSingle();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeFloat property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator float(BinTreeFloat property) => property.Value;
}
