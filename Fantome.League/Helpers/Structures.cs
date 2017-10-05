﻿using System;
using System.Globalization;
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
            sw.Write(string.Format(format, this.X, this.Y, this.Z));
        }

        /// <summary>
        /// Calculates a Cross product from two <see cref="Vector3"/>
        /// </summary>q
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

    /// <summary>
    /// Represents a Vector containing four floats
    /// </summary>
    public class Vector4 : IEquatable<Vector4>
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
    }

    /// <summary>
    /// Represents a Vector containing two bytes
    /// </summary>
    public class Vector2Byte
    {
        /// <summary>
        /// The X component
        /// </summary>
        public byte X { get; set; }
        /// <summary>
        /// The Y component
        /// </summary>
        public byte Y { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Vector2Byte"/> instance
        /// </summary>
        public Vector2Byte(byte x, byte y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector2Byte"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public Vector2Byte(BinaryReader br)
        {
            this.X = br.ReadByte();
            this.Y = br.ReadByte();
        }

        /// <summary>
        /// Writes this <see cref="Vector2Byte"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.X);
            bw.Write(this.Y);
        }
    }

    /// <summary>
    /// Represents a Vector containing three bytes
    /// </summary>
    public class Vector3Byte
    {
        /// <summary>
        /// The X component
        /// </summary>
        public byte X { get; set; }
        /// <summary>
        /// The Y component
        /// </summary>
        public byte Y { get; set; }
        /// <summary>
        /// The Z component
        /// </summary>
        public byte Z { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Vector3Byte"/> instance
        /// </summary>
        public Vector3Byte(byte x, byte y, byte z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3Byte"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public Vector3Byte(BinaryReader br)
        {
            this.X = br.ReadByte();
            this.Y = br.ReadByte();
            this.Z = br.ReadByte();
        }

        /// <summary>
        /// Writes this <see cref="Vector3Byte"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.X);
            bw.Write(this.Y);
            bw.Write(this.Z);
        }
    }

    /// <summary>
    /// Represents a Vector containing four bytes
    /// </summary>
    public class Vector4Byte
    {
        /// <summary>
        /// The X component
        /// </summary>
        public byte X { get; set; }
        /// <summary>
        /// The Y component
        /// </summary>
        public byte Y { get; set; }
        /// <summary>
        /// The Z component
        /// </summary>
        public byte Z { get; set; }
        /// <summary>
        /// The W component
        /// </summary>
        public byte W { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Vector4Byte"/> instance
        /// </summary>
        public Vector4Byte(byte x, byte y, byte z, byte w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector4Byte"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public Vector4Byte(BinaryReader br)
        {
            this.X = br.ReadByte();
            this.Y = br.ReadByte();
            this.Z = br.ReadByte();
            this.W = br.ReadByte();
        }

        /// <summary>
        /// Writes this <see cref="Vector4Byte"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.X);
            bw.Write(this.Y);
            bw.Write(this.Z);
            bw.Write(this.W);
        }
    }

    #region Colors
    /// <summary>
    /// Represents a 24-bit RGB Color using bytes
    /// </summary>
    public class ColorRGBVector3Byte
    {
        /// <summary>
        /// Red
        /// </summary>
        public byte R { get; set; }
        /// <summary>
        /// Green
        /// </summary>
        public byte G { get; set; }
        /// <summary>
        /// Blue
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ColorRGBVector3Byte"/> instance
        /// </summary>
        public ColorRGBVector3Byte(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        /// <summary>
        /// Initializes a new <see cref="ColorRGBVector3Byte"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br"></param>
        public ColorRGBVector3Byte(BinaryReader br)
        {
            this.R = br.ReadByte();
            this.G = br.ReadByte();
            this.B = br.ReadByte();
        }

        /// <summary>
        /// Writes this <see cref="ColorRGBVector3Byte"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.R);
            bw.Write(this.G);
            bw.Write(this.B);
        }

        /// <summary>
        /// Writes this <see cref="ColorRGBVector3Byte"/> into a <see cref="StreamWriter"/> using the specified format
        /// </summary>
        /// <param name="sw">The <see cref="StreamWriter"/> to write to</param>
        /// <param name="format">Format that should be used for writing</param>
        public void Write(StreamWriter sw, string format)
        {
            sw.Write(string.Format(format, this.R, this.G, this.B));
        }
    }

    /// <summary>
    /// Represents a 128-bit RGBA Color using floats
    /// </summary>
    public class ColorRGBAVector4
    {
        /// <summary>
        /// Red
        /// </summary>
        public float R { get; set; }
        /// <summary>
        /// Green
        /// </summary>
        public float G { get; set; }
        /// <summary>
        /// Blue
        /// </summary>
        public float B { get; set; }
        /// <summary>
        /// Alpha
        /// </summary>
        public float A { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ColorRGBAVector4"/>
        /// </summary>
        public ColorRGBAVector4(float r, float g, float b, float a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// Initializes a new <see cref="ColorRGBAVector4"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public ColorRGBAVector4(BinaryReader br)
        {
            this.R = br.ReadSingle();
            this.G = br.ReadSingle();
            this.B = br.ReadSingle();
            this.A = br.ReadSingle();
        }

        /// <summary>
        /// Writes this <see cref="ColorRGBAVector4"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.R);
            bw.Write(this.G);
            bw.Write(this.B);
            bw.Write(this.A);
        }

        /// <summary>
        /// Writes this <see cref="ColorRGBAVector4"/> into a <see cref="StreamWriter"/> using the specified format
        /// </summary>
        /// <param name="sw">The <see cref="StreamWriter"/> to write to</param>
        /// <param name="format">Format that should be used for writing</param>
        public void Write(StreamWriter sw, string format)
        {
            sw.Write(string.Format(format, this.R, this.G, this.B, this.A));
        }
    }

    /// <summary>
    /// Represents a 32-bit RGBA Color using bytes
    /// </summary>
    public class ColorRGBAVector4Byte
    {
        /// <summary>
        /// Red
        /// </summary>
        public byte R { get; set; }
        /// <summary>
        /// Green
        /// </summary>
        public byte G { get; set; }
        /// <summary>
        /// Blue
        /// </summary>
        public byte B { get; set; }
        /// <summary>
        /// Alpha
        /// </summary>
        public byte A { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ColorRGBAVector4Byte"/>
        /// </summary>
        public ColorRGBAVector4Byte(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// Initializes a new <see cref="ColorRGBAVector4Byte"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public ColorRGBAVector4Byte(BinaryReader br)
        {
            this.R = br.ReadByte();
            this.G = br.ReadByte();
            this.B = br.ReadByte();
            this.A = br.ReadByte();
        }

        /// <summary>
        /// Writes this <see cref="ColorRGBAVector4Byte"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.R);
            bw.Write(this.G);
            bw.Write(this.B);
            bw.Write(this.A);
        }

        /// <summary>
        /// Writes this <see cref="ColorRGBAVector4Byte"/> into a <see cref="StreamWriter"/> using the specified format
        /// </summary>
        /// <param name="sw">The <see cref="StreamWriter"/> to write to</param>
        /// <param name="format">Format that should be used for writing</param>
        public void Write(StreamWriter sw, string format)
        {
            sw.Write(string.Format(format, this.R, this.G, this.B, this.A));
        }
    }

    /// <summary>
    /// Represents a 32-bit BGRA Color using bytes
    /// </summary>
    public class ColorBGRAVector4Byte
    {
        /// <summary>
        /// Blue
        /// </summary>
        public byte B { get; set; }
        /// <summary>
        /// Green
        /// </summary>
        public byte G { get; set; }
        /// <summary>
        /// Red
        /// </summary>
        public byte R { get; set; }
        /// <summary>
        /// Alpha
        /// </summary>
        public byte A { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ColorBGRAVector4Byte"/>
        /// </summary>
        public ColorBGRAVector4Byte(byte b, byte g, byte r, byte a)
        {
            this.B = b;
            this.G = g;
            this.R = r;
            this.A = a;
        }

        /// <summary>
        /// Initializes a new <see cref="ColorBGRAVector4Byte"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public ColorBGRAVector4Byte(BinaryReader br)
        {
            this.B = br.ReadByte();
            this.G = br.ReadByte();
            this.R = br.ReadByte();
            this.A = br.ReadByte();
        }

        /// <summary>
        /// Writes this <see cref="ColorBGRAVector4Byte"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.B);
            bw.Write(this.G);
            bw.Write(this.R);
            bw.Write(this.A);
        }

        /// <summary>
        /// Writes this <see cref="ColorBGRAVector4Byte"/> into a <see cref="StreamWriter"/> using the specified format
        /// </summary>
        /// <param name="sw">The <see cref="StreamWriter"/> to write to</param>
        /// <param name="format">Format that should be used for writing</param>
        public void Write(StreamWriter sw, string format)
        {
            sw.Write(string.Format(format, this.B, this.G, this.R, this.A));
        }
    }
    #endregion
    #region Structures
    /// <summary>
    /// Represents an Axis-Aligned Bounding Box
    /// </summary>
    public class R3DBoundingBox
    {
        /// <summary>
        /// The Org component
        /// </summary>
        public Vector3 Org { get; set; }
        /// <summary>
        /// The Size component
        /// </summary>
        public Vector3 Size { get; set; }

        /// <summary>
        /// Initializes a new <see cref="R3DBoundingBox"/> instance
        /// </summary>
        /// <param name="org"></param>
        /// <param name="size"></param>
        public R3DBoundingBox(Vector3 org, Vector3 size)
        {
            this.Org = org;
            this.Size = size;
        }

        /// <summary>
        /// Initializes a new <see cref="R3DBoundingBox"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public R3DBoundingBox(BinaryReader br)
        {
            this.Org = new Vector3(br);
            this.Size = new Vector3(br);
        }

        /// <summary>
        /// Writes this <see cref="R3DBoundingBox"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            this.Org.Write(bw);
            this.Size.Write(bw);
        }
    }

    /// <summary>
    /// Represents an Axis-Aligned Bounding Box
    /// </summary>
    public class R3DBox
    {
        /// <summary>
        /// The Min component
        /// </summary>
        public Vector3 Min { get; private set; }
        /// <summary>
        /// The Max component
        /// </summary>
        public Vector3 Max { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="R3DBox"/> instance
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public R3DBox(Vector3 min, Vector3 max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Initializes a new <see cref="R3DBox"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public R3DBox(BinaryReader br)
        {
            this.Min = new Vector3(br);
            this.Max = new Vector3(br);
        }

        /// <summary>
        /// Writes this <see cref="R3DBox"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw"></param>
        public void Write(BinaryWriter bw)
        {
            this.Min.Write(bw);
            this.Max.Write(bw);
        }

        /// <summary>
        /// Calculates the proportions of this <see cref="R3DBox"/>
        /// </summary>
        public Vector3 GetProportions()
        {
            return this.Max - this.Min;
        }

        /// <summary>
        /// Determines wheter this <see cref="R3DBox"/> contains the <see cref="Vector3"/> <paramref name="point"/>
        /// </summary>
        /// <param name="point">The containing point</param>
        /// <returns>Wheter this <see cref="R3DBox"/> contains the <see cref="Vector3"/> <paramref name="point"/></returns>
        public bool ContainsPoint(Vector3 point)
        {
            return ((point.X >= Min.X) && (point.X <= Max.X) && (point.Y >= Min.Y) && (point.Y <= Max.Y) && (point.Z >= Min.Z) && (point.Z <= Max.Z));
        }
    }

    /// <summary>
    /// Represents a Sphere
    /// </summary>
    public class R3DSphere
    {
        /// <summary>
        /// The position of the <see cref="R3DSphere"/>
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// The radius of the <see cref="R3DSphere"/>
        /// </summary>
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
            this.Position = new Vector3(br);
            this.Radius = br.ReadSingle();
        }

        /// <summary>
        /// Writes this <see cref="R3DSphere"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            Position.Write(bw);
            bw.Write(Radius);
        }
    }

    /// <summary>
    /// Represents a transformation Matrix
    /// </summary>
    public class R3DMatrix44
    {
        public float m11 { get; private set; }
        public float m12 { get; private set; }
        public float m13 { get; private set; }
        public float m14 { get; private set; }
        public float m21 { get; private set; }
        public float m22 { get; private set; }
        public float m23 { get; private set; }
        public float m24 { get; private set; }
        public float m31 { get; private set; }
        public float m32 { get; private set; }
        public float m33 { get; private set; }
        public float m34 { get; private set; }
        public float m41 { get; private set; }
        public float m42 { get; private set; }
        public float m43 { get; private set; }
        public float m44 { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="R3DMatrix44"/> instance
        /// </summary>
        public R3DMatrix44()
        {
            this.Clear();
        }

        /// <summary>
        /// Initializes a new <see cref="R3DMatrix44"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br"></param>
        public R3DMatrix44(BinaryReader br)
        {
            this.m11 = br.ReadSingle();
            this.m12 = br.ReadSingle();
            this.m13 = br.ReadSingle();
            this.m14 = br.ReadSingle();
            this.m21 = br.ReadSingle();
            this.m22 = br.ReadSingle();
            this.m23 = br.ReadSingle();
            this.m24 = br.ReadSingle();
            this.m31 = br.ReadSingle();
            this.m32 = br.ReadSingle();
            this.m33 = br.ReadSingle();
            this.m34 = br.ReadSingle();
            this.m41 = br.ReadSingle();
            this.m42 = br.ReadSingle();
            this.m43 = br.ReadSingle();
            this.m44 = br.ReadSingle();
        }

        /// <summary>
        /// Clears all values
        /// </summary>
        public void Clear()
        {
            this.m11 = 0;
            this.m12 = 0;
            this.m13 = 0;
            this.m14 = 0;
            this.m21 = 0;
            this.m22 = 0;
            this.m23 = 0;
            this.m24 = 0;
            this.m31 = 0;
            this.m32 = 0;
            this.m33 = 0;
            this.m34 = 0;
            this.m41 = 0;
            this.m42 = 0;
            this.m43 = 0;
            this.m44 = 0;
        }

        /// <summary>
        /// Writes this <see cref="R3DMatrix44"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.m11);
            bw.Write(this.m12);
            bw.Write(this.m13);
            bw.Write(this.m14);
            bw.Write(this.m21);
            bw.Write(this.m22);
            bw.Write(this.m23);
            bw.Write(this.m24);
            bw.Write(this.m31);
            bw.Write(this.m32);
            bw.Write(this.m33);
            bw.Write(this.m34);
            bw.Write(this.m41);
            bw.Write(this.m42);
            bw.Write(this.m43);
            bw.Write(this.m44);
        }
    }
    #endregion
}