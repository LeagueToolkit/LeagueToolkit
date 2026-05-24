using System.Buffers.Binary;

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

    /// <summary>
    /// Gets the chunk checksum
    /// </summary>
    public ulong Checksum => this._checksum;

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
        int size = major >= 2 ? TOC_SIZE_V3 : 24;
        Span<byte> buffer = stackalloc byte[size];
        br.ReadExactly(buffer);

        return Read(buffer, major);
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

    internal static WadChunk Read(ReadOnlySpan<byte> entry, byte major)
    {
        ulong xxhash = BinaryPrimitives.ReadUInt64LittleEndian(entry[..8]);
        long dataOffset = BinaryPrimitives.ReadUInt32LittleEndian(entry[8..12]);
        int compressedSize = BinaryPrimitives.ReadInt32LittleEndian(entry[12..16]);
        int uncompressedSize = BinaryPrimitives.ReadInt32LittleEndian(entry[16..20]);

        byte type_subChunkCount = entry[20];
        int subChunkCount = type_subChunkCount >> 4;
        WadChunkCompression chunkCompression = (WadChunkCompression)(type_subChunkCount & 0xF);

        bool isDuplicated = entry[21] != 0;
        ushort startSubChunk = BinaryPrimitives.ReadUInt16LittleEndian(entry[22..24]);
        ulong checksum = major >= 2 ? BinaryPrimitives.ReadUInt64LittleEndian(entry[24..32]) : 0;

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
