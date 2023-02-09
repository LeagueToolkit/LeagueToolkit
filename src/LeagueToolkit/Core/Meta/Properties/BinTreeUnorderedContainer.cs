using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="List{T}"/> value
/// </summary>
[DebuggerDisplay("{GetDebuggerDisplay(), nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeUnorderedContainer : BinTreeContainer
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.UnorderedContainer;

    /// <summary>
    /// Creates a new <see cref="BinTreeUnorderedContainer"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="elementType">The property type of an element in the container</param>
    /// <param name="elements">The elements of the container</param>
    public BinTreeUnorderedContainer(uint nameHash, BinPropertyType elementType, IEnumerable<BinTreeProperty> elements)
        : base(nameHash, elementType, elements) { }

    internal BinTreeUnorderedContainer(BinaryReader br, uint nameHash, bool useLegacyType = false)
        : base(br, nameHash, useLegacyType) { }

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        if (other is not BinTreeUnorderedContainer unorderedContainer)
            return false;

        return this.Elements.SequenceEqual(unorderedContainer.Elements);
    }
}
