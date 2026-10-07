using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using System.IO.Compression;

namespace LeagueToolkit.Tests.Core.Wad;

public sealed class WadRangeReadTests
{
    [Theory]
    [InlineData(WadChunkCompression.None)]
    [InlineData(WadChunkCompression.GZip)]
    [InlineData(WadChunkCompression.Zstd)]
    [InlineData(WadChunkCompression.ZstdChunked)]
    public void ReadsPrefixesAndRangesAndPreservesCompleteReads(WadChunkCompression compression)
    {
        using var fixture = new Archive(compression);
        byte[] buffer = new byte[37];
        foreach (int offset in new[] { 0, 13, 4090, 4096, 8190, 9000, fixture.Content.Length - 3 })
        {
            Array.Fill(buffer, (byte)0xcc);
            int expected = Math.Min(buffer.Length, fixture.Content.Length - offset);
            Assert.Equal(expected, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer, offset));
            Assert.Equal(fixture.Content.AsSpan(offset, expected).ToArray(), buffer[..expected]);
            Assert.All(buffer[expected..], value => Assert.Equal(0xcc, value));
        }
        // Partial streaming must not leave decoder state that breaks the existing full-read API.
        using var complete = fixture.Wad.LoadChunkDecompressed(fixture.Chunk);
        Assert.Equal(fixture.Content, complete.Span.ToArray());
        Assert.Equal(4, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer.AsSpan(0, 4)));
        Assert.Equal(fixture.Content[..4], buffer[..4]);
    }

    [Theory]
    [InlineData(WadChunkCompression.None)]
    [InlineData(WadChunkCompression.GZip)]
    [InlineData(WadChunkCompression.Zstd)]
    [InlineData(WadChunkCompression.ZstdChunked)]
    public void SupportsPathAndHashOverloadsAndEmptyReads(WadChunkCompression compression)
    {
        using var fixture = new Archive(compression);
        byte[] buffer = new byte[12];
        Assert.Equal(12, fixture.Wad.ReadChunkDecompressed(Archive.ContentPath, buffer, 7));
        Assert.Equal(fixture.Content[7..19], buffer);
        Assert.Equal(12, fixture.Wad.ReadChunkDecompressed(fixture.Chunk.PathHash, buffer, 21));
        Assert.Equal(fixture.Content[21..33], buffer);
        fixture.Stream.ResetReads();
        Assert.Equal(0, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, Span<byte>.Empty));
        Assert.Equal(0, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer, fixture.Content.Length));
        Assert.Equal(0, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer, int.MaxValue));
        Assert.Equal(0, fixture.Stream.BytesRead);
    }

    [Theory]
    [InlineData(WadChunkCompression.None)]
    [InlineData(WadChunkCompression.GZip)]
    [InlineData(WadChunkCompression.Zstd)]
    [InlineData(WadChunkCompression.ZstdChunked)]
    public void PrefixReadDoesNotReadTheWholeStoredPayload(WadChunkCompression compression)
    {
        using var fixture = new Archive(compression, size: 2 * 1024 * 1024);
        byte[] buffer = new byte[4];
        fixture.Stream.ResetReads();
        Assert.Equal(4, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer));
        Assert.Equal(fixture.Content[..4], buffer);
        Assert.True(fixture.Stream.BytesRead < fixture.Chunk.CompressedSize / 4,
            $"Read {fixture.Stream.BytesRead} bytes from {fixture.Chunk.CompressedSize} stored bytes.");
    }

    [Fact]
    public void ChunkedRangeSkipsUnrequestedDamagedSubchunks()
    {
        using var fixture = new Archive(WadChunkCompression.ZstdChunked, corruptFirst: true);
        byte[] buffer = new byte[16];
        fixture.Stream.ResetReads();
        Assert.Equal(16, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer, 4100));
        Assert.Equal(fixture.Content[4100..4116], buffer);
        Assert.InRange(fixture.Stream.BytesRead, 16, 16384);
        Assert.Throws<ZstdSharp.ZstdException>(() => fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer));
        Assert.Equal(16, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer, 4100));
    }

    [Fact]
    public void ChunkedPrefixSpansSeveralSmallSubchunks()
    {
        using var fixture = new Archive(WadChunkCompression.ZstdChunked, firstSize: 2);
        byte[] buffer = new byte[10];
        Assert.Equal(10, fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer));
        Assert.Equal(fixture.Content[..10], buffer);
    }

    [Theory]
    [InlineData(WadChunkCompression.GZip)]
    [InlineData(WadChunkCompression.Zstd)]
    public void ThrowsWhenRequestedDecodedContentIsTruncated(WadChunkCompression compression)
    {
        using var fixture = new Archive(compression, decodedSize: 65537);
        Assert.Throws<EndOfStreamException>(() =>
            fixture.Wad.ReadChunkDecompressed(fixture.Chunk, new byte[fixture.Content.Length + 1]));
    }

    [Fact]
    public void RejectsInvalidOffsetsMetadataAndDisposedArchives()
    {
        using var fixture = new Archive(WadChunkCompression.None);
        byte[] buffer = new byte[4];
        Assert.Throws<ArgumentOutOfRangeException>(() => fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer, -1));
        using var negativeStored = new Archive(WadChunkCompression.None, storedSize: -1);
        using var negativeDecoded = new Archive(WadChunkCompression.None, decodedSize: -1);
        using var mismatched = new Archive(WadChunkCompression.None, decodedSize: 10);
        using var satellite = new Archive(WadChunkCompression.None, declaredCompression: WadChunkCompression.Satellite);
        Assert.Throws<InvalidDataException>(() => negativeStored.Wad.ReadChunkDecompressed(negativeStored.Chunk, buffer));
        Assert.Throws<InvalidDataException>(() => negativeDecoded.Wad.ReadChunkDecompressed(negativeDecoded.Chunk, buffer));
        Assert.Throws<InvalidDataException>(() => mismatched.Wad.ReadChunkDecompressed(mismatched.Chunk, buffer));
        Assert.Throws<NotSupportedException>(() => satellite.Wad.ReadChunkDecompressed(satellite.Chunk, buffer));
        fixture.Wad.Dispose();
        Assert.Throws<ObjectDisposedException>(() => fixture.Wad.ReadChunkDecompressed(fixture.Chunk, buffer));
    }

    [Fact]
    public void RejectsMissingAndInconsistentSubchunkTables()
    {
        using var missing = new Archive(WadChunkCompression.None, declaredCompression: WadChunkCompression.ZstdChunked);
        Assert.Throws<InvalidDataException>(() => missing.Wad.ReadChunkDecompressed(missing.Chunk, new byte[4]));
        using var chunked = new Archive(WadChunkCompression.ZstdChunked, storedSize: 1);
        Assert.Throws<InvalidDataException>(() => chunked.Wad.ReadChunkDecompressed(chunked.Chunk, new byte[4]));
    }

    private sealed class Archive : IDisposable
    {
        public const string ContentPath = "assets/range.bin";
        private readonly string _root = Path.Combine(Path.GetTempPath(), $"LeagueToolkitRange_{Guid.NewGuid():N}");
        public byte[] Content { get; }
        public CountingFileStream Stream { get; }
        public WadFile Wad { get; }
        public WadChunk Chunk { get; }

        public Archive(WadChunkCompression compression, int size = 65536, bool corruptFirst = false, int firstSize = 4096,
            int? storedSize = null, int? decodedSize = null, WadChunkCompression? declaredCompression = null)
        {
            Content = new byte[size];
            new Random(1234).NextBytes(Content);
            using var compressor = new ZstdSharp.Compressor();
            byte[] payload;
            using var table = new MemoryStream();
            bool chunked = compression == WadChunkCompression.ZstdChunked;
            if (chunked)
            {
                byte[][] decoded = { Content[..firstSize], Content[firstSize..8192], Content[8192..] };
                byte[][] stored = { compressor.Wrap(decoded[0]).ToArray(), decoded[1], compressor.Wrap(decoded[2]).ToArray() };
                if (corruptFirst) stored[0][0] ^= 0xff;
                using var writer = new BinaryWriter(table, System.Text.Encoding.UTF8, leaveOpen: true);
                for (int index = 0; index < stored.Length; index++)
                {
                    writer.Write(stored[index].Length); writer.Write(decoded[index].Length); writer.Write(0UL);
                }
                payload = stored.SelectMany(bytes => bytes).ToArray();
            }
            else if (compression == WadChunkCompression.Zstd)
                payload = compressor.Wrap(Content).ToArray();
            else if (compression == WadChunkCompression.GZip)
            {
                using var output = new MemoryStream();
                using (var gzip = new GZipStream(output, CompressionMode.Compress, leaveOpen: true)) gzip.Write(Content);
                payload = output.ToArray();
            }
            else payload = Content;

            string directory = Path.Combine(_root, "Game", "data");
            Directory.CreateDirectory(directory);
            string path = Path.Combine(directory, "test.wad.client");
            using (var file = File.Create(path))
            using (var writer = new BinaryWriter(file))
            {
                writer.Write("RW"u8); writer.Write((byte)3); writer.Write((byte)1);
                writer.Write(new byte[256]); writer.Write(0UL); writer.Write(chunked ? 2 : 1);
                int start = 272 + 32 * (chunked ? 2 : 1);
                WriteEntry(writer, XxHash64Ext.Hash(ContentPath), start, storedSize ?? payload.Length,
                    decodedSize ?? Content.Length, declaredCompression ?? compression, chunked ? 3 : 0);
                if (chunked) WriteEntry(writer, XxHash64Ext.Hash("data/test.wad.subchunktoc"), start + payload.Length,
                    (int)table.Length, (int)table.Length, WadChunkCompression.None, 0);
                writer.Write(payload);
                if (chunked) writer.Write(table.ToArray());
            }
            Stream = new CountingFileStream(path);
            Wad = new WadFile(Stream);
            Chunk = Wad.FindChunk(ContentPath);
        }

        private static void WriteEntry(BinaryWriter writer, ulong hash, int offset, int stored, int decoded,
            WadChunkCompression compression, int subchunks)
        {
            writer.Write(hash); writer.Write((uint)offset); writer.Write(stored); writer.Write(decoded);
            writer.Write((byte)((subchunks << 4) | (int)compression)); writer.Write((byte)0);
            writer.Write((ushort)0); writer.Write(0UL);
        }

        public void Dispose()
        {
            Wad.Dispose();
            Directory.Delete(_root, recursive: true);
        }
    }

    private sealed class CountingFileStream(string path) : FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
    {
        public long BytesRead { get; private set; }
        public void ResetReads() => BytesRead = 0;
        public override int Read(Span<byte> buffer)
        {
            int read = base.Read(buffer);
            BytesRead += read;
            return read;
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            BytesRead += read;
            return read;
        }
    }
}
