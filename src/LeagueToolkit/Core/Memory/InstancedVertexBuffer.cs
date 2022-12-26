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
        public int VertexCount { get; }

        public IReadOnlyList<VertexBuffer> VertexBuffers => this._vertexBuffers;
        private readonly List<VertexBuffer> _vertexBuffers;

        private bool _isDisposed;

        public InstancedVertexBuffer(IEnumerable<VertexBuffer> vertexBuffers)
        {
            ValidateVertexBuffers(vertexBuffers);

            this.VertexCount = vertexBuffers.First().VertexCount;
            this._vertexBuffers = new(vertexBuffers);
        }

        public VertexElementAccessor GetAccessor(ElementName element)
        {
            ThrowIfDisposed();

            foreach (VertexBuffer vertexBuffer in this.VertexBuffers)
            {
                if (vertexBuffer.TryGetAccessor(element, out VertexElementAccessor accessor))
                    return accessor;
            }

            throw new KeyNotFoundException(
                $"Instanced vertex buffer does not contain a vertex buffer with element: {element}"
            );
        }

        private void ValidateVertexBuffers(IEnumerable<VertexBuffer> vertexBuffers)
        {
            Guard.IsNotNull(vertexBuffers);
            Guard.IsGreaterThan(vertexBuffers.Count(), 0, nameof(vertexBuffers));

            IEnumerable<ElementName> elements = vertexBuffers.SelectMany(x => x.Elements.Keys);
            IEnumerable<ElementName> distinctElements = elements.Distinct();

            // Check that vertex buffers do not have overlapping elements
            if (elements.Count() != distinctElements.Count())
                ThrowHelper.ThrowArgumentException(
                    nameof(vertexBuffers),
                    $"Vertex buffers contain overlapping elements"
                );

            // Check that all vertex buffers have the same vertex count
            int vertexCount = vertexBuffers.First().VertexCount;
            if (!vertexBuffers.All(vertexBuffer => vertexBuffer.VertexCount == vertexCount))
                ThrowHelper.ThrowArgumentException(
                    nameof(vertexBuffers),
                    $"Vertex buffers do not have equal vertex counts"
                );
        }

        private void ThrowIfDisposed()
        {
            if (this._isDisposed)
                ThrowHelper.ThrowObjectDisposedException(
                    nameof(VertexBuffer),
                    "Cannot use a disposed Instanced Vertex Buffer"
                );
        }

        private void Dispose(bool disposing)
        {
            if (this._isDisposed is false)
            {
                if (disposing)
                {
                    this._vertexBuffers?.ForEach(vertexBuffer => vertexBuffer?.Dispose());
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
