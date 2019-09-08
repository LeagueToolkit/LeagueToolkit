using System.IO;

namespace Fantome.Libraries.League.Helpers.Structures
{
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
        /// Creates a clone of a <see cref="Vector3Byte"/> object
        /// </summary>
        /// <param name="vector3Byte">The <see cref="Vector3Byte"/> to clone</param>
        public Vector3Byte(Vector3Byte vector3Byte)
        {
            this.X = vector3Byte.X;
            this.Y = vector3Byte.Y;
            this.Z = vector3Byte.Z;
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
}
