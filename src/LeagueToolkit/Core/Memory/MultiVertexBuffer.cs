using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueToolkit.Core.Memory
{
    public sealed class MultiVertexBuffer : IDisposable
    {
        public IReadOnlyList<VertexBuffer> VertexBuffers => this._vertexBuffers;
        private readonly List<VertexBuffer> _vertexBuffers;

        private bool _isDisposed;

        public MultiVertexBuffer(IEnumerable<VertexBuffer> vertexBuffers)
        {
            CheckVertexBuffersForOverlappingElements(vertexBuffers);

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
                $"Multi-vertex buffer does not contain a vertex buffer with element: {element}"
            );
        }

        private void CheckVertexBuffersForOverlappingElements(IEnumerable<VertexBuffer> vertexBuffers)
        {
            IEnumerable<ElementName> elements = vertexBuffers.SelectMany(x => x.Elements.Keys);
            IEnumerable<ElementName> distinctElements = elements.Distinct();

            if (elements.Count() != distinctElements.Count())
                ThrowHelper.ThrowArgumentException(
                    nameof(vertexBuffers),
                    $"The provided vertex buffers contain overlapping elements"
                );
        }

        private void ThrowIfDisposed()
        {
            if (this._isDisposed)
                ThrowHelper.ThrowObjectDisposedException(
                    nameof(VertexBuffer),
                    "Cannot use a disposed multi-vertex buffer"
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
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
