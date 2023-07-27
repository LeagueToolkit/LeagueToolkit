namespace LeagueToolkit.Core.Wad;

/// <summary>
/// Represents a file entry in a <see cref="WadFile"/>
/// </summary>
public readonly struct WadChunk
{
    internal const int TOC_SIZE_V3 = 32;

    /// <summary>
    /// Gets the lowercase path of the chunk hashed using <see cref="System.IO.Hashing.XxHash64"/>
    /// </summary>
    public ulong PathHash { get; }

    /// <summary>
    /// Gets the offset to the data of the chunk
    /// </summary>
    public long DataOffset { get; }

    /// <summary>
    /// Gets the compressed size of the chunk
    /// </summary>
    public int CompressedSize { get; }

    /// <summary>
    /// Gets the uncompressed size of the chunk
    /// </summary>
    public int UncompressedSize { get; }

    /// <summary>
    /// Gets the compression of the chunk data
    /// </summary>
    public WadChunkCompression Compression { get; }

    /// <summary>
    /// Gets a value indicating whether the data of this chunk is duplicated
    /// </summary>
    public bool IsDuplicated { get; }

    /// <summary>
    /// Gets the sub-chunk count
    /// </summary>
    public int SubChunkCount { get; }

    /// <summary>
    /// Gets the start sub-chunk index
    /// </summary>
    public int StartSubChunk { get; }

    private readonly ulong _checksum;

    internal WadChunk(
        ulong pathHash,
        long dataOffset,
        int compressedSize,
        int uncompressedSize,
        WadChunkCompression compression,
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
        this.Compression = compression;
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
        bw.Write((byte)this.Compression);
        bw.Write(this.IsDuplicated);
        bw.Write((ushort)0);
        bw.Write(this._checksum);
    }
}

/// <summary>
/// Represents the compression type used for the data of a <see cref="WadChunk"/>
/// </summary>
public enum WadChunkCompression : byte
{
    /// <summary>
    /// No compression
    /// </summary>
    None,

    /// <summary>
    /// GZip compression
    /// </summary>
    GZip,

    /// <summary>
    /// The data of this chunk contains a string file redirect
    /// </summary>
    Satellite,

    /// <summary>
    /// ZStandard compression
    /// </summary>
    Zstd,

    /// <summary>
    /// Chunked ZStandard compression using sub-chunks
    /// </summary>
    ZstdChunked
}
