using LeagueToolkit.Core.Wad;
using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a value that links to a <see cref="WadChunk.PathHash"/>
/// </summary>
[DebuggerDisplay("{_debuggerDisplay, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeWadChunkLink : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.WadChunkLink;

    /// <summary>
    /// Gets the value of the property
    /// </summary>
    public ulong Value { get; set; }

    private string _debuggerDisplay => string.Format("{0:x}", this.Value);

    /// <summary>
    /// Creates a new <see cref="BinTreeWadChunkLink"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeWadChunkLink(uint nameHash, ulong value) : base(nameHash) => this.Value = value;

    internal BinTreeWadChunkLink(BinaryReader br, uint nameHash) : base(nameHash) => this.Value = br.ReadUInt64();

    protected override void WriteContent(BinaryWriter bw) => bw.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 8;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeWadChunkLink property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };
}
