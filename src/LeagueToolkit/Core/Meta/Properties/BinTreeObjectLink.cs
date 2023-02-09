using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a value that links to a <see cref="BinTreeObject.PathHash"/>
/// </summary>
[DebuggerDisplay("{_debuggerDisplay, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeObjectLink : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.ObjectLink;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public uint Value { get; set; }

    private string _debuggerDisplay => string.Format("{0:x}", this.Value);

    /// <summary>
    /// Creates a new <see cref="BinTreeObjectLink"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeObjectLink(uint nameHash, uint value) : base(nameHash) => this.Value = value;

    internal BinTreeObjectLink(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt32();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 4;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeObjectLink property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator uint(BinTreeObjectLink property) => property.Value;
}
