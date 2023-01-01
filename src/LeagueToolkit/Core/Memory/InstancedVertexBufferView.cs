using CommunityToolkit.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Represents a view into an instanced/sharded vertex buffer by wrapping multiple vertex buffer views<br></br>
    /// Refer to this <seealso href="https://www.braynzarsoft.net/viewtutorial/q16390-33-instancing-with-indexed-primitives">
    /// blogpost on how instancing works in D3D11</seealso> for a deeper explanation
    /// </summary>
    public readonly struct InstancedVertexBufferView : IInstancedVertexBufferView
    {
        public int VertexCount { get; }
        public IReadOnlyList<IVertexBufferView> Buffers { get; }

        /// <summary>
        /// Creates a new <see cref="InstancedVertexBufferView"/> object with the specified parameters
        /// </summary>
        /// <param name="vertexCount">The vertex count of all buffers</param>
        /// <param name="buffers">The buffers to wrap</param>
        public InstancedVertexBufferView(int vertexCount, IReadOnlyList<IVertexBufferView> buffers)
        {
            Guard.IsGreaterThan(vertexCount, 0, nameof(vertexCount));
            ValidateBuffers(vertexCount, buffers);

            this.VertexCount = vertexCount;
            this.Buffers = buffers;
        }

        private void ValidateBuffers(int vertexCount, IReadOnlyList<IVertexBufferView> vertexBuffers)
        {
            Guard.IsNotNull(vertexBuffers, nameof(vertexBuffers));
            Guard.HasSizeGreaterThanOrEqualTo(vertexBuffers, 1, nameof(vertexBuffers));

            IEnumerable<ElementName> elements = vertexBuffers.SelectMany(x => x.Elements.Keys);
            IEnumerable<ElementName> distinctElements = elements.Distinct();

            // Check that vertex buffers do not have overlapping elements
            if (elements.Count() != distinctElements.Count())
                ThrowHelper.ThrowArgumentException(
                    nameof(vertexBuffers),
                    $"Vertex buffers must not have overlapping elements"
                );

            // Check that all vertex buffers have the same vertex count
            if (!vertexBuffers.All(vertexBuffer => vertexBuffer.VertexCount == vertexCount))
                ThrowHelper.ThrowArgumentException(
                    nameof(vertexBuffers),
                    $"Vertex buffers must have equal vertex counts"
                );
        }
    }
}
