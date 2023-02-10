using LeagueToolkit.Core.SceneGraph;
using LeagueToolkit.Helpers.Exceptions;
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
        public BucketedGeometry BucketGrid { get; set; }

        /// <summary>
        /// Initializes a new <see cref="WorldGeometry"/>
        /// </summary>
        /// <param name="models">Models of this <see cref="WorldGeometry"/></param>
        /// <param name="bucketGrid"><see cref="BucketGrid"/> of this <see cref="WorldGeometry"/></param>
        public WorldGeometry(List<WorldGeometryModel> models, BucketedGeometry bucketGrid)
        {
            this.Models = models;
            this.BucketGrid = bucketGrid;
        }

        public WorldGeometry(Stream stream)
        {
            using BinaryReader br = new(stream, Encoding.UTF8, true);

            string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (magic is not "WGEO")
                throw new InvalidFileSignatureException();

            // I'm pretty sure there was version 6 for a short while before the transition to mapgeo
            uint version = br.ReadUInt32();
            if (version is not (5 or 4))
                throw new UnsupportedFileVersionException();

            uint modelCount = br.ReadUInt32();
            uint faceCount = br.ReadUInt32();

            for (int i = 0; i < modelCount; i++)
            {
                this.Models.Add(new WorldGeometryModel(br));
            }

            if (version == 5)
            {
                this.BucketGrid = new BucketedGeometry(br);
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
