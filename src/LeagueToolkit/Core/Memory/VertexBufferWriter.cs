using CommunityToolkit.Diagnostics;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Wraps a memory region for writing vertex data
    /// </summary>
    public class VertexBufferWriter
    {
        public IReadOnlyDictionary<ElementName, VertexBufferElementDescriptor> Elements => this._elements;
        private readonly Dictionary<ElementName, VertexBufferElementDescriptor> _elements = new();

        public Memory<byte> Buffer { get; }
        public int VertexStride { get; }

        public VertexBufferWriter(IEnumerable<VertexElement> elements, Memory<byte> buffer)
        {
            Guard.IsNotNull(elements, nameof(elements));

            // Store offset of each element for reading
            int currentElementOffset = 0;
            foreach (VertexElement element in VertexBuffer.SanitizeElements(elements))
            {
                this._elements.Add(element.Name, new(element, currentElementOffset));

                currentElementOffset += element.GetSize();
            }

            this.Buffer = buffer;
            this.VertexStride = this._elements.Values.Sum(descriptor => descriptor.Element.GetSize());

            VertexBuffer.ValidateBufferDimensions(buffer.Span, this.VertexStride);
        }

        #region Writing API
        public void WriteFloat(int index, ElementName element, float value)
        {
            ValidateAccessFormat(element, ElementFormat.X_Float32);
            Write(index, element, value);
        }

        public void WriteVector2(int index, ElementName element, Vector2 value)
        {
            ValidateAccessFormat(element, ElementFormat.XY_Float32);
            Write(index, element, value);
        }

        public void WriteVector3(int index, ElementName element, Vector3 value)
        {
            ValidateAccessFormat(element, ElementFormat.XYZ_Float32);
            Write(index, element, value);
        }

        public void WriteVector4(int index, ElementName element, Vector4 value)
        {
            ValidateAccessFormat(element, ElementFormat.XYZW_Float32);
            Write(index, element, value);
        }

        public void WriteColorBgraU8(int index, ElementName element, Color value)
        {
            ValidateAccessFormat(element, ElementFormat.BGRA_Packed8888);

            Span<byte> valueBytes = stackalloc byte[Color.GetFormatSize(ColorFormat.BgraU8)];
            value.Write(valueBytes, ColorFormat.BgraU8);

            Write(index, element, valueBytes);
        }

        public void WriteColorRgbaU8(int index, ElementName element, Color value)
        {
            ValidateAccessFormat(element, ElementFormat.RGBA_Packed8888);

            Span<byte> valueBytes = stackalloc byte[Color.GetFormatSize(ColorFormat.RgbaU8)];
            value.Write(valueBytes, ColorFormat.RgbaU8);

            Write(index, element, valueBytes);
        }

        public void WriteZyxwU8(int index, ElementName element, (byte z, byte y, byte x, byte w) value)
        {
            ValidateAccessFormat(element, ElementFormat.ZYXW_Packed8888);
            Write(index, element, stackalloc byte[4] { value.z, value.y, value.x, value.w });
        }

        public void WriteXyzwU8(int index, ElementName element, (byte x, byte y, byte z, byte w) value)
        {
            ValidateAccessFormat(element, ElementFormat.XYZW_Packed8888);
            Write(index, element, stackalloc byte[4] { value.x, value.y, value.z, value.w });
        }

        private void Write<TValue>(int index, ElementName element, TValue value) where TValue : struct
        {
            int offset = this.VertexStride * index + this.Elements[element].Offset;
            MemoryMarshal.Write(this.Buffer[offset..].Span, ref value);
        }

        private void Write(int index, ElementName element, ReadOnlySpan<byte> bytes)
        {
            int offset = this.VertexStride * index + this.Elements[element].Offset;
            bytes.CopyTo(this.Buffer[offset..].Span);
        }

        private void ValidateAccessFormat(ElementName element, ElementFormat writingFormat)
        {
            ElementFormat format = this._elements[element].Element.Format;
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
