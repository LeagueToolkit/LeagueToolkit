using System;
using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a Vector containing four floats
    /// </summary>
    public struct Vector4 : IEquatable<Vector4>
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
        /// The Z component
        /// </summary>
        public float Z { get; set; }
        /// <summary>
        /// The W component
        /// </summary>
        public float W { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Vector4"/> instance
        /// </summary>
        public Vector4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Intializes a new <see cref="Vector4"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public Vector4(BinaryReader br)
        {
            this.X = br.ReadSingle();
            this.Y = br.ReadSingle();
            this.Z = br.ReadSingle();
            this.W = br.ReadSingle();
        }

        /// <summary>
        /// Creates a clone of a <see cref="Vector4"/> object
        /// </summary>
        /// <param name="vector4">The <see cref="Vector4"/> to clone</param>
        public Vector4(Vector4 vector4)
        {
            this.X = vector4.X;
            this.Y = vector4.Y;
            this.Z = vector4.Z;
            this.W = vector4.W;
        }

        /// <summary>
        /// Writes this <see cref="Vector4"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.X);
            bw.Write(this.Y);
            bw.Write(this.Z);
            bw.Write(this.W);
        }

        /// <summary>
        /// Determines wheter this <see cref="Vector4"/> is equal to <paramref name="other"/>
        /// </summary>
        /// <param name="other">The <see cref="Vector4"/> to compare to</param>
        /// <returns>Wheter <paramref name="other"/> is equal to this <see cref="Vector4"/></returns>
        public bool Equals(Vector4 other)
        {
            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z) && (this.W == other.W);
        }

        /// <summary>
        /// Subtracts two <see cref="Vector4"/>
        /// </summary>
        public static Vector4 operator +(Vector4 x, Vector4 y)
        {
            return new Vector4(x.X + y.X, x.Y + y.Y, x.Z + y.Z, x.W + y.W);
        }

        /// <summary>
        /// Adds two <see cref="Vector4"/>
        /// </summary>
        public static Vector4 operator -(Vector4 x, Vector4 y)
        {
            return new Vector4(x.X - y.X, x.Y - y.Y, x.Z - y.Z, x.W - y.W);
        }

        public static bool operator ==(Vector4 a, Vector4 b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Vector4 a, Vector4 b)
        {
            return !a.Equals(b);
        }

        public static implicit operator System.Numerics.Vector4(Vector4 v) => new System.Numerics.Vector4(v.X, v.Y, v.Z, v.W);
        public static implicit operator Vector4(System.Numerics.Vector4 v) => new Vector4(v.X, v.Y, v.Z, v.W);
    }
}
