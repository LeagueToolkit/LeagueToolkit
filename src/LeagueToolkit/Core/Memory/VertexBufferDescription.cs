using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Describes the format of a <see cref="VertexBuffer"/> stride <br></br>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/direct3d11/overviews-direct3d-11-resources-buffers-vertex-how-to">
    /// D3D11 - How to: Create a Vertex Buffer
    /// </seealso>
    /// </summary>
    [DebuggerDisplay("VertexBufferDescription<{DebuggerDisplay,nq}>")]
    public readonly struct VertexBufferDescription : IEquatable<VertexBufferDescription>
    {
        /// <summary>
        /// The usage of a vertex buffer during rendering
        /// </summary>
        public VertexBufferUsage Usage { get; }

        /// <summary>
        /// Bitmask of all the <see cref="ElementName"/> present in <see cref="Elements"/>
        /// </summary>
        public VertexBufferElementFlags DescriptionFlags { get; }

        /// <summary>
        /// The elements of a vertex inside a vertex buffer
        /// </summary>
        /// <remarks>The elements are stored in-memory in the same order</remarks>
        public IReadOnlyList<VertexElement> Elements => this._elements;
        private readonly VertexElement[] _elements;

        internal string DebuggerDisplay => string.Join(' ', this._elements.Select(x => x.Name));

        /// <summary>
        /// Creates a new <see cref="VertexBufferDescription"/> with the specified
        /// <paramref name="usage"/> and <paramref name="elements"/>
        /// </summary>
        public VertexBufferDescription(VertexBufferUsage usage, IEnumerable<VertexElement> elements)
        {
            this.Usage = usage;
            this._elements = elements.ToArray();
            this.DescriptionFlags = GetElementFlags(this._elements.Select(elem => elem.Name));
        }

        internal static VertexBufferDescription ReadFromMapGeometry(BinaryReader br, int version)
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
        /// Gets the size of the vertex described by <see cref="Elements"/>
        /// </summary>
        public int GetVertexSize() => this._elements.Sum(element => element.GetSize());

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

        #region Equals implementation
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
        #endregion
    }

    /// <summary>
    /// Identifies the usage of a <see cref="VertexBuffer"/> during rendering
    /// </summary>
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
    public enum VertexBufferElementFlags
    {
        Position = 1 << ElementName.Position,
        BlendWeight = 1 << ElementName.BlendWeight,
        Normal = 1 << ElementName.Normal,
        FogCoordinate = 1 << ElementName.FogCoordinate,
        PrimaryColor = 1 << ElementName.PrimaryColor,
        SecondaryColor = 1 << ElementName.SecondaryColor,
        BlendIndex = 1 << ElementName.BlendIndex,
        DiffuseUV = 1 << ElementName.Texcoord0,
        Texcoord1 = 1 << ElementName.Texcoord1,
        Texcoord2 = 1 << ElementName.Texcoord2,
        Texcoord3 = 1 << ElementName.Texcoord3,
        Texcoord4 = 1 << ElementName.Texcoord4,
        Texcoord5 = 1 << ElementName.Texcoord5,
        Texcoord6 = 1 << ElementName.Texcoord6,
        LightmapUV = 1 << ElementName.Texcoord7,

        Tangent = 1 << ElementName.Tangent,
    }
}
