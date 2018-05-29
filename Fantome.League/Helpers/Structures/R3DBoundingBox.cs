using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents an Axis-Aligned Bounding Box
    /// </summary>
    public class R3DBoundingBox
    {
        /// <summary>
        /// The Org component
        /// </summary>
        public Vector3 Org { get; set; }
        /// <summary>
        /// The Size component
        /// </summary>
        public Vector3 Size { get; set; }

        /// <summary>
        /// Initializes a new <see cref="R3DBoundingBox"/>
        /// </summary>
        /// <param name="org">The Org component</param>
        /// <param name="size">The Size component</param>
        public R3DBoundingBox(Vector3 org, Vector3 size)
        {
            this.Org = org;
            this.Size = size;
        }

        /// <summary>
        /// Initializes a new <see cref="R3DBoundingBox"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public R3DBoundingBox(BinaryReader br)
        {
            this.Org = new Vector3(br);
            this.Size = new Vector3(br);
        }

        /// <summary>
        /// Writes this <see cref="R3DBoundingBox"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            this.Org.Write(bw);
            this.Size.Write(bw);
        }
    }
}