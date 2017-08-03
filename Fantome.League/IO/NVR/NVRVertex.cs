using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.NVR
{
    public class NVRVertex
    {
        public Vector3 Position { get; private set; }
        public const int Size = 12;
        public const NVRVertexType Type = NVRVertexType.NVRVERTEX;

        public NVRVertex(BinaryReader br)
        {
            this.Position = new Vector3(br);
        }

        public NVRVertex(Vector3 position)
        {
            this.Position = position;
        }

        public virtual void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
        }

        public virtual NVRVertexType GetVertexType()
        {
            return Type;
        }

        public virtual int GetSize()
        {
            return Size;
        }

        private static bool ContainsGroundKeyword(string texture)
        {
            return texture.Contains("_floor") || texture.Contains("_dirt") || texture.Contains("grass") || texture.Contains("RiverBed") || texture.Contains("_project") || texture.Contains("tile_");
        }

        public static NVRVertexType GetVertexTypeFromMaterial(NVRMaterial mat)
        {
            if (mat.Type == NVRMaterialType.MATERIAL_TYPE_DEFAULT)
            {
                if (mat.Flags.HasFlag(NVRMaterialFlags.ColoredVertex) || mat.Flags.HasFlag(NVRMaterialFlags.GroundVertex))
                {
                    if (mat.Flags.HasFlag(NVRMaterialFlags.GroundVertex) && ContainsGroundKeyword(mat.Channels[0].Name))
                    {
                        return NVRVertexType.NVRVERTEX_GROUND_8;
                    }
                    else
                    {
                        return NVRVertexType.NVRVERTEX_8;
                    }
                }
                else
                {
                    return NVRVertexType.NVRVERTEX_4;
                }
            }
            else if (mat.Type == NVRMaterialType.MATERIAL_TYPE_FOUR_BLEND)
            {
                return NVRVertexType.NVRVERTEX_12;
            }
            else
            {
                return NVRVertexType.NVRVERTEX_4;
            }
        }
    }

    public class NVRVertex4 : NVRVertex
    {
        public Vector3 Normal { get; set; }
        public Vector2 UV { get; set; }
        public ColorBGRAVector4Byte DiffuseColor { get; set; }
        public new const int Size = 36;
        public new const NVRVertexType Type = NVRVertexType.NVRVERTEX_4;

        public NVRVertex4(BinaryReader br) : base(br)
        {
            this.Normal = new Vector3(br);
            this.UV = new Vector2(br);
            this.DiffuseColor = new ColorBGRAVector4Byte(br);
        }

        public NVRVertex4(Vector3 position) : base(position)
        {
        }

        public NVRVertex4(Vector3 position, Vector3 normal, Vector2 UV, ColorBGRAVector4Byte diffuseColor) : base(position)
        {
            this.Normal = normal;
            this.UV = UV;
            this.DiffuseColor = diffuseColor;
        }

        public override void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.Normal.Write(bw);
            this.UV.Write(bw);
            this.DiffuseColor.Write(bw);
        }

        public override NVRVertexType GetVertexType()
        {
            return Type;
        }

        public override int GetSize()
        {
            return Size;
        }
    }

    public class NVRVertex8 : NVRVertex
    {
        public Vector3 Normal { get; set; }
        public Vector2 UV { get; set; }
        public ColorBGRAVector4Byte DiffuseColor { get; set; }
        public ColorBGRAVector4Byte EmissiveColor { get; set; }
        public new const int Size = 40;
        public new const NVRVertexType Type = NVRVertexType.NVRVERTEX_8;

        public NVRVertex8(BinaryReader br) : base(br)
        {
            this.Normal = new Vector3(br);
            this.UV = new Vector2(br);
            this.DiffuseColor = new ColorBGRAVector4Byte(br);
            this.EmissiveColor = new ColorBGRAVector4Byte(br);
        }

        public NVRVertex8(Vector3 position) : base(position)
        {
        }

        public NVRVertex8(Vector3 position, Vector3 normal, Vector2 UV, ColorBGRAVector4Byte diffuseColor, ColorBGRAVector4Byte emissiveColor) : base(position)
        {
            this.Normal = normal;
            this.UV = UV;
            this.DiffuseColor = diffuseColor;
            this.EmissiveColor = emissiveColor;
        }

        public override void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.Normal.Write(bw);
            this.UV.Write(bw);
            this.DiffuseColor.Write(bw);
            this.EmissiveColor.Write(bw);
        }

        public override NVRVertexType GetVertexType()
        {
            return Type;
        }

        public override int GetSize()
        {
            return Size;
        }
    }

    public class NVRVertexGround8 : NVRVertex
    {
        public Vector3 Normal { get; set; }
        public Vector2 Unknown { get; set; }
        public ColorBGRAVector4Byte DiffuseColor { get; set; }
        public ColorBGRAVector4Byte EmissiveColor { get; set; }
        public new const int Size = 40;
        public new const NVRVertexType Type = NVRVertexType.NVRVERTEX_GROUND_8;

        public NVRVertexGround8(BinaryReader br) : base(br)
        {
            this.Normal = new Vector3(br);
            this.Unknown = new Vector2(br);
            this.DiffuseColor = new ColorBGRAVector4Byte(br);
            this.EmissiveColor = new ColorBGRAVector4Byte(br);
        }

        public NVRVertexGround8(Vector3 position, Vector3 normal, Vector2 unknown, ColorBGRAVector4Byte diffuseColor, ColorBGRAVector4Byte emissiveColor) : base(position)
        {
            this.Normal = normal;
            this.Unknown = unknown;
            this.DiffuseColor = diffuseColor;
            this.EmissiveColor = emissiveColor;
        }

        public override void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.Normal.Write(bw);
            this.Unknown.Write(bw);
            this.DiffuseColor.Write(bw);
            this.EmissiveColor.Write(bw);
        }

        public override NVRVertexType GetVertexType()
        {
            return Type;
        }

        public override int GetSize()
        {
            return Size;
        }
    }

    public class NVRVertex12 : NVRVertex
    {
        public Vector3 Normal { get; set; }
        public Vector2 Unknown { get; set; }
        public Vector2 UV { get; set; }
        public ColorBGRAVector4Byte DiffuseColor { get; set; }
        public new const int Size = 44;
        public new const NVRVertexType Type = NVRVertexType.NVRVERTEX_12;

        public NVRVertex12(BinaryReader br) : base(br)
        {
            this.Normal = new Vector3(br);
            this.Unknown = new Vector2(br);
            this.UV = new Vector2(br);
            this.DiffuseColor = new ColorBGRAVector4Byte(br);
        }

        public NVRVertex12(Vector3 position, Vector3 normal, Vector2 unknown, Vector2 UV, ColorBGRAVector4Byte diffuseColor) : base(position)
        {
            this.Normal = normal;
            this.Unknown = unknown;
            this.UV = UV;
            this.DiffuseColor = diffuseColor;
        }

        public override void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.Normal.Write(bw);
            this.Unknown.Write(bw);
            this.UV.Write(bw);
            this.DiffuseColor.Write(bw);
        }

        public override NVRVertexType GetVertexType()
        {
            return Type;
        }

        public override int GetSize()
        {
            return Size;
        }
    }

    public enum NVRVertexType
    {
        NVRVERTEX,
        NVRVERTEX_4,
        NVRVERTEX_8,
        NVRVERTEX_GROUND_8,
        NVRVERTEX_12
    }
}
