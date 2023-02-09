using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{_debuggerDisplay, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeUnorderedContainer : BinTreeContainer
{
    public override BinPropertyType Type => BinPropertyType.UnorderedContainer;

    public BinTreeUnorderedContainer(
        uint nameHash,
        BinPropertyType propertiesType,
        IEnumerable<BinTreeProperty> properties
    ) : base(nameHash, propertiesType, properties) { }

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
