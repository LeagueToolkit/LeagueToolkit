using CommunityToolkit.Diagnostics;
using System.Collections.Generic;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Provides an interface for accessing an instanced vertex buffer
    /// </summary>
    public interface IInstancedVertexBufferView
    {
        /// <summary>Gets the vertex count of all buffers</summary>
        int VertexCount { get; }

        /// <summary>
        /// The sharded vertex buffers which form the <see cref="IInstancedVertexBufferView"/>
        /// </summary>
        /// <remarks>
        /// Each buffer is guaranteed to contain unique/non-overlapping elements relative to the other buffers.<br></br>
        /// The first buffer is assumed to be the "instanced" buffer.
        /// </remarks>
        IReadOnlyList<IVertexBufferView> Buffers { get; }

        /// <summary>
        /// Creates a new <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The element to access</param>
        VertexElementAccessor GetAccessor(ElementName elementName) => GetAccessorInternal(this.Buffers, elementName);

        /// <summary>
        /// Creates a <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The name of the element to create an accessor for</param>
        /// <param name="accessor">If the element is found, contains the <see cref="VertexElementAccessor"/> for it,
        /// otherwise, the accessor is set to <see langword="default"/></param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="IInstancedVertexBufferView"/> has a <see cref="IVertexBufferView"/>
        /// which contains a <see cref="VertexElement"/> with the specified name; otherwise, <see langword="false"/>
        /// </returns>
        bool TryGetAccessor(ElementName elementName, out VertexElementAccessor accessor) =>
            TryGetAccessorInternal(this.Buffers, elementName, out accessor);

        internal static VertexElementAccessor GetAccessorInternal(
            IEnumerable<IVertexBufferView> buffers,
            ElementName elementName
        )
        {
            foreach (IVertexBufferView vertexBufferView in buffers)
            {
                if (vertexBufferView.TryGetAccessor(elementName, out VertexElementAccessor accessor))
                    return accessor;
            }

            throw new KeyNotFoundException(
                $"${nameof(IInstancedVertexBufferView)} does not contain a vertex buffer with element: {elementName}"
            );
        }

        internal static bool TryGetAccessorInternal(
            IEnumerable<IVertexBufferView> buffers,
            ElementName elementName,
            out VertexElementAccessor accessor
        )
        {
            foreach (IVertexBufferView vertexBufferView in buffers)
            {
                if (vertexBufferView.TryGetAccessor(elementName, out accessor))
                    return true;
            }

            accessor = default;
            return false;
        }
    }
}
