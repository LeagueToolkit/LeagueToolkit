using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
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
        /// Creates a clone of a <see cref="ColorRGBVector3Byte"/> object
        /// </summary>
        /// <param name="colorRGBVector3Byte">The <see cref="ColorRGBVector3Byte"/> to clone</param>
        public ColorRGBVector3Byte(ColorRGBVector3Byte colorRGBVector3Byte)
        {
            this.R = colorRGBVector3Byte.R;
            this.G = colorRGBVector3Byte.G;
            this.B = colorRGBVector3Byte.B;
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
}