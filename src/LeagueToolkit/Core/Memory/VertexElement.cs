using System;
using System.Diagnostics;
using System.IO;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Describes an element of a vertex stored in <see cref="VertexBuffer"/>
    /// <br></br>
    /// Please refer to <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/ns-d3d11-d3d11_input_element_desc">
    /// the D3D11 description of an element</seealso> for more info
    /// </summary>
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

        /// <value>
        /// The <seealso href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics">
        /// semantic name</seealso> of this element
        /// </value>
        public ElementName Name { get; }

        /// <summary>
        /// The format of this element's value
        /// </summary>
        public ElementFormat Format { get; }

        /// <summary>
        /// Creates a new <see cref="VertexElement"/> with the specified <paramref name="name"/> and <paramref name="format"/>
        /// </summary>
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

        /// <returns>The size of an element's value</returns>
        public int GetSize() => GetFormatSize(this.Format);

        /// <returns>The size of <paramref name="format"/></returns>
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

        public static bool operator ==(VertexElement left, VertexElement right) => left.Equals(right);

        public static bool operator !=(VertexElement left, VertexElement right) => !left.Equals(right);

        public bool Equals(VertexElement other) => this.Name == other.Name && this.Format == other.Format;

        public override bool Equals(object obj) =>
            obj switch
            {
                VertexElement other => Equals(other),
                _ => false
            };

        public override int GetHashCode() => HashCode.Combine(this.Name, this.Format);
    }

    /// <summary>
    /// The values of <see cref="ElementName"/> are used as Stream ID's for the renderer. <br></br>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics#vertex-shader-semantics">
    /// D3D HLSL Vertex Shader Semantics
    /// </seealso>
    /// </summary>
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
