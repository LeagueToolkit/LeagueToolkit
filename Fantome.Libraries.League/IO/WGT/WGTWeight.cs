using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.WGT
{
    /// <summary>
    /// Represents a Weight Entry inside of a <see cref="WGTFile"/>
    /// </summary>
    public class WGTWeight
    {
        /// <summary>
        /// Bone Indices of this <see cref="WGTWeight"/>
        /// </summary>
        public Vector4Byte BoneIndices { get; set; }
        /// <summary>
        /// Weights of this <see cref="WGTWeight"/>
        /// </summary>
        public Vector4 Weights { get; set; }

        /// <summary>
        /// Initializes a new <see cref="WGTWeight"/>
        /// </summary>
        /// <param name="weights">Weights of this <see cref="WGTWeight"/></param>
        /// <param name="boneIndices">Bone Indices of this <see cref="WGTWeight"/></param>
        public WGTWeight(Vector4 weights, Vector4Byte boneIndices)
        {
            this.Weights = weights;
            this.BoneIndices = boneIndices;
        }

        /// <summary>
        /// Initializes a new <see cref="WGTWeight"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public WGTWeight(BinaryReader br)
        {
            this.BoneIndices = new Vector4Byte(br);
            this.Weights = new Vector4(br);
        }

        /// <summary>
        /// Writes this <see cref="WGTWeight"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            this.BoneIndices.Write(bw);
            this.Weights.Write(bw);
        }
    }
}
