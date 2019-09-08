using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.WorldGeometry
{
    /// <summary>
    /// Represents a World Geometry (WGEO) file
    /// </summary>
    public class WGEOFile
    {
        /// <summary>
        /// Models of this <see cref="WGEOFile"/>
        /// </summary>
        public List<WGEOModel> Models { get; set; } = new List<WGEOModel>();
        /// <summary>
        /// <see cref="WGEOBucketGeometry"/> of this <see cref="WGEOFile"/>
        /// </summary>
        public WGEOBucketGeometry BucketGeometry { get; set; }

        /// <summary>
        /// Initializes a new empty <see cref="WGEOFile"/>
        /// </summary>
        public WGEOFile() { }

        /// <summary>
        /// Initializes a new <see cref="WGEOFile"/>
        /// </summary>
        /// <param name="models">Models of this <see cref="WGEOFile"/></param>
        /// <param name="bucketGeometry"><see cref="WGEOBucketGeometry"/> of this <see cref="WGEOFile"/></param>
        public WGEOFile(List<WGEOModel> models, WGEOBucketGeometry bucketGeometry)
        {
            this.Models = models;
            this.BucketGeometry = bucketGeometry;
        }

        /// <summary>
        /// Initalizes a new <see cref="WGEOFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">Location to read from</param>
        public WGEOFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initializes a new <see cref="WGEOFile"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        public WGEOFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "WGEO")
                {
                    throw new Exception("This is not a valid WGEO file");
                }

                uint version = br.ReadUInt32();
                if (version != 5 && version != 4)
                {
                    throw new Exception("This WGEO file version is not supported");
                }

                uint modelCount = br.ReadUInt32();
                uint faceCount = br.ReadUInt32();

                for(int i = 0; i < modelCount; i++)
                {
                    this.Models.Add(new WGEOModel(br));
                }

                if (version == 5)
                {
                    this.BucketGeometry = new WGEOBucketGeometry(br);
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="WGEOFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">Location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="WGEOFile"/> into the specified stream
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                uint faceCount = 0;
                bw.Write(Encoding.ASCII.GetBytes("WGEO"));
                bw.Write(this.BucketGeometry == null ? 4 : 5);
                bw.Write(this.Models.Count);
                foreach (WGEOModel model in this.Models)
                {
                    faceCount += (uint)model.Indices.Count / 3;
                }
                bw.Write(faceCount);

                foreach (WGEOModel model in this.Models)
                {
                    model.Write(bw);
                }

                this.BucketGeometry?.Write(bw);
            }
        }
    }
}
