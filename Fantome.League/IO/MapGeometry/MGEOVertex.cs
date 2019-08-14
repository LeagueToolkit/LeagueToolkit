using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOVertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 DiffuseUV { get; set; }
        public Vector2 LightmapUV { get; set; }

        public MGEOVertex() { }

        public MGEOVertex(BinaryReader br, List<MGEOVertexElement> elements)
        {
            foreach(MGEOVertexElement element in elements)
            {
                if(element.Name == MGEOVertexElementName.Position)
                {
                    this.Position = new Vector3(br);
                }
                else if(element.Name == MGEOVertexElementName.Normal)
                {
                    this.Normal = new Vector3(br);
                }
                else if (element.Name == MGEOVertexElementName.DiffuseUV)
                {
                    this.DiffuseUV = new Vector2(br);
                }
                else if(element.Name == MGEOVertexElementName.LightmapUV)
                {
                    this.LightmapUV = new Vector2(br);
                }
                else
                {
                    throw new Exception("Unknown Element Type: " + element.Name);
                }
            }
        }

        internal float[] ToFloatArray(uint vertexSize)
        {
            float[] array = new float[vertexSize / 4];
            int currentPosition = 0;

            if(this.Position != null)
            {
                array[currentPosition++] = this.Position.X;
                array[currentPosition++] = this.Position.Y;
                array[currentPosition++] = this.Position.Z;
            }
            if(this.Normal != null)
            {
                array[currentPosition++] = this.Normal.X;
                array[currentPosition++] = this.Normal.Y;
                array[currentPosition++] = this.Normal.Z;
            }
            if (this.DiffuseUV != null)
            {
                array[currentPosition++] = this.DiffuseUV.X;
                array[currentPosition++] = this.DiffuseUV.Y;
            }
            if (this.LightmapUV != null)
            {
                array[currentPosition++] = this.LightmapUV.X;
                array[currentPosition++] = this.LightmapUV.Y;
            }

            return array;
        }

        public static MGEOVertex Combine(MGEOVertex a, MGEOVertex b)
        {
            MGEOVertex vertex = new MGEOVertex();

            vertex.Position = (a.Position == null && b.Position != null) ? b.Position : a.Position;
            vertex.Normal = (a.Normal == null && b.Normal != null) ? b.Normal : a.Normal;
            vertex.DiffuseUV = (a.DiffuseUV == null && b.DiffuseUV != null) ? b.DiffuseUV : a.DiffuseUV;
            vertex.LightmapUV = (a.LightmapUV == null && b.LightmapUV != null) ? b.LightmapUV : a.LightmapUV;

            return vertex;
        }

        public void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.Normal.Write(bw);
            this.DiffuseUV.Write(bw);
            this.LightmapUV.Write(bw);
        }
    }
}
