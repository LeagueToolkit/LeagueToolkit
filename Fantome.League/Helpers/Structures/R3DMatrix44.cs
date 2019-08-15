using System;
using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a transformation Matrix
    /// </summary>
    public class R3DMatrix44
    {
        public float M11 { get; private set; }
        public float M12 { get; private set; }
        public float M13 { get; private set; }
        public float M14 { get; private set; }
        public float M21 { get; private set; }
        public float M22 { get; private set; }
        public float M23 { get; private set; }
        public float M24 { get; private set; }
        public float M31 { get; private set; }
        public float M32 { get; private set; }
        public float M33 { get; private set; }
        public float M34 { get; private set; }
        public float M41 { get; private set; }
        public float M42 { get; private set; }
        public float M43 { get; private set; }
        public float M44 { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="R3DMatrix44"/> instance
        /// </summary>
        public R3DMatrix44()
        {
            this.Clear();
        }

        /// <summary>
        /// Initializes a new <see cref="R3DMatrix44"/> instance
        /// </summary>
        public R3DMatrix44(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M14 = m14;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M24 = m24;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
            this.M34 = m34;
            this.M41 = m41;
            this.M42 = m42;
            this.M43 = m43;
            this.M44 = m44;
        }

        /// <summary>
        /// Initializes a new <see cref="R3DMatrix44"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br"></param>
        public R3DMatrix44(BinaryReader br)
        {
            this.M11 = br.ReadSingle();
            this.M12 = br.ReadSingle();
            this.M13 = br.ReadSingle();
            this.M14 = br.ReadSingle();
            this.M21 = br.ReadSingle();
            this.M22 = br.ReadSingle();
            this.M23 = br.ReadSingle();
            this.M24 = br.ReadSingle();
            this.M31 = br.ReadSingle();
            this.M32 = br.ReadSingle();
            this.M33 = br.ReadSingle();
            this.M34 = br.ReadSingle();
            this.M41 = br.ReadSingle();
            this.M42 = br.ReadSingle();
            this.M43 = br.ReadSingle();
            this.M44 = br.ReadSingle();
        }

        /// <summary>
        /// Creates a clone of a <see cref="R3DMatrix44"/> object
        /// </summary>
        /// <param name="r3dMatrix44">The <see cref="R3DMatrix44"/> to clone</param>
        public R3DMatrix44(R3DMatrix44 r3dMatrix44)
        {
            this.M11 = r3dMatrix44.M11;
            this.M12 = r3dMatrix44.M12;
            this.M13 = r3dMatrix44.M13;
            this.M14 = r3dMatrix44.M14;
            this.M21 = r3dMatrix44.M21;
            this.M22 = r3dMatrix44.M22;
            this.M23 = r3dMatrix44.M23;
            this.M24 = r3dMatrix44.M24;
            this.M31 = r3dMatrix44.M31;
            this.M32 = r3dMatrix44.M32;
            this.M33 = r3dMatrix44.M33;
            this.M34 = r3dMatrix44.M34;
            this.M41 = r3dMatrix44.M41;
            this.M42 = r3dMatrix44.M42;
            this.M43 = r3dMatrix44.M43;
            this.M44 = r3dMatrix44.M44;
        }

        /// <summary>
        /// Resets this <see cref="R3DMatrix44"/> to an Identity Matrix
        /// </summary>
        public void Clear()
        {
            this.M11 = 1;
            this.M12 = 0;
            this.M13 = 0;
            this.M14 = 0;
            this.M21 = 0;
            this.M22 = 1;
            this.M23 = 0;
            this.M24 = 0;
            this.M31 = 0;
            this.M32 = 0;
            this.M33 = 1;
            this.M34 = 0;
            this.M41 = 0;
            this.M42 = 0;
            this.M43 = 0;
            this.M44 = 1;
        }

        /// <summary>
        /// Writes this <see cref="R3DMatrix44"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.M11);
            bw.Write(this.M12);
            bw.Write(this.M13);
            bw.Write(this.M14);
            bw.Write(this.M21);
            bw.Write(this.M22);
            bw.Write(this.M23);
            bw.Write(this.M24);
            bw.Write(this.M31);
            bw.Write(this.M32);
            bw.Write(this.M33);
            bw.Write(this.M34);
            bw.Write(this.M41);
            bw.Write(this.M42);
            bw.Write(this.M43);
            bw.Write(this.M44);
        }

        /// <summary>
        /// Returns the Translation Vector of this <see cref="R3DMatrix44"/>
        /// </summary>
        public Vector3 GetTranslation()
        {
            return new Vector3(this.M14, this.M24, this.M34);
        }

        /// <summary>
        /// Returns the Rotation of this <see cref="R3DMatrix44"/>
        /// </summary>
        public Quaternion GetRotation()
        {
            Quaternion result = new Quaternion(0, 0, 0, 0);
            float sqrt;
            float half;
            float scale = this.M11 + this.M22 + this.M33;

            if (scale > 0.0f)
            {
                sqrt = (float)Math.Sqrt(scale + 1.0f);
                result.W = sqrt * 0.5f;
                sqrt = 0.5f / sqrt;

                result.X = (this.M23 - this.M32) * sqrt;
                result.Y = (this.M31 - this.M13) * sqrt;
                result.Z = (this.M12 - this.M21) * sqrt;
            }
            else if ((this.M11 >= this.M22) && (this.M11 >= this.M33))
            {
                sqrt = (float)Math.Sqrt(1.0f + this.M11 - this.M22 - this.M33);
                half = 0.5f / sqrt;

                result.X = 0.5f * sqrt;
                result.Y = (this.M12 + this.M21) * half;
                result.Z = (this.M13 + this.M31) * half;
                result.W = (this.M23 - this.M32) * half;
            }
            else if (this.M22 > this.M33)
            {
                sqrt = (float)Math.Sqrt(1.0f + this.M22 - this.M11 - this.M33);
                half = 0.5f / sqrt;

                result.X = (this.M21 + this.M12) * half;
                result.Y = 0.5f * sqrt;
                result.Z = (this.M32 + this.M23) * half;
                result.W = (this.M31 - this.M13) * half;
            }
            else
            {
                sqrt = (float)Math.Sqrt(1.0f + this.M33 - this.M11 - this.M22);
                half = 0.5f / sqrt;

                result.X = (this.M31 + this.M13) * half;
                result.Y = (this.M32 + this.M23) * half;
                result.Z = 0.5f * sqrt;
                result.W = (this.M12 - this.M21) * half;
            }

            return result;
        }

        /// <summary>
        /// Returns the Scale of this <see cref="R3DMatrix44"/>
        /// </summary>
        public Vector3 GetScale()
        {
            return new Vector3()
            {
                X = new Vector3(this.M11, this.M12, this.M13).Magnitude,
                Y = new Vector3(this.M21, this.M22, this.M23).Magnitude,
                Z = new Vector3(this.M31, this.M32, this.M33).Magnitude
            };
        }

        /// <summary>
        /// Returns the fourth row of this <see cref="R3DMatrix44"/>
        /// </summary>
        public Vector3 GetFourthRow()
        {
            return new Vector3(this.M41, this.M42, this.M43);
        }

        /// <summary>
        /// Inserts a <see cref="Vector3"/> into the 4th row of this <see cref="R3DMatrix44"/>
        /// </summary>
        public void InsertFourthRow(Vector3 row)
        {
            this.M41 = row.X;
            this.M42 = row.Y;
            this.M43 = row.Z;
        }

        /// <summary>
        /// Returns the Inverse of this <see cref="R3DMatrix44"/>
        /// </summary>
        public R3DMatrix44 Inverse()
        {
            float t11 = this.M23 * this.M34 * this.M42
                - this.M24 * this.M33 * this.M42
                + this.M24 * this.M32 * this.M43
                - this.M22 * this.M34 * this.M43
                - this.M23 * this.M32 * this.M44
                + this.M22 * this.M33 * this.M44;

            float t12 = this.M14 * this.M33 * this.M42
                - this.M13 * this.M34 * this.M42
                - this.M14 * this.M32 * this.M43
                + this.M12 * this.M34 * this.M43
                + this.M13 * this.M32 * this.M44
                - this.M12 * this.M33 * this.M44;

            float t13 = this.M13 * this.M24 * this.M42
                - this.M14 * this.M23 * this.M42
                + this.M14 * this.M22 * this.M43
                - this.M12 * this.M24 * this.M43
                - this.M13 * this.M22 * this.M44
                + this.M12 * this.M23 * this.M44;

            float t14 = this.M14 * this.M23 * this.M32
                - this.M13 * this.M24 * this.M32
                - this.M14 * this.M22 * this.M33
                + this.M12 * this.M24 * this.M33
                + this.M13 * this.M22 * this.M34
                - this.M12 * this.M23 * this.M34;

            float inverseDeterminant = 1 / (this.M11 * t11 + this.M21 * t12 + this.M31 * t13 + this.M41 * t14);

            return new R3DMatrix44()
            {
                M11 = t11 * inverseDeterminant,
                M12 = (this.M24 * this.M33 * this.M41 - this.M23 * this.M34 * this.M41 - this.M24 * this.M31 * this.M43 + this.M21 * this.M34 * this.M43 + this.M23 * this.M31 * this.M44 - this.M21 * this.M33 * this.M44) * inverseDeterminant,
                M13 = (this.M22 * this.M34 * this.M41 - this.M24 * this.M32 * this.M41 + this.M24 * this.M31 * this.M42 - this.M21 * this.M34 * this.M42 - this.M22 * this.M31 * this.M44 + this.M21 * this.M32 * this.M44) * inverseDeterminant,
                M14 = (this.M23 * this.M32 * this.M41 - this.M22 * this.M33 * this.M41 - this.M23 * this.M31 * this.M42 + this.M21 * this.M33 * this.M42 + this.M22 * this.M31 * this.M43 - this.M21 * this.M32 * this.M43) * inverseDeterminant,

                M21 = t12 * inverseDeterminant,
                M22 = (this.M13 * this.M34 * this.M41 - this.M14 * this.M33 * this.M41 + this.M14 * this.M31 * this.M43 - this.M11 * this.M34 * this.M43 - this.M13 * this.M31 * this.M44 + this.M11 * this.M33 * this.M44) * inverseDeterminant,
                M23 = (this.M14 * this.M32 * this.M41 - this.M12 * this.M34 * this.M41 - this.M14 * this.M31 * this.M42 + this.M11 * this.M34 * this.M42 + this.M12 * this.M31 * this.M44 - this.M11 * this.M32 * this.M44) * inverseDeterminant,
                M24 = (this.M12 * this.M33 * this.M41 - this.M13 * this.M32 * this.M41 + this.M13 * this.M31 * this.M42 - this.M11 * this.M33 * this.M42 - this.M12 * this.M31 * this.M43 + this.M11 * this.M32 * this.M43) * inverseDeterminant,

                M31 = t13 * inverseDeterminant,
                M32 = (this.M14 * this.M23 * this.M41 - this.M13 * this.M24 * this.M41 - this.M14 * this.M21 * this.M43 + this.M11 * this.M24 * this.M43 + this.M13 * this.M21 * this.M44 - this.M11 * this.M23 * this.M44) * inverseDeterminant,
                M33 = (this.M12 * this.M24 * this.M41 - this.M14 * this.M22 * this.M41 + this.M14 * this.M21 * this.M42 - this.M11 * this.M24 * this.M42 - this.M12 * this.M21 * this.M44 + this.M11 * this.M22 * this.M44) * inverseDeterminant,
                M34 = (this.M13 * this.M22 * this.M41 - this.M12 * this.M23 * this.M41 - this.M13 * this.M21 * this.M42 + this.M11 * this.M23 * this.M42 + this.M12 * this.M21 * this.M43 - this.M11 * this.M22 * this.M43) * inverseDeterminant,

                M41 = t14 * inverseDeterminant,
                M42 = (this.M13 * this.M24 * this.M31 - this.M14 * this.M23 * this.M31 + this.M14 * this.M21 * this.M33 - this.M11 * this.M24 * this.M33 - this.M13 * this.M21 * this.M34 + this.M11 * this.M23 * this.M34) * inverseDeterminant,
                M43 = (this.M14 * this.M22 * this.M31 - this.M12 * this.M24 * this.M31 - this.M14 * this.M21 * this.M32 + this.M11 * this.M24 * this.M32 + this.M12 * this.M21 * this.M34 - this.M11 * this.M22 * this.M34) * inverseDeterminant,
                M44 = (this.M12 * this.M23 * this.M31 - this.M13 * this.M22 * this.M31 + this.M13 * this.M21 * this.M32 - this.M11 * this.M23 * this.M32 - this.M12 * this.M21 * this.M33 + this.M11 * this.M22 * this.M33) * inverseDeterminant
            };
        }

        /// <summary>
        /// Returns the Determinant of this <see cref="R3DMatrix44"/>
        /// </summary>
        public float Determinant()
        {
            return this.M41 *
                (+this.M14 * this.M23 * this.M32
                - this.M13 * this.M24 * this.M32
                - this.M14 * this.M22 * this.M33
                + this.M12 * this.M24 * this.M33
                + this.M13 * this.M22 * this.M34
                - this.M12 * this.M23 * this.M34)

                 + this.M42 *
                (+this.M11 * this.M23 * this.M34
                - this.M11 * this.M24 * this.M33
                + this.M14 * this.M21 * this.M33
                - this.M13 * this.M21 * this.M34
                + this.M13 * this.M24 * this.M31
                - this.M14 * this.M23 * this.M31)

                + this.M43 *
               (+this.M11 * this.M24 * this.M32
                - this.M11 * this.M22 * this.M34
                - this.M14 * this.M21 * this.M32
                + this.M12 * this.M21 * this.M34
                + this.M14 * this.M22 * this.M31
                - this.M12 * this.M24 * this.M31)

                + this.M44 *
                (-this.M13 * this.M22 * this.M31
                - this.M11 * this.M23 * this.M32
                + this.M11 * this.M22 * this.M33
                + this.M13 * this.M21 * this.M32
                - this.M12 * this.M21 * this.M33
                + this.M12 * this.M23 * this.M31);
        }

        /// <summary>
        /// Applies the transformation of this matrix to the specified <see cref="Vector3"/>
        /// </summary>
        /// <param name="vector"><see cref="Vector3"/> to transform</param>
        /// <returns>The transformed <see cref="Vector3"/></returns>
        public Vector3 ApplyTransformation(Vector3 vector)
        {
            return new Vector3
            {
                X = (this.M11 * vector.X) + (this.M12 * vector.Y) + (this.M13 * vector.Z) + this.M14,
                Y = (this.M21 * vector.X) + (this.M22 * vector.Y) + (this.M23 * vector.Z) + this.M24,
                Z = (this.M31 * vector.X) + (this.M32 * vector.Y) + (this.M33 * vector.Z) + this.M34
            };
        }

        /// <summary>
        /// Creates a Transformation <see cref="R3DMatrix44"/> from Translation
        /// </summary>
        public static R3DMatrix44 FromTranslation(Vector3 translation)
        {
            return new R3DMatrix44()
            {
                M14 = translation.X,
                M24 = translation.Y,
                M34 = translation.Z
            };
        }

        /// <summary>
        /// Creates a Transformation <see cref="R3DMatrix44"/> from Rotation
        /// </summary>
        public static R3DMatrix44 FromRotation(Vector3 rotation)
        {
            return FromRotation(Quaternion.FromEuler(rotation));
        }

        /// <summary>
        /// Creates a Transformation <see cref="R3DMatrix44"/> from Rotation
        /// </summary>
        public static R3DMatrix44 FromRotation(Quaternion q)
        {
            return new R3DMatrix44()
            {
                M11 = 1 - 2 * (q.Y * q.Y + q.Z * q.Z),
                M12 = 2 * (q.X * q.Y + q.Z * q.W),
                M13 = 2 * (q.X * q.Z - q.Y * q.W),
                M21 = 2 * (q.X * q.Y - q.Z * q.W),
                M22 = 1 - 2 * (q.X * q.X + q.Z * q.Z),
                M23 = 2 * (q.Y * q.Z + q.X * q.W),
                M31 = 2 * (q.X * q.Z + q.Y * q.W),
                M32 = 2 * (q.Y * q.Z - q.X * q.W),
                M33 = 1 - 2 * (q.X * q.X + q.Y * q.Y),
            };
        }

        /// <summary>
        /// Creates a Transformation <see cref="R3DMatrix44"/> from Scale
        /// </summary>
        public static R3DMatrix44 FromScale(Vector3 scale)
        {
            return new R3DMatrix44()
            {
                M11 = scale.X,
                M22 = scale.Y,
                M33 = scale.Z
            };
        }

        /// <summary>
        /// Creates a Transformation <see cref="R3DMatrix44"/>
        /// </summary>
        public static R3DMatrix44 CreateTransformation(Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            return CreateTransformation(translation, Quaternion.FromEuler(rotation), scale);
        }

        /// <summary>
        /// Creates a Transformation <see cref="R3DMatrix44"/>
        /// </summary>
        public static R3DMatrix44 CreateTransformation(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            return FromTranslation(translation) * FromRotation(rotation) * FromScale(scale);
        }

        public static R3DMatrix44 operator *(R3DMatrix44 a, R3DMatrix44 b)
        {
            return new R3DMatrix44()
            {
                M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
                M21 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
                M31 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
                M41 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

                M12 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
                M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
                M32 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
                M42 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

                M13 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
                M23 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
                M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
                M43 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

                M14 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
                M24 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
                M34 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
                M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44,
            };
        }
    }
}
