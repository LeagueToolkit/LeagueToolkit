using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="ushort"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeU16 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.U16;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public ushort Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeU16"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeU16(uint nameHash, ushort value) : base(nameHash) => this.Value = value;

    internal BinTreeU16(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt16();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 2;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeU16 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator ushort(BinTreeU16 property) => property.Value;
}
