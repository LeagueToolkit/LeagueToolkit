using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="byte"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeU8 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.U8;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public byte Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeU8"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeU8(uint nameHash, byte value) : base(nameHash) => this.Value = value;

    internal BinTreeU8(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadByte();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? 5 : 0) + 1;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeU8 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator byte(BinTreeU8 property) => property.Value;
}
