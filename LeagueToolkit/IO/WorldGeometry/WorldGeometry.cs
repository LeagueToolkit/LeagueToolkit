using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Structures.BucketGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueToolkit.IO.WorldGeometry
{
    /// <summary>
    /// Represents a World Geometry (WGEO) file
    /// </summary>
    public class WorldGeometry
    {
        /// <summary>
        /// Models of this <see cref="WorldGeometry"/>
        /// </summary>
        public List<WorldGeometryModel> Models { get; set; } = new List<WorldGeometryModel>();
        /// <summary>
        /// <see cref="WGEOBucketGeometry"/> of this <see cref="WorldGeometry"/>
        /// </summary>
        public BucketGrid BucketGrid { get; set; }

        /// <summary>
        /// Initializes a new empty <see cref="WorldGeometry"/>
        /// </summary>
        public WorldGeometry() { }

        /// <summary>
        /// Initializes a new <see cref="WorldGeometry"/>
        /// </summary>
        /// <param name="models">Models of this <see cref="WorldGeometry"/></param>
        /// <param name="bucketGrid"><see cref="BucketGrid"/> of this <see cref="WorldGeometry"/></param>
        public WorldGeometry(List<WorldGeometryModel> models, BucketGrid bucketGrid)
        {
            this.Models = models;
            this.BucketGrid = bucketGrid;
        }

        /// <summary>
        /// Initalizes a new <see cref="WorldGeometry"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">Location to read from</param>
        public WorldGeometry(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initializes a new <see cref="WorldGeometry"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        public WorldGeometry(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "WGEO")
                {
                    throw new InvalidFileSignatureException();
                }

                uint version = br.ReadUInt32();
                if (version != 5 && version != 4)
                {
                    throw new UnsupportedFileVersionException();
                }

                uint modelCount = br.ReadUInt32();
                uint faceCount = br.ReadUInt32();

                for(int i = 0; i < modelCount; i++)
                {
                    this.Models.Add(new WorldGeometryModel(br));
                }

                if (version == 5)
                {
                    this.BucketGrid = new BucketGrid(br);
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="WorldGeometry"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">Location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="WorldGeometry"/> into the specified stream
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream, bool leaveOpen = false)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen))
            {
                uint faceCount = 0;
                bw.Write(Encoding.ASCII.GetBytes("WGEO"));
                bw.Write(this.BucketGrid == null ? 4 : 5);
                bw.Write(this.Models.Count);
                foreach (WorldGeometryModel model in this.Models)
                {
                    faceCount += (uint)model.Indices.Count / 3;
                }
                bw.Write(faceCount);

                foreach (WorldGeometryModel model in this.Models)
                {
                    model.Write(bw);
                }

                this.BucketGrid?.Write(bw);
            }
        }
    }
}
