using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="uint"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeU32 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.U32;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public uint Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeU32"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeU32(uint nameHash, uint value) : base(nameHash) => this.Value = value;

    internal BinTreeU32(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt32();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeU32 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator uint(BinTreeU32 property) => property.Value;
}
