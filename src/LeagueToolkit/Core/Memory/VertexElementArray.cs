using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Provides an read-only interface for indexing and enumerating vertex element values described by <see cref="VertexElementAccessor"/>
    /// </summary>
    /// <typeparam name="TElement">The type of the serialized element value</typeparam>
    public readonly struct VertexElementArray<TElement> : IReadOnlyList<TElement>
        where TElement : struct
    {
        public int Count => this.Accessor.VertexCount;

        /// <summary>
        /// The accessor which is used by the <see cref="VertexElementArray{TElement}"/>
        /// to fetch element values from memory
        /// </summary>
        public VertexElementAccessor Accessor { get; }

        /// <summary>
        /// Creates a new <see cref="VertexElementArray{TElement}"/>
        /// </summary>
        /// <param name="accessor">The accessor to use for fetching element values from memory</param>
        public VertexElementArray(VertexElementAccessor accessor)
        {
            this.Accessor = accessor;
        }

        private TElement ReadElement(int offset)
        {
            var elementSize = this.Accessor.Element.GetSize();
            var elementMemory = this.Accessor.BufferView.Span.Slice(offset, elementSize);

            return MemoryMarshal.Read<TElement>(elementMemory);
        }

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
