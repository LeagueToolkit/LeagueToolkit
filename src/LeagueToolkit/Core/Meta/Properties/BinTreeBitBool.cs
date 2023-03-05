using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a bitmask <see cref="byte"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeBitBool : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.BitBool;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public bool Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeBitBool"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeBitBool(uint nameHash, bool value) : base(nameHash) => this.Value = value;

    internal BinTreeBitBool(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadBoolean();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeBitBool property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator bool(BinTreeBitBool property) => property.Value;
}
