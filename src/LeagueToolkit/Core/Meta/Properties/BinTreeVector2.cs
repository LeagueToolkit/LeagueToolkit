using LeagueToolkit.Utils.Extensions;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="Vector2"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeVector2 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Vector2;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public Vector2 Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeVector2"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeVector2(uint nameHash, Vector2 value) : base(nameHash) => this.Value = value;

    internal BinTreeVector2(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadVector2();

    protected override void WriteContent(BinaryWriter bw) => bw.WriteVector2(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeVector2 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Vector2(BinTreeVector2 property) => property.Value;
}
