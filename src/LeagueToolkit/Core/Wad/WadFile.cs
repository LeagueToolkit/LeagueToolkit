using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using System.IO.Compression;
using System.Text;
using XXHash3NET;

namespace LeagueToolkit.Core.Wad;

public sealed class WadFile : IDisposable
{
    internal const int HEADER_SIZE_V3 = 272;

    public IReadOnlyDictionary<ulong, WadChunk> Chunks => this._chunks;
    private readonly Dictionary<ulong, WadChunk> _chunks;

    private readonly FileStream _stream;

    public bool IsDiposed { get; private set; }

    internal WadFile(IEnumerable<WadChunk> chunks)
    {
        this._chunks = new(chunks.Select(x => new KeyValuePair<ulong, WadChunk>(x.PathHash, x)));
    }

    public WadFile(string path)
    {
        // Open wad stream and store it for later
        this._stream = File.OpenRead(path);
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

    public MemoryOwner<byte> LoadChunk(string path) => LoadChunk(XXHash64.Compute(path.ToLower()));

    public MemoryOwner<byte> LoadChunk(ulong pathHash)
    {
        WadChunk chunk = FindChunk(pathHash);

        return LoadChunk(chunk);
    }

    public MemoryOwner<byte> LoadChunk(WadChunk chunk)
    {
        MemoryOwner<byte> chunkDataOwner = MemoryOwner<byte>.Allocate(chunk.CompressedSize);
        this._stream.Read(chunkDataOwner.Span);

        return chunkDataOwner;
    }

    public Stream OpenChunk(string path) => OpenChunk(XXHash64.Compute(path.ToLower()));

    public Stream OpenChunk(ulong pathHash)
    {
        WadChunk chunk = FindChunk(pathHash);

        return OpenChunk(chunk);
    }

    public Stream OpenChunk(WadChunk chunk)
    {
        MemoryOwner<byte> chunkData = LoadChunk(chunk);

        return chunk.CompressionType switch
        {
            WadChunkCompression.None => chunkData.AsStream(),
            WadChunkCompression.GZip => new GZipStream(chunkData.AsStream(), CompressionMode.Decompress),
            WadChunkCompression.Satellite
                => throw new NotImplementedException("Opening satellite chunks is not supported"),
            // zstd handles frames for us so we can ignore chunks
            WadChunkCompression.Zstd
            or WadChunkCompression.ZstdChunked
                => new ZstdNet.DecompressionStream(chunkData.AsStream()),
            _ => throw new InvalidOperationException($"Invalid chunk compression type: {chunk.CompressionType}")
        };
    }

    public WadChunk FindChunk(string path) => FindChunk(XXHash64.Compute(path.ToLower()));

    public WadChunk FindChunk(ulong pathHash)
    {
        if (!this._chunks.TryGetValue(pathHash, out WadChunk chunk))
            ThrowHelper.ThrowArgumentException(nameof(pathHash), $"Failed to find chunk: {pathHash}");

        return chunk;
    }

    private void Dispose(bool disposing)
    {
        if (this.IsDiposed)
            return;

        if (disposing)
        {
            this._stream?.Dispose();
        }

        this.IsDiposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
