using System;
using System.Diagnostics;
using System.IO;

namespace LeagueToolkit.Core.Memory
{
    /// <summary>
    /// Describes an element of a vertex stored in a vertex buffer<br></br>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/direct3d11/overviews-direct3d-11-resources-buffers-vertex-how-to">
    /// D3D11 - How to: Create a Vertex Buffer
    /// </seealso>
    /// </summary>
    // Riot::X3D::VERTEXELEMENT
    [DebuggerDisplay("{Name} - {Format}")]
    public readonly struct VertexElement : IEquatable<VertexElement>
    {
        public static readonly VertexElement POSITION = new(ElementName.Position, ElementFormat.XYZ_Float32);
        public static readonly VertexElement BLEND_WEIGHT = new(ElementName.BlendWeight, ElementFormat.XYZW_Float32);
        public static readonly VertexElement NORMAL = new(ElementName.Normal, ElementFormat.XYZ_Float32);
        public static readonly VertexElement FOG_COORDINATE = new(ElementName.FogCoordinate, ElementFormat.X_Float32);
        public static readonly VertexElement PRIMARY_COLOR =
            new(ElementName.PrimaryColor, ElementFormat.BGRA_Packed8888);
        public static readonly VertexElement SECONDARY_COLOR =
            new(ElementName.SecondaryColor, ElementFormat.BGRA_Packed8888);
        public static readonly VertexElement BLEND_INDEX = new(ElementName.BlendIndex, ElementFormat.XYZW_Packed8888);
        public static readonly VertexElement TEXCOORD_0 = new(ElementName.Texcoord0, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_1 = new(ElementName.Texcoord1, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_2 = new(ElementName.Texcoord2, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_3 = new(ElementName.Texcoord3, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_4 = new(ElementName.Texcoord4, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_5 = new(ElementName.Texcoord5, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_6 = new(ElementName.Texcoord6, ElementFormat.XY_Float32);
        public static readonly VertexElement TEXCOORD_7 = new(ElementName.Texcoord7, ElementFormat.XY_Float32);
        public static readonly VertexElement TANGENT = new(ElementName.Tangent, ElementFormat.XYZW_Float32);

        /// <summary> The semantic name of the <see cref="VertexElement"/> </summary>
        public ElementName Name { get; }

        /// <summary> The format of the <see cref="VertexElement"/> </summary>
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

        /// <summary>Gets the size of the <see cref="VertexElement"/> value</summary>
        /// <returns>The size of the <see cref="VertexElement"/> value</returns>
        public int GetSize() => GetFormatSize(this.Format);

        /// <summary>Gets the size of the specified <see cref="ElementFormat"/></summary>
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
                ElementFormat.XY_Packed1616 => 4,
                ElementFormat.XYZ_Packed161616 => 8,
                ElementFormat.XYZW_Packed16161616 => 8,
                ElementFormat.XY_Packed88 => 2,
                ElementFormat.XYZ_Packed888 => 3,
                ElementFormat.XYZW_Packed8888 => 4,
                _ => throw new NotImplementedException($"Unsupported {nameof(VertexElement)} format: {format}")
            };
        }

        #region Equals implementation
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
        #endregion
    }

    /// <summary>
    /// The values of <see cref="ElementName"/> are used as Stream ID's for the renderer. <br></br>
    /// Refer to the <seealso href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics#vertex-shader-semantics">
    /// DirectX/HLSL </seealso>
    /// or <seealso href="https://www.khronos.org/opengl/wiki/Vertex_Specification">
    /// OpenGL </seealso>
    /// documentation
    /// </summary>
    // Riot::Renderer::Mesh::Elem
    public enum ElementName : int
    {
        Position, // StreamIndex -> 0
        BlendWeight, // StreamIndex -> 1
        Normal, // StreamIndex -> 2
        FogCoordinate, // StreamIndex -> 5
        PrimaryColor, // StreamIndex -> 3
        SecondaryColor, // StreamIndex -> 4
        BlendIndex, // StreamIndex -> 7
        Texcoord0, // StreamIndex -> 8
        Texcoord1, // StreamIndex -> 9
        Texcoord2, // StreamIndex -> 10
        Texcoord3, // StreamIndex -> 11
        Texcoord4, // StreamIndex -> 12
        Texcoord5, // StreamIndex -> 13
        Texcoord6, // StreamIndex -> 14
        Texcoord7, // StreamIndex -> 15

        Tangent // Riot's enum doesn't have this so not sure how they map it
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
        XY_Packed1616,
        XYZ_Packed161616,
        XYZW_Packed16161616,

        // These are placeholders
        XY_Packed88,
        XYZ_Packed888,
        XYZW_Packed8888
    }
}
