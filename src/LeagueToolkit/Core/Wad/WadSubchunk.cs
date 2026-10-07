namespace LeagueToolkit.Core.Wad;

/// <summary>
/// Describes a sub-chunk in a chunked WAD payload
/// </summary>
public readonly struct WadSubchunk
{
    /// <summary>
    /// Gets the stored size of the sub-chunk
    /// </summary>
    public int CompressedSize { get; }

    /// <summary>
    /// Gets the decompressed size of the sub-chunk
    /// </summary>
    public int UncompressedSize { get; }

    /// <summary>
    /// Gets the checksum of the sub-chunk
    /// </summary>
    public ulong Checksum { get; }

    /// <summary>
    /// Creates a new <see cref="WadSubchunk"/> descriptor
    /// </summary>
    /// <param name="compressedSize">The stored size of the sub-chunk</param>
    /// <param name="uncompressedSize">The decompressed size of the sub-chunk</param>
    /// <param name="checksum">The checksum of the sub-chunk</param>
    public WadSubchunk(int compressedSize, int uncompressedSize, ulong checksum = 0)
    {
        this.CompressedSize = compressedSize;
        this.UncompressedSize = uncompressedSize;
        this.Checksum = checksum;
    }
}
