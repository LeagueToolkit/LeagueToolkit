using System;
using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOVertexElement : IEquatable<MGEOVertexElement>
    {
        public MGEOVertexElementName Name { get; set; }
        public MGEOVertexElementFormat Format { get; set; }

        public MGEOVertexElement(MGEOVertexElementName name, MGEOVertexElementFormat format)
        {
            this.Name = name;
            this.Format = format;
        }

        public MGEOVertexElement(BinaryReader br)
        {
            this.Name = (MGEOVertexElementName)br.ReadUInt32();
            this.Format = (MGEOVertexElementFormat)br.ReadUInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Name);
            bw.Write((uint)this.Format);
        }

        public uint GetElementSize()
        {
            uint size = 0;

            if(this.Format == MGEOVertexElementFormat.XYZ_Float32)
            {
                size = 12;
            }
            else if(this.Format == MGEOVertexElementFormat.XY_Float32)
            {
                size = 8;
            }

            return size;
        }

        public bool Equals(MGEOVertexElement other)
        {
            return (this.Name == other.Name) && (this.Format == other.Format);
        }
    }

    public enum MGEOVertexElementName : uint
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

    public enum MGEOVertexElementFormat : uint
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
