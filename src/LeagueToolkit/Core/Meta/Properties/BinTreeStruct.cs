using CommunityToolkit.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

public class BinTreeStruct : BinTreeProperty, IBinTreeParent
{
    public override BinPropertyType Type => BinPropertyType.Struct;
    public uint ClassHash { get; private set; }

    public IReadOnlyList<BinTreeProperty> Properties => this._properties;
    protected List<BinTreeProperty> _properties = new();

    public BinTreeStruct(uint nameHash, uint classHash, IEnumerable<BinTreeProperty> properties) : base(nameHash)
    {
        this.ClassHash = classHash;

        // Verify properties
        foreach (BinTreeProperty property in properties)
        {
            if (properties.Any(x => x.NameHash == property.NameHash && x != property))
            {
                throw new ArgumentException($"Found two properties with the same name hash: {property.NameHash}");
            }
        }

        this._properties = properties.ToList();
    }

    internal BinTreeStruct(BinaryReader br, uint nameHash, bool useLegacyType = false) : base(nameHash)
    {
        this.ClassHash = br.ReadUInt32();
        if (this.ClassHash is 0)
            return; // Skip

        uint size = br.ReadUInt32();
        long contentOffset = br.BaseStream.Position;

        ushort propertyCount = br.ReadUInt16();
        for (int i = 0; i < propertyCount; i++)
            this._properties.Add(Read(br, useLegacyType));

        if (br.BaseStream.Position != contentOffset + size)
            ThrowHelper.ThrowInvalidDataException(
                $"Invalid size: {br.BaseStream.Position - contentOffset}, expected {size}"
            );
    }

    protected override void WriteContent(BinaryWriter bw)
    {
        bw.Write(this.ClassHash);
        if (this.ClassHash is 0)
            return; // Skip

        bw.Write(GetContentSize());

        bw.Write((ushort)this.Properties.Count);
        foreach (BinTreeProperty property in this.Properties)
            property.Write(bw, writeHeader: true);
    }

    public void AddProperty(BinTreeProperty property)
    {
        if (this._properties.Any(x => x.NameHash == property.NameHash))
            throw new InvalidOperationException("A property with the same name already exists");

        this._properties.Add(property);
    }

    public bool RemoveProperty(BinTreeProperty property) => this._properties.Remove(property);

    public bool RemoveProperty(uint nameHash) =>
        RemoveProperty(this._properties.FirstOrDefault(x => x.NameHash == nameHash));

    internal override int GetSize(bool includeHeader) =>
        this.ClassHash switch
        {
            0 => (includeHeader ? HEADER_SIZE : 0) + 4,
            _ => (includeHeader ? HEADER_SIZE : 0) + 4 + 4 + GetContentSize()
        };

    private int GetContentSize() => 2 + this.Properties.Sum(x => x.GetSize(includeHeader: true));

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        if (other is BinTreeStruct otherProperty && other is not BinTreeEmbedded)
        {
            if (this.ClassHash != otherProperty.ClassHash)
                return false;
            if (this._properties.Count != otherProperty._properties.Count)
                return false;

            for (int i = 0; i < this._properties.Count; i++)
            {
                if (!this._properties[i].Equals(otherProperty._properties[i]))
                    return false;
            }
        }

        return true;
    }
}
