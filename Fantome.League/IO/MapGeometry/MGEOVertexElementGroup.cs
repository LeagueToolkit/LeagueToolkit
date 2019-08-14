using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOVertexElementGroup : IEquatable<MGEOVertexElementGroup>
    {
        public MGEOVertexElementGroupUsage Usage { get; private set; }
        public List<MGEOVertexElement> VertexElements { get; private set; } = new List<MGEOVertexElement>();

        public MGEOVertexElementGroup(BinaryReader br)
        {
            this.Usage = (MGEOVertexElementGroupUsage)br.ReadUInt32();

            uint vertexElementCount = br.ReadUInt32();
            for(int i = 0; i < vertexElementCount; i++)
            {
                this.VertexElements.Add(new MGEOVertexElement(br));
            }

            br.BaseStream.Seek(8 * (15 - vertexElementCount), SeekOrigin.Current);
        }

        public MGEOVertexElementGroup(MGEOVertex vertex)
        {
            this.Usage = MGEOVertexElementGroupUsage.Static;

            if(vertex.Position != null)
            {
                this.VertexElements.Add(new MGEOVertexElement(MGEOVertexElementName.Position, MGEOVertexElementFormat.XYZ_Float32));
            }
            if (vertex.Normal != null)
            {
                this.VertexElements.Add(new MGEOVertexElement(MGEOVertexElementName.Normal, MGEOVertexElementFormat.XYZ_Float32));
            }
            if (vertex.DiffuseUV != null)
            {
                this.VertexElements.Add(new MGEOVertexElement(MGEOVertexElementName.DiffuseUV, MGEOVertexElementFormat.XY_Float32));
            }
            if (vertex.LightmapUV != null)
            {
                this.VertexElements.Add(new MGEOVertexElement(MGEOVertexElementName.LightmapUV, MGEOVertexElementFormat.XY_Float32));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Usage);
            bw.Write(this.VertexElements.Count);

            foreach(MGEOVertexElement vertexElement in this.VertexElements)
            {
                vertexElement.Write(bw);
            }

            for(int i = 0; i < 15 - this.VertexElements.Count; i++)
            {
                new MGEOVertexElement(MGEOVertexElementName.Position, MGEOVertexElementFormat.XYZW_Float32).Write(bw);
            }
        }

        public uint GetVertexSize()
        {
            uint size = 0;

            foreach(MGEOVertexElement vertexElement in this.VertexElements)
            {
                size += vertexElement.GetElementSize();
            }

            return size;
        }

        public bool Equals(MGEOVertexElementGroup other)
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

    public enum MGEOVertexElementGroupUsage : uint
    {
        Static,
        Dynamic,
        Stream
    }
}
