using Fantome.Libraries.League.Helpers.Extensions;
using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.SimpleSkin
{
    public class SimpleSkinVertex
    {
        public Vector3 Position { get; set; }
        public byte[] BoneIndices { get; set; }
        public float[] Weights { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 UV { get; set; }
        public Color? Color { get; set; }

        public SimpleSkinVertex(Vector3 position, byte[] boneIndices, float[] weights, Vector3 normal, Vector2 uv)
        {
            this.Position = position;
            this.BoneIndices = boneIndices;
            this.Weights = weights;
            this.Normal = normal;
            this.UV = uv;
        }
        public SimpleSkinVertex(Vector3 position, byte[] boneIndices, float[] weights, Vector3 normal, Vector2 uv, Color color)
        {
            this.Position = position;
            this.BoneIndices = boneIndices;
            this.Weights = weights;
            this.Normal = normal;
            this.UV = uv;
            this.Color = color;
        }
        public SimpleSkinVertex(BinaryReader br, SimpleSkinVertexType vertexType)
        {
            this.Position = new Vector3(br);
            this.BoneIndices = new byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() };
            this.Weights = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            this.Normal = new Vector3(br);
            this.UV = new Vector2(br);

            if (vertexType == SimpleSkinVertexType.Color)
            {
                this.Color = br.ReadColor(ColorFormat.RgbaU8);
            }
        }

        public void Write(BinaryWriter bw, SimpleSkinVertexType vertexType)
        {
            this.Position.Write(bw);

            for (int i = 0; i < 4; i++)
            {
                bw.Write(this.BoneIndices[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                bw.Write(this.Weights[i]);
            }

            this.Normal.Write(bw);
            this.UV.Write(bw);

            if (vertexType == SimpleSkinVertexType.Color)
            {
                if (this.Color.HasValue)
                {
                    bw.WriteColor(this.Color.Value, ColorFormat.RgbaU8);
                }
                else
                {
                    bw.WriteColor(new Color(0, 0, 0, 255), ColorFormat.RgbaU8);
                }

            }
        }
    }

    public enum SimpleSkinVertexType : uint
    {
        Basic,
        Color
    }
}
