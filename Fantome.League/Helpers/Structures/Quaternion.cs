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
        /// Normalizes this <see cref="Quaternion"/>
        /// </summary>
        public Quaternion GetNormalized()
        {
            float magnitude = (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);

            return new Quaternion()
            {
                X = this.X / magnitude,
                Y = this.Y / magnitude,
                Z = this.Z / magnitude,
                W = this.W / magnitude
            };
        }

        /// <summary>
        /// Returns the rotation of this <see cref="Quaternion"/> in euler angles
        /// </summary>
        public Vector3 ToEuler()
        {
            /*float sinX = 2 * (this.W * this.X + this.Y * this.Z);
            float cosX = 1 - 2 * (this.X * this.X + this.Y * this.Y);
            float sinY = 2 * (this.W * this.Y - this.Z * this.X);
            float sinZ = 2 * (this.W * this.Z + this.X * this.Y);
            float cosZ = 1 - 2 * (this.Y * this.Y + this.Z * this.Z);

            float y = 0;
            if(Math.Abs(sinY) >= 1)
            {
                if(sinY < 0)
                {
                    y = -(float)(Math.PI / 2);
                }
                else
                {
                    y = (float)(Math.PI / 2);
                }
            }
            else
            {
                y = (float)Math.Asin(sinY);
            }

            return new Vector3()
            {
                X = (float)Math.Round(Utils.ToDegrees((float)Math.Atan2(sinX, cosX)), 4),
                Y = (float)Math.Round(Utils.ToDegrees(y), 4),
                Z = (float)Math.Round(Utils.ToDegrees((float)Math.Atan2(sinZ, cosZ)), 4)
            };*/

            R3DMatrix44 m = R3DMatrix44.FromRotation(this);

            float sy = (float)Math.Sqrt(m.M11 * m.M11 + m.M21 * m.M21);

            bool singular = sy < 1e-6;
            float x = 0;
            float y = 0;
            float z = 0;
            if(!singular)
            {
                x = (float)Math.Round(Utils.ToDegrees((float)Math.Atan2(m.M32, m.M33)), 4);
                y = (float)Math.Round(Utils.ToDegrees((float)Math.Atan2(-m.M31, sy)), 4);
                z = (float)Math.Round(Utils.ToDegrees((float)Math.Atan2(m.M21, m.M11)), 4);
            }
            else
            {
                x = (float)Math.Round(Utils.ToDegrees((float)Math.Atan2(-m.M23, m.M22)), 4);
                y = (float)Math.Round(Utils.ToDegrees((float)Math.Atan2(-m.M31, sy)), 4);
                z = 0;
            }

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Return a Rotation Quaternion from the specified <see cref="Vector3"/>
        /// </summary>
        public static Quaternion FromEuler(Vector3 v)
        {
            float c1 = (float)Math.Cos(Utils.ToRadian(v.X) / 2);
            float c2 = (float)Math.Cos(Utils.ToRadian(v.Y) / 2);
            float c3 = (float)Math.Cos(Utils.ToRadian(v.Z) / 2);

            float s1 = (float)Math.Sin(Utils.ToRadian(v.X) / 2);
            float s2 = (float)Math.Sin(Utils.ToRadian(v.Y) / 2);
            float s3 = (float)Math.Sin(Utils.ToRadian(v.Z) / 2);

            return new Quaternion()
            {
                X = s1 * c2 * c3 + c1 * s2 * s3,
                Y = c1 * s2 * c3 - s1 * c2 * s3,
                Z = c1 * c2 * s3 + s1 * s2 * c3,
                W = c1 * c2 * c3 - s1 * s2 * s3
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

            return result.GetNormalized();
        }
    }
}