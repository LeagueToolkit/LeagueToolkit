using Fantome.League.Helpers.Structures;
using System.Diagnostics;
using System.IO;

namespace Fantome.League.IO.SKN
{
    [DebuggerDisplay("[ {Position.X}, {Position.Y}, {Position.Z} ]")]
    public class SKNVertex
    {
        private bool IsTangent { get; set; }
        public Vector3 Position { get; private set; }
        public Vector4Byte BoneIndices { get; private set; }
        public Vector4 Weights { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector2 UV { get; private set; }
        public Vector4Byte Tangent { get; private set; }

        public SKNVertex(Vector3 Position)
        {
            this.IsTangent = false;
            this.Position = Position;
        }

        public SKNVertex(BinaryReader br, bool IsTangent)
        {
            this.Position = new Vector3(br);
            this.BoneIndices = new Vector4Byte(br);
            this.Weights = new Vector4(br);
            this.Normal = new Vector3(br);
            this.UV = new Vector2(br);
            if (IsTangent)
            {
                this.Tangent = new Vector4Byte(br);
                this.IsTangent = IsTangent;
            }
        }

        public void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.BoneIndices.Write(bw);
            this.Weights.Write(bw);
            this.Normal.Write(bw);
            this.UV.Write(bw);
            if (this.IsTangent)
                this.Tangent.Write(bw);
        }

        public void SetWeight(Vector4Byte BoneIndices, Vector4 Weights)
        {
            this.BoneIndices = BoneIndices;
            this.Weights = Weights;
        }

        public void SetNormal(Vector3 Normal)
        {
            this.Normal = Normal;
        }

        public void SetUV(Vector2 UV)
        {
            this.UV = UV;
        }
    }
}
