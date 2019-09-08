using System;
using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a 32-bit RGBA Color using bytes
    /// </summary>
    public class ColorRGBAVector4Byte : IEquatable<ColorRGBAVector4Byte>
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
        /// Creates a clone of a <see cref="ColorRGBAVector4Byte"/> object
        /// </summary>
        /// <param name="colorRGBVector4Byte">The <see cref="ColorRGBAVector4Byte"/> to clone</param>
        public ColorRGBAVector4Byte(ColorRGBAVector4Byte colorRGBVector4Byte)
        {
            this.R = colorRGBVector4Byte.R;
            this.G = colorRGBVector4Byte.G;
            this.B = colorRGBVector4Byte.B;
            this.A = colorRGBVector4Byte.A;
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

        public bool Equals(ColorRGBAVector4Byte other)
        {
            return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
        }
    }
}
