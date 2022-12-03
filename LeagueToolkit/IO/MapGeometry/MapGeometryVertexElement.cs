using System;
using System.Diagnostics;
using System.IO;

namespace LeagueToolkit.IO.MapGeometry
{
    [DebuggerDisplay("{Name} - {Format}")]
    public class MapGeometryVertexElement : IEquatable<MapGeometryVertexElement>
    {
        public MapGeometryVertexElementName Name { get; set; }
        public MapGeometryVertexElementFormat Format { get; set; }

        public MapGeometryVertexElement(MapGeometryVertexElementName name, MapGeometryVertexElementFormat format)
        {
            this.Name = name;
            this.Format = format;
        }

        public MapGeometryVertexElement(BinaryReader br)
        {
            this.Name = (MapGeometryVertexElementName)br.ReadUInt32();
            this.Format = (MapGeometryVertexElementFormat)br.ReadUInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Name);
            bw.Write((uint)this.Format);
        }

        public int GetElementSize()
        {
            int size = 0;

            if (this.Format == MapGeometryVertexElementFormat.XYZ_Float32)
            {
                size = 12;
            }
            else if (this.Format == MapGeometryVertexElementFormat.XY_Float32)
            {
                size = 8;
            }
            else if (this.Format == MapGeometryVertexElementFormat.BGRA_Packed8888)
            {
                size = 4;
            }
            else throw new InvalidOperationException($"unsupported {nameof(MapGeometryVertexElement)} format: {this.Format}");

            return size;
        }

        public bool Equals(MapGeometryVertexElement other) => (this.Name == other.Name) && (this.Format == other.Format);
    }

    public enum MapGeometryVertexElementName : uint
    {
        Position,
        BlendWeight,
        Normal,
        PrimaryColor,
        SecondaryColor,
        FogCoordinate,
        BlendIndex,
        DiffuseUV,
        Texcoord1,
        Texcoord2,
        Texcoord3,
        Texcoord4,
        Texcoord5,
        Texcoord6,
        LightmapUV,
        StreamIndexCount
    }

    public enum MapGeometryVertexElementFormat : uint
    {
        X_Float32,
        XY_Float32,
        XYZ_Float32,
        XYZW_Float32,
        BGRA_Packed8888,
        ZYXW_Packed8888,
        RGBA_Packed8888,
        XYZW_Packed8888
    }
}
