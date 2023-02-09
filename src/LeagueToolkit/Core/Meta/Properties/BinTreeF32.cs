using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="float"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeF32 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.F32;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeF32"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeF32(uint nameHash, float value) : base(nameHash) => this.Value = value;

    internal BinTreeF32(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadSingle();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeF32 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator float(BinTreeF32 property) => property.Value;
}
