using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Memory
{
    public readonly struct VertexElementArray<TElement> : IReadOnlyList<TElement> where TElement : struct
    {
        public int Count => this.Accessor.VertexCount;

        public VertexElementAccessor Accessor { get; }

        public VertexElementArray(VertexElementAccessor accessor)
        {
            this.Accessor = accessor;
        }

        private TElement ReadElement(int offset) =>
            MemoryMarshal.Read<TElement>(this.Accessor.BufferView.Span[offset..]);

        public TElement this[int index] =>
            ReadElement(this.Accessor.VertexStride * index + this.Accessor.ElementOffset);

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
