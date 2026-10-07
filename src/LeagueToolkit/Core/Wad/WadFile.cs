using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Hashing;
using System;
using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace LeagueToolkit.Core.Wad;

/// <summary>
/// Represents a WAD archive
/// </summary>
/// <remarks>
/// Use <see cref="WadBuilder"/> to build WAD files
/// </remarks>
public sealed class WadFile : IDisposable
{
    internal const int HEADER_SIZE_V3 = 272;
    private const string GAME_DIRECTORY_NAME = "Game";

    /// <summary>
    /// Gets the chunks
    /// </summary>
    public IReadOnlyDictionary<ulong, WadChunk> Chunks => this._chunks;
    private readonly Dictionary<ulong, WadChunk> _chunks;

    public ReadOnlyMemory<WadSubchunk> Subchunks => this._subchunkTocMemory is null
        ? ReadOnlyMemory<WadSubchunk>.Empty
        : this._subchunkTocMemory.Memory.Cast<byte, WadSubchunk>();
    private readonly MemoryOwner<byte> _subchunkTocMemory;

    private readonly FileStream _stream;
    private readonly WadChunkDecompressor _chunkDecompressor = new();

    /// <summary>
    /// Gets a value indicating whether the archive has been disposed of
    /// </summary>
    public bool IsDisposed { get; private set; }

    internal WadFile(IEnumerable<WadChunk> chunks)
    {
        this._chunks = new(chunks.Select(x => new KeyValuePair<ulong, WadChunk>(x.PathHash, x)));
    }

    /// <summary>
    /// Creates a new <see cref="WadFile"/> object using the specified parameters
    /// </summary>
    /// <param name="path">The path of the <see cref="WadFile"/></param>
    public WadFile(string path) : this(File.OpenRead(path)) { }

    /// <summary>
    /// Creates a new <see cref="WadFile"/> object using the specified parameters
    /// </summary>
    /// <param name="stream">The stream to read the <see cref="WadFile"/> from</param>
    public WadFile(FileStream stream)
    {
        Guard.IsNotNull(stream, nameof(stream));

        this._stream = stream;
        using BinaryReader br = new(this._stream, Encoding.UTF8, true);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(2));
        if (magic is not "RW")
            ThrowHelper.ThrowInvalidDataException($"Invalid file signature: {magic}");

        byte major = br.ReadByte();
        byte minor = br.ReadByte();
        if (major > 3)
            ThrowHelper.ThrowInvalidDataException($"Invalid version: {major}.{minor}");

        ulong dataChecksum = 0; // probably not "dataChecksum"

        if (major is 2)
        {
            byte ecdsaLength = br.ReadByte();
            br.BaseStream.Seek(83, SeekOrigin.Current); // ECDSA Signature

            dataChecksum = br.ReadUInt64();
        }
        else if (major is 3)
        {
            br.BaseStream.Seek(256, SeekOrigin.Current); // ECDSA Signature
            dataChecksum = br.ReadUInt64();
        }

        if (major is 1 or 2)
        {
            ushort tocStartOffset = br.ReadUInt16();
            ushort tocFileEntrySize = br.ReadUInt16();
        }

        // Read chunks
        int chunkCount = br.ReadInt32();
        this._chunks = new(chunkCount);
        for (int i = 0; i < chunkCount; i++)
        {
            WadChunk chunk = WadChunk.Read(br, major);

            if (!this._chunks.TryAdd(chunk.PathHash, chunk))
                ThrowHelper.ThrowInvalidDataException($"Tried to read a chunk which already exists: {chunk.PathHash}");
        }

