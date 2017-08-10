using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.MOB
{
    public class MOBFile
    {
        public List<MOBObject> Objects { get; private set; }

        public MOBFile(List<MOBObject> objects)
        {
            this.Objects = objects;
        }

        public MOBFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "OPAM")
                {
                    throw new Exception("This is not a valid MOB file");
                }

                uint version = br.ReadUInt32();
                if (version != 2)
                {
                    throw new Exception("This version is not supported");
                }

                uint objectCount = br.ReadUInt32();
                br.ReadUInt32();

                for (int i = 0; i < objectCount; i++)
                {
                    this.Objects.Add(new MOBObject(br));
                }
            }
        }

        public void Write(string fileLocation)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileLocation)))
            {
                bw.Write(Encoding.ASCII.GetBytes("OPAM"));
                bw.Write((uint)2);
                bw.Write(this.Objects.Count);
                bw.Write((uint)0);

                foreach (MOBObject mobObject in this.Objects)
                {
                    mobObject.Write(bw);
                }
            }
        }
    }
}
