using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Utils.Extensions;
using System;
using System.IO.Compression;
using System.Text;
using XXHash3NET;

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

    public ReadOnlyMemory<WadSubchunk> Subchunks => this._subchunkTocMemory.Memory.Cast<byte, WadSubchunk>();
    private readonly MemoryOwner<byte> _subchunkTocMemory;

    private readonly FileStream _stream;
    private readonly ZstdSharp.Decompressor _zstdDecompressor = new();

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

        // Read subchunk table
        // May this substring not crash till the end of times, amen :^)
        string subchunkTocPath = Path.ChangeExtension(
            stream.Name[(stream.Name.LastIndexOf(GAME_DIRECTORY_NAME) + GAME_DIRECTORY_NAME.Length + 1)..]
                .ToLower()
                .Replace(Path.DirectorySeparatorChar, '/'),
            "subchunktoc"
        );
        if (this._chunks.TryGetValue(XXHash64.Compute(subchunkTocPath), out WadChunk subchunkTocChunk))
            this._subchunkTocMemory = LoadChunkDecompressed(subchunkTocChunk);
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
        using Stream decompressionStream = OpenChunk(chunk);
        MemoryOwner<byte> decompressedChunk = MemoryOwner<byte>.Allocate(chunk.UncompressedSize);

        decompressionStream.ReadExact(decompressedChunk.Span);

        return decompressedChunk;
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

        return chunk.Compression switch
        {
            WadChunkCompression.None => chunkData.AsStream(),
            WadChunkCompression.GZip => new GZipStream(chunkData.AsStream(), CompressionMode.Decompress),
            WadChunkCompression.Satellite
                => throw new NotImplementedException("Opening satellite chunks is not supported"),
            WadChunkCompression.Zstd => new ZstdSharp.DecompressionStream(chunkData.AsStream()),
            WadChunkCompression.ZstdChunked => DecompressZstdChunkedChunk(chunk, chunkData).AsStream(),

            _ => throw new InvalidOperationException($"Invalid chunk compression type: {chunk.Compression}")
        };
    }

    /// <summary>
    /// Searches for a chunk with the specified path
    /// </summary>
    /// <param name="path">The path of the <see cref="WadChunk"/></param>
    /// <returns>The found chunk</returns>
    public WadChunk FindChunk(string path)
    {
        Guard.IsNotNullOrEmpty(path, nameof(path));

        return FindChunk(XXHash64.Compute(path.ToLower()));
    }

    /// <summary>
    /// Searches for a chunk with the specified path hash
    /// </summary>
    /// <param name="pathHash">The lowercase path of the <see cref="WadChunk"/> hashed using <see cref="XXHash64"/></param>
    /// <returns>The found chunk</returns>
    public WadChunk FindChunk(ulong pathHash)
    {
        if (!this._chunks.TryGetValue(pathHash, out WadChunk chunk))
            ThrowHelper.ThrowArgumentException(nameof(pathHash), $"Failed to find chunk: {pathHash}");

        return chunk;
    }

    private MemoryOwner<byte> DecompressZstdChunkedChunk(WadChunk chunk, MemoryOwner<byte> chunkData)
    {
        var decompressedData = MemoryOwner<byte>.Allocate(chunk.UncompressedSize);
        ReadOnlySpan<WadSubchunk> subchunks = this.Subchunks.Span.Slice(chunk.StartSubChunk, chunk.SubChunkCount);

        int rawOffset = 0;
        int decompressedOffset = 0;
        for (int i = 0; i < subchunks.Length; i++)
        {
            WadSubchunk subchunk = subchunks[i];

            // Try decompressing subchunk
            bool successfullyDecompressed = this._zstdDecompressor.TryUnwrap(
                chunkData.Span.Slice(rawOffset, subchunk.CompressedSize),
                decompressedData.Span[decompressedOffset..],
                out _
            );

            // If decompression failed, copy raw data to output buffer
            if (successfullyDecompressed is not true)
                chunkData.Span.CopyTo(decompressedData.Span[decompressedOffset..]);

            rawOffset += subchunk.CompressedSize;
            decompressedOffset += subchunk.UncompressedSize;
        }

        // Dispose of raw chunk data
        chunkData.Dispose();

        return decompressedData;
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
        }

        this.IsDisposed = true;
    }
}
