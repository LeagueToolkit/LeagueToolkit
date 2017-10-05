using System.IO;

namespace Fantome.Libraries.League.IO.WorldGeometry
{
    /// <summary>
    /// Represents a <see cref="WGEOBucket"/> inside of a <see cref="WGEOBucketGeometry"/>
    /// </summary>
    public class WGEOBucket
    {
        public float MaxStickOutX { get; private set; }
        public float MaxStickOutY { get; private set; }
        public uint StartIndex { get; private set; }
        public uint Vertex { get; private set; }
        public ushort InsideFaceCount { get; private set; }
        public ushort StickingOutFaceCount { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="WGEOBucket"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public WGEOBucket(BinaryReader br)
        {
            this.MaxStickOutX = br.ReadSingle();
            this.MaxStickOutY = br.ReadSingle();
            this.StartIndex = br.ReadUInt32();
            this.Vertex = br.ReadUInt32();
            this.InsideFaceCount = br.ReadUInt16();
            this.StickingOutFaceCount = br.ReadUInt16();
        }

        /// <summary>
        /// Writes this <see cref="WGEOBucket"/> into the specified <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.MaxStickOutX);
            bw.Write(this.MaxStickOutY);
            bw.Write(this.StartIndex);
            bw.Write(this.Vertex);
            bw.Write(this.InsideFaceCount);
            bw.Write(this.StickingOutFaceCount);
        }
    }
}
