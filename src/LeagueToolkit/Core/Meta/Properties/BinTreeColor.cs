using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="Color"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeColor : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Color;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public Color Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeColor"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeColor(uint nameHash, Color value) : base(nameHash) => this.Value = value;

    internal BinTreeColor(BinaryReader br, uint nameHash) : base(nameHash) =>
        this.Value = br.ReadColor(ColorFormat.RgbaU8);

    protected override void WriteContent(BinaryWriter bw) => bw.WriteColor(this.Value, ColorFormat.RgbaU8);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeColor property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Color(BinTreeColor property) => property.Value;

    public static implicit operator Vector4(BinTreeColor property) => property.Value;
}
