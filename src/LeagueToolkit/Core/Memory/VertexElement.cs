using System;
using System.Diagnostics;
using System.IO;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Describes a vertex element
    /// </summary>
    // https://learn.microsoft.com/en-us/windows/win32/api/d3d11/ns-d3d11-d3d11_input_element_desc
    // Riot::X3D::VERTEXELEMENT
    [DebuggerDisplay("{Name} - {Format}")]
    public readonly struct VertexElement : IEquatable<VertexElement>
    {
        public static readonly VertexElement POSITION = new(ElementName.Position, ElementFormat.XYZ_Float32);
        public static readonly VertexElement BLEND_WEIGHT = new(ElementName.BlendWeight, ElementFormat.XYZW_Float32);
        public static readonly VertexElement NORMAL = new(ElementName.Normal, ElementFormat.XYZ_Float32);
        public static readonly VertexElement MAYBE_TANGENT = new(ElementName.MaybeTangent, ElementFormat.XYZW_Float32);
        public static readonly VertexElement BASE_COLOR = new(ElementName.BaseColor, ElementFormat.BGRA_Packed8888);
        public static readonly VertexElement FOG_COORDINATE = new(ElementName.FogCoordinate, ElementFormat.X_Float32);
        public static readonly VertexElement BLEND_INDEX = new(ElementName.BlendIndex, ElementFormat.XYZW_Packed8888);
        public static readonly VertexElement DIFFUSE_UV = new(ElementName.DiffuseUV, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_1 = new(ElementName.Texcoord1, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_2 = new(ElementName.Texcoord2, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_3 = new(ElementName.Texcoord3, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_4 = new(ElementName.Texcoord4, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_5 = new(ElementName.Texcoord5, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_6 = new(ElementName.Texcoord6, ElementFormat.XY_Float32);
        public static readonly VertexElement LIGHTMAP_UV = new(ElementName.LightmapUV, ElementFormat.XY_Float32);

        public ElementName Name { get; }
        public ElementFormat Format { get; }

        public VertexElement(ElementName name, ElementFormat format)
        {
            this.Name = name;
            this.Format = format;
        }

        internal VertexElement(BinaryReader br)
        {
            this.Name = (ElementName)br.ReadUInt32();
            this.Format = (ElementFormat)br.ReadUInt32();
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write((uint)this.Name);
            bw.Write((uint)this.Format);
        }

        public int GetSize() => GetFormatSize(this.Format);

        public static int GetFormatSize(ElementFormat format)
        {
            return format switch
            {
                ElementFormat.X_Float32 => 4,
                ElementFormat.XY_Float32 => 8,
                ElementFormat.XYZ_Float32 => 12,
                ElementFormat.XYZW_Float32 => 16,
                ElementFormat.BGRA_Packed8888 => 4,
                ElementFormat.ZYXW_Packed8888 => 4,
                ElementFormat.RGBA_Packed8888 => 4,
                ElementFormat.XYZW_Packed8888 => 4,
                _ => throw new NotImplementedException($"Unsupported {nameof(VertexElement)} format: {format}")
            };
        }

        public bool Equals(VertexElement other) => this.Name == other.Name && this.Format == other.Format;
    }

    // The values of this enum are used as stream indices for the renderer
    // https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics#vertex-shader-semantics
    // Riot::Renderer::Mesh::Elem
    public enum ElementName : int
    {
        Position,
        BlendWeight,
        Normal,
        MaybeTangent,
        BaseColor,
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

    // Riot::Renderer::Mesh::ElemFormat
    public enum ElementFormat : uint
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
