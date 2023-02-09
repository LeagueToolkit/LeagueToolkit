using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property which imitates an embedded struct
/// </summary>
[DebuggerDisplay("{GetDebuggerDisplay(), nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeEmbedded : BinTreeStruct
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Embedded;

    /// <summary>
    /// Creates a new <see cref="BinTreeEmbedded"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="metaClassHash">The hashed meta class name</param>
    /// <param name="properties">The properties of the class</param>
    public BinTreeEmbedded(uint nameHash, uint metaClassHash, IEnumerable<BinTreeProperty> properties)
        : base(nameHash, metaClassHash, properties) { }

    internal BinTreeEmbedded(BinaryReader br, uint nameHash, bool useLegacyType = false)
        : base(br, nameHash, useLegacyType) { }

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        if (other is not BinTreeEmbedded embedded)
            return false;

        if (this.ClassHash != embedded.ClassHash)
            return false;

        return this.Properties.SequenceEqual(embedded.Properties);
    }
}
