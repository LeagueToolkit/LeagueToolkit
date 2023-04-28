namespace LeagueToolkit.Core.Wad;

public readonly struct WadSubchunk
{
    public int CompressedSize { get; }
    public int UncompressedSize { get; }
    public ulong Checksum { get; }
}
