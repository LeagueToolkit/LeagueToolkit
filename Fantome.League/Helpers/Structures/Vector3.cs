using System;
using System.Globalization;
using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a float Vector with 3 components
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
        /// Initializes a new <see cref="Vector3"/> instance
        /// </summary>
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public Vector3(BinaryReader br)
        {
            this.X = br.ReadSingle();
            this.Y = br.ReadSingle();
            this.Z = br.ReadSingle();
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> from a <see cref="StreamReader"/>
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
            sw.Write(string.Format(format, this.X, this.Y, this.Z));
        }

        /// <summary>
        /// Calculates the Cross product of <paramref name="x"/> and <paramref name="y"/>
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
        public static float Distance(Vector3 x, Vector3 y)
        {
            return (float)Math.Sqrt(Math.Pow(x.X - y.X, 2) - Math.Pow(x.Y - y.Y, 2) - Math.Pow(x.Z - y.Z, 2));
        }

        /// <summary>
        /// Determines wheter this <see cref="Vector3"/> is equal to <paramref name="other"/>
        /// </summary>
        /// <param name="other">The <see cref="Vector3"/> to compare to</param>
        public bool Equals(Vector3 other)
        {
            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z);
        }

        /// <summary>
        /// Adds together the components of <paramref name="x"/> and <paramref name="y"/>
        /// </summary>
        public static Vector3 operator +(Vector3 x, Vector3 y)
        {
            return new Vector3(x.X + y.X, x.Y + y.Y, x.Z + y.Z);
        }

        /// <summary>
        /// Subtracts the components of <paramref name="x"/> by <paramref name="y"/>
        /// </summary>
        public static Vector3 operator -(Vector3 x, Vector3 y)
        {
            return new Vector3(x.X - y.X, x.Y - y.Y, x.Z - y.Z);
        }
    }
}