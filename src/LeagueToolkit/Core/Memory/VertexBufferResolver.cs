using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LeagueToolkit.Core.Memory
{
    public sealed class VertexBufferResolver
    {
        public void Resolve(IReadOnlyList<IVertexBufferView> buffers, IVertexBufferView source)
        {
            for (int i = 0; i < buffers.Count; i++)
            {
                IVertexBufferView candidate = buffers[i];

                ResolveCandidate(source, candidate);
            }
        }

        public void ResolveCandidate(IVertexBufferView source, IVertexBufferView candidate)
        {
            if (ReferenceEquals(candidate, source)) { } // Buffers are the same

            if (candidate.VertexCount != source.VertexCount)
            {
                // isn't valid candidate
            }

            // Vertex counts match, check if content is the same
            if (candidate.Description != source.Description)
            {
                // isn't valid candidate
            }

            // Descriptions match, resolve matching elements
            List<ElementName> matchingElements = ResolveMatchingElements(candidate, source);
            if (matchingElements.SequenceEqual(candidate.Elements.Keys))
            {
                // Buffers are the same 1:1
            }
        }

        private List<ElementName> ResolveMatchingElements(IVertexBufferView source, IVertexBufferView candidate)
        {
            List<ElementName> matchingElements = new(candidate.Elements.Count);
            foreach (var (name, descriptor) in candidate.Elements)
            {
                int elementSize = descriptor.Element.GetSize();
                bool areElementsEqual = true;
                for (int i = 0; i < candidate.VertexCount; i++)
                {
                    ReadOnlySpan<byte> candidateSpan = candidate.View.Span.Slice(
                        i * candidate.VertexStride + descriptor.Offset,
                        elementSize
                    );
                    ReadOnlySpan<byte> sourceSpan = source.View.Span.Slice(
                        i * source.VertexStride + descriptor.Offset,
                        elementSize
                    );

                    if (candidateSpan.SequenceEqual(sourceSpan) is false)
                    {
                        areElementsEqual = false;
                        break;
                    }
                }

                if (areElementsEqual)
                    matchingElements.Add(name);
            }

            return matchingElements;
        }
    }
}
