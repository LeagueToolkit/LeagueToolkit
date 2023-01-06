using System.IO;

namespace LeagueToolkit.Core.SceneGraph
{
    /// <summary>
    /// Represents a bucket inside of a <see cref="BucketedGeometry"/> scene graph
    /// </summary>
    public readonly struct GeometryBucket
    {
        /// <summary>Gets the Max Stick Out X bound</summary>
        public float MaxStickOutX { get; }

        /// <summary>Gets the Max Stick Out Z bound</summary>
        public float MaxStickOutZ { get; }

        /// <summary>Gets the bucket's start index</summary>
        public uint StartIndex { get; }

        /// <summary>Gets the bucket's base vertex</summary>
        public uint BaseVertex { get; }

        /// <summary>Gets the count of faces inside the bucket</summary>
        public ushort InsideFaceCount { get; }

        /// <summary>Gets the count of faces sticking out of the bucket</summary>
        public ushort StickingOutFaceCount { get; }

        internal GeometryBucket(BinaryReader br)
        {
            this.MaxStickOutX = br.ReadSingle();
            this.MaxStickOutZ = br.ReadSingle();
            this.StartIndex = br.ReadUInt32();
            this.BaseVertex = br.ReadUInt32();
            this.InsideFaceCount = br.ReadUInt16();
            this.StickingOutFaceCount = br.ReadUInt16();
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write(this.MaxStickOutX);
            bw.Write(this.MaxStickOutZ);
            bw.Write(this.StartIndex);
            bw.Write(this.BaseVertex);
            bw.Write(this.InsideFaceCount);
            bw.Write(this.StickingOutFaceCount);
        }
    }
}
