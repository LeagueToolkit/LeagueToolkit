using LeagueToolkit.Core.Renderer;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Memory
{
    public readonly struct VertexElementAccessor
    {
        public ElementName Name { get; }
        public ElementFormat Format { get; }

        private readonly Memory<byte> _buffer;
        private readonly int _stride;
        private readonly int _elementOffset;

        public VertexElementAccessor(VertexElement description, Memory<byte> buffer, int stride, int elementOffset)
        {
            this.Name = description.Name;
            this.Format = description.Format;

            this._buffer = buffer;
            this._stride = stride;
            this._elementOffset = elementOffset;
        }

        public VertexElementArray<float> AsFloatArray() => AsArray<float>();

        public VertexElementArray<Vector2> AsVector2Array() => AsArray<Vector2>();

        public VertexElementArray<Vector3> AsVector3Array() => AsArray<Vector3>();

        public VertexElementArray<Vector4> AsVector4Array() => AsArray<Vector4>();

        public VertexElementArray<(byte r, byte g, byte b, byte a)> AsColorArray() =>
            AsArray<(byte r, byte g, byte b, byte a)>();

        public VertexElementArray<(byte x, byte y, byte z, byte w)> AsByte4Array() =>
            AsArray<(byte x, byte y, byte z, byte w)>();

        private VertexElementArray<TElement> AsArray<TElement>() where TElement : unmanaged =>
            new(this.Name, this.Format, this._buffer, this._stride, this._elementOffset);
    }
}
