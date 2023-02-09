using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="bool"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeBool : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Bool;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public bool Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeBool"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeBool(uint nameHash, bool value) : base(nameHash) => this.Value = value;

    internal BinTreeBool(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadBoolean();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeBool property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator bool(BinTreeBool property) => property.Value;
}
