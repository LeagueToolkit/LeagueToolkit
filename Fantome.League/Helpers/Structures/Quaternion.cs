using System;
using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a Rotation Quaternion
    /// </summary>
    public class Quaternion
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
        /// Initializes a new <see cref="Quaternion"/>
        /// </summary>
        public Quaternion() { }

        /// <summary>
        /// Initializes a new <see cref="Quaternion"/>
        /// </summary>
        public Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Quaternion"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public Quaternion(BinaryReader br)
        {
            this.X = br.ReadSingle();
            this.Y = br.ReadSingle();
            this.Z = br.ReadSingle();
            this.W = br.ReadSingle();
        }

        /// <summary>
        /// Writes this <see cref="Quaternion"/> into a <see cref="BinaryWriter"/>
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
        /// Returns the Conjugate of this <see cref="Quaternion"/>
        /// </summary>
        public Quaternion Conjugate()
        {
            return new Quaternion()
            {
                X = -this.X,
                Y = -this.Y,
                Z = -this.Z,
                W = this.W
            };
        }

        /// <summary>
        /// Multiplies the components of this Quaternion by a scalar
        /// </summary>
        /// <param name="scalar">The scalar to multiply by</param>
        public void Scale(float scalar)
        {
            this.X *= scalar;
            this.Y *= scalar;
            this.Z *= scalar;
            this.W *= scalar;
        }

        /// <summary>
        /// Normalizes this <see cref="Quaternion"/>
        /// </summary>
        public void Normalize()
        {
            float normalizer = (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W));
            this.X /= normalizer;
            this.Y /= normalizer;
            this.Z /= normalizer;
            this.W /= normalizer;
        }

        /// <summary>
        /// Returns a <see cref="Quaternion"/> that represents the rotation of the given matrix
        /// </summary>
        public static Quaternion FromTransformationMatrix(R3DMatrix44 matrix)
        {
            Quaternion result = new Quaternion(0, 0, 0, 0);
            float sqrt;
            float half;
            float scale = matrix.M11 + matrix.M22 + matrix.M33;

            if (scale > 0.0f)
            {
                sqrt = (float)Math.Sqrt(scale + 1.0f);
                result.W = sqrt * 0.5f;
                sqrt = 0.5f / sqrt;

                result.X = (matrix.M23 - matrix.M32) * sqrt;
                result.Y = (matrix.M31 - matrix.M13) * sqrt;
                result.Z = (matrix.M12 - matrix.M21) * sqrt;
            }
            else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                sqrt = (float)Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                half = 0.5f / sqrt;

                result.X = 0.5f * sqrt;
                result.Y = (matrix.M12 + matrix.M21) * half;
                result.Z = (matrix.M13 + matrix.M31) * half;
                result.W = (matrix.M23 - matrix.M32) * half;
            }
            else if (matrix.M22 > matrix.M33)
            {
                sqrt = (float)Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                half = 0.5f / sqrt;

                result.X = (matrix.M21 + matrix.M12) * half;
                result.Y = 0.5f * sqrt;
                result.Z = (matrix.M32 + matrix.M23) * half;
                result.W = (matrix.M31 - matrix.M13) * half;
            }
            else
            {
                sqrt = (float)Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                half = 0.5f / sqrt;

                result.X = (matrix.M31 + matrix.M13) * half;
                result.Y = (matrix.M32 + matrix.M23) * half;
                result.Z = 0.5f * sqrt;
                result.W = (matrix.M12 - matrix.M21) * half;
            }

            return result;
        }

        /// <summary>
        /// Adds <paramref name="x"/> and <paramref name="y"/> together
        /// </summary>
        public static Quaternion operator +(Quaternion x, Quaternion y)
        {
            return new Quaternion()
            {
                X = x.X + y.X,
                Y = x.Y + y.Y,
                Z = x.Z + y.Z,
                W = x.W + y.W
            };
        }

        /// <summary>
        /// Multiplies <paramref name="x"/> and <paramref name="y"/> together
        /// </summary>
        public static Quaternion operator *(Quaternion x, Quaternion y)
        {
            return new Quaternion()
            {
                X = x.X * y.W + x.Y * y.Z - x.Z * y.Y + x.W * y.X,
                Y = -x.X * y.Z + x.Y * y.W + x.Z * y.X + x.W * y.Y,
                Z = x.X * y.Y - x.Y * y.X + x.Z * y.W + x.W * y.Z,
                W = -x.X * y.X - x.Y * y.Y - x.Z * y.Z + x.W * y.W
            };
        }
    }
}