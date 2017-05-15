using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fantome.League.Helpers.Structures;

namespace Fantome.League.IO.NVR
{
    public class NVRVertex
    {
        public Vector3 Position { get; private set; }
        public virtual int Size { get; protected set; } = 12;
        public NVRVertexType Type { get; protected set; } = NVRVertexType.NVRVERTEX;

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
    }

    public class NVRVertex4 : NVRVertex
    {
        public Vector3 Normal { get; private set; }
        public Vector2 UV { get; private set; }
        public ColorVector4Byte DiffuseColor { get; private set; }
        public override int Size { get; protected set; } = 36;

        public NVRVertex4(BinaryReader br) : base(br)
        {
            base.Type = NVRVertexType.NVRVERTEX_4;
            this.Normal = new Vector3(br);
            this.UV = new Vector2(br);
            this.DiffuseColor = new ColorVector4Byte(br);
        }

        public NVRVertex4(Vector3 position, Vector3 normal, Vector2 UV, ColorVector4Byte diffuseColor) : base(position)
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
    }

    public class NVRVertex8 : NVRVertex
    {
        public Vector3 Normal { get; private set; }
        public Vector2 UV { get; private set; }
        public ColorVector4Byte DiffuseColor { get; private set; }
        public ColorVector4Byte EmissiveColor { get; private set; }
        public override int Size { get; protected set; } = 40;

        public NVRVertex8(BinaryReader br) : base(br)
        {
            base.Type = NVRVertexType.NVRVERTEX_8;
            this.Normal = new Vector3(br);
            this.UV = new Vector2(br);
            this.DiffuseColor = new ColorVector4Byte(br);
            this.EmissiveColor = new ColorVector4Byte(br);
        }

        public NVRVertex8(Vector3 position, Vector3 normal, Vector2 UV, ColorVector4Byte diffuseColor, ColorVector4Byte emissiveColor) : base(position)
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
    }

    public class NVRVertexGround8 : NVRVertex
    {
        public Vector3 Normal { get; private set; }
        public Vector2 Unknown { get; private set; }
        public ColorVector4Byte DiffuseColor { get; private set; }
        public ColorVector4Byte EmissiveColor { get; private set; }
        public override int Size { get; protected set; } = 40;

        public NVRVertexGround8(BinaryReader br) : base(br)
        {
            base.Type = NVRVertexType.NVRVERTEX_GROUND_8;
            this.Normal = new Vector3(br);
            this.Unknown = new Vector2(br);
            this.DiffuseColor = new ColorVector4Byte(br);
            this.EmissiveColor = new ColorVector4Byte(br);
        }

        public NVRVertexGround8(Vector3 position, Vector3 normal, Vector2 unknown, ColorVector4Byte diffuseColor, ColorVector4Byte emissiveColor) : base(position)
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
    }

    public class NVRVertex12 : NVRVertex
    {
        public Vector3 Normal { get; private set; }
        public Vector2 Unknown { get; private set; }
        public Vector2 UV { get; private set; }
        public ColorVector4Byte DiffuseColor { get; private set; }
        public override int Size { get; protected set; } = 44;

        public NVRVertex12(BinaryReader br) : base(br)
        {
            base.Type = NVRVertexType.NVRVERTEX_12;
            this.Normal = new Vector3(br);
            this.Unknown = new Vector2(br);
            this.UV = new Vector2(br);
            this.DiffuseColor = new ColorVector4Byte(br);
        }

        public NVRVertex12(Vector3 position, Vector3 normal, Vector2 unknown, Vector2 UV, ColorVector4Byte diffuseColor) : base(position)
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
