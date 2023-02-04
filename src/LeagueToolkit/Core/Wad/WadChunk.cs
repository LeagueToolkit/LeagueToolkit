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

    private readonly ulong _checksum;

    internal WadChunk(
        ulong pathHash,
        long dataOffset,
        int compressedSize,
        int uncompressedSize,
        WadChunkCompression compressionType,
        bool isDuplicated,
        int subChunkCount,
        int startSubChunk,
        ulong checksum
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

        this._checksum = checksum;
    }

    internal static WadChunk Read(BinaryReader br, byte major)
    {
        ulong xxhash = br.ReadUInt64();

        long dataOffset = br.ReadUInt32();
        int compressedSize = br.ReadInt32();
        int uncompressedSize = br.ReadInt32();

        byte type_subChunkCount = br.ReadByte();
        int subChunkCount = type_subChunkCount >> 4;
        WadChunkCompression chunkCompression = (WadChunkCompression)(type_subChunkCount & 0xF);

        bool isDuplicated = br.ReadBoolean();
        ushort startSubChunk = br.ReadUInt16();
        ulong checksum = major switch
        {
            >= 2 => br.ReadUInt64(),
            _ => 0
        };

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
            chunkCompression,
            isDuplicated,
            subChunkCount,
            startSubChunk,
            checksum
        );
    }

    internal void Write(BinaryWriter bw)
    {
        bw.Write(this.PathHash);
        bw.Write((uint)this.DataOffset);
        bw.Write(this.CompressedSize);
        bw.Write(this.UncompressedSize);
        bw.Write((byte)this.CompressionType);
        bw.Write(this.IsDuplicated);
        bw.Write((ushort)0);
        bw.Write(this._checksum);
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
