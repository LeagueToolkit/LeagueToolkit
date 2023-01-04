using System.Numerics;

namespace LeagueToolkit.Core.Primitives
{
    /// <summary>Represents a Sphere</summary>
    public struct Sphere
    {
        /// <summary>
        /// Represents a <see cref="Sphere"/> located at <see cref="Vector3.Zero"/> with a radius of <see cref="float.MaxValue"/>
        /// </summary>
        public static readonly Sphere INFINITE = new(Vector3.Zero, float.MaxValue);

        /// <summary>The sphere's position</summary>
        public Vector3 Position;

        /// <summary>The sphere's radius</summary>
        public float Radius;

        /// <summary>
        /// Creates a new <see cref="Sphere"/> object with the specified parameters
        /// </summary>
        /// <param name="position">The position of the <see cref="Sphere"/></param>
        /// <param name="radius">The radius of the <see cref="Sphere"/></param>
        public Sphere(Vector3 position, float radius)
        {
            this.Position = position;
            this.Radius = radius;
        }
    }
}
