using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="int"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeI32 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.I32;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeI32"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeI32(uint nameHash, int value) : base(nameHash) => this.Value = value;

    internal BinTreeI32(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadInt32();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeI32 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator int(BinTreeI32 property) => property.Value;
}
