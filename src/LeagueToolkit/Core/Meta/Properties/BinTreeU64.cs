using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="ulong"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeU64 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.U64;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public ulong Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeU64"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeU64(uint nameHash, ulong value) : base(nameHash) => this.Value = value;

    internal BinTreeU64(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeU64 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator ulong(BinTreeU64 property) => property.Value;
}
