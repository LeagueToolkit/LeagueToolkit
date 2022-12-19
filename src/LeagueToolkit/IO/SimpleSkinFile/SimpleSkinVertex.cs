using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.SimpleSkinFile
{
    // TODO: This shouldn't be a class
    public class SimpleSkinVertex
    {
        public Vector3 Position { get; set; }
        public byte[] BoneIndices { get; set; }
        public float[] Weights { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 UV { get; set; }
        // TODO: Verify whether BaseColor is in RGBA or BGRA format
        public Color? BaseColor { get; set; }
        public Vector4? Tangent { get; set; }

        public SimpleSkinVertex(Vector3 position, byte[] boneIndices, float[] weights, Vector3 normal, Vector2 uv)
        {
            this.Position = position;
            this.BoneIndices = boneIndices;
            this.Weights = weights;
            this.Normal = normal;
            this.UV = uv;
        }

        public SimpleSkinVertex(
            Vector3 position,
            byte[] boneIndices,
            float[] weights,
            Vector3 normal,
            Vector2 uv,
            Color baseColor
        ) : this(position, boneIndices, weights, normal, uv)
        {
            this.BaseColor = baseColor;
        }

        public SimpleSkinVertex(
            Vector3 position,
            byte[] boneIndices,
            float[] weights,
            Vector3 normal,
            Vector2 uv,
            Color baseColor,
            Vector4 tangent
        ) : this(position, boneIndices, weights, normal, uv, baseColor)
        {
            this.Tangent = tangent;
        }

        internal SimpleSkinVertex(BinaryReader br, SimpleSkinVertexType vertexType)
        {
            this.Position = br.ReadVector3();
            this.BoneIndices = new byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() };
            this.Weights = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            this.Normal = br.ReadVector3();
            this.UV = br.ReadVector2();

            if (vertexType is SimpleSkinVertexType.Color or SimpleSkinVertexType.Tangent)
            {
                this.BaseColor = br.ReadColor(ColorFormat.RgbaU8);
            }
            if (vertexType is SimpleSkinVertexType.Tangent)
            {
                this.Tangent = br.ReadVector4();
            }
        }

        internal void Write(BinaryWriter bw, SimpleSkinVertexType vertexTypeToWrite)
        {
            // Verify that the requested vertex type to write
            // matches the description of this vertex
            SimpleSkinVertexType vertexType = GetVertexType();
            if (vertexType != vertexTypeToWrite)
            {
                throw new InvalidOperationException(
                    $"Vertex type inconsistency, expected to write vertex type: {vertexType} but received {vertexTypeToWrite}"
                );
            }

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

            if (vertexTypeToWrite is SimpleSkinVertexType.Color or SimpleSkinVertexType.Tangent)
            {
                bw.WriteColor(this.BaseColor ?? new(255, 255, 255, 255), ColorFormat.RgbaU8);
            }
            if (vertexTypeToWrite is SimpleSkinVertexType.Tangent)
            {
                bw.WriteVector4(this.Tangent ?? Vector4.UnitW);
            }
        }

        public SimpleSkinVertexType GetVertexType()
        {
            if (this.Tangent is not null)
            {
                return SimpleSkinVertexType.Tangent;
            }
            else if (this.BaseColor is not null)
            {
                return SimpleSkinVertexType.Color;
            }
            else
            {
                return SimpleSkinVertexType.Basic;
            }
        }

        internal static int GetVertexTypeSize(SimpleSkinVertexType vertexType)
        {
            return vertexType switch
            {
                SimpleSkinVertexType.Basic => 52,
                SimpleSkinVertexType.Color => 56,
                SimpleSkinVertexType.Tangent => 72,
            };
        }
    }

    public enum SimpleSkinVertexType : uint
    {
        Basic,
        Color,
        Tangent
    }
}
