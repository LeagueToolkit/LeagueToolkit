using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="long"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeI64 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.I64;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public long Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeI64"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeI64(uint nameHash, long value) : base(nameHash) => this.Value = value;

    internal BinTreeI64(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeI64 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator long(BinTreeI64 property) => property.Value;
}
