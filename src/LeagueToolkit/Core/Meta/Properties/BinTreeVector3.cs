using LeagueToolkit.Utils.Extensions;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="Vector3"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeVector3 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Vector3;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public Vector3 Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeVector3"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeVector3(uint nameHash, Vector3 value) : base(nameHash) => this.Value = value;

    internal BinTreeVector3(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadVector3();

    protected override void WriteContent(BinaryWriter bw) => bw.WriteVector3(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 12;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeVector3 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Vector3(BinTreeVector3 property) => property.Value;
}
