namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeEmbedded : BinTreeStruct
{
    public override BinPropertyType Type => BinPropertyType.Embedded;

    public BinTreeEmbedded(uint nameHash, uint metaClassHash, IEnumerable<BinTreeProperty> properties)
        : base(nameHash, metaClassHash, properties) { }

    internal BinTreeEmbedded(BinaryReader br, uint nameHash) : base(br, nameHash) { }

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        if (other is not BinTreeEmbedded embedded)
            return false;

        if (this.ClassHash != embedded.ClassHash)
            return false;

        if (this._properties.Count != embedded._properties.Count)
            return false;

        return this._properties.SequenceEqual(embedded._properties);
    }
}
