using LeagueToolkit.Helpers.Extensions;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.Helpers.Structures
{
    /// <summary>
    /// Represents a Sphere
    /// </summary>
    public class R3DSphere
    {
        public static readonly R3DSphere Infinite = new R3DSphere(Vector3.Zero, float.MaxValue);

        public Vector3 Position;
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
            this.Position = br.ReadVector3();
            this.Radius = br.ReadSingle();
        }

        /// <summary>
        /// Creates a clone of a <see cref="R3DSphere"/> object
        /// </summary>
        /// <param name="r3dSphere">The <see cref="R3DSphere"/> to clone</param>
        public R3DSphere(R3DSphere r3dSphere)
        {
            this.Position = r3dSphere.Position;
            this.Radius = r3dSphere.Radius;
        }

        /// <summary>
        /// Writes this <see cref="R3DSphere"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.WriteVector3(this.Position);
            bw.Write(this.Radius);
        }
    }
}