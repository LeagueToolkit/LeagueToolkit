using CommunityToolkit.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueToolkit.Core.Memory
{
    // https://www.braynzarsoft.net/viewtutorial/q16390-33-instancing-with-indexed-primitives
    public sealed class InstancedVertexBuffer : IDisposable
    {
        public VertexBufferDescription Description { get; }

        public int VertexCount { get; }

        public IReadOnlyList<VertexBuffer> Buffers => this._buffers;
        private readonly List<VertexBuffer> _buffers;

        private bool _isDisposed;

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

        public VertexElementAccessor GetAccessor(ElementName element)
        {
            ThrowIfDisposed();

            foreach (VertexBuffer vertexBuffer in this.Buffers)
            {
                if (vertexBuffer.TryGetAccessor(element, out VertexElementAccessor accessor))
                    return accessor;
            }

            throw new KeyNotFoundException(
                $"Instanced vertex buffer does not contain a vertex buffer with element: {element}"
            );
        }

        public bool TryGetAccessor(ElementName element, out VertexElementAccessor accessor)
        {
            ThrowIfDisposed();

            foreach (VertexBuffer vertexBuffer in this.Buffers)
            {
                if (vertexBuffer.TryGetAccessor(element, out accessor))
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
