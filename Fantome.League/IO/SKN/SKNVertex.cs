using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.SKN
{
    public class SKNVertex
    {
        public Vector3 Position { get; private set; }
        public Vector4Byte BoneIndices { get; private set; }
        public Vector4 Weights { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector2 UV { get; private set; }
        public Vector4Byte Tangent { get; private set; }

        public SKNVertex(Vector3 position, Vector4Byte boneIndices, Vector4 weights, Vector3 normal, Vector2 uv)
        {
            this.Position = Position;
            this.BoneIndices = boneIndices;
            this.Weights = weights;
            this.Normal = normal;
            this.UV = uv;
        }

        public SKNVertex(Vector3 position, Vector4Byte boneIndices, Vector4 weights, Vector3 normal, Vector2 uv, Vector4Byte tangent)
        {
            this.Position = Position;
            this.BoneIndices = boneIndices;
            this.Weights = weights;
            this.Normal = normal;
            this.UV = uv;
            this.Tangent = tangent;
        }

        public SKNVertex(BinaryReader br, bool isTangent)
        {
            this.Position = new Vector3(br);
            this.BoneIndices = new Vector4Byte(br);
            this.Weights = new Vector4(br);
            this.Normal = new Vector3(br);
            this.UV = new Vector2(br);
            if (isTangent)
            {
                this.Tangent = new Vector4Byte(br);
            }
        }

        public void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.BoneIndices.Write(bw);
            this.Weights.Write(bw);
            this.Normal.Write(bw);
            this.UV.Write(bw);
            if (this.Tangent != null)
            {
                this.Tangent.Write(bw);
            }
        }
    }
}
