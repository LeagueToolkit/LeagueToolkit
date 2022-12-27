using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Represents a wrapped memory region which contains vertex data<br></br>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/direct3d11/overviews-direct3d-11-resources-buffers-vertex-how-to">
    /// D3D11 - How to: Create a Vertex Buffer
    /// </seealso>
    /// </summary>
    /// <remarks>If you are the owner of a <see cref="VertexBuffer"/> instance, make sure to dispose it after you're done using it</remarks>
    public sealed class VertexBuffer : IDisposable
    {
        /// <value>
        /// The description of this <see cref="VertexBuffer"/>
        /// </value>
        public VertexBufferDescription Description { get; }

        /// <summary>
        /// The elements of this <see cref="VertexBuffer"/>
        /// </summary>
        public IReadOnlyDictionary<ElementName, VertexBufferElementDescriptor> Elements => this._elements;
        private readonly Dictionary<ElementName, VertexBufferElementDescriptor> _elements = new();

        /// <summary>
        /// Provides a read-only view into the buffer
        /// </summary>
        public ReadOnlyMemory<byte> View => this._buffer.Memory;

        /// <value>
        /// The size of a vertex inside the buffer
        /// </value>
        public int VertexStride { get; }

        /// <value>
        /// The vertex count of the buffer
        /// </value>
        public int VertexCount { get; }

        private readonly MemoryOwner<byte> _buffer;

        private bool _isDisposed;

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

        /// <summary>
        /// Creates a new <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The element to access</param>
        public VertexElementAccessor GetAccessor(ElementName elementName)
        {
            ThrowIfDisposed();

            if (this._elements.TryGetValue(elementName, out VertexBufferElementDescriptor foundDescriptor))
                return new(foundDescriptor.Element, this._buffer.Memory, this.VertexStride, foundDescriptor.Offset);

            throw new KeyNotFoundException($"Vertex buffer does not contain vertex element: {elementName}");
        }

        /// <summary>
        /// Creates a <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The name of the element to create an accessor over</param>
        /// <param name="accessor">If the element is found, contains the <see cref="VertexElementAccessor"/> for it,
        /// otherwise, the accessor is set to default</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="VertexBuffer"/> contains a <see cref="VertexElement"/>
        /// with the specified name; otherwise, <see langword="false"/>
        /// </returns>
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

            int stride = SanitizeElements(elements).Sum(element => element.GetSize());

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
