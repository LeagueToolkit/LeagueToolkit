using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.SimpleSkinFile
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
            this.Position = br.ReadVector3();
            this.BoneIndices = new byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() };
            this.Weights = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            this.Normal = br.ReadVector3();
            this.UV = br.ReadVector2();

            if (vertexType == SimpleSkinVertexType.Color)
            {
                this.Color = br.ReadColor(ColorFormat.RgbaU8);
            }
        }

        public void Write(BinaryWriter bw, SimpleSkinVertexType vertexType)
        {
            bw.WriteVector3(this.Position);

            for (int i = 0; i < 4; i++)
            {
                bw.Write(this.BoneIndices[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                bw.Write(this.Weights[i]);
            }

            bw.WriteVector3(this.Normal);
            bw.WriteVector2(this.UV);

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
