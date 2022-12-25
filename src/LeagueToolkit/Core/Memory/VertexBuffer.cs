using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueToolkit.Core.Memory
{
    public sealed class VertexBuffer
    {
        public VertexElementGroupUsage Usage { get; private set; }

        public IReadOnlyList<VertexElement> Elements => this._elements;
        private readonly List<VertexElement> _elements = new();

        private readonly MemoryOwner<byte> _buffer;
        private readonly int _stride;

        public VertexBuffer(
            VertexElementGroupUsage usage,
            IEnumerable<VertexElement> elements,
            MemoryOwner<byte> buffer
        )
        {
            Guard.IsNotNull(buffer, nameof(buffer));
            if (buffer.Span.IsEmpty)
                ThrowHelper.ThrowArgumentException(nameof(buffer.Span), "Buffer cannot be empty.");

            this.Usage = usage;
            this._elements = new(SanitizeElements(elements));
            this._buffer = buffer;

            this._stride = this._elements.Sum(element => element.GetSize());

            if (buffer.Length % this._stride != 0)
                ThrowHelper.ThrowArgumentException(
                    nameof(buffer),
                    "Buffer size must be a multiple of its stride size."
                );
        }

        public VertexElementAccessor GetAccessor(ElementName elementName)
        {
            // Search for accessor element and calculate element offset
            VertexElement accessorElement = new();
            int accessorElementOffset = 0;
            foreach (VertexElement element in this.Elements)
            {
                if (element.Name == elementName)
                {
                    accessorElement = element;
                    break;
                }

                // If the current element isn't the requested one, increment the
                // accessor element offset by the size of the current element
                accessorElementOffset += element.GetSize();
            }

            if (accessorElementOffset == this._stride)
            {
                ThrowHelper.ThrowInvalidOperationException($"Failed to find vertex element: {elementName}");
            }

            return new(accessorElement, this._buffer.Memory, this._stride, accessorElementOffset);
        }

        public static MemoryOwner<byte> AllocateForElements(IEnumerable<VertexElement> elements, int vertexCount)
        {
            Guard.IsNotNull(elements, nameof(elements));

            IEnumerable<VertexElement> sanitizedElements = SanitizeElements(elements);
            int stride = elements.Sum(element => element.GetSize());

            return MemoryOwner<byte>.Allocate(vertexCount * stride, AllocationMode.Clear);
        }

        internal static IEnumerable<VertexElement> SanitizeElements(IEnumerable<VertexElement> elements)
        {
            Guard.IsNotNull(elements, nameof(elements));

            // Check that all elements are unique
            IEnumerable<VertexElement> distinctElements = elements.Distinct();
            if (distinctElements.Count() != elements.Count())
            {
                ThrowHelper.ThrowArgumentException(nameof(distinctElements), "Contains duplicate vertex elements.");
            }

            // Sort elements from lowest to highest Name (stream index)
            return distinctElements.OrderBy(element => element.Name);
        }
    }
}
