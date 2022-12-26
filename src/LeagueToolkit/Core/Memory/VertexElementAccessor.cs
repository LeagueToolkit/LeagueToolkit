using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Renderer;
using System;
using System.Numerics;

namespace LeagueToolkit.Core.Memory
{
    public readonly struct VertexElementAccessor
    {
        public ElementName Name { get; }
        public ElementFormat Format { get; }

        public ReadOnlyMemory<byte> BufferView { get; }
        public int VertexStride { get; }
        public int VertexCount { get; }
        public int ElementOffset { get; }

        public VertexElementAccessor(
            VertexElement description,
            ReadOnlyMemory<byte> buffer,
            int stride,
            int elementOffset
        )
        {
            VertexBuffer.ValidateBufferDimensions(buffer.Span, stride);

            this.Name = description.Name;
            this.Format = description.Format;

            this.BufferView = buffer;
            this.VertexStride = stride;
            this.VertexCount = buffer.Length / stride;
            this.ElementOffset = elementOffset;
        }

        public VertexElementArray<float> AsFloatArray()
        {
            ValidateAccessFormat(ElementFormat.X_Float32);
            return AsArray<float>();
        }

        public VertexElementArray<Vector2> AsVector2Array()
        {
            ValidateAccessFormat(ElementFormat.XY_Float32);
            return AsArray<Vector2>();
        }

        public VertexElementArray<Vector3> AsVector3Array()
        {
            ValidateAccessFormat(ElementFormat.XYZ_Float32);
            return AsArray<Vector3>();
        }

        public VertexElementArray<Vector4> AsVector4Array()
        {
            ValidateAccessFormat(ElementFormat.XYZW_Float32);
            return AsArray<Vector4>();
        }

        public VertexElementArray<(byte b, byte g, byte r, byte a)> AsBgraU8Array()
        {
            ValidateAccessFormat(ElementFormat.BGRA_Packed8888);
            return AsArray<(byte b, byte g, byte r, byte a)>();
        }

        public VertexElementArray<(byte r, byte g, byte b, byte a)> AsRgbaU8Array()
        {
            ValidateAccessFormat(ElementFormat.RGBA_Packed8888);
            return AsArray<(byte r, byte g, byte b, byte a)>();
        }

        public VertexElementArray<(byte z, byte y, byte x, byte w)> AsZyxwU8Array()
        {
            ValidateAccessFormat(ElementFormat.ZYXW_Packed8888);
            return AsArray<(byte z, byte y, byte x, byte w)>();
        }

        public VertexElementArray<(byte x, byte y, byte z, byte w)> AsXyzwU8Array()
        {
            ValidateAccessFormat(ElementFormat.XYZW_Packed8888);
            return AsArray<(byte x, byte y, byte z, byte w)>();
        }

        private VertexElementArray<TElement> AsArray<TElement>() where TElement : struct => new(this);

        private void ValidateAccessFormat(ElementFormat accessFormat)
        {
            if (accessFormat != this.Format)
            {
                ThrowHelper.ThrowInvalidOperationException($"Cannot use a {this.Format} accessor as {accessFormat}");
            }
        }
    }
}
