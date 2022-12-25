using CommunityToolkit.Diagnostics;
using System;
using System.Diagnostics;
using System.IO;

namespace LeagueToolkit.Core.Renderer
{
    /// <summary>
    /// Describes a vertex element
    /// </summary>
    //https://learn.microsoft.com/en-us/windows/win32/api/d3d11/ns-d3d11-d3d11_input_element_desc
    [DebuggerDisplay("{Name} - {Format}")]
    public struct VertexElement : IEquatable<VertexElement>
    {
        public VertexElementName Name { get; private set; }
        public VertexElementFormat Format { get; private set; }

        public VertexElement(VertexElementName name, VertexElementFormat format)
        {
            this.Name = name;
            this.Format = format;
        }

        internal VertexElement(BinaryReader br)
        {
            this.Name = (VertexElementName)br.ReadUInt32();
            this.Format = (VertexElementFormat)br.ReadUInt32();
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Name);
            bw.Write((uint)this.Format);
        }

        public int GetElementSize() => GetFormatSize(this.Format);

        public static int GetFormatSize(VertexElementFormat format)
        {
            return format switch
            {
                VertexElementFormat.X_Float32 => 4,
                VertexElementFormat.XY_Float32 => 8,
                VertexElementFormat.XYZ_Float32 => 12,
                VertexElementFormat.XYZW_Float32 => 16,
                VertexElementFormat.BGRA_Packed8888 => 4,
                VertexElementFormat.ZYXW_Packed8888 => 4,
                VertexElementFormat.RGBA_Packed8888 => 4,
                VertexElementFormat.XYZW_Packed8888 => 4,
                _ => throw new NotImplementedException($"Unsupported {nameof(VertexElement)} format: {format}")
            };
        }

        public bool Equals(VertexElement other) => (this.Name == other.Name) && (this.Format == other.Format);
    }

    // The values of this enum are used as stream indices for the renderer
    // https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics#vertex-shader-semantics
    public enum VertexElementName : int
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
        LightmapUV
    }

    public enum VertexElementFormat : uint
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
