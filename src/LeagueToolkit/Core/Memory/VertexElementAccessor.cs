using System;
using System.Numerics;
using CommunityToolkit.Diagnostics;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Contains the necessary information for accessing a <see cref="VertexElement"/> inside of a vertex buffer
    /// </summary>
    public readonly struct VertexElementAccessor
    {
        /// <summary>
        /// Describes the element of the accessor
        /// </summary>
        public VertexElement Element { get; }

        /// <summary>
        /// Provides a read-only view into the buffer
        /// </summary>
        public ReadOnlyMemory<byte> BufferView { get; }

        /// <summary>
        /// The size of a vertex inside the buffer
        /// </summary>
        public int VertexStride { get; }

        /// <summary>
        /// The vertex count of the buffer
        /// </summary>
        public int VertexCount { get; }

        /// <summary>
        /// The byte offset of the element inside a vertex stride in the buffer
        /// </summary>
        public int ElementOffset { get; }

        /// <summary>
        /// The size of the element described by this accessor
        /// </summary>
        public int ElementSize { get; }

        /// <summary>
        /// Creates a new <see cref="VertexElementAccessor"/> for the specified <paramref name="element"/> inside of <paramref name="buffer"/>
        /// </summary>
        /// <param name="element">The element this accessor should describe</param>
        /// <param name="buffer">The buffer which contains the described <paramref name="element"/></param>
        /// <param name="stride">The size of a vertex stride inside <paramref name="buffer"/></param>
        /// <param name="elementOffset">The byte offset of <paramref name="element"/> inside of a <paramref name="buffer"/> vertex stride</param>
        public VertexElementAccessor(VertexElement element, ReadOnlyMemory<byte> buffer, int stride, int elementOffset)
        {
            VertexBuffer.ValidateBufferDimensions(buffer.Span, stride);

            this.Element = element;
            this.BufferView = buffer;
            this.VertexStride = stride;
            this.VertexCount = buffer.Length / stride;
            this.ElementOffset = elementOffset;
            this.ElementSize = element.GetSize();
        }

        public ReadOnlySpan<byte> DecodeAt(int index)
        {
            return this.BufferView.Span.Slice(this.VertexStride * index + this.ElementOffset, this.ElementSize);
        }

        /// <summary> Creates an array for accessing the element described by this accessor</summary>
        /// <returns>An array of elements with <see cref="ElementFormat.X_Float32"/></returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the format of the described element does not match</remarks>
        public VertexElementArray<float> AsFloatArray()
        {
            ValidateAccessFormat(ElementFormat.X_Float32);
            return AsArray<float>();
        }

        /// <summary> Creates an array for accessing the element described by this accessor</summary>
        /// <returns>An array of elements with <see cref="ElementFormat.XY_Float32"/></returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the format of the described element does not match</remarks>
        public VertexElementArray<Vector2> AsVector2Array()
        {
            ValidateAccessFormat(ElementFormat.XY_Float32);
            return AsArray<Vector2>();
        }

        /// <summary> Creates an array for accessing the element described by this accessor</summary>
        /// <returns>An array of elements with <see cref="ElementFormat.XYZ_Float32"/></returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the format of the described element does not match</remarks>
        public VertexElementArray<Vector3> AsVector3Array()
        {
            ValidateAccessFormat(ElementFormat.XYZ_Float32);
            return AsArray<Vector3>();
        }

        /// <summary> Creates an array for accessing the element described by this accessor</summary>
        /// <returns>An array of elements with <see cref="ElementFormat.XYZW_Float32"/></returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the format of the described element does not match</remarks>
        public VertexElementArray<Vector4> AsVector4Array()
        {
            ValidateAccessFormat(ElementFormat.XYZW_Float32);
            return AsArray<Vector4>();
        }

        /// <summary> Creates an array for accessing the element described by this accessor</summary>
        /// <returns>An array of elements with <see cref="ElementFormat.BGRA_Packed8888"/></returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the format of the described element does not match</remarks>
        public VertexElementArray<(byte b, byte g, byte r, byte a)> AsBgraU8Array()
        {
            ValidateAccessFormat(ElementFormat.BGRA_Packed8888);
            return AsArray<(byte b, byte g, byte r, byte a)>();
        }

        /// <summary> Creates an array for accessing the element described by this accessor</summary>
        /// <returns>An array of elements with <see cref="ElementFormat.RGBA_Packed8888"/></returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the format of the described element does not match</remarks>
        public VertexElementArray<(byte r, byte g, byte b, byte a)> AsRgbaU8Array()
        {
            ValidateAccessFormat(ElementFormat.RGBA_Packed8888);
            return AsArray<(byte r, byte g, byte b, byte a)>();
        }

        /// <summary> Creates an array for accessing the element described by this accessor</summary>
        /// <returns>An array of elements with <see cref="ElementFormat.ZYXW_Packed8888"/></returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the format of the described element does not match</remarks>
        public VertexElementArray<(byte z, byte y, byte x, byte w)> AsZyxwU8Array()
        {
            ValidateAccessFormat(ElementFormat.ZYXW_Packed8888);
            return AsArray<(byte z, byte y, byte x, byte w)>();
        }

        public VertexElementArray<(Half, Half)> AsXyF16Array()
        {
            ValidateAccessFormat(ElementFormat.XY_Packed1616);
            return AsArray<(Half, Half)>();
        }

        /// <summary> Creates an array for accessing the element described by this accessor</summary>
        /// <returns>An array of elements with <see cref="ElementFormat.XYZW_Packed8888"/></returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the format of the described element does not match</remarks>
        public VertexElementArray<(byte x, byte y, byte z, byte w)> AsXyzwU8Array()
        {
            ValidateAccessFormat(ElementFormat.XYZW_Packed8888);
            return AsArray<(byte x, byte y, byte z, byte w)>();
        }

        public VertexElementArray<(Half, Half, Half)> AsXyzF16Array()
        {
            ValidateAccessFormat(ElementFormat.XYZ_Packed161616);
            return AsArray<(Half, Half, Half)>();
        }

        private VertexElementArray<TElement> AsArray<TElement>()
            where TElement : struct => new(this);

        private void ValidateAccessFormat(ElementFormat accessFormat)
        {
            if (accessFormat != this.Element.Format)
            {
                ThrowHelper.ThrowInvalidOperationException(
                    $"Cannot use a {this.Element.Format} accessor as {accessFormat}"
                );
            }
        }
    }
}
