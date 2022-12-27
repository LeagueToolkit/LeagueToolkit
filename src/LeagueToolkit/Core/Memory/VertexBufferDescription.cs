using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Describes the format of a vertex buffer stride
    /// </summary>
    //https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nf-d3d11-id3d11device-createinputlayout
    [DebuggerDisplay("VertexElementGroup<{DebuggerDisplay,nq}>")]
    public readonly struct VertexBufferDescription : IEquatable<VertexBufferDescription>
    {
        public VertexBufferUsage Usage { get; }

        /// <summary>
        /// Bitmask of all the <see cref="ElementName"/> present in <see cref="Elements"/>
        /// </summary>
        public VertexBufferElementFlags DescriptionFlags { get; }

        /// <summary>
        /// The elements
        /// </summary>
        public IReadOnlyList<VertexElement> Elements => this._elements;
        private readonly VertexElement[] _elements;

        internal string DebuggerDisplay => string.Join(' ', this._elements.Select(x => x.Name));

        public VertexBufferDescription(VertexBufferUsage usage, IEnumerable<VertexElement> elements)
        {
            this.Usage = usage;
            this._elements = elements.ToArray();
            this.DescriptionFlags = GetElementFlags(this._elements.Select(elem => elem.Name));
        }

        internal static VertexBufferDescription ReadFromMapGeometry(BinaryReader br)
        {
            VertexBufferUsage usage = (VertexBufferUsage)br.ReadUInt32();
            uint vertexElementCount = br.ReadUInt32();

            return new(usage, ReadElements().ToArray());

            IEnumerable<VertexElement> ReadElements()
            {
                for (int i = 0; i < vertexElementCount; i++)
                {
                    yield return new(br);
                }

                // Skip past unused default elements
                br.BaseStream.Seek(8 * (15 - vertexElementCount), SeekOrigin.Current);
            }
        }

        internal void WriteToMapGeometry(BinaryWriter bw)
        {
            bw.Write((uint)this.Usage);
            bw.Write(this._elements.Length);

            foreach (VertexElement vertexElement in this._elements)
            {
                vertexElement.Write(bw);
            }

            // Write default unused elements
            for (int i = 0; i < 15 - this._elements.Length; i++)
            {
                new VertexElement(ElementName.Position, ElementFormat.XYZW_Float32).Write(bw);
            }
        }

        /// <summary>
        /// Gets the size of the described vertex
        /// </summary>
        public int GetVertexSize()
        {
            int size = 0;

            foreach (VertexElement vertexElement in this._elements)
            {
                size += vertexElement.GetSize();
            }

            return size;
        }

        /// <summary>
        /// Generates a <see cref="VertexBufferElementFlags"/> bitmask for <paramref name="elements"/>
        /// </summary>
        public static VertexBufferElementFlags GetElementFlags(IEnumerable<ElementName> elements)
        {
            VertexBufferElementFlags descriptionFlags = 0;

            foreach (ElementName element in elements)
            {
                descriptionFlags |= (VertexBufferElementFlags)(1 << (int)element);
            }

            return descriptionFlags;
        }

        public static bool operator ==(VertexBufferDescription left, VertexBufferDescription right) =>
            left.Equals(right);

        public static bool operator !=(VertexBufferDescription left, VertexBufferDescription right) =>
            !left.Equals(right);

        public bool Equals(VertexBufferDescription other)
        {
            // If usage is not the same
            if (this.Usage != other.Usage)
            {
                return false;
            }

            // Check if Vertex Element count is the same
            if (this._elements.Length == other._elements.Length)
            {
                // If Vertex Element count is the same, compare them
                return this._elements.SequenceEqual(other._elements);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj) =>
            obj switch
            {
                VertexBufferDescription other => Equals(other),
                _ => false
            };

        public override int GetHashCode() => HashCode.Combine(this.Usage, this._elements);
    }

    public enum VertexBufferUsage : uint
    {
        /// <summary>
        /// Static Vertex Data
        /// </summary>
        Static,

        /// <summary>
        /// Dynamic Vertex Data (can be changed frequently)
        /// </summary>
        /// <remarks>
        /// ⚠️ NOT OFFICIALLY SUPPORTED ⚠️
        /// </remarks>
        Dynamic,

        /// <summary>
        /// Streaming Vertex Data (changed every frame)
        /// </summary>
        /// <remarks>
        /// ⚠️ NOT OFFICIALLY SUPPORTED ⚠️
        /// </remarks>
        Stream
    }

    [Flags]
    public enum VertexBufferElementFlags : ushort
    {
        Position = 1 << ElementName.Position,
        BlendWeight = 1 << ElementName.BlendWeight,
        Normal = 1 << ElementName.Normal,
        MaybeTangent = 1 << ElementName.MaybeTangent,
        BaseColor = 1 << ElementName.BaseColor,
        FogCoordinate = 1 << ElementName.FogCoordinate,
        BlendIndex = 1 << ElementName.BlendIndex,
        DiffuseUV = 1 << ElementName.DiffuseUV,
        Texcoord1 = 1 << ElementName.Texcoord1,
        Texcoord2 = 1 << ElementName.Texcoord2,
        Texcoord3 = 1 << ElementName.Texcoord3,
        Texcoord4 = 1 << ElementName.Texcoord4,
        Texcoord5 = 1 << ElementName.Texcoord5,
        Texcoord6 = 1 << ElementName.Texcoord6,
        LightmapUV = 1 << ElementName.LightmapUV
    }
}
