using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.SimpleSkin
{
    public class SKNVertex
    {
        /// <summary>
        /// Position of this <see cref="SKNVertex"/>
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// Bone Indices of this <see cref="SKNVertex"/>
        /// </summary>
        public Vector4Byte BoneIndices { get; set; }
        /// <summary>
        /// Weights of this <see cref="SKNVertex"/>
        /// </summary>
        public Vector4 Weights { get; set; }
        /// <summary>
        /// Normal of this <see cref="SKNVertex"/>
        /// </summary>
        public Vector3 Normal { get; set; }
        /// <summary>
        /// UV of this <see cref="SKNVertex"/>
        /// </summary>
        public Vector2 UV { get; set; }
        /// <summary>
        /// Tangent of this <see cref="SKNVertex"/>
        /// </summary>
        public Vector4Byte Tangent { get; set; }

        /// <summary>
        /// Initializes a new <see cref="SKNVertex"/>
        /// </summary>
        /// <param name="position">Position of this <see cref="SKNVertex"/></param>
        /// <param name="boneIndices">Bone Indices of this <see cref="SKNVertex"/></param>
        /// <param name="weights">Weights of this <see cref="SKNVertex"/></param>
        /// <param name="normal">Normal of this <see cref="SKNVertex"/></param>
        /// <param name="uv">UV of this <see cref="SKNVertex"/></param>
        public SKNVertex(Vector3 position, Vector4Byte boneIndices, Vector4 weights, Vector3 normal, Vector2 uv)
        {
            this.Position = Position;
            this.BoneIndices = boneIndices;
            this.Weights = weights;
            this.Normal = normal;
            this.UV = uv;
        }

        /// <summary>
        /// Initializes a new <see cref="SKNVertex"/>
        /// </summary>
        /// <param name="position">Position of this <see cref="SKNVertex"/></param>
        /// <param name="boneIndices">Bone Indices of this <see cref="SKNVertex"/></param>
        /// <param name="weights">Weights of this <see cref="SKNVertex"/></param>
        /// <param name="normal">Normal of this <see cref="SKNVertex"/></param>
        /// <param name="uv">UV of this <see cref="SKNVertex"/></param>
        /// <param name="tangent">Tangent of this <see cref="SKNVertex"/></param>
        public SKNVertex(Vector3 position, Vector4Byte boneIndices, Vector4 weights, Vector3 normal, Vector2 uv, Vector4Byte tangent)
        {
            this.Position = Position;
            this.BoneIndices = boneIndices;
            this.Weights = weights;
            this.Normal = normal;
            this.UV = uv;
            this.Tangent = tangent;
        }

        /// <summary>
        /// Intializes a new <see cref="SKNVertex"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="isTangent">Whether this <see cref="SKNVertex"/> has a Tangent</param>
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

        /// <summary>
        /// Writes this <see cref="SKNVertex"/> into the specified <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
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
