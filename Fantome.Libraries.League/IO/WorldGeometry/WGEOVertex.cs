using System.IO;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.WorldGeometry
{
    /// <summary>
    /// Represents a <see cref="WGEOVertex"/> inside of a <see cref="WGEOModel"/>
    /// </summary>
    public class WGEOVertex
    {
        /// <summary>
        /// Position of this <see cref="WGEOVertex"/>
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// UV of this <see cref="WGEOVertex"/>
        /// </summary>
        public Vector2 UV { get; set; }

        /// <summary>
        /// Initializes a new <see cref="WGEOVertex"/>
        /// </summary>
        /// <param name="position">Position of this <see cref="WGEOVertex"/></param>
        /// <param name="uv">UV of this <see cref="WGEOVertex"/></param>
        public WGEOVertex(Vector3 position, Vector2 uv)
        {
            this.Position = position;
            this.UV = uv;
        }

        /// <summary>
        /// Initializes a new <see cref="WGEOVertex"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public WGEOVertex(BinaryReader br)
        {
            this.Position = new Vector3(br);
            this.UV = new Vector2(br);
        }

        /// <summary>
        /// Writes this <see cref="WGEOVertex"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.UV.Write(bw);
        }
    }
}
