using CommunityToolkit.Diagnostics;
using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="List{T}"/> value
/// </summary>
[DebuggerDisplay("{GetDebuggerDisplay(), nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public class BinTreeContainer : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Container;

    /// <summary>
    /// Gets the property type of an element
    /// </summary>
    public BinPropertyType ElementType { get; }

    /// <summary>
    /// Gets the elements of the container
    /// </summary>
    public IReadOnlyList<BinTreeProperty> Elements => this._elements;
    protected List<BinTreeProperty> _elements = new();

    /// <summary>
    /// Creates a new <see cref="BinTreeContainer"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="elementType">The property type of an element in the container</param>
    /// <param name="elements">The elements of the container</param>
    public BinTreeContainer(uint nameHash, BinPropertyType elementType, IEnumerable<BinTreeProperty> elements)
        : base(nameHash)
    {
        this.ElementType = elementType;
        this._elements = elements.ToList();

        foreach (BinTreeProperty element in this.Elements)
            ValidateElementType(element);
    }

    internal BinTreeContainer(BinaryReader br, uint nameHash, bool useLegacyType = false) : base(nameHash)
    {
        this.ElementType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte(), useLegacyType);
        uint size = br.ReadUInt32();
        long contentOffset = br.BaseStream.Position;

        uint valueCount = br.ReadUInt32();
        for (int i = 0; i < valueCount; i++)
            this._elements.Add(ReadPropertyContent(0, this.ElementType, br, useLegacyType));

        if (br.BaseStream.Position != contentOffset + size)
            ThrowHelper.ThrowInvalidDataException(
                $"Invalid size: {br.BaseStream.Position - contentOffset}, expected {size}"
            );
    }

    protected override void WriteContent(BinaryWriter bw)
    {
        bw.Write((byte)this.ElementType);
        bw.Write(GetContentSize());
        bw.Write(this._elements.Count);

        foreach (BinTreeProperty property in this._elements)
        {
            ValidateElementType(property);

            property.Write(bw, writeHeader: false);
        }
    }

    /// <summary>
    /// Adds an element into the container
    /// </summary>
    /// <param name="element">The element to add</param>
    public void Add(BinTreeProperty element)
    {
        ValidateElementType(element);

        this._elements.Add(element);
    }

    /// <summary>
    /// Removes the specified element from the container
    /// </summary>
    /// <param name="element">The element to remove</param>
    /// <returns><see langword="true"/> if <paramref name="element"/> was successfully removed; otherwise <see langword="false"/></returns>
    public bool Remove(BinTreeProperty element) => this._elements.Remove(element);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1 + 4 + GetContentSize();

    private int GetContentSize() => 4 + this.Elements.Sum(x => x.GetSize(includeHeader: false));

    private void ValidateElementType(BinTreeProperty element)
    {
        if (element.Type != this.ElementType)
            ThrowHelper.ThrowArgumentException(
                nameof(element),
                $"Property: {element.NameHash}: {element.Type} does not match the specified container element type: {this.ElementType}"
            );
    }

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        if (other is BinTreeUnorderedContainer)
            return false;

        if (other is not BinTreeContainer otherContainer)
            return false;

        return this.Elements.SequenceEqual(otherContainer.Elements);
    }

    private string GetDebuggerDisplay() => string.Format("Container<{0}>", this.ElementType);
}
