using System;
using System.Collections.Generic;
using System.IO;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometryVertexElementGroup : IEquatable<MapGeometryVertexElementGroup>
    {
        public MapGeometryVertexElementGroupUsage Usage { get; private set; }
        public List<MapGeometryVertexElement> VertexElements { get; private set; } = new List<MapGeometryVertexElement>();

        public MapGeometryVertexElementGroup(BinaryReader br)
        {
            this.Usage = (MapGeometryVertexElementGroupUsage)br.ReadUInt32();

            uint vertexElementCount = br.ReadUInt32();
            for(int i = 0; i < vertexElementCount; i++)
            {
                this.VertexElements.Add(new MapGeometryVertexElement(br));
            }

            br.BaseStream.Seek(8 * (15 - vertexElementCount), SeekOrigin.Current);
        }

        public MapGeometryVertexElementGroup(MapGeometryVertex vertex)
        {
            this.Usage = MapGeometryVertexElementGroupUsage.Static;

            if(vertex.Position != null)
            {
                this.VertexElements.Add(new MapGeometryVertexElement(MapGeometryVertexElementName.Position, MapGeometryVertexElementFormat.XYZ_Float32));
            }
            if (vertex.Normal != null)
            {
                this.VertexElements.Add(new MapGeometryVertexElement(MapGeometryVertexElementName.Normal, MapGeometryVertexElementFormat.XYZ_Float32));
            }
            if (vertex.DiffuseUV != null)
            {
                this.VertexElements.Add(new MapGeometryVertexElement(MapGeometryVertexElementName.DiffuseUV, MapGeometryVertexElementFormat.XY_Float32));
            }
            if (vertex.LightmapUV != null)
            {
                this.VertexElements.Add(new MapGeometryVertexElement(MapGeometryVertexElementName.LightmapUV, MapGeometryVertexElementFormat.XY_Float32));
            }
            if (vertex.SecondaryColor != null)
            {
                this.VertexElements.Add(new MapGeometryVertexElement(MapGeometryVertexElementName.SecondaryColor, MapGeometryVertexElementFormat.BGRA_Packed8888));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Usage);
            bw.Write(this.VertexElements.Count);

            foreach(MapGeometryVertexElement vertexElement in this.VertexElements)
            {
                vertexElement.Write(bw);
            }

            for(int i = 0; i < 15 - this.VertexElements.Count; i++)
            {
                new MapGeometryVertexElement(MapGeometryVertexElementName.Position, MapGeometryVertexElementFormat.XYZW_Float32).Write(bw);
            }
        }

        public int GetVertexSize()
        {
            int size = 0;

            foreach(MapGeometryVertexElement vertexElement in this.VertexElements)
            {
                size += vertexElement.GetElementSize();
            }

            return size;
        }

        public bool Equals(MapGeometryVertexElementGroup other)
        {
            bool result = false;

            if (this.Usage != other.Usage)
            {
                return false;
            }

            if (this.VertexElements.Count == other.VertexElements.Count)
            {
                for (int i = 0; i < this.VertexElements.Count; i++)
                {
                    result = this.VertexElements[i].Equals(other.VertexElements[i]);
                }
            }
            else
            {
                return false;
            }


            return result;
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
