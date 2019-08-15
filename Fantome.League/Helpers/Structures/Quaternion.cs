using System;
using Utils = Fantome.Libraries.League.Helpers.Utilities.Utilities;

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
        /// Initializes a new <see cref="Quaternion"/> instance
        /// </summary>
        public Quaternion()
        {

        }

        /// <summary>
        /// Initializes a new <see cref="Quaternion"/> instance
        /// </summary>
        public Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Creates a clone of a <see cref="Quaternion"/> object
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to clone</param>
        public Quaternion(Quaternion quaternion)
        {
            this.X = quaternion.X;
            this.Y = quaternion.Y;
            this.Z = quaternion.Z;
            this.W = quaternion.W;
        }

        /// <summary>
        /// Returns the rotation of this <see cref="Quaternion"/> in euler angles
        /// </summary>
        public Vector3 ToEuler()
        {
            float sqrY = this.Y * this.Y;
            float t0 = -2 * (sqrY + this.Z * this.Z) + 1;
            float t1 = +2 * (this.X * this.Y - this.W * this.Z);
            float t2 = -2 * (this.X * this.Z + this.W * this.Y);
            float t3 = +2 * (this.Y * this.Z - this.W * this.X);
            float t4 = -2 * (this.X * this.X + sqrY) + 1;

            t2 = t2 > 1 ? 1 : t2;
            t2 = t2 < -1 ? -1 : t2;

            return new Vector3()
            {
                X = Utils.ToDegrees((float)Math.Atan2(t3, t4)),
                Y = Utils.ToDegrees((float)Math.Atan2(t1, t0)),
                Z = Utils.ToDegrees((float)Math.Asin(t2))
            };
        }

        /// <summary>
        /// Return a Rotation Quaternion from the specified <see cref="Vector3"/>
        /// </summary>
        public static Quaternion FromEuler(Vector3 v)
        {
            float t0 = (float)Math.Cos(v.Y * 0.5f);
            float t1 = (float)Math.Sin(v.Y * 0.5f);
            float t2 = (float)Math.Cos(v.X * 0.5f);
            float t3 = (float)Math.Sin(v.X * 0.5f);
            float t4 = (float)Math.Cos(v.Z * 0.5f);
            float t5 = (float)Math.Sin(v.Z * 0.5f);

            return new Quaternion()
            {
                W = (t0 * t2 * t4) + (t1 * t3 * t5),
                X = (t0 * t3 * t4) - (t1 * t2 * t5),
                Y = (t0 * t2 * t5) + (t1 * t3 * t4),
                Z = (t1 * t2 * t4) - (t0 * t3 * t5)
            };
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
    }
}