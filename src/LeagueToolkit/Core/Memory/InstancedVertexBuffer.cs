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
    public sealed class InstancedVertexBuffer : IInstancedVertexBufferView, IDisposable
    {
        public IInstancedVertexBufferView View => this;

        public int VertexCount { get; }

        public IReadOnlyList<IVertexBufferView> Buffers => this._buffers;
        private readonly List<VertexBuffer> _buffers;

        /// <summary>Gets a value indicating whether the instanced vertex buffer has been disposed of</summary>
        public bool IsDisposed { get; private set; }

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
        }

        public VertexElementAccessor GetAccessor(ElementName elementName) =>
            IInstancedVertexBufferView.GetAccessorInternal(this.Buffers, elementName);

        public bool TryGetAccessor(ElementName elementName, out VertexElementAccessor accessor) =>
            IInstancedVertexBufferView.TryGetAccessorInternal(this.Buffers, elementName, out accessor);

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

        private void Dispose(bool disposing)
        {
            if (this.IsDisposed is false)
            {
                if (disposing)
                {
                    this._buffers?.ForEach(vertexBuffer => vertexBuffer?.Dispose());
                }

                this.IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
