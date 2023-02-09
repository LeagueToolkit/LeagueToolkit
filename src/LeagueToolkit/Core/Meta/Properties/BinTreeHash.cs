using LeagueToolkit.Hashing;
using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="Fnv1a"/> hash value
/// </summary>
[DebuggerDisplay("{_debuggerDisplay, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeHash : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Hash;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public uint Value { get; set; }

    private string _debuggerDisplay => string.Format("{0:x}", this.Value);

    /// <summary>
    /// Creates a new <see cref="BinTreeHash"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeHash(uint nameHash, uint value) : base(nameHash) => this.Value = value;

    internal BinTreeHash(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt32();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeHash property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator uint(BinTreeHash property) => property.Value;
}
