using System.Collections;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Memory;

public readonly struct IndexArray : IReadOnlyList<uint>
{
    public IndexFormat Format { get; }

    public int Count { get; }
    public int Stride { get; }
    public ReadOnlyMemory<byte> Buffer { get; }

    public uint this[int index] =>
        this.Format switch
        {
            IndexFormat.U16 => MemoryMarshal.Read<ushort>(this.Buffer.Span[(index * this.Stride)..]),
            IndexFormat.U32 => MemoryMarshal.Read<uint>(this.Buffer.Span[(index * this.Stride)..]),
            _ => throw new InvalidOperationException()
        };

    public IndexArray(IndexFormat format, ReadOnlyMemory<byte> buffer)
    {
        IndexBuffer.ValidateBufferDimensions(format, buffer.Span);

        int stride = IndexBuffer.GetFormatSize(format);

        this.Format = format;
        this.Count = buffer.Length / stride;
        this.Stride = stride;
        this.Buffer = buffer;
    }

    public IndexArray Slice(int start, int length) =>
        new(this.Format, this.Buffer.Slice(start * this.Stride, length * this.Stride));

    public IEnumerator<uint> GetEnumerator()
    {
        for (int i = 0; i < this.Count; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
