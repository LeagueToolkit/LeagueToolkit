using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Memory
{
    public class VertexBufferWriter
    {
        private readonly VertexElementGroupUsage _usage;
        private readonly VertexElement[] _elements;

        private readonly Memory<byte> _buffer;
        private readonly int _stride;
        private readonly int _vertexCount;

        private readonly Dictionary<ElementName, int> _elementOffsets;

        public VertexBufferWriter(
            VertexElementGroupUsage usage,
            IEnumerable<VertexElement> elements,
            Memory<byte> buffer
        )
        {
            Guard.IsNotNull(elements, nameof(elements));
            if (buffer.IsEmpty)
                ThrowHelper.ThrowArgumentException(nameof(buffer), "Buffer cannot be empty.");

            this._usage = usage;
            this._elements = VertexBuffer.SanitizeElements(elements).ToArray();

            this._buffer = buffer;
            this._stride = this._elements.Sum(element => element.GetSize());

            if (buffer.Length % this._stride != 0)
                ThrowHelper.ThrowArgumentException(
                    nameof(buffer),
                    "Buffer size must be a multiple of its stride size."
                );

            // Store offset of each element for writing
            int currentElementOffset = 0;
            this._elementOffsets = new(this._elements.Length);
            foreach (VertexElement element in this._elements)
            {
                this._elementOffsets.Add(element.Name, currentElementOffset);

                currentElementOffset += element.GetSize();
            }
        }

        public void WriteFloat(int index, ElementName element, float value) => Write(index, element, value);

        public void WriteVector2(int index, ElementName element, Vector2 value) => Write(index, element, value);

        public void WriteVector3(int index, ElementName element, Vector3 value) => Write(index, element, value);

        public void WriteVector4(int index, ElementName element, Vector4 value) => Write(index, element, value);

        public void WriteColorBgraU8(int index, ElementName element, Color value)
        {
            Span<byte> valueBytes = stackalloc byte[Color.FormatSize(ColorFormat.BgraU8)];
            value.Write(valueBytes, ColorFormat.BgraU8);

            Write(index, element, valueBytes);
        }

        public void WriteColorRgbaU8(int index, ElementName element, Color value)
        {
            Span<byte> valueBytes = stackalloc byte[Color.FormatSize(ColorFormat.RgbaU8)];
            value.Write(valueBytes, ColorFormat.RgbaU8);

            Write(index, element, valueBytes);
        }

        public void WriteByte4Zyxw(int index, ElementName element, (byte x, byte y, byte z, byte w) value) =>
            Write(index, element, stackalloc byte[4] { value.z, value.y, value.x, value.w });

        public void WriteByte4Xyzw(int index, ElementName element, (byte x, byte y, byte z, byte w) value) =>
            Write(index, element, stackalloc byte[4] { value.x, value.y, value.z, value.w });

        private void Write<TValue>(int index, ElementName element, TValue value) where TValue : struct
        {
            int offset = this._stride * index + this._elementOffsets[element];
            MemoryMarshal.Write(this._buffer[offset..].Span, ref value);
        }

        private void Write(int index, ElementName element, ReadOnlySpan<byte> bytes)
        {
            int offset = this._stride * index + this._elementOffsets[element];

            bytes.CopyTo(this._buffer[offset..].Span);
        }
    }
}
