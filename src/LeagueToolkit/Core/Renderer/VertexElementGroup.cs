using LeagueToolkit.IO.MapGeometryFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LeagueToolkit.Core.Renderer
{
    /// <summary>
    /// Describes the format of a vertex buffer stride
    /// </summary>
    //https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nf-d3d11-id3d11device-createinputlayout
    [DebuggerDisplay("VertexElementGroup<{DebuggerDisplay,nq}>")]
    public struct VertexElementGroup : IEquatable<VertexElementGroup>
    {
        public VertexElementGroupUsage Usage { get; private set; }

        /// <summary>
        /// Bitmask of all the <see cref="VertexElementName"/> present in <see cref="Elements"/>
        /// </summary>
        public VertexElementGroupDescriptionFlags DescriptionFlags { get; private set; }

        /// <summary>
        /// The elements
        /// </summary>
        public IReadOnlyList<VertexElement> Elements => this._elements;
        private readonly List<VertexElement> _elements = new();

        internal string DebuggerDisplay => string.Join(' ', this._elements.Select(x => x.Name));

        public VertexElementGroup(VertexElementGroupUsage usage, IEnumerable<VertexElement> elements)
        {
            this.Usage = usage;
            this._elements = new(elements);
            this.DescriptionFlags = GetDescriptionFlags(elements.Select(elem => elem.Name));
        }

        internal VertexElementGroup(BinaryReader br)
        {
            this.Usage = (VertexElementGroupUsage)br.ReadUInt32();

            uint vertexElementCount = br.ReadUInt32();
            for (int i = 0; i < vertexElementCount; i++)
            {
                this._elements.Add(new VertexElement(br));
            }

            this.DescriptionFlags = GetDescriptionFlags(this._elements.Select(elem => elem.Name));

            br.BaseStream.Seek(8 * (15 - vertexElementCount), SeekOrigin.Current);
        }

        public VertexElementGroup(MapGeometryVertex vertex)
        {
            this.Usage = VertexElementGroupUsage.Static;

            if (vertex.Position is not null)
            {
                this._elements.Add(new VertexElement(VertexElementName.Position, VertexElementFormat.XYZ_Float32));
            }
            if (vertex.Normal is not null)
            {
                this._elements.Add(new VertexElement(VertexElementName.Normal, VertexElementFormat.XYZ_Float32));
            }
            if (vertex.DiffuseUV is not null)
            {
                this._elements.Add(new VertexElement(VertexElementName.DiffuseUV, VertexElementFormat.XY_Float32));
            }
            if (vertex.LightmapUV is not null)
            {
                this._elements.Add(new VertexElement(VertexElementName.LightmapUV, VertexElementFormat.XY_Float32));
            }
            if (vertex.SecondaryColor is not null)
            {
                this._elements.Add(
                    new VertexElement(VertexElementName.SecondaryColor, VertexElementFormat.BGRA_Packed8888)
                );
            }

            this.DescriptionFlags = GetDescriptionFlags(this._elements.Select(elem => elem.Name));
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Usage);
            bw.Write(this._elements.Count);

            foreach (VertexElement vertexElement in this._elements)
            {
                vertexElement.Write(bw);
            }

            for (int i = 0; i < 15 - this._elements.Count; i++)
            {
                new VertexElement(VertexElementName.Position, VertexElementFormat.XYZW_Float32).Write(bw);
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
                size += vertexElement.GetElementSize();
            }

            return size;
        }

        /// <summary>
        /// Generates a <see cref="VertexElementGroupDescriptionFlags"/> bitmask for <paramref name="elements"/>
        /// </summary>
        public static VertexElementGroupDescriptionFlags GetDescriptionFlags(IEnumerable<VertexElementName> elements)
        {
            VertexElementGroupDescriptionFlags descriptionFlags = 0;

            foreach (VertexElementName element in elements)
            {
                descriptionFlags |= (VertexElementGroupDescriptionFlags)(1 << (int)element);
            }

            return descriptionFlags;
        }

        public bool Equals(VertexElementGroup other)
        {
            // If usage is not the same
            if (this.Usage != other.Usage)
            {
                return false;
            }

            // Check if Vertex Element count is the same
            if (this._elements.Count == other._elements.Count)
            {
                // If Vertex Element count is the same, compare them
                return Enumerable.SequenceEqual(this._elements, other._elements);
            }
            else
            {
                return false;
            }
        }
    }

    public enum VertexElementGroupUsage : uint
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
    public enum VertexElementGroupDescriptionFlags : ushort
    {
        Position = 1 << VertexElementName.Position,
        BlendWeight = 1 << VertexElementName.BlendWeight,
        Normal = 1 << VertexElementName.Normal,
        PrimaryColor = 1 << VertexElementName.PrimaryColor,
        SecondaryColor = 1 << VertexElementName.SecondaryColor,
        FogCoordinate = 1 << VertexElementName.FogCoordinate,
        BlendIndex = 1 << VertexElementName.BlendIndex,
        DiffuseUV = 1 << VertexElementName.DiffuseUV,
        Texcoord1 = 1 << VertexElementName.Texcoord1,
        Texcoord2 = 1 << VertexElementName.Texcoord2,
        Texcoord3 = 1 << VertexElementName.Texcoord3,
        Texcoord4 = 1 << VertexElementName.Texcoord4,
        Texcoord5 = 1 << VertexElementName.Texcoord5,
        Texcoord6 = 1 << VertexElementName.Texcoord6,
        LightmapUV = 1 << VertexElementName.LightmapUV
    }
}
