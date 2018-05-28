using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a Sphere
    /// </summary>
    public class R3DSphere
    {
        /// <summary>
        /// The position of the <see cref="R3DSphere"/>
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// The radius of the <see cref="R3DSphere"/>
        /// </summary>
        public float Radius;

        /// <summary>
        /// Initializes a new <see cref="R3DSphere"/> instance
        /// </summary>
        /// <param name="position">Position of the sphere</param>
        /// <param name="radius">Radius of the sphere</param>
        public R3DSphere(Vector3 position, float radius)
        {
            this.Position = position;
            this.Radius = radius;
        }

        /// <summary>
        /// Initializes a new <see cref="R3DSphere"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public R3DSphere(BinaryReader br)
        {
            this.Position = new Vector3(br);
            this.Radius = br.ReadSingle();
        }

        /// <summary>
        /// Writes this <see cref="R3DSphere"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            bw.Write(this.Radius);
        }
    }
}