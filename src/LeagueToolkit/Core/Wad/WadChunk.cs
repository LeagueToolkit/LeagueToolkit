namespace LeagueToolkit.Core.Wad;

public readonly struct WadChunk
{
    internal const int TOC_SIZE_V3 = 32;

    public ulong PathHash { get; }

    public long DataOffset { get; }
    public int CompressedSize { get; }
    public int UncompressedSize { get; }
    public WadChunkCompression CompressionType { get; }
    public bool IsDuplicated { get; }

    public int SubChunkCount { get; }
    public int StartSubChunk { get; }

    internal WadChunk(
        ulong pathHash,
        long dataOffset,
        int compressedSize,
        int uncompressedSize,
        WadChunkCompression compressionType,
        bool isDuplicated,
        int subChunkCount,
        int startSubChunk
    )
    {
        this.PathHash = pathHash;

        this.DataOffset = dataOffset;
        this.CompressedSize = compressedSize;
        this.UncompressedSize = uncompressedSize;
        this.CompressionType = compressionType;
        this.IsDuplicated = isDuplicated;

        this.SubChunkCount = subChunkCount;
        this.StartSubChunk = startSubChunk;
    }

    internal static WadChunk Read(BinaryReader br, byte major)
    {
        ulong xxhash = br.ReadUInt64();

        long dataOffset = br.ReadUInt32();
        int compressedSize = br.ReadInt32();
        int uncompressedSize = br.ReadInt32();

        byte type_subChunkCount = br.ReadByte();
        int subChunkCount = type_subChunkCount >> 4;
        WadChunkCompression dataType = (WadChunkCompression)(type_subChunkCount & 0xF);

        bool isDuplicated = br.ReadBoolean();
        ushort startSubChunk = br.ReadUInt16();

        if (major >= 2)
        {
            // We don't care about checksum, so skip it to prevent useless micro-allocations
            // If (major == 3 && minor >= 1) checksumType = XXHash3
            // Else checksumType = SHA256
            br.BaseStream.Seek(8, SeekOrigin.Current);
        }

        //if (this.Type == WadEntryType.FileRedirection)
        //{
        //    long currentPosition = br.BaseStream.Position;
        //    br.BaseStream.Seek(this._dataOffset, SeekOrigin.Begin);
        //    this.FileRedirection = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
        //    br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
        //}

        return new(
            xxhash,
            dataOffset,
            compressedSize,
            uncompressedSize,
            dataType,
            isDuplicated,
            subChunkCount,
            startSubChunk
        );
    }
}

public enum WadChunkCompression : byte
{
    None,
    GZip,
    Satellite,
    Zstd,
    ZstdChunked
}
