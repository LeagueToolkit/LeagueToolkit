using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Represents a wrapped memory region which contains vertex data<br></br>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/direct3d11/overviews-direct3d-11-resources-buffers-vertex-how-to">
    /// D3D11 - How to: Create a Vertex Buffer
    /// </seealso>
    /// </summary>
    /// <remarks>If you are the owner of a <see cref="VertexBuffer"/> instance, make sure to dispose it after you're done using it</remarks>
    public sealed class VertexBuffer : IVertexBufferView, IDisposable
    {
        public VertexBufferDescription Description { get; }

        public IReadOnlyDictionary<ElementName, VertexBufferElementDescriptor> Elements => this._elements;
        private readonly Dictionary<ElementName, VertexBufferElementDescriptor> _elements = [];

        public ReadOnlyMemory<byte> View => this._buffer.Memory;

        public int VertexStride { get; }
        public int VertexCount { get; }

        public bool IsDisposed { get; private set; }

        private readonly MemoryOwner<byte> _buffer;

        private VertexBuffer(VertexBufferUsage usage, IEnumerable<VertexElement> elements, MemoryOwner<byte> buffer)
        {
            Guard.IsNotNull(elements);
            Guard.IsNotNull(buffer, nameof(buffer));

            this.Description = new(usage, elements);

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

        /// <summary>
        /// Creates a new <see cref="VertexBuffer"/> instance
        /// </summary>
        /// <param name="usage">The usage of the buffer</param>
        /// <param name="elements">The elements of a vertex in the buffer</param>
        /// <param name="buffer">The owned memory instance of the raw buffer</param>
        /// <remarks>This function takes ownership of <paramref name="buffer"/>, you must not store a reference of it</remarks>
        public static VertexBuffer Create(
            VertexBufferUsage usage,
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
                accessor = default;
                return false;
            }
        }

        /// <summary>
        /// Allocates an owned buffer for the specified vertex elements and count
        /// </summary>
        /// <param name="elements">The elements of the buffer's vertex</param>
        /// <param name="vertexCount">The vertex count of the buffer</param>
        public static MemoryOwner<byte> AllocateForElements(IEnumerable<VertexElement> elements, int vertexCount)
        {
            Guard.IsNotNull(elements, nameof(elements));

            // We don't need to order the elements here because vertex size will be the same regardless of order
            int stride = SanitizeElements(elements).Sum(element => element.GetSize());

            return MemoryOwner<byte>.Allocate(vertexCount * stride, AllocationMode.Clear);
        }

        internal static IEnumerable<VertexElement> SanitizeElements(IEnumerable<VertexElement> elements)
        {
            Guard.IsNotNull(elements, nameof(elements));

            return SanitizeElementsForDuplication(elements);
        }

        // TODO: Expose element ordering API
        internal static IEnumerable<VertexElement> SanitizeElements<TElementKey>(
            IEnumerable<VertexElement> elements,
            Func<VertexElement, TElementKey> keySelector
        )
        {
            Guard.IsNotNull(elements, nameof(elements));
            Guard.IsNotNull(keySelector, nameof(keySelector));

            return SanitizeElementsForDuplication(elements).OrderBy(keySelector);
        }

        internal static IEnumerable<VertexElement> SanitizeElementsForDuplication(IEnumerable<VertexElement> elements)
        {
            // Check that all elements are unique
            IEnumerable<VertexElement> distinctElements = elements.Distinct();
            if (distinctElements.Count() != elements.Count())
                ThrowHelper.ThrowArgumentException(nameof(distinctElements), "Contains duplicate vertex elements.");

            return distinctElements;
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
            if (this.IsDisposed)
                ThrowHelper.ThrowObjectDisposedException(nameof(VertexBuffer), "Cannot use a disposed vertex buffer");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this._buffer?.Dispose();
                }

                this.IsDisposed = true;
            }
        }
    }

    public readonly struct VertexBufferElementDescriptor(VertexElement element, int offset)
    {
        public VertexElement Element { get; } = element;
        public int Offset { get; } = offset;
    }
}
