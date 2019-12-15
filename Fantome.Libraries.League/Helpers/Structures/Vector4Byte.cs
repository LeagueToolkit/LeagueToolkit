using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
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
        /// Creates a clone of a <see cref="Vector4Byte"/> object
        /// </summary>
        /// <param name="vector4Byte">The <see cref="Vector4Byte"/> to clone</param>
        public Vector4Byte(Vector4Byte vector4Byte)
        {
            this.X = vector4Byte.X;
            this.Y = vector4Byte.Y;
            this.Z = vector4Byte.Z;
            this.W = vector4Byte.W;
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
}