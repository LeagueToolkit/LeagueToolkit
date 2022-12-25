using LeagueToolkit.Core.Renderer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace LeagueToolkit.Core.Memory
{
    public readonly struct VertexElementArray<TElement> : IReadOnlyList<TElement> where TElement : unmanaged
    {
        public ElementName Name { get; }
        public ElementFormat Format { get; }

        public int Count { get; }

        private readonly Memory<byte> _buffer;
        private readonly int _stride;
        private readonly int _elementOffset;

        public VertexElementArray(
            ElementName name,
            ElementFormat format,
            Memory<byte> buffer,
            int stride,
            int elementOffset
        )
        {
            this.Name = name;
            this.Format = format;

            this.Count = buffer.Length / stride;
            this._buffer = buffer;
            this._stride = stride;
            this._elementOffset = elementOffset;
        }

        private TElement ReadElement(int offset) => MemoryMarshal.Read<TElement>(this._buffer.Span[offset..]);

        public TElement this[int index] => ReadElement(this._stride * index + this._elementOffset);

        public IEnumerator<TElement> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