        // Load subchunk toc
        if(stream.Name.Contains(GAME_DIRECTORY_NAME))
        {
            // Read subchunk table
            // May this substring not crash till the end of times, amen :^)
            string subchunkTocPath = Path.ChangeExtension(
                stream.Name[(stream.Name.LastIndexOf(GAME_DIRECTORY_NAME, StringComparison.Ordinal) + GAME_DIRECTORY_NAME.Length + 1)..]
                    .ToLowerInvariant()
                    .Replace(Path.DirectorySeparatorChar, '/'),
                "subchunktoc"
            );

            if (this._chunks.TryGetValue(XxHash64Ext.Hash(subchunkTocPath), out WadChunk subchunkTocChunk))
                this._subchunkTocMemory = LoadChunkDecompressed(subchunkTocChunk);
        }
    }

    internal void WriteDescriptor(Stream stream)
    {
        using BinaryWriter bw = new(stream, Encoding.UTF8, true);

        bw.Write("RW"u8);
        bw.Write((byte)3); // major
        bw.Write((byte)1); // minor

        Span<byte> signature = stackalloc byte[256];
        signature.Clear();

        bw.Write(signature);
        bw.Write((ulong)0);

        bw.Write(this._chunks.Count);
        foreach (WadChunk chunk in this._chunks.Values.OrderBy(x => x.PathHash))
        {
            chunk.Write(bw);
        }
    }

    /// <summary>
    /// Loads the raw data of the specified chunk into memory
    /// </summary>
    /// <param name="path">The path of the chunk to load</param>
    /// <returns>A <see cref="MemoryOwner{T}"/> object with the loaded chunk data</returns>
    public MemoryOwner<byte> LoadChunk(string path) => LoadChunk(FindChunk(path));

    /// <summary>
    /// Loads the raw data of the specified chunk into memory
    /// </summary>
    /// <param name="pathHash">The path hash of the chunk to load</param>
    /// <returns>A <see cref="MemoryOwner{T}"/> object with the loaded chunk data</returns>
    public MemoryOwner<byte> LoadChunk(ulong pathHash) => LoadChunk(FindChunk(pathHash));

    /// <summary>
    /// Loads the raw data of the specified chunk into memory
    /// </summary>
    /// <param name="chunk">The chunk to load</param>
    /// <returns>A <see cref="MemoryOwner{T}"/> object with the loaded chunk data</returns>
    public MemoryOwner<byte> LoadChunk(WadChunk chunk)
    {
        MemoryOwner<byte> chunkDataOwner = MemoryOwner<byte>.Allocate(chunk.CompressedSize);

        this._stream.Seek(chunk.DataOffset, SeekOrigin.Begin);
        this._stream.Read(chunkDataOwner.Span);

        return chunkDataOwner;
    }

    /// <summary>
    /// Loads the decompressed data of the specified chunk into memory
    /// </summary>
    /// <param name="path">The path of the chunk to load</param>
    /// <returns>A <see cref="MemoryOwner{T}"/> object with the loaded decompresed chunk data</returns>
    public MemoryOwner<byte> LoadChunkDecompressed(string path) => LoadChunkDecompressed(FindChunk(path));

    /// <summary>
    /// Loads the decompressed data of the specified chunk into memory
    /// </summary>
    /// <param name="pathHash">The path hash of the chunk to load</param>
    /// <returns>A <see cref="MemoryOwner{T}"/> object with the loaded decompresed chunk data</returns>
    public MemoryOwner<byte> LoadChunkDecompressed(ulong pathHash) => LoadChunkDecompressed(FindChunk(pathHash));

    /// <summary>
    /// Loads the decompressed data of the specified chunk into memory
    /// </summary>
    /// <param name="chunk">The chunk to load</param>
    /// <returns>A <see cref="MemoryOwner{T}"/> object with the loaded decompresed chunk data</returns>
    public MemoryOwner<byte> LoadChunkDecompressed(WadChunk chunk)
    {
        using MemoryOwner<byte> chunkData = LoadChunk(chunk);
        return this._chunkDecompressor.Decompress(
            chunkData.Memory,
            chunk.Compression,
            chunk.UncompressedSize,
            GetSubchunks(chunk)
        );
    }

    /// <summary>
    /// Reads a range of decompressed chunk bytes into the supplied buffer.
    /// </summary>
    /// <param name="path">The path of the chunk to read</param>
    /// <param name="destination">The buffer that determines the maximum number of bytes to read</param>
    /// <param name="offset">The starting offset in the decompressed content</param>
    /// <returns>The number of bytes read, limited by the remaining decompressed content</returns>
    public int ReadChunkDecompressed(string path, Span<byte> destination, int offset = 0)
        => ReadChunkDecompressed(FindChunk(path), destination, offset);

    /// <summary>
    /// Reads a range of decompressed chunk bytes into the supplied buffer.
    /// </summary>
    /// <param name="pathHash">The lowercase path hash of the chunk to read</param>
    /// <param name="destination">The buffer that determines the maximum number of bytes to read</param>
    /// <param name="offset">The starting offset in the decompressed content</param>
    /// <returns>The number of bytes read, limited by the remaining decompressed content</returns>
    public int ReadChunkDecompressed(ulong pathHash, Span<byte> destination, int offset = 0)
        => ReadChunkDecompressed(FindChunk(pathHash), destination, offset);

    /// <summary>
    /// Reads a range of decompressed chunk bytes without loading the whole chunk.
    /// </summary>
    /// <param name="chunk">The chunk to read</param>
    /// <param name="destination">The buffer that determines the maximum number of bytes to read</param>
    /// <param name="offset">The starting offset in the decompressed content</param>
    /// <returns>The number of bytes read; zero for an empty buffer or an offset at or beyond the end</returns>
    /// <remarks>
    /// Uncompressed and chunked payloads seek to the requested range. GZip and ordinary Zstd
    /// decode the preceding content before reading it. The unread payload is not validated.
    /// Like other archive operations, calls on the same instance must not run concurrently.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The offset is negative</exception>
    /// <exception cref="ObjectDisposedException">The archive has been disposed</exception>
    /// <exception cref="NotSupportedException">The chunk is a satellite redirect</exception>
    /// <exception cref="InvalidDataException">The chunk metadata is invalid</exception>
    /// <exception cref="EndOfStreamException">The requested content is truncated</exception>
    public int ReadChunkDecompressed(WadChunk chunk, Span<byte> destination, int offset = 0)
    {
        ObjectDisposedException.ThrowIf(this.IsDisposed, this);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        if (chunk.Compression == WadChunkCompression.Satellite)
            throw new NotSupportedException("Reading satellite chunks is not supported");
        if (chunk.CompressedSize < 0 || chunk.UncompressedSize < 0 ||
            chunk.DataOffset < 0 || chunk.DataOffset > this._stream.Length - chunk.CompressedSize)
            throw new InvalidDataException("Invalid WAD chunk metadata");
        ReadOnlySpan<WadSubchunk> subchunks = GetSubchunks(chunk).Span;
        WadChunkDecompressor.ValidateMetadata(chunk.CompressedSize, chunk.Compression, chunk.UncompressedSize, subchunks);
        if (destination.IsEmpty || offset >= chunk.UncompressedSize)
            return 0;

        int count = Math.Min(destination.Length, chunk.UncompressedSize - offset);
        destination = destination[..count];
        if (chunk.Compression != WadChunkCompression.ZstdChunked)
        {
            this._chunkDecompressor.ReadRange(this._stream, chunk.DataOffset, chunk.CompressedSize, chunk.Compression, destination, offset);
            return count;
        }

        long storedOffset = chunk.DataOffset;
        int written = 0;
        foreach (WadSubchunk subchunk in subchunks)
        {
            if (offset >= subchunk.UncompressedSize)
                offset -= subchunk.UncompressedSize;
            else
            {
                int length = Math.Min(count - written, subchunk.UncompressedSize - offset);
                Span<byte> target = destination.Slice(written, length);
                try
                {
                    this._chunkDecompressor.ReadRange(this._stream, storedOffset, subchunk.CompressedSize, WadChunkCompression.Zstd, target, offset);
                }
                catch (ZstdSharp.ZstdException) when (subchunk.CompressedSize == subchunk.UncompressedSize)
                {
                    this._chunkDecompressor.ReadRange(this._stream, storedOffset, subchunk.CompressedSize, WadChunkCompression.None, target, offset);
                }
                written += length;
                offset = 0;
                if (written == count)
                    break;
            }
            storedOffset += subchunk.CompressedSize;
        }
        return written;
    }

    /// <summary>
    /// Opens a decompression stream for the specified chunk
    /// </summary>
    /// <param name="path">The path of the chunk to open a stream for</param>
    /// <returns>A <see cref="Stream"/> object that can be used to decompress the data using <see cref="Stream.CopyTo(Stream)"/></returns>
    public Stream OpenChunk(string path) => OpenChunk(FindChunk(path));

    /// <summary>
    /// Opens a decompression stream for the specified chunk
    /// </summary>
    /// <param name="pathHash">The path hash of the chunk to open a stream for</param>
    /// <returns>A <see cref="Stream"/> object that can be used to decompress the data using <see cref="Stream.CopyTo(Stream)"/></returns>
    public Stream OpenChunk(ulong pathHash) => OpenChunk(FindChunk(pathHash));

    /// <summary>
    /// Opens a decompression stream for the specified chunk
    /// </summary>
    /// <param name="chunk">The chunk to open a stream for</param>
    /// <returns>A <see cref="Stream"/> object that can be used to decompress the data using <see cref="Stream.CopyTo(Stream)"/></returns>
    public Stream OpenChunk(WadChunk chunk)
    {
        MemoryOwner<byte> chunkData = LoadChunk(chunk);

        if (chunk.Compression is WadChunkCompression.ZstdChunked)
        {
            using (chunkData)
            {
                return this._chunkDecompressor
                    .Decompress(chunkData.Memory, chunk.Compression, chunk.UncompressedSize, GetSubchunks(chunk))
                    .AsStream();
            }
        }

        try
        {
            return chunk.Compression switch
            {
                WadChunkCompression.None => chunkData.AsStream(),
                WadChunkCompression.GZip => new GZipStream(chunkData.AsStream(), CompressionMode.Decompress),
                WadChunkCompression.Satellite
                    => throw new NotImplementedException("Opening satellite chunks is not supported"),
                WadChunkCompression.Zstd => new ZstdSharp.DecompressionStream(chunkData.AsStream()),
                _ => throw new InvalidOperationException($"Invalid chunk compression type: {chunk.Compression}")
            };
        }
        catch
        {
            chunkData.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Searches for a chunk with the specified path
    /// </summary>
    /// <param name="path">The path of the <see cref="WadChunk"/></param>
    /// <returns>The found chunk</returns>
    public WadChunk FindChunk(string path)
    {
        Guard.IsNotNullOrEmpty(path, nameof(path));

        return FindChunk(XxHash64Ext.Hash(path.ToLowerInvariant()));
    }

    /// <summary>
    /// Searches for a chunk with the specified path hash
    /// </summary>
    /// <param name="pathHash">The lowercase path of the <see cref="WadChunk"/> hashed using <see cref="System.IO.Hashing.XxHash64"/></param>
    /// <returns>The found chunk</returns>
    public WadChunk FindChunk(ulong pathHash)
    {
        if (!this._chunks.TryGetValue(pathHash, out WadChunk chunk))
            ThrowHelper.ThrowArgumentException(nameof(pathHash), $"Failed to find chunk: {pathHash}");

        return chunk;
    }

    private ReadOnlyMemory<WadSubchunk> GetSubchunks(WadChunk chunk)
    {
        if (chunk.Compression is not WadChunkCompression.ZstdChunked)
            return ReadOnlyMemory<WadSubchunk>.Empty;

        if (chunk.StartSubChunk < 0
            || chunk.SubChunkCount <= 0
            || chunk.StartSubChunk > this.Subchunks.Length - chunk.SubChunkCount)
            ThrowHelper.ThrowInvalidDataException("The WAD chunk references an invalid sub-chunk table range");

        return this.Subchunks.Slice(chunk.StartSubChunk, chunk.SubChunkCount);
    }

    /// <summary>
    /// Disposes the <see cref="WadFile"/> object and the wrapped <see cref="Stream"/> instance
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.IsDisposed)
            return;

        if (disposing)
        {
            this._stream?.Dispose();
            this._subchunkTocMemory?.Dispose();
            this._chunkDecompressor.Dispose();
        }

        this.IsDisposed = true;
    }
}
