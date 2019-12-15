using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.WGT
{
    /// <summary>
    /// Represents a Weight File which is used in old League of Legends versions together with SCO
    /// </summary>
    public class WGTFile
    {
        /// <summary>
        /// Weights of this <see cref="WGTFile"/>
        /// </summary>
        public List<WGTWeight> Weights { get; private set; } = new List<WGTWeight>();

        /// <summary>
        /// Initializes a blank <see cref="WGTFile"/>
        /// </summary>
        public WGTFile() { }

        /// <summary>
        /// Initializes a new <see cref="WGTFile"/>
        /// </summary>
        /// <param name="weights">Weights of this <see cref="WGTFile"/></param>
        public WGTFile(List<WGTWeight> weights)
        {
            this.Weights = weights;
        }

        /// <summary>
        /// Initializes a new <see cref="WGTFile"/>
        /// </summary>
        /// <param name="weights">Weight data of this <see cref="WGTFile"/></param>
        /// <param name="boneIndices">Bone Index data of this <see cref="WGTFile"/></param>
        public WGTFile(List<Vector4> weights, List<Vector4Byte> boneIndices)
        {
            if (weights.Count != boneIndices.Count)
            {
                throw new Exception("Weights and Bone Indices have to be synchronized");
            }

            for (int i = 0; i < weights.Count; i++)
            {
                this.Weights.Add(new WGTWeight(weights[i], boneIndices[i]));
            }
        }

        /// <summary>
        /// Initializes a new <see cref="WGTFile"/> from the spcified location
        /// </summary>
        /// <param name="fileLocation">Location to read from</param>
        public WGTFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initializes a new <see cref="WGTFile"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        public WGTFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                if (magic != "r3d2wght")
                {
                    throw new Exception("This is not a valid WGT file");
                }

                uint version = br.ReadUInt32();
                if (version != 1)
                {
                    throw new Exception("This WGT file version is not supported");
                }

                uint skeletonId = br.ReadUInt32();
                uint weightCount = br.ReadUInt32();

                for (int i = 0; i < weightCount; i++)
                {
                    this.Weights.Add(new WGTWeight(br));
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="WGTFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">Location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="WGTFile"/> into a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> to write to</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(Encoding.ASCII.GetBytes("r3d2wght"));
                bw.Write(1);
                bw.Write(0);
                bw.Write(this.Weights.Count);

                foreach (WGTWeight weight in this.Weights)
                {
                    weight.Write(bw);
                }
            }
        }
    }
}
