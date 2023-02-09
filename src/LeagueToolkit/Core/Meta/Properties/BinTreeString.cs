using System.Diagnostics;
using System.Text;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="string"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeString : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.String;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="BinTreeString"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeString(uint nameHash, string value) : base(nameHash) => this.Value = value;

    internal BinTreeString(BinaryReader br, uint nameHash) : base(nameHash) =>
        this.Value = Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));

    protected override void WriteContent(BinaryWriter bw)
    {
        bw.Write((ushort)this.Value.Length);
        bw.Write(Encoding.UTF8.GetBytes(this.Value));
    }

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 2 + this.Value.Length;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeString property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator string(BinTreeString property) => property.Value;
}
