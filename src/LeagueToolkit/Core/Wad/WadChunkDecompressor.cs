using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Wad;

/// <summary>
/// Decompresses WAD chunk payloads independently from a <see cref="WadFile"/>
/// </summary>
/// <remarks>
/// Instances are reusable but are not thread-safe.
/// </remarks>
public sealed class WadChunkDecompressor : IDisposable
{
    private readonly ZstdSharp.Decompressor _zstdDecompressor = new();

    /// <summary>
    /// Gets a value indicating whether the decompressor has been disposed of
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Decompresses an externally stored WAD chunk payload
    /// </summary>
    /// <param name="chunkData">The stored chunk payload</param>
    /// <param name="compression">The compression used by the chunk</param>
    /// <param name="uncompressedSize">The expected decompressed size</param>
    /// <param name="subchunks">The sub-chunk table required by <see cref="WadChunkCompression.ZstdChunked"/></param>
    /// <returns>An owner containing the decompressed chunk data</returns>
    /// <exception cref="InvalidDataException">The payload or its metadata is inconsistent</exception>
    /// <exception cref="NotSupportedException">The compression type cannot be decompressed</exception>
    /// <exception cref="ObjectDisposedException">The decompressor has been disposed of</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="uncompressedSize"/> is negative</exception>
    /// <exception cref="ZstdSharp.ZstdException">A Zstandard payload cannot be decompressed</exception>
    public MemoryOwner<byte> Decompress(
        ReadOnlyMemory<byte> chunkData,
        WadChunkCompression compression,
        int uncompressedSize,
        ReadOnlyMemory<WadSubchunk> subchunks = default
    )
    {
        ThrowIfDisposed();
        ArgumentOutOfRangeException.ThrowIfNegative(uncompressedSize);

        ValidateMetadata(chunkData.Length, compression, uncompressedSize, subchunks.Span);

        MemoryOwner<byte> decompressedData = MemoryOwner<byte>.Allocate(uncompressedSize);
        try
        {
            switch (compression)
            {
                case WadChunkCompression.None:
                    chunkData.Span.CopyTo(decompressedData.Span);
                    break;
                case WadChunkCompression.GZip:
                    DecompressGZip(chunkData, decompressedData.Span);
                    break;
                case WadChunkCompression.Zstd:
                    DecompressZstd(chunkData.Span, decompressedData.Span);
                    break;
                case WadChunkCompression.ZstdChunked:
                    DecompressZstdChunked(chunkData.Span, decompressedData.Span, subchunks.Span);
                    break;
                case WadChunkCompression.Satellite:
                    throw new NotSupportedException("Satellite chunks cannot be decompressed");
                default:
                    throw new InvalidDataException($"Invalid WAD chunk compression type: {compression}");
            }

            return decompressedData;
        }
        catch
        {
            decompressedData.Dispose();
            throw;
        }
    }

    internal void ReadRange(Stream source, long storedOffset, int storedLength, WadChunkCompression compression,
        Span<byte> destination, int offset)
    {
        ThrowIfDisposed();
        if (compression == WadChunkCompression.None)
        {
            source.Seek(storedOffset + offset, SeekOrigin.Begin);
            source.ReadExactly(destination);
            return;
        }
        source.Seek(storedOffset, SeekOrigin.Begin);
        using var input = new ChunkDataStream(source, storedLength);
        if (compression == WadChunkCompression.GZip)
        {
            using var gzip = new GZipStream(input, CompressionMode.Decompress, leaveOpen: true);
            ReadDecodedRange(gzip, destination, offset);
        }
        else
        {
            // A range read leaves the frame incomplete; reset before reusing the decoder.
            this._zstdDecompressor.ResetStream();
            using var zstd = new ZstdSharp.DecompressionStream(input, this._zstdDecompressor,
                bufferSize: 4096, checkEndOfStream: false, preserveDecompressor: true, leaveOpen: true);
            ReadDecodedRange(zstd, destination, offset);
        }
    }

    private static void ReadDecodedRange(Stream stream, Span<byte> destination, int offset)
    {
        if (offset > 0)
        {
            Span<byte> skipped = stackalloc byte[4096];
            while (offset > 0)
            {
                int length = Math.Min(offset, skipped.Length);
                stream.ReadExactly(skipped[..length]);
                offset -= length;
            }
        }
        stream.ReadExactly(destination);
    }

    private sealed class ChunkDataStream(Stream source, int remaining) : Stream
    {
        public override int Read(Span<byte> buffer)
        {
            int read = source.Read(buffer[..Math.Min(buffer.Length, remaining)]);
            remaining -= read;
            return read;
        }

