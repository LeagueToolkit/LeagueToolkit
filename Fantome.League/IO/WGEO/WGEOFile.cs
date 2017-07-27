using Fantome.Libraries.League.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.WGEO
{
    [DebuggerDisplay("[ Version: {Version} ]")]
    public class WGEOFile
    {
        public UInt32 Version { get; private set; }
        public List<WGEOModel> Models { get; private set; } = new List<WGEOModel>();
        public WGEOBucketGeometry BucketGeometry { get; private set; }

        public WGEOFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (Magic != "WGEO")
                    throw new InvalidFileMagicException();

                this.Version = br.ReadUInt32();
                if (this.Version != 5)
                    throw new UnsupportedFileVersionException();

                UInt32 ModelCount = br.ReadUInt32();
                UInt32 FaceCount = br.ReadUInt32();

                for(int i = 0; i < ModelCount; i++)
                {
                    Models.Add(new WGEOModel(br));
                }

                BucketGeometry = new WGEOBucketGeometry(br);
            }
        }

        public void Write(string Location)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(Location)))
            {
                UInt32 faceCount = 0;
                bw.Write("WGEO".ToCharArray());
                bw.Write((UInt32)5);
                bw.Write((UInt32)this.Models.Count);
                foreach (WGEOModel Model in this.Models)
                {
                    faceCount += (uint)Model.Indices.Count / 3;
                }
                bw.Write(faceCount);

                foreach (WGEOModel Model in this.Models)
                {
                    Model.Write(bw);
                }

                BucketGeometry.Write(bw);
            }
        }
    }
}
