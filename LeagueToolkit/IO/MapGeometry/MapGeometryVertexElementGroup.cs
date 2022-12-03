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
        public List<MapGeometryVertexElement> VertexElements { get; private set; } = new();

        internal string DebuggerDisplay => string.Join(' ', this.VertexElements.Select(x => x.Name));

        public MapGeometryVertexElementGroup(
            MapGeometryVertexElementGroupUsage usage,
            IEnumerable<MapGeometryVertexElement> vertexElements
        )
        {
            this.Usage = usage;
            this.VertexElements = new(vertexElements);
        }

        public MapGeometryVertexElementGroup(BinaryReader br)
        {
            this.Usage = (MapGeometryVertexElementGroupUsage)br.ReadUInt32();

            uint vertexElementCount = br.ReadUInt32();
            for (int i = 0; i < vertexElementCount; i++)
            {
                this.VertexElements.Add(new MapGeometryVertexElement(br));
            }

            br.BaseStream.Seek(8 * (15 - vertexElementCount), SeekOrigin.Current);
        }

        public MapGeometryVertexElementGroup(MapGeometryVertex vertex)
        {
            this.Usage = MapGeometryVertexElementGroupUsage.Static;

            if (vertex.Position is not null)
            {
                this.VertexElements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.Position,
                        MapGeometryVertexElementFormat.XYZ_Float32
                    )
                );
            }
            if (vertex.Normal is not null)
            {
                this.VertexElements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.Normal,
                        MapGeometryVertexElementFormat.XYZ_Float32
                    )
                );
            }
            if (vertex.DiffuseUV is not null)
            {
                this.VertexElements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.DiffuseUV,
                        MapGeometryVertexElementFormat.XY_Float32
                    )
                );
            }
            if (vertex.LightmapUV is not null)
            {
                this.VertexElements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.LightmapUV,
                        MapGeometryVertexElementFormat.XY_Float32
                    )
                );
            }
            if (vertex.SecondaryColor is not null)
            {
                this.VertexElements.Add(
                    new MapGeometryVertexElement(
                        MapGeometryVertexElementName.SecondaryColor,
                        MapGeometryVertexElementFormat.BGRA_Packed8888
                    )
                );
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Usage);
            bw.Write(this.VertexElements.Count);

            foreach (MapGeometryVertexElement vertexElement in this.VertexElements)
            {
                vertexElement.Write(bw);
            }

            for (int i = 0; i < 15 - this.VertexElements.Count; i++)
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

            foreach (MapGeometryVertexElement vertexElement in this.VertexElements)
            {
                size += vertexElement.GetElementSize();
            }

            return size;
        }

        public bool Equals(MapGeometryVertexElementGroup other)
        {
            // If usage is not the same
            if (this.Usage != other.Usage)
            {
                return false;
            }

            // Check if Vertex Element count is the same
            if (this.VertexElements.Count == other.VertexElements.Count)
            {
                // If Vertex Element count is the same, compare them
                return Enumerable.SequenceEqual(this.VertexElements, other.VertexElements);
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
}
