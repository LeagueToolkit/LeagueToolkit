using System;
using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a Vector containing two floats
    /// </summary>
    public class Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        /// The X component
        /// </summary>
        public float X { get; set; }
        /// <summary>
        /// The Y component
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Vector2"/> instance
        /// </summary>
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector2"/> instance by reading the components from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br"><see cref="BinaryReader"/> to read from</param>
        public Vector2(BinaryReader br)
        {
            this.X = br.ReadSingle();
            this.Y = br.ReadSingle();
        }

        /// <summary>
        /// Writes the components of a <see cref="Vector2"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.X);
            bw.Write(this.Y);
        }

        /// <summary>
        /// Determines wheter this <see cref="Vector2"/> is equal to <paramref name="other"/>
        /// </summary>
        /// <param name="other"><see cref="Vector2"/> to compare to</param>
        /// <returns>Wheter this <see cref="Vector2"/> is equal to <paramref name="other"/></returns>
        public bool Equals(Vector2 other)
        {
            return (this.X == other.X) && (this.Y == other.Y);
        }

        /// <summary>
        /// Adds 2 <see cref="Vector2"/> togeather
        /// </summary>
        public static Vector2 operator +(Vector2 x, Vector2 y)
        {
            return new Vector2(x.X + y.X, x.Y + y.Y);
        }

        /// <summary>
        /// Subtracts 2 <see cref="Vector2"/>
        /// </summary>
        public static Vector2 operator -(Vector2 x, Vector2 y)
        {
            return new Vector2(x.X - y.X, x.Y - y.Y);
        }
    }
}