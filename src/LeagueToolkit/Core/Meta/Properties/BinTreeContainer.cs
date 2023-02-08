using CommunityToolkit.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

public class BinTreeContainer : BinTreeProperty, IBinTreeParent
{
    public override BinPropertyType Type => BinPropertyType.Container;

    public BinPropertyType ElementType { get; }

    public IReadOnlyList<BinTreeProperty> Elements => this._elements;
    protected List<BinTreeProperty> _elements = new();

    public BinTreeContainer(uint nameHash, BinPropertyType propertiesType, IEnumerable<BinTreeProperty> elements)
        : base(nameHash)
    {
        this.ElementType = propertiesType;
        this._elements = elements.ToList();

        foreach (BinTreeProperty element in this.Elements)
            ValidateElementType(element);
    }

    internal BinTreeContainer(BinaryReader br, uint nameHash) : base(nameHash)
    {
        this.ElementType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
        uint size = br.ReadUInt32();

        uint valueCount = br.ReadUInt32();
        for (int i = 0; i < valueCount; i++)
            this._elements.Add(Read(br, this.ElementType));
    }

    protected override void WriteContent(BinaryWriter bw)
    {
        bw.Write((byte)BinUtilities.PackType(this.ElementType));
        bw.Write(GetContentSize());
        bw.Write(this._elements.Count);

        foreach (BinTreeProperty property in this._elements)
        {
            ValidateElementType(property);

            property.Write(bw, false);
        }
    }

    public void Add(BinTreeProperty element)
    {
        ValidateElementType(element);

        if (this._elements.Any(x => x.NameHash == element.NameHash))
            ThrowHelper.ThrowArgumentException(nameof(element), "A Property with the same name hash already exists");

        this._elements.Add(element);
    }

    public bool Remove(BinTreeProperty element) => this._elements.Remove(element);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 4 + 1 + GetContentSize();

    private int GetContentSize() => 4 + this.Elements.Sum(x => x.GetSize(includeHeader: false));

    private void ValidateElementType(BinTreeProperty element)
    {
        if (element.Type != this.ElementType)
            ThrowHelper.ThrowInvalidOperationException(
                $"Property: {element.NameHash}: {element.Type} does not match the specified container element type: {this.ElementType}"
            );
    }

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        if (other is BinTreeContainer otherProperty && other is not BinTreeUnorderedContainer)
        {
            if (this._elements.Count != otherProperty._elements.Count)
                return false;
            if (this.ElementType != otherProperty.ElementType)
                return false;

            for (int i = 0; i < this._elements.Count; i++)
            {
                if (!this._elements[i].Equals(otherProperty._elements[i]))
                    return false;
            }
        }

        return true;
    }
}