        public override int Read(byte[] buffer, int offset, int count) => Read(buffer.AsSpan(offset, count));
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    internal static void ValidateMetadata(
        int compressedSize,
        WadChunkCompression compression,
        int uncompressedSize,
        ReadOnlySpan<WadSubchunk> subchunks
    )
    {
        if (!Enum.IsDefined(compression))
            ThrowHelper.ThrowInvalidDataException($"Invalid WAD chunk compression type: {compression}");

        if (compression is WadChunkCompression.Satellite)
            throw new NotSupportedException("Satellite chunks cannot be decompressed");

        if (compression is WadChunkCompression.None && compressedSize != uncompressedSize)
            ThrowHelper.ThrowInvalidDataException(
                $"Uncompressed chunk size mismatch. Expected {uncompressedSize}, got {compressedSize}"
            );

        if (compression is not WadChunkCompression.ZstdChunked)
            return;

        if (subchunks.IsEmpty)
            ThrowHelper.ThrowInvalidDataException("Zstd chunked data requires a sub-chunk table");

        int totalCompressedSize = 0;
        int totalUncompressedSize = 0;
        try
        {
            foreach (WadSubchunk subchunk in subchunks)
            {
                if (subchunk.CompressedSize <= 0 || subchunk.UncompressedSize <= 0)
                    ThrowHelper.ThrowInvalidDataException("The sub-chunk table contains an invalid size");

                totalCompressedSize = checked(totalCompressedSize + subchunk.CompressedSize);
                totalUncompressedSize = checked(totalUncompressedSize + subchunk.UncompressedSize);
            }
        }
        catch (OverflowException exception)
        {
            throw new InvalidDataException("The sub-chunk table exceeds the supported size", exception);
        }

        if (totalCompressedSize != compressedSize || totalUncompressedSize != uncompressedSize)
            ThrowHelper.ThrowInvalidDataException(
                $"Sub-chunk table size mismatch. Compressed {totalCompressedSize}/{compressedSize}, uncompressed {totalUncompressedSize}/{uncompressedSize}"
            );
    }

    private static void DecompressGZip(ReadOnlyMemory<byte> source, Span<byte> destination)
    {
        Stream sourceStream = MemoryMarshal.TryGetArray(source, out ArraySegment<byte> segment)
            ? new MemoryStream(segment.Array!, segment.Offset, segment.Count, false)
            : new MemoryStream(source.ToArray(), false);

        using (sourceStream)
        using (var decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
        {
            int totalRead = 0;
            while (totalRead < destination.Length)
            {
                int read = decompressionStream.Read(destination[totalRead..]);
                if (read == 0)
                    ThrowHelper.ThrowInvalidDataException("GZip chunk output is smaller than its declared size");

                totalRead += read;
            }

            if (decompressionStream.ReadByte() != -1)
                ThrowHelper.ThrowInvalidDataException("GZip chunk output exceeds its declared size");
        }
    }

    private void DecompressZstd(ReadOnlySpan<byte> source, Span<byte> destination)
    {
        int written = this._zstdDecompressor.Unwrap(source, destination);
        if (written != destination.Length)
            ThrowHelper.ThrowInvalidDataException(
                $"Zstd chunk size mismatch. Expected {destination.Length}, got {written}"
            );
    }

    private void DecompressZstdChunked(
        ReadOnlySpan<byte> source,
        Span<byte> destination,
        ReadOnlySpan<WadSubchunk> subchunks
    )
    {
        int compressedOffset = 0;
        int decompressedOffset = 0;

        foreach (WadSubchunk subchunk in subchunks)
        {
            ReadOnlySpan<byte> subchunkSource = source.Slice(compressedOffset, subchunk.CompressedSize);
            Span<byte> subchunkDestination = destination.Slice(decompressedOffset, subchunk.UncompressedSize);

            try
            {
                DecompressZstd(subchunkSource, subchunkDestination);
            }
            catch (ZstdSharp.ZstdException) when (subchunk.CompressedSize == subchunk.UncompressedSize)
            {
                subchunkSource.CopyTo(subchunkDestination);
            }

            compressedOffset += subchunk.CompressedSize;
            decompressedOffset += subchunk.UncompressedSize;
        }
    }

    /// <summary>
    /// Disposes the decompressor
    /// </summary>
    public void Dispose()
    {
        if (this.IsDisposed)
            return;

        this._zstdDecompressor.Dispose();
        this.IsDisposed = true;
        GC.SuppressFinalize(this);
    }

    private void ThrowIfDisposed()
    {
        if (this.IsDisposed)
            ThrowHelper.ThrowObjectDisposedException(
                nameof(WadChunkDecompressor),
                "Cannot use a disposed WAD chunk decompressor"
            );
    }
}
