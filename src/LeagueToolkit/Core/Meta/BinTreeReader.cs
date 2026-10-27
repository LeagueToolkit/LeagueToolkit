using System.Text;

namespace LeagueToolkit.Core.Meta;

/// <summary>
/// The reader a <see cref="BinTree"/> is read through. It carries the stream offsets of the
/// <see cref="BinPropertyType.Hash"/> values that are stored in 8 bytes, which
/// <see cref="BinHashWidthResolver"/> finds for an object that does not read with 4-byte hashes.
/// </summary>
internal sealed class BinTreeReader : BinaryReader
{
    /// <summary>Stream positions at which a <see cref="BinPropertyType.Hash"/> value is 8 bytes wide.</summary>
    public HashSet<long> WideHashOffsets { get; } = new();

    public BinTreeReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }

    public bool IsWideHashAt(long position) => this.WideHashOffsets.Count > 0 && this.WideHashOffsets.Contains(position);
}
