using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LeagueToolkit.IO.MapGeometryFile
{
    /// <summary>
    /// Describes the format of a vertex buffer stride
    /// </summary>
    [DebuggerDisplay("VertexElementGroup<{DebuggerDisplay,nq}>")]
    public struct MapGeometryVertexElementGroup : IEquatable<MapGeometryVertexElementGroup>
    {
        public MapGeometryVertexElementGroupUsage Usage { get; private set; }
     
        /// <summary>
        /// Bitmask of all the <see cref="MapGeometryVertexElementName"/> present in <see cref="Elements"/>
        /// </summary>
        public VertexElementGroupDescriptionFlags DescriptionFlags { get; private set; }

        /// <summary>
        /// The elements 
        /// </summary>
        public IReadOnlyList<MapGeometryVertexElement> Elements => this._elements;
        private readonly List<MapGeometryVertexElement> _elements = new();

        internal string DebuggerDisplay => string.Join(' ', this._elements.Select(x => x.Name));

        public MapGeometryVertexElementGroup(
            MapGeometryVertexElementGroupUsage usage,
            IEnumerable<MapGeometryVertexElement> elements
        )
        {
            this.Usage = usage;
            this._elements = new(elements);
            this.DescriptionFlags = GetDescriptionFlags(elements.Select(elem => elem.Name));
        }

        internal MapGeometryVertexElementGroup(BinaryReader br)
        {
            this.Usage = (MapGeometryVertexElementGroupUsage)br.ReadUInt32();

            uint vertexElementCount = br.ReadUInt32();
            for (int i = 0; i < vertexElementCount; i++)
            {
                this._elements.Add(new MapGeometryVertexElement(br));
            }

            this.DescriptionFlags = GetDescriptionFlags(this._elements.Select(elem => elem.Name));

            br.BaseStream.Seek(8 * (15 - vertexElementCount), SeekOrigin.Current);
        }

        public MapGeometryVertexElementGroup(MapGeometryVertex vertex)
        {
            this.Usage = MapGeometryVertexElementGroupUsage.Static;

            if (vertex.Position is not null)
            {
                this._elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.Position,
                        MapGeometryVertexElementFormat.XYZ_Float32
                    )
                );
            }
            if (vertex.Normal is not null)
            {
                this._elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.Normal,
                        MapGeometryVertexElementFormat.XYZ_Float32
                    )
                );
            }
            if (vertex.DiffuseUV is not null)
            {
                this._elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.DiffuseUV,
                        MapGeometryVertexElementFormat.XY_Float32
                    )
                );
            }
            if (vertex.LightmapUV is not null)
            {
                this._elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.LightmapUV,
                        MapGeometryVertexElementFormat.XY_Float32
                    )
                );
            }
            if (vertex.SecondaryColor is not null)
            {
                this._elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.SecondaryColor,
                        MapGeometryVertexElementFormat.BGRA_Packed8888
                    )
                );
            }

            this.DescriptionFlags = GetDescriptionFlags(this._elements.Select(elem => elem.Name));
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Usage);
            bw.Write(this._elements.Count);

            foreach (MapGeometryVertexElement vertexElement in this._elements)
            {
                vertexElement.Write(bw);
            }

            for (int i = 0; i < 15 - this._elements.Count; i++)
            {
                new MapGeometryVertexElement(
                    MapGeometryVertexElementName.Position,
                    MapGeometryVertexElementFormat.XYZW_Float32
                ).Write(bw);
            }
        }

        /// <summary>
        /// Gets the size of the described vertex
        /// </summary>
        public int GetVertexSize()
        {
            int size = 0;

            foreach (MapGeometryVertexElement vertexElement in this._elements)
            {
                size += vertexElement.GetElementSize();
            }

            return size;
        }

        /// <summary>
        /// Generates a <see cref="VertexElementGroupDescriptionFlags"/> bitmask for <paramref name="elements"/>
        /// </summary>
        public static VertexElementGroupDescriptionFlags GetDescriptionFlags(
            IEnumerable<MapGeometryVertexElementName> elements
        )
        {
            VertexElementGroupDescriptionFlags descriptionFlags = 0;

            foreach (MapGeometryVertexElementName element in elements)
            {
                descriptionFlags |= (VertexElementGroupDescriptionFlags)(1 << (int)element);
            }

            return descriptionFlags;
        }

        public bool Equals(MapGeometryVertexElementGroup other)
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

    public enum MapGeometryVertexElementGroupUsage : uint
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
        Position = 1 << MapGeometryVertexElementName.Position,
        BlendWeight = 1 << MapGeometryVertexElementName.BlendWeight,
        Normal = 1 << MapGeometryVertexElementName.Normal,
        PrimaryColor = 1 << MapGeometryVertexElementName.PrimaryColor,
        SecondaryColor = 1 << MapGeometryVertexElementName.SecondaryColor,
        FogCoordinate = 1 << MapGeometryVertexElementName.FogCoordinate,
        BlendIndex = 1 << MapGeometryVertexElementName.BlendIndex,
        DiffuseUV = 1 << MapGeometryVertexElementName.DiffuseUV,
        Texcoord1 = 1 << MapGeometryVertexElementName.Texcoord1,
        Texcoord2 = 1 << MapGeometryVertexElementName.Texcoord2,
        Texcoord3 = 1 << MapGeometryVertexElementName.Texcoord3,
        Texcoord4 = 1 << MapGeometryVertexElementName.Texcoord4,
        Texcoord5 = 1 << MapGeometryVertexElementName.Texcoord5,
        Texcoord6 = 1 << MapGeometryVertexElementName.Texcoord6,
        LightmapUV = 1 << MapGeometryVertexElementName.LightmapUV,
        StreamIndexCount = 1 << MapGeometryVertexElementName.StreamIndexCount
    }
}
