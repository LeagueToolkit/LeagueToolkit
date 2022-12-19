using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using LeagueToolkit.Helpers;
using LeagueToolkit.Helpers.Extensions;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometryVertex
    {
        public Vector3? Position { get; set; }
        public Vector3? Normal { get; set; }
        public Vector2? DiffuseUV { get; set; }
        public Vector2? LightmapUV { get; set; }
        public Color? SecondaryColor { get; set; }

        public MapGeometryVertex() { }
        public MapGeometryVertex(Vector3 position, Vector3 normal, Vector2 diffuseUV)
        {
            this.Position = position;
            this.Normal = normal;
            this.DiffuseUV = diffuseUV;
        }
        public MapGeometryVertex(Vector3 position, Vector3 normal, Vector2 diffuseUV, Vector2 lightmapUV) : this(position, normal, diffuseUV)
        {
            this.LightmapUV = lightmapUV;
        }
        public MapGeometryVertex(BinaryReader br, List<MapGeometryVertexElement> elements)
        {
            foreach(MapGeometryVertexElement element in elements)
            {
                if(element.Name == MapGeometryVertexElementName.Position)
                {
                    this.Position = br.ReadVector3();
                }
                else if(element.Name == MapGeometryVertexElementName.Normal)
                {
                    this.Normal = br.ReadVector3();
                }
                else if (element.Name == MapGeometryVertexElementName.DiffuseUV)
                {
                    this.DiffuseUV = br.ReadVector2();
                }
                else if(element.Name == MapGeometryVertexElementName.LightmapUV)
                {
                    this.LightmapUV = br.ReadVector2();
                }
                else if(element.Name == MapGeometryVertexElementName.SecondaryColor)
                {
                    this.SecondaryColor = br.ReadColor(ColorFormat.BgraU8);
                }
                else
                {
                    throw new Exception("Unknown Element Type: " + element.Name);
                }
            }
        }

        internal byte[] ToArray(int vertexSize)
        {
            byte[] array = new byte[vertexSize];
            int currentPosition = 0;

            if(this.Position != null)
            {
                Memory.CopyStructureToBuffer(array, currentPosition, this.Position.Value);
                currentPosition += this.Position.Value.RawSize();
            }
            if(this.Normal != null)
            {
                Memory.CopyStructureToBuffer(array, currentPosition, this.Normal.Value);
                currentPosition += this.Normal.Value.RawSize();
            }
            if (this.DiffuseUV != null)
            {
                Memory.CopyStructureToBuffer(array, currentPosition, this.DiffuseUV.Value);
                currentPosition += this.DiffuseUV.Value.RawSize();
            }
            if (this.LightmapUV != null)
            {
                Memory.CopyStructureToBuffer(array, currentPosition, this.LightmapUV.Value);
                currentPosition += this.LightmapUV.Value.RawSize();
            }
            if (this.SecondaryColor != null)
            {
                byte[] colorBuffer = this.SecondaryColor.Value.GetBytes(ColorFormat.BgraU8);
                Buffer.BlockCopy(colorBuffer, 0, array, currentPosition, colorBuffer.Length);
                currentPosition += colorBuffer.Length;
            }

            return array;
        }

        internal int Size() 
        {
            int size = 0;

            if (this.Position != null) { size += this.Position.Value.RawSize(); }
            if (this.Normal != null) { size += this.Normal.Value.RawSize(); }
            if (this.DiffuseUV != null) { size += this.DiffuseUV.Value.RawSize(); }
            if (this.LightmapUV != null) { size += this.LightmapUV.Value.RawSize(); }
            if (this.SecondaryColor != null) { size += Color.FormatSize(ColorFormat.BgraU8); }

            return size;
        }

        public static MapGeometryVertex Combine(MapGeometryVertex a, MapGeometryVertex b)
        {
            return new MapGeometryVertex() 
            {
                Position = (a.Position == null && b.Position != null) ? b.Position : a.Position,
                Normal = (a.Normal == null && b.Normal != null) ? b.Normal : a.Normal,
                DiffuseUV = (a.DiffuseUV == null && b.DiffuseUV != null) ? b.DiffuseUV : a.DiffuseUV,
                LightmapUV = (a.LightmapUV == null && b.LightmapUV != null) ? b.LightmapUV : a.LightmapUV,
                SecondaryColor = (a.SecondaryColor == null && b.SecondaryColor != null) ? b.SecondaryColor : a.SecondaryColor
            };
        }

        public void Write(BinaryWriter bw)
        {
            if (this.Position is Vector3 position) bw.WriteVector3(position);
            if (this.Normal is Vector3 normal) bw.WriteVector3(normal);
            if (this.DiffuseUV is Vector2 diffuseUv) bw.WriteVector2(diffuseUv);
            if (this.LightmapUV is Vector2 lightmapUv) bw.WriteVector2(lightmapUv);
            if (this.SecondaryColor is Color color) bw.WriteColor(color, ColorFormat.BgraU8);
        }
    }
}
