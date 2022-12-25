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
        public VertexElementGroupUsage Usage { get; }

        public IReadOnlyDictionary<ElementName, (VertexElement element, int offset)> Elements => this._elements;
        private readonly Dictionary<ElementName, (VertexElement element, int offset)> _elements = new();

        public Memory<byte> Buffer { get; }
        public int Stride { get; }

        public VertexBufferWriter(
            VertexElementGroupUsage usage,
            IEnumerable<VertexElement> elements,
            Memory<byte> buffer
        )
        {
            Guard.IsNotNull(elements, nameof(elements));
            if (buffer.IsEmpty)
                ThrowHelper.ThrowArgumentException(nameof(buffer), "Buffer cannot be empty.");

            this.Usage = usage;

            // Store offset of each element for reading
            int currentElementOffset = 0;
            foreach (VertexElement element in VertexBuffer.SanitizeElements(elements))
            {
                this._elements.Add(element.Name, (element, currentElementOffset));

                currentElementOffset += element.GetSize();
            }

            this.Buffer = buffer;
            this.Stride = this._elements.Values.Sum(entry => entry.element.GetSize());

            if (buffer.Length % this.Stride != 0)
                ThrowHelper.ThrowArgumentException(
                    nameof(buffer),
                    "Buffer size must be a multiple of its stride size."
                );
        }

        #region Writing API
        public void WriteFloat(int index, ElementName element, float value)
        {
            ValidateAccessFormat(this._elements[element].element.Format, ElementFormat.X_Float32);
            Write(index, element, value);
        }

        public void WriteVector2(int index, ElementName element, Vector2 value)
        {
            ValidateAccessFormat(this._elements[element].element.Format, ElementFormat.XY_Float32);
            Write(index, element, value);
        }

        public void WriteVector3(int index, ElementName element, Vector3 value)
        {
            ValidateAccessFormat(this._elements[element].element.Format, ElementFormat.XYZ_Float32);
            Write(index, element, value);
        }

        public void WriteVector4(int index, ElementName element, Vector4 value)
        {
            ValidateAccessFormat(this._elements[element].element.Format, ElementFormat.XYZW_Float32);
            Write(index, element, value);
        }

        public void WriteColorBgraU8(int index, ElementName element, Color value)
        {
            ValidateAccessFormat(this._elements[element].element.Format, ElementFormat.BGRA_Packed8888);

            Span<byte> valueBytes = stackalloc byte[Color.GetFormatSize(ColorFormat.BgraU8)];
            value.Write(valueBytes, ColorFormat.BgraU8);

            Write(index, element, valueBytes);
        }

        public void WriteColorRgbaU8(int index, ElementName element, Color value)
        {
            ValidateAccessFormat(this._elements[element].element.Format, ElementFormat.RGBA_Packed8888);

            Span<byte> valueBytes = stackalloc byte[Color.GetFormatSize(ColorFormat.RgbaU8)];
            value.Write(valueBytes, ColorFormat.RgbaU8);

            Write(index, element, valueBytes);
        }

        public void WriteZyxwU8(int index, ElementName element, (byte z, byte y, byte x, byte w) value)
        {
            ValidateAccessFormat(this._elements[element].element.Format, ElementFormat.ZYXW_Packed8888);
            Write(index, element, stackalloc byte[4] { value.z, value.y, value.x, value.w });
        }

        public void WriteXyzwU8(int index, ElementName element, (byte x, byte y, byte z, byte w) value)
        {
            ValidateAccessFormat(this._elements[element].element.Format, ElementFormat.XYZW_Packed8888);
            Write(index, element, stackalloc byte[4] { value.x, value.y, value.z, value.w });
        }

        private void Write<TValue>(int index, ElementName element, TValue value) where TValue : struct
        {
            int offset = this.Stride * index + this.Elements[element].offset;
            MemoryMarshal.Write(this.Buffer[offset..].Span, ref value);
        }

        private void Write(int index, ElementName element, ReadOnlySpan<byte> bytes)
        {
            int offset = this.Stride * index + this.Elements[element].offset;

            bytes.CopyTo(this.Buffer[offset..].Span);
        }

        private void ValidateAccessFormat(ElementFormat format, ElementFormat writingFormat)
        {
            if (writingFormat != format)
            {
                ThrowHelper.ThrowInvalidOperationException(
                    $"Cannot write a {writingFormat} value because expected format is {format}"
                );
            }
        }
        #endregion
    }
}
