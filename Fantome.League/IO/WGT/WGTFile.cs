using Fantome.League.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.League.IO.WGT
{
    public class WGTFile
    {
        public UInt32 SkeletonID { get; private set; }
        public List<WGTWeight> Weights { get; private set; } = new List<WGTWeight>();
        public WGTFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                if (Magic != "r3d2wght")
                    throw new InvalidFileMagicException();

                UInt32 Version = br.ReadUInt32();
                if (Version != 1)
                    throw new UnsupportedFileVersionException();

                this.SkeletonID = br.ReadUInt32();
                UInt32 WeightCount = br.ReadUInt32();
                
                for(int i = 0; i < WeightCount; i++)
                {
                    this.Weights.Add(new WGTWeight(br));
                }
            }
        }
        public void Write(string Location)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(Location)))
            {
                bw.Write("r3d2wght".ToCharArray());
                bw.Write((UInt32)1);
                bw.Write(this.SkeletonID);
                bw.Write((UInt32)this.Weights.Count);

                foreach(WGTWeight Weight in this.Weights)
                {
                    Weight.Write(bw);
                }
            }
        }
    }
}
