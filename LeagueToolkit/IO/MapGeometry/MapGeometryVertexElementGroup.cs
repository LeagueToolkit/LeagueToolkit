using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LeagueToolkit.IO.MapGeometry
{
    [DebuggerDisplay("VertexElementGroup<{DebuggerDisplay,nq}>")]
    public class MapGeometryVertexElementGroup : IEquatable<MapGeometryVertexElementGroup>
    {
        public MapGeometryVertexElementGroupUsage Usage { get; private set; }
        public VertexElementGroupDescriptionFlags DescriptionFlags { get; private set; }
        public List<MapGeometryVertexElement> Elements { get; private set; } = new();

        internal string DebuggerDisplay => string.Join(' ', this.Elements.Select(x => x.Name));

        public MapGeometryVertexElementGroup(
            MapGeometryVertexElementGroupUsage usage,
            IEnumerable<MapGeometryVertexElement> elements
        )
        {
            this.Usage = usage;
            this.Elements = new(elements);
            this.DescriptionFlags = GenerateDescriptionFlags(elements.Select(elem => elem.Name));
        }

        public MapGeometryVertexElementGroup(BinaryReader br)
        {
            this.Usage = (MapGeometryVertexElementGroupUsage)br.ReadUInt32();

            uint vertexElementCount = br.ReadUInt32();
            for (int i = 0; i < vertexElementCount; i++)
            {
                this.Elements.Add(new MapGeometryVertexElement(br));
            }

            this.DescriptionFlags = GenerateDescriptionFlags(this.Elements.Select(elem => elem.Name));

            br.BaseStream.Seek(8 * (15 - vertexElementCount), SeekOrigin.Current);
        }

        public MapGeometryVertexElementGroup(MapGeometryVertex vertex)
        {
            this.Usage = MapGeometryVertexElementGroupUsage.Static;

            if (vertex.Position is not null)
            {
                this.Elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.Position,
                        MapGeometryVertexElementFormat.XYZ_Float32
                    )
                );
            }
            if (vertex.Normal is not null)
            {
                this.Elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.Normal,
                        MapGeometryVertexElementFormat.XYZ_Float32
                    )
                );
            }
            if (vertex.DiffuseUV is not null)
            {
                this.Elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.DiffuseUV,
                        MapGeometryVertexElementFormat.XY_Float32
                    )
                );
            }
            if (vertex.LightmapUV is not null)
            {
                this.Elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.LightmapUV,
                        MapGeometryVertexElementFormat.XY_Float32
                    )
                );
            }
            if (vertex.SecondaryColor is not null)
            {
                this.Elements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.SecondaryColor,
                        MapGeometryVertexElementFormat.BGRA_Packed8888
                    )
                );
            }

            this.DescriptionFlags = GenerateDescriptionFlags(this.Elements.Select(elem => elem.Name));
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Usage);
            bw.Write(this.Elements.Count);

            foreach (MapGeometryVertexElement vertexElement in this.Elements)
            {
                vertexElement.Write(bw);
            }

            for (int i = 0; i < 15 - this.Elements.Count; i++)
            {
                new MapGeometryVertexElement(
                    MapGeometryVertexElementName.Position,
                    MapGeometryVertexElementFormat.XYZW_Float32
                ).Write(bw);
            }
        }

        public int GetVertexSize()
        {
            int size = 0;

            foreach (MapGeometryVertexElement vertexElement in this.Elements)
            {
                size += vertexElement.GetElementSize();
            }

            return size;
        }

        public static VertexElementGroupDescriptionFlags GenerateDescriptionFlags(
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
            if (this.Elements.Count == other.Elements.Count)
            {
                // If Vertex Element count is the same, compare them
                return Enumerable.SequenceEqual(this.Elements, other.Elements);
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
        Dynamic,

        /// <summary>
        /// Streaming Vertex Data (changed every frame)
        /// </summary>
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
