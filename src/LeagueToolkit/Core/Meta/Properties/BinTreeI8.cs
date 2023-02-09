using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="sbyte"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeI8 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.I8;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public sbyte Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeI8"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeI8(uint nameHash, sbyte value) : base(nameHash) => this.Value = value;

    internal BinTreeI8(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadSByte();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeI8 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator sbyte(BinTreeI8 property) => property.Value;
}
