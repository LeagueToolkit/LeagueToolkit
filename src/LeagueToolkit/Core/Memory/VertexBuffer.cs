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

        public IReadOnlyDictionary<ElementName, VertexBufferElementDescriptor> Elements => this._elements;
        private readonly Dictionary<ElementName, VertexBufferElementDescriptor> _elements = new();

        public ReadOnlyMemory<byte> View => this._buffer.Memory;
        public int VertexStride { get; }
        public int VertexCount { get; }

        private readonly MemoryOwner<byte> _buffer;

        private bool _isDisposed;

        private VertexBuffer(
            VertexElementGroupUsage usage,
            IEnumerable<VertexElement> elements,
            MemoryOwner<byte> buffer
        )
        {
            Guard.IsNotNull(elements);
            Guard.IsNotNull(buffer, nameof(buffer));

            this.Usage = usage;

            // Store offset of each element for reading
            int currentElementOffset = 0;
            foreach (VertexElement element in SanitizeElements(elements))
            {
                this._elements.Add(element.Name, new(element, currentElementOffset));

                currentElementOffset += element.GetSize();
            }

            this._buffer = buffer;
            this.VertexStride = this._elements.Values.Sum(descriptor => descriptor.Element.GetSize());
            this.VertexCount = this._buffer.Length / this.VertexStride;

            ValidateBufferDimensions(buffer.Span, this.VertexStride);
        }

        public static VertexBuffer Create(
            VertexElementGroupUsage usage,
            IEnumerable<VertexElement> elements,
            MemoryOwner<byte> buffer
        ) => new(usage, elements, buffer);

        public VertexElementAccessor GetAccessor(ElementName elementName)
        {
            ThrowIfDisposed();

            if (this._elements.TryGetValue(elementName, out VertexBufferElementDescriptor foundDescriptor))
                return new(foundDescriptor.Element, this._buffer.Memory, this.VertexStride, foundDescriptor.Offset);

            throw new KeyNotFoundException($"Vertex buffer does not contain vertex element: {elementName}");
        }

        public bool TryGetAccessor(ElementName elementName, out VertexElementAccessor accessor)
        {
            ThrowIfDisposed();

            if (this.Elements.TryGetValue(elementName, out VertexBufferElementDescriptor foundDescriptor))
            {
                accessor = new(foundDescriptor.Element, this._buffer.Memory, this.VertexStride, foundDescriptor.Offset);
                return true;
            }
            else
            {
                accessor = new();
                return false;
            }
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

        internal static void ValidateBufferDimensions(ReadOnlySpan<byte> buffer, int vertexStride)
        {
            if (buffer.IsEmpty)
                ThrowHelper.ThrowArgumentException(nameof(buffer), "Buffer cannot be empty.");

            if (buffer.Length % vertexStride != 0)
                ThrowHelper.ThrowArgumentException(
                    nameof(buffer),
                    "Buffer size must be a multiple of its stride size."
                );
        }

        private void ThrowIfDisposed()
        {
            if (this._isDisposed)
                ThrowHelper.ThrowObjectDisposedException(nameof(VertexBuffer), "Cannot use a disposed vertex buffer");
        }

        private void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._buffer?.Dispose();
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

    public readonly struct VertexBufferElementDescriptor
    {
        public VertexElement Element { get; }
        public int Offset { get; }

        public VertexBufferElementDescriptor(VertexElement element, int offset)
        {
            this.Element = element;
            this.Offset = offset;
        }
    }
}
