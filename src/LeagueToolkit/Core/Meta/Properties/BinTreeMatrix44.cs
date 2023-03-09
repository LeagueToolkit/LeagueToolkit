using LeagueToolkit.Utils.Extensions;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="Matrix4x4"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeMatrix44 : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Matrix44;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public Matrix4x4 Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeMatrix44"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeMatrix44(uint nameHash, Matrix4x4 value) : base(nameHash) => this.Value = value;

    internal BinTreeMatrix44(BinaryReader br, uint nameHash) : base(nameHash) =>
        this.Value = br.ReadMatrix4x4RowMajor();

    protected override void WriteContent(BinaryWriter bw) => bw.WriteMatrix4x4RowMajor(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 64;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeMatrix44 property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator Matrix4x4(BinTreeMatrix44 property) => property.Value;
}
