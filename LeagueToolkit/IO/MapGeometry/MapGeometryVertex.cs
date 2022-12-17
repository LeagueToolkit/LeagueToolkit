using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using LeagueToolkit.Helpers;
using LeagueToolkit.Helpers.Extensions;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometry
{
    // TODO: Convert to struct
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

        public MapGeometryVertex(Vector3 position, Vector3 normal, Vector2 diffuseUV, Vector2 lightmapUV)
            : this(position, normal, diffuseUV)
        {
            this.LightmapUV = lightmapUV;
        }

        internal static void ReadAndCombineElements(
            MapGeometryVertex vertex,
            MapGeometryVertexElementGroup vertexDescription,
            BinaryReader br
        )
        {
            foreach (MapGeometryVertexElement element in vertexDescription.Elements)
            {
                if (element.Name == MapGeometryVertexElementName.Position)
                {
                    vertex.Position = br.ReadVector3();
                }
                else if (element.Name == MapGeometryVertexElementName.Normal)
                {
                    vertex.Normal = br.ReadVector3();
                }
                else if (element.Name == MapGeometryVertexElementName.DiffuseUV)
                {
                    vertex.DiffuseUV = br.ReadVector2();
                }
                else if (element.Name == MapGeometryVertexElementName.LightmapUV)
                {
                    vertex.LightmapUV = br.ReadVector2();
                }
                else if (element.Name == MapGeometryVertexElementName.SecondaryColor)
                {
                    vertex.SecondaryColor = br.ReadColor(ColorFormat.BgraU8);
                }
                else
                {
                    throw new Exception("Unknown Element Type: " + element.Name);
                }
            }
        }

        internal void Write(BinaryWriter bw)
        {
            if (this.Position is Vector3 position)
            {
                bw.WriteVector3(position);
            }
            if (this.Normal is Vector3 normal)
            {
                bw.WriteVector3(normal);
            }
            if (this.DiffuseUV is Vector2 diffuseUv)
            {
                bw.WriteVector2(diffuseUv);
            }
            if (this.LightmapUV is Vector2 lightmapUv)
            {
                bw.WriteVector2(lightmapUv);
            }
            if (this.SecondaryColor is Color secondaryColor)
            {
                bw.WriteColor(secondaryColor, ColorFormat.BgraU8);
            }
        }
    }
}
