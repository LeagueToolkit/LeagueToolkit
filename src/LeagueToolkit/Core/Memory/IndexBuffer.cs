using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;

namespace LeagueToolkit.Core.Memory;

public sealed class IndexBuffer : IDisposable
{
    public IndexFormat Format { get; }

    public int Count { get; }
    public int Stride { get; }

    public ReadOnlyMemory<byte> Buffer => this._buffer.Memory;

    private readonly MemoryOwner<byte> _buffer;

    public bool IsDisposed { get; private set; }

    private IndexBuffer(IndexFormat format, MemoryOwner<byte> buffer)
    {
        ValidateBufferDimensions(format, buffer.Span);

        int stride = GetFormatSize(format);

        this.Format = format;
        this.Count = buffer.Length / stride;
        this.Stride = stride;
        this._buffer = buffer;
    }

    public IndexArray AsArray() => new(this.Format, this.Buffer);

    public static IndexBuffer Create(IndexFormat format, MemoryOwner<byte> buffer) => new(format, buffer);

    public static int GetFormatSize(IndexFormat format) =>
        format switch
        {
            IndexFormat.U16 => sizeof(ushort),
            IndexFormat.U32 => sizeof(uint),
            _ => throw new ArgumentOutOfRangeException(nameof(format), $"Invalid index format: {format}")
        };

    internal static void ValidateBufferDimensions(IndexFormat format, ReadOnlySpan<byte> buffer)
    {
        int indexSize = GetFormatSize(format);
        if (buffer.Length % indexSize != 0)
            ThrowHelper.ThrowArgumentException(nameof(buffer), "Buffer size must be a multiple of index size");
    }

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
            this._buffer?.Dispose();

        this.IsDisposed = true;
    }
}

public enum IndexFormat
{
    U16,
    U32
}
