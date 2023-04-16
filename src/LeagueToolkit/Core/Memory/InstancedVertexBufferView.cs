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
    public readonly struct InstancedVertexBufferView
    {
        /// <summary>Gets the vertex count of all buffers</summary>
        public int VertexCount { get; }

        /// <summary>
        /// Gets the sharded vertex buffers that make up the full buffer
        /// </summary>
        /// <remarks>
        /// Each buffer is guaranteed to contain unique/non-overlapping elements relative to the other buffers.<br></br>
        /// The first buffer is assumed to be the "instanced" buffer.
        /// </remarks>
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

        private static void ValidateBuffers(int vertexCount, IReadOnlyList<IVertexBufferView> vertexBuffers)
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

        /// <summary>
        /// Creates a new <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The element to access</param>
        public VertexElementAccessor GetAccessor(ElementName elementName)
        {
            foreach (IVertexBufferView vertexBufferView in this.Buffers)
            {
                if (vertexBufferView.TryGetAccessor(elementName, out VertexElementAccessor accessor))
                    return accessor;
            }

            throw new KeyNotFoundException(
                $"${nameof(InstancedVertexBufferView)} does not contain a vertex buffer with element: {elementName}"
            );
        }

        /// <summary>
        /// Creates a <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The name of the element to create an accessor for</param>
        /// <param name="accessor">If the element is found, contains the <see cref="VertexElementAccessor"/> for it,
        /// otherwise, the accessor is set to <see langword="default"/></param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="InstancedVertexBufferView"/> has an <see cref="IVertexBufferView"/>
        /// which contains a <see cref="VertexElement"/> with the specified name; otherwise, <see langword="false"/>
        /// </returns>
        public bool TryGetAccessor(ElementName elementName, out VertexElementAccessor accessor)
        {
            foreach (IVertexBufferView vertexBufferView in this.Buffers)
            {
                if (vertexBufferView.TryGetAccessor(elementName, out accessor))
                    return true;
            }

            accessor = default;
            return false;
        }
    }
}
