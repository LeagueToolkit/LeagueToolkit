using CommunityToolkit.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace LeagueToolkit.Core.Memory
{
    public readonly struct InstancedVertexBufferView : IInstancedVertexBufferView
    {
        public int VertexCount { get; }
        public IReadOnlyList<IVertexBufferView> Buffers { get; }

        public InstancedVertexBufferView(int vertexCount, IReadOnlyList<IVertexBufferView> buffers)
        {
            Guard.IsGreaterThan(vertexCount, 0, nameof(vertexCount));
            ValidateBuffers(buffers);

            this.VertexCount = vertexCount;
            this.Buffers = buffers;
        }

        private void ValidateBuffers(IReadOnlyList<IVertexBufferView> vertexBuffers)
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
            int vertexCount = vertexBuffers.First().VertexCount;
            if (!vertexBuffers.All(vertexBuffer => vertexBuffer.VertexCount == vertexCount))
                ThrowHelper.ThrowArgumentException(
                    nameof(vertexBuffers),
                    $"Vertex buffers must have equal vertex counts"
                );
        }
    }
}
