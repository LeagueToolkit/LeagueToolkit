using LeagueToolkit.Core.Wad;
using System.IO.Compression;
using System.Text;
using Xunit;

namespace LeagueToolkit.Tests.Core.Wad;

public class WadChunkDecompressorTests
{
    [Fact]
    public void DecompressCopiesUncompressedPayload()
    {
        byte[] expected = "uncompressed payload"u8.ToArray();
        using var decompressor = new WadChunkDecompressor();

        using var actual = decompressor.Decompress(expected, WadChunkCompression.None, expected.Length);

        Assert.Equal(expected, actual.Span.ToArray());
    }

    [Fact]
    public void DecompressRejectsInvalidUncompressedSize()
    {
        byte[] payload = "payload"u8.ToArray();
        using var decompressor = new WadChunkDecompressor();

        Assert.Throws<InvalidDataException>(
            () => decompressor.Decompress(payload, WadChunkCompression.None, payload.Length + 1)
        );
    }

    [Fact]
    public void DecompressSupportsGZipPayload()
    {
        byte[] expected = Encoding.UTF8.GetBytes(string.Concat(Enumerable.Repeat("gzip payload ", 32)));
        byte[] compressed = CompressGZip(expected);
        using var decompressor = new WadChunkDecompressor();

        using var actual = decompressor.Decompress(compressed, WadChunkCompression.GZip, expected.Length);

        Assert.Equal(expected, actual.Span.ToArray());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void DecompressRejectsIncorrectGZipOutputSize(int sizeDelta)
    {
        byte[] expected = "gzip payload"u8.ToArray();
        byte[] compressed = CompressGZip(expected);
        using var decompressor = new WadChunkDecompressor();

        Assert.Throws<InvalidDataException>(
            () => decompressor.Decompress(compressed, WadChunkCompression.GZip, expected.Length + sizeDelta)
        );
    }

    [Fact]
    public void DecompressSupportsZstdPayload()
    {
        byte[] expected = Encoding.UTF8.GetBytes(string.Concat(Enumerable.Repeat("zstd payload ", 32)));
        using var compressor = new ZstdSharp.Compressor();
        byte[] compressed = compressor.Wrap(expected).ToArray();
        using var decompressor = new WadChunkDecompressor();

        using var actual = decompressor.Decompress(compressed, WadChunkCompression.Zstd, expected.Length);

        Assert.Equal(expected, actual.Span.ToArray());
    }

    [Fact]
    public void DecompressSupportsMixedZstdChunkedPayload()
    {
        byte[] compressedOutput = Encoding.UTF8.GetBytes(string.Concat(Enumerable.Repeat("compressed subchunk ", 32)));
        byte[] rawOutput = "raw-subchunk-data"u8.ToArray();
        using var compressor = new ZstdSharp.Compressor();
        byte[] compressedInput = compressor.Wrap(compressedOutput).ToArray();
        byte[] payload = [.. compressedInput, .. rawOutput];
        WadSubchunk[] subchunks =
        [
            new(compressedInput.Length, compressedOutput.Length),
            new(rawOutput.Length, rawOutput.Length)
        ];
        byte[] expected = [.. compressedOutput, .. rawOutput];
        using var decompressor = new WadChunkDecompressor();

        using var actual = decompressor.Decompress(
            payload,
            WadChunkCompression.ZstdChunked,
            expected.Length,
            subchunks
        );

        Assert.Equal(expected, actual.Span.ToArray());
    }

    [Fact]
    public void DecompressRejectsMissingZstdSubchunkTable()
    {
        using var decompressor = new WadChunkDecompressor();

        Assert.Throws<InvalidDataException>(
            () => decompressor.Decompress(new byte[] { 1, 2, 3 }, WadChunkCompression.ZstdChunked, 3)
        );
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    public void DecompressRejectsInvalidZstdSubchunkSizes(int compressedSize, int uncompressedSize)
    {
        WadSubchunk[] subchunks = [new(compressedSize, uncompressedSize)];
        using var decompressor = new WadChunkDecompressor();

        Assert.Throws<InvalidDataException>(
            () => decompressor.Decompress(
                new byte[] { 1 },
                WadChunkCompression.ZstdChunked,
                1,
                subchunks
            )
        );
    }

    [Theory]
    [InlineData(2, 4)]
    [InlineData(4, 2)]
    public void DecompressRejectsMismatchedZstdSubchunkTable(int compressedSize, int uncompressedSize)
    {
        byte[] payload = [1, 2, 3];
        WadSubchunk[] subchunks = [new(compressedSize, uncompressedSize)];
        using var decompressor = new WadChunkDecompressor();

        Assert.Throws<InvalidDataException>(
            () => decompressor.Decompress(
                payload,
                WadChunkCompression.ZstdChunked,
                payload.Length,
                subchunks
            )
        );
    }

    [Fact]
    public void DecompressDoesNotTreatCorruptCompressedSubchunkAsRaw()
    {
        byte[] payload = [1, 2, 3, 4];
        WadSubchunk[] subchunks = [new(payload.Length, payload.Length + 1)];
        using var decompressor = new WadChunkDecompressor();

        Assert.Throws<ZstdSharp.ZstdException>(
            () => decompressor.Decompress(
                payload,
                WadChunkCompression.ZstdChunked,
                payload.Length + 1,
                subchunks
            )
        );
    }

    [Fact]
    public void DecompressRejectsUseAfterDispose()
    {
        var decompressor = new WadChunkDecompressor();
        decompressor.Dispose();

        Assert.Throws<ObjectDisposedException>(
            () => decompressor.Decompress(ReadOnlyMemory<byte>.Empty, WadChunkCompression.None, 0)
        );
    }

    [Fact]
    public void DecompressRejectsSatellitePayload()
    {
        using var decompressor = new WadChunkDecompressor();

        Assert.Throws<NotSupportedException>(
            () => decompressor.Decompress(ReadOnlyMemory<byte>.Empty, WadChunkCompression.Satellite, 0)
        );
    }

    private static byte[] CompressGZip(byte[] data)
    {
        using var output = new MemoryStream();
        using (var compressionStream = new GZipStream(output, CompressionMode.Compress, true))
        {
            compressionStream.Write(data);
        }

        return output.ToArray();
    }
}
