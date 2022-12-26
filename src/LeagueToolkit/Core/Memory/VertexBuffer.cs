using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueToolkit.Core.Memory
{
    public sealed class VertexBuffer : IDisposable
    {
        public VertexElementGroupUsage Usage { get; }

        public IReadOnlyDictionary<ElementName, (VertexElement element, int offset)> Elements => this._elements;
        private readonly Dictionary<ElementName, (VertexElement element, int offset)> _elements = new();

        public ReadOnlyMemory<byte> View => this._buffer.Memory;
        public int Stride { get; }

        private readonly MemoryOwner<byte> _buffer;

        private bool _isDisposed;

        private VertexBuffer(
            VertexElementGroupUsage usage,
            IEnumerable<VertexElement> elements,
            MemoryOwner<byte> buffer
        )
        {
            Guard.IsNotNull(buffer, nameof(buffer));
            if (buffer.Span.IsEmpty)
                ThrowHelper.ThrowArgumentException(nameof(buffer.Span), "Buffer cannot be empty.");

            this.Usage = usage;

            // Store offset of each element for reading
            int currentElementOffset = 0;
            foreach (VertexElement element in SanitizeElements(elements))
            {
                this._elements.Add(element.Name, (element, currentElementOffset));

                currentElementOffset += element.GetSize();
            }

            this._buffer = buffer;
            this.Stride = this._elements.Values.Sum(entry => entry.element.GetSize());

            if (buffer.Length % this.Stride != 0)
                ThrowHelper.ThrowArgumentException(
                    nameof(buffer),
                    "Buffer size must be a multiple of its stride size."
                );
        }

        public static VertexBuffer Create(
            VertexElementGroupUsage usage,
            IEnumerable<VertexElement> elements,
            MemoryOwner<byte> buffer
        ) => new(usage, elements, buffer);

        public VertexElementAccessor GetAccessor(ElementName elementName)
        {
            if (this._isDisposed)
                ThrowHelper.ThrowObjectDisposedException(nameof(VertexBuffer), "Cannot use a disposed vertex buffer");

            if (this._elements.TryGetValue(elementName, out var foundElement) is false)
                ThrowHelper.ThrowArgumentException(
                    nameof(elementName),
                    $"Vertex buffer does not contain vertex element: {elementName}"
                );

            return new(foundElement.element, this._buffer.Memory, this.Stride, foundElement.offset);
        }

        public static MemoryOwner<byte> AllocateForElements(IEnumerable<VertexElement> elements, int vertexCount)
        {
            Guard.IsNotNull(elements, nameof(elements));

            IEnumerable<VertexElement> sanitizedElements = SanitizeElements(elements);
            int stride = elements.Sum(element => element.GetSize());

            return MemoryOwner<byte>.Allocate(vertexCount * stride, AllocationMode.Clear);
        }

        internal static IEnumerable<VertexElement> SanitizeElements(IEnumerable<VertexElement> elements)
        {
            Guard.IsNotNull(elements, nameof(elements));

            // Check that all elements are unique
            IEnumerable<VertexElement> distinctElements = elements.Distinct();
            if (distinctElements.Count() != elements.Count())
                ThrowHelper.ThrowArgumentException(nameof(distinctElements), "Contains duplicate vertex elements.");

            // Sort elements from lowest to highest Name (stream index)
            return distinctElements.OrderBy(element => element.Name);
        }

        private void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._buffer.Dispose();
                }

                this._isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
