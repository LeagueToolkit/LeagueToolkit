using CommunityToolkit.HighPerformance.Buffers;
using System;
using System.Collections.Generic;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Exposes an interface for accessing a vertex buffer
    /// </summary>
    public interface IVertexBufferView
    {
        /// <summary>The description of the buffer</summary>
        VertexBufferDescription Description { get; }

        /// <summary>The elements of a vertex in the buffer</summary>
        IReadOnlyDictionary<ElementName, VertexBufferElementDescriptor> Elements { get; }

        /// <summary>Provides a read-only view into the buffer</summary>
        ReadOnlyMemory<byte> View { get; }

        /// <summary>Gets the size of a vertex inside the buffer</summary>
        int VertexStride { get; }

        /// <summary>Gets the vertex count of the buffer</summary>
        int VertexCount { get; }

        /// <summary>Gets a value indicating whether the vertex buffer has been disposed of</summary>
        public bool IsDisposed { get; }

        /// <summary>
        /// Creates a new <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The element to access</param>
        VertexElementAccessor GetAccessor(ElementName elementName);

        /// <summary>
        /// Creates a <see cref="VertexElementAccessor"/> for the specified element
        /// </summary>
        /// <param name="elementName">The name of the element to create an accessor for</param>
        /// <param name="accessor">If the element is found, contains the <see cref="VertexElementAccessor"/> for it,
        /// otherwise, the accessor is set to <see langword="default"/></param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="VertexBuffer"/> contains a <see cref="VertexElement"/>
        /// with the specified name; otherwise, <see langword="false"/>
        /// </returns>
        bool TryGetAccessor(ElementName elementName, out VertexElementAccessor accessor);
    }
}
