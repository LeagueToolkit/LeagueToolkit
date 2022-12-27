using CommunityToolkit.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Represents an instanced/sharded vertex buffer<br></br>
    /// Refer to this <seealso href="https://www.braynzarsoft.net/viewtutorial/q16390-33-instancing-with-indexed-primitives">
    /// blogpost on how instancing works in D3D11</seealso> for a deeper explanation
    /// </summary>
    /// <remarks>
    /// If you are the owner of an <see cref="InstancedVertexBuffer"/> instance,
    /// make sure to dispose it after you're done using it;
    /// doing so will also dispose all contained instances of <see cref="VertexBuffer"/>
    /// </remarks>
    public sealed class InstancedVertexBuffer : IDisposable
    {
        /// <summary>
        /// The merged/full description of the <see cref="InstancedVertexBuffer"/>
        /// </summary>
        public VertexBufferDescription Description { get; }

        /// <value>
        /// The vertex count of all buffers
        /// </value>
        public int VertexCount { get; }

        /// <summary>
        /// The sharded vertex buffers which form the <see cref="InstancedVertexBuffer"/>
        /// </summary>
        /// <remarks>
        /// Each buffer is guaranteed to contain unique/non-overlapping elements relative to the other buffers.<br></br>
        /// The first buffer is assumed to be the "instanced" buffer.
        /// </remarks>
        public IReadOnlyList<VertexBuffer> Buffers => this._buffers;
        private readonly List<VertexBuffer> _buffers;

        private bool _isDisposed;

        /// <summary>
        /// Creates a new <see cref="InstancedVertexBuffer"/> from the specified buffers
        /// </summary>
        /// <param name="buffers">The sharded vertex buffers</param>
        /// <remarks>
        /// Each buffer should contain unique/non-overlapping elements relative to the other buffers.<br></br>
        /// The first buffer is assumed to be the "instanced" buffer.
        /// </remarks>
        public InstancedVertexBuffer(IEnumerable<VertexBuffer> buffers)
        {
            ValidateVertexBuffers(buffers);

            this.VertexCount = buffers.First().VertexCount;
            this._buffers = new(buffers);

            // Create a merged description from the sharded buffers
            // assuming that all elements are unique
            this.Description = new(
                VertexBufferUsage.Static,
                buffers
                    .SelectMany(
                        vertexBuffer =>
                            vertexBuffer.Elements.Values.Select(elementDescriptor => elementDescriptor.Element)
                    )
                    .OrderBy(element => element.Name)
            );
        }

        /// <summary>
        /// Creates a new <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The element to access</param>
        public VertexElementAccessor GetAccessor(ElementName elementName)
        {
            ThrowIfDisposed();

            foreach (VertexBuffer vertexBuffer in this.Buffers)
            {
                if (vertexBuffer.TryGetAccessor(elementName, out VertexElementAccessor accessor))
                    return accessor;
            }

            throw new KeyNotFoundException(
                $"Instanced vertex buffer does not contain a vertex buffer with element: {elementName}"
            );
        }

        /// <summary>
        /// Creates a <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The name of the element to create an accessor for</param>
        /// <param name="accessor">If the element is found, contains the <see cref="VertexElementAccessor"/> for it,
        /// otherwise, the accessor is set to <see langword="default"/></param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="InstancedVertexBuffer"/> has a <see cref="VertexBuffer"/>
        /// which contains a <see cref="VertexElement"/> with the specified name; otherwise, <see langword="false"/>
        /// </returns>
        public bool TryGetAccessor(ElementName elementName, out VertexElementAccessor accessor)
        {
            ThrowIfDisposed();

            foreach (VertexBuffer vertexBuffer in this.Buffers)
            {
                if (vertexBuffer.TryGetAccessor(elementName, out accessor))
                    return true;
            }

            accessor = default;
            return false;
        }

        private static void ValidateVertexBuffers(IEnumerable<VertexBuffer> vertexBuffers)
        {
            Guard.IsNotNull(vertexBuffers);
            Guard.IsGreaterThan(vertexBuffers.Count(), 0, nameof(vertexBuffers));

            IEnumerable<ElementName> elements = vertexBuffers.SelectMany(x => x.Elements.Keys);
            IEnumerable<ElementName> distinctElements = elements.Distinct();

            // Check that vertex buffers do not have overlapping elements
            if (elements.Count() != distinctElements.Count())
                ThrowHelper.ThrowArgumentException(
                    nameof(vertexBuffers),
                    $"Vertex buffers must not have overlapping elements"
                );

            // Check that all vertex buffers have the same vertex count
            int vertexCount = vertexBuffers.First().VertexCount;
            if (!vertexBuffers.All(vertexBuffer => vertexBuffer.VertexCount == vertexCount))
                ThrowHelper.ThrowArgumentException(
                    nameof(vertexBuffers),
                    $"Vertex buffers must have equal vertex counts"
                );
        }

        private void ThrowIfDisposed()
        {
            if (this._isDisposed)
                ThrowHelper.ThrowObjectDisposedException(
                    nameof(VertexBuffer),
                    $"Cannot use a disposed {nameof(InstancedVertexBuffer)}"
                );
        }

        private void Dispose(bool disposing)
        {
            if (this._isDisposed is false)
            {
                if (disposing)
                {
                    this._buffers?.ForEach(vertexBuffer => vertexBuffer?.Dispose());
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
