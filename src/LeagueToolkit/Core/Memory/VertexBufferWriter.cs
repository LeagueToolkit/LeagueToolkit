using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Primitives;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Wraps a memory region and exposes an interface for writing vertex data into it
    /// </summary>
    public sealed class VertexBufferWriter
    {
        /// <summary>
        /// The elements of a vertex inside <see cref="_buffer"/>
        /// </summary>
        public IReadOnlyDictionary<ElementName, VertexBufferElementDescriptor> Elements => this._elements;
        private readonly Dictionary<ElementName, VertexBufferElementDescriptor> _elements = [];

        /// <summary>
        /// The size of a vertex inside <see cref="_buffer"/>
        /// </summary>
        public int VertexStride { get; }

        private readonly Memory<byte> _buffer;

        /// <summary>
        /// Creates a new <see cref="VertexBufferWriter"/> over a specified memory region
        /// </summary>
        /// <param name="elements">The elements of a vertex inside <paramref name="buffer"/></param>
        /// <param name="buffer">The memory region for writing the vertex data</param>
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

            this._buffer = buffer;
            this.VertexStride = this._elements.Values.Sum(descriptor => descriptor.Element.GetSize());

            VertexBuffer.ValidateBufferDimensions(buffer.Span, this.VertexStride);
        }

        #region Writing API
        /// <summary>
        /// Writes a <see cref="ElementFormat.X_Float32"/> value for the specified element of a vertex at the specified index
        /// </summary>
        /// <param name="index">The index of the vertex to use for writing</param>
        /// <param name="element">The element into which <paramref name="value"/> should be written</param>
        /// <param name="value">The value to write for the specified element</param>
        public void WriteFloat(int index, ElementName element, float value)
        {
            ValidateAccessFormat(element, ElementFormat.X_Float32);
            Write(index, element, value);
        }

        /// <summary>
        /// Writes a <see cref="ElementFormat.XY_Float32"/> value for the specified element of a vertex at the specified index
        /// </summary>
        /// <param name="index">The index of the vertex to use for writing</param>
        /// <param name="element">The element into which <paramref name="value"/> should be written</param>
        /// <param name="value">The value to write for the specified element</param>
        public void WriteVector2(int index, ElementName element, Vector2 value)
        {
            ValidateAccessFormat(element, ElementFormat.XY_Float32);
            Write(index, element, value);
        }

        /// <summary>
        /// Writes a <see cref="ElementFormat.XYZ_Float32"/> value for the specified element of a vertex at the specified index
        /// </summary>
        /// <param name="index">The index of the vertex to use for writing</param>
        /// <param name="element">The element into which <paramref name="value"/> should be written</param>
        /// <param name="value">The value to write for the specified element</param>
        public void WriteVector3(int index, ElementName element, Vector3 value)
        {
            ValidateAccessFormat(element, ElementFormat.XYZ_Float32);
            Write(index, element, value);
        }

        /// <summary>
        /// Writes a <see cref="ElementFormat.XYZW_Float32"/> value for the specified element of a vertex at the specified index
        /// </summary>
        /// <param name="index">The index of the vertex to use for writing</param>
        /// <param name="element">The element into which <paramref name="value"/> should be written</param>
        /// <param name="value">The value to write for the specified element</param>
        public void WriteVector4(int index, ElementName element, Vector4 value)
        {
            ValidateAccessFormat(element, ElementFormat.XYZW_Float32);
            Write(index, element, value);
        }

        /// <summary>
        /// Writes a <see cref="ElementFormat.BGRA_Packed8888"/> value for the specified element of a vertex at the specified index
        /// </summary>
        /// <param name="index">The index of the vertex to use for writing</param>
        /// <param name="element">The element into which <paramref name="value"/> should be written</param>
        /// <param name="value">The value to write for the specified element</param>
        public void WriteColorBgraU8(int index, ElementName element, Color value)
        {
            ValidateAccessFormat(element, ElementFormat.BGRA_Packed8888);

            Span<byte> valueBytes = stackalloc byte[Color.GetFormatSize(ColorFormat.BgraU8)];
            value.Write(valueBytes, ColorFormat.BgraU8);

            Write(index, element, valueBytes);
        }

        /// <summary>
        /// Writes a <see cref="ElementFormat.RGBA_Packed8888"/> value for the specified element of a vertex at the specified index
        /// </summary>
        /// <param name="index">The index of the vertex to use for writing</param>
        /// <param name="element">The element into which <paramref name="value"/> should be written</param>
        /// <param name="value">The value to write for the specified element</param>
        public void WriteColorRgbaU8(int index, ElementName element, Color value)
        {
            ValidateAccessFormat(element, ElementFormat.RGBA_Packed8888);

            Span<byte> valueBytes = stackalloc byte[Color.GetFormatSize(ColorFormat.RgbaU8)];
            value.Write(valueBytes, ColorFormat.RgbaU8);

            Write(index, element, valueBytes);
        }

        /// <summary>
        /// Writes a <see cref="ElementFormat.ZYXW_Packed8888"/> value for the specified element of a vertex at the specified index
        /// </summary>
        /// <param name="index">The index of the vertex to use for writing</param>
        /// <param name="element">The element into which <paramref name="value"/> should be written</param>
        /// <param name="value">The value to write for the specified element</param>
        public void WriteZyxwU8(int index, ElementName element, (byte z, byte y, byte x, byte w) value)
        {
            ValidateAccessFormat(element, ElementFormat.ZYXW_Packed8888);
            Write(index, element, stackalloc byte[4] { value.z, value.y, value.x, value.w });
        }

        /// <summary>
        /// Writes a <see cref="ElementFormat.XYZW_Packed8888"/> value for the specified element of a vertex at the specified index
        /// </summary>
        /// <param name="index">The index of the vertex to use for writing</param>
        /// <param name="element">The element into which <paramref name="value"/> should be written</param>
        /// <param name="value">The value to write for the specified element</param>
        public void WriteXyzwU8(int index, ElementName element, (byte x, byte y, byte z, byte w) value)
        {
            ValidateAccessFormat(element, ElementFormat.XYZW_Packed8888);
            Write(index, element, stackalloc byte[4] { value.x, value.y, value.z, value.w });
        }

        private void Write<TValue>(int index, ElementName element, TValue value)
            where TValue : struct
        {
            int offset = this.VertexStride * index + this.Elements[element].Offset;
            MemoryMarshal.Write(this._buffer[offset..].Span, ref value);
        }

        private void Write(int index, ElementName element, ReadOnlySpan<byte> bytes)
        {
            int offset = this.VertexStride * index + this.Elements[element].Offset;
            bytes.CopyTo(this._buffer[offset..].Span);
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
