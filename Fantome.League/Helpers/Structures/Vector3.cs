using System;
using System.Globalization;
using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a Vector containing three floats
    /// </summary>
    public class Vector3 : IEquatable<Vector3>
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
        /// Length of this <see cref="Vector3"/>
        /// </summary>
        public float Magnitude { get => (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z)); }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> instance
        /// </summary>
        public Vector3()
        {

        }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> instance
        /// </summary>
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public Vector3(BinaryReader br)
        {
            this.X = br.ReadSingle();
            this.Y = br.ReadSingle();
            this.Z = br.ReadSingle();
        }

        /// <summary>
        /// Creates a clone of a <see cref="Vector3"/> object
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> to clone</param>
        public Vector3(Vector3 vector3)
        {
            this.X = vector3.X;
            this.Y = vector3.Y;
            this.Z = vector3.Z;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> instance from a <see cref="StreamReader"/>
        /// </summary>
        /// <param name="sr">The <see cref="StreamReader"/> to read from</param>
        public Vector3(StreamReader sr)
        {
            string[] input = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            this.X = float.Parse(input[0], CultureInfo.InvariantCulture.NumberFormat);
            this.Y = float.Parse(input[1], CultureInfo.InvariantCulture.NumberFormat);
            this.Z = float.Parse(input[2], CultureInfo.InvariantCulture.NumberFormat);
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
        }

        /// <summary>
        /// Writes this <see cref="Vector3"/> into a <see cref="StreamWriter"/> using the specified format
        /// </summary>
        /// <param name="sw"><see cref="StreamWriter"/> to write to</param>
        /// <param name="format">Format that should be used for writing</param>
        public void Write(StreamWriter sw, string format)
        {
            sw.Write(format, this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Returns a normalized <see cref="Vector3"/>
        /// </summary>
        public Vector3 Normalized()
        {
            if (this.Magnitude == 0)
            {
                return this;
            }
            else
            {
                return new Vector3()
                {
                    X = this.X / this.Magnitude,
                    Y = this.Y / this.Magnitude,
                    Z = this.Z / this.Magnitude
                };
            }
        }

        /// <summary>
        /// Calculates a Cross product from two <see cref="Vector3"/>
        /// </summary>
        public static Vector3 Cross(Vector3 x, Vector3 y)
        {
            return new Vector3(
                (x.Y * y.Z) - (x.Z * y.Y),
                (x.Z * y.X) - (x.X * y.Z),
                (x.X * y.Y) - (x.Y * y.X));
        }

        /// <summary>
        /// Calculates the distance between <paramref name="x"/> and <paramref name="y"/>
        /// </summary>
        /// <returns>The distance between <paramref name="x"/> and <paramref name="y"/></returns>
        public static float Distance(Vector3 x, Vector3 y)
        {
            return (float)Math.Sqrt(Math.Pow(x.X - y.X, 2) - Math.Pow(x.Y - y.Y, 2) - Math.Pow(x.Z - y.Z, 2));
        }

        public static Vector3 FromRotationMatrix(R3DMatrix44 m)
        {
            float x = 0;
            float y = Utilities.Clamp(m.M13, -1, 1);
            float z = 0;

            return new Vector3()
            {
                X = (float)Math.Round(Utilities.ToDegrees(x), 4),
                Y = (float)Math.Round(Utilities.ToDegrees(x), 4),
                Z = (float)Math.Round(Utilities.ToDegrees(x), 4)
            };
        }

        /// <summary>
        /// Determines wheter this <see cref="Vector3"/> is equal to <paramref name="other"/>
        /// </summary>
        /// <param name="other">The <see cref="Vector3"/> to compare to</param>
        /// <returns>Wheter <paramref name="other"/> is equal to this <see cref="Vector3"/></returns>
        public bool Equals(Vector3 other)
        {
            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z);
        }

        /// <summary>
        /// Adds two <see cref="Vector3"/>
        /// </summary>
        public static Vector3 operator +(Vector3 x, Vector3 y)
        {
            return new Vector3(x.X + y.X, x.Y + y.Y, x.Z + y.Z);
        }

        /// <summary>
        /// Subtracts two <see cref="Vector3"/>
        /// </summary>
        public static Vector3 operator -(Vector3 x, Vector3 y)
        {
            return new Vector3(x.X - y.X, x.Y - y.Y, x.Z - y.Z);
        }
    }
}
