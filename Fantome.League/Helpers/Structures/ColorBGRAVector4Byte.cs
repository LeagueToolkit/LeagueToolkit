using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
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
        /// Creates a clone of a <see cref="ColorBGRAVector4Byte"/> object
        /// </summary>
        /// <param name="colorRGBAVector4Byte">The <see cref="ColorBGRAVector4Byte"/> to clone</param>
        public ColorBGRAVector4Byte(ColorBGRAVector4Byte colorRGBAVector4Byte)
        {
            this.R = colorRGBAVector4Byte.R;
            this.G = colorRGBAVector4Byte.G;
            this.B = colorRGBAVector4Byte.B;
            this.A = colorRGBAVector4Byte.A;
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
            sw.Write(format, this.B, this.G, this.R, this.A);
        }
    }
}
