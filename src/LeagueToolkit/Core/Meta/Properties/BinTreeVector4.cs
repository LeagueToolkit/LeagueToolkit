using LeagueToolkit.Utils.Extensions;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="Vector4"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeVector4 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Vector4;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public Vector4 Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeVector4"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeVector4(uint nameHash, Vector4 value) : base(nameHash) => this.Value = value;

    internal BinTreeVector4(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadVector4();

    protected override void WriteContent(BinaryWriter bw) => bw.WriteVector4(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 16;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeVector4 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Vector4(BinTreeVector4 property) => property.Value;
}
