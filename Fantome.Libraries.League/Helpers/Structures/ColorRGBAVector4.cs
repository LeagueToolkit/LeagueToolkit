using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
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

        public ColorRGBAVector4()
        {

        }

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
        /// Creates a clone of a <see cref="ColorRGBAVector4"/> object
        /// </summary>
        /// <param name="colorRGBAVector4">The <see cref="ColorRGBAVector4"/> to clone</param>
        public ColorRGBAVector4(ColorRGBAVector4 colorRGBAVector4)
        {
            this.R = colorRGBAVector4.R;
            this.G = colorRGBAVector4.G;
            this.B = colorRGBAVector4.B;
            this.A = colorRGBAVector4.A;
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
}