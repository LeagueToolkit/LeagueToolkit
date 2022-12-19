using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.Helpers.Extensions;
using System.Numerics;

namespace LeagueToolkit.IO.NVR
{
    public class NVRVertex
    {
        public Vector3 Position { get; private set; }
        public const int Size = 12;
        public const NVRVertexType Type = NVRVertexType.NVRVERTEX;

        public NVRVertex(BinaryReader br)
        {
            this.Position = br.ReadVector3();
        }

        public NVRVertex(Vector3 position)
        {
            this.Position = position;
        }

        public virtual void Write(BinaryWriter bw)
        {
            bw.WriteVector3(this.Position);
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

        public static bool IsGroundType(NVRMaterial mat)
        {
            return mat.Flags.HasFlag(NVRMaterialFlags.GroundVertex) && ContainsGroundKeyword(mat.Channels[0].Name);
        }

        public static NVRVertexType GetVertexTypeFromMaterial(NVRMaterial mat)
        {
            if (mat.Type == NVRMaterialType.MATERIAL_TYPE_FOUR_BLEND)
            {
                return NVRVertexType.NVRVERTEX_12;
            }
            else if (mat.Type == NVRMaterialType.MATERIAL_TYPE_DEFAULT && mat.Flags.HasFlag(NVRMaterialFlags.ColoredVertex))
            {
                return NVRVertexType.NVRVERTEX_8;
            }
            return NVRVertexType.NVRVERTEX_4;
        }
    }

    public class NVRVertex4 : NVRVertex
    {
        public Vector3 Normal { get; set; }
        public Vector2 UV { get; set; }
        public Color DiffuseColor { get; set; }
        public new const int Size = 36;
        public new const NVRVertexType Type = NVRVertexType.NVRVERTEX_4;

        public NVRVertex4(BinaryReader br) : base(br)
        {
            this.Normal = br.ReadVector3();
            this.UV = br.ReadVector2();
            this.DiffuseColor = br.ReadColor(ColorFormat.BgraU8);
        }

        public NVRVertex4(Vector3 position) : base(position)
        {
        }

        public NVRVertex4(Vector3 position, Vector3 normal, Vector2 UV, Color diffuseColor) : base(position)
        {
            this.Normal = normal;
            this.UV = UV;
            this.DiffuseColor = diffuseColor;
        }

        public override void Write(BinaryWriter bw)
        {
            bw.WriteVector3(this.Position);
            bw.WriteVector3(this.Normal);
            bw.WriteVector2(this.UV);

            bw.WriteColor(this.DiffuseColor, ColorFormat.BgraU8);
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
        public Color DiffuseColor { get; set; }
        public Color EmissiveColor { get; set; }
        public new const int Size = 40;
        public new const NVRVertexType Type = NVRVertexType.NVRVERTEX_8;

        public NVRVertex8(BinaryReader br) : base(br)
        {
            this.Normal = br.ReadVector3();
            this.UV = br.ReadVector2();
            this.DiffuseColor = br.ReadColor(ColorFormat.BgraU8);
            this.EmissiveColor = br.ReadColor(ColorFormat.BgraU8);
        }

        public NVRVertex8(Vector3 position) : base(position) { }

        public NVRVertex8(Vector3 position, Vector3 normal, Vector2 UV, Color diffuseColor, Color emissiveColor) : base(position)
        {
            this.Normal = normal;
            this.UV = UV;
            this.DiffuseColor = diffuseColor;
            this.EmissiveColor = emissiveColor;
        }

        public override void Write(BinaryWriter bw)
        {
            bw.WriteVector3(this.Position);
            bw.WriteVector3(this.Normal);
            bw.WriteVector2(this.UV);
            bw.WriteColor(this.DiffuseColor, ColorFormat.BgraU8);
            bw.WriteColor(this.EmissiveColor, ColorFormat.BgraU8);
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
        public Color DiffuseColor { get; set; }
        public new const int Size = 44;
        public new const NVRVertexType Type = NVRVertexType.NVRVERTEX_12;

        public NVRVertex12(BinaryReader br) : base(br)
        {
            this.Normal = br.ReadVector3();
            this.Unknown = br.ReadVector2();
            this.UV = br.ReadVector2();
            this.DiffuseColor = br.ReadColor(ColorFormat.BgraU8);
        }

        public NVRVertex12(Vector3 position, Vector3 normal, Vector2 unknown, Vector2 UV, Color diffuseColor) : base(position)
        {
            this.Normal = normal;
            this.Unknown = unknown;
            this.UV = UV;
            this.DiffuseColor = diffuseColor;
        }

        public override void Write(BinaryWriter bw)
        {
            bw.WriteVector3(this.Position);
            bw.WriteVector3(this.Normal);
            bw.WriteVector2(this.Unknown);
            bw.WriteVector2(this.UV);
            bw.WriteColor(this.DiffuseColor, ColorFormat.BgraU8);
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
        NVRVERTEX_12
    }
}
