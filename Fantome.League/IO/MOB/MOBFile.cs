using Fantome.League.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.League.IO.MOB
{
    public class MOBFile
    {
        public List<MOBObject> Objects { get; private set; } = new List<MOBObject>();
        public MOBFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (Magic != "OPAM")
                    throw new InvalidFileMagicException();

                UInt32 Version = br.ReadUInt32();
                if (Version != 2)
                    throw new UnsupportedFileVersionException();

                UInt32 ObjectCount = br.ReadUInt32();
                br.ReadUInt32();

                for(int i = 0; i < ObjectCount; i++)
                {
                    this.Objects.Add(new MOBObject(br));
                }
            }
        }
        public void Write(string Location)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(Location)))
            {
                bw.Write("OPAM".ToCharArray());
                bw.Write((UInt32)2);
                bw.Write((UInt32)this.Objects.Count);
                bw.Write((UInt32)0);

                foreach(MOBObject Object in this.Objects)
                {
                    Object.Write(bw);
                }
            }
        }
    }
}
