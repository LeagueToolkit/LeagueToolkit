using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="short"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeI16 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.I16;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public short Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeI16"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeI16(uint nameHash, short value) : base(nameHash) => this.Value = value;

    internal BinTreeI16(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadInt16();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 2;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeI16 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator short(BinTreeI16 property) => property.Value;
}
