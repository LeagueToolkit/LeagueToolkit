using Fantome.Libraries.League.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BinTree
    {
        public List<string> Dependencies { get; private set; } = new();
        public List<BinTreeObject> Objects { get; private set; } = new();

        public BinTree(string fileLocation) : this(File.OpenRead(fileLocation))
        {

        }
        public BinTree(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "PROP")
                {
                    throw new InvalidFileSignatureException();
                }

                uint version = br.ReadUInt32();
                if (version != 1 && version != 2)
                {
                    throw new UnsupportedFileVersionException();
                }

                if (version >= 2)
                {
                    uint dependencyCount = br.ReadUInt32();
                    for (int i = 0; i < dependencyCount; i++)
                    {
                        this.Dependencies.Add(Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt16())));
                    }
                }

                uint objectCount = br.ReadUInt32();
                for (int i = 0; i < objectCount; i++)
                {
                    this.Objects.Add(BinTreeObject.ReadHeader(br));
                }

                foreach (BinTreeObject treeObject in this.Objects)
                {
                    treeObject.ReadData(br);
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.OpenWrite(fileLocation));
        }
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(Encoding.ASCII.GetBytes("PROP"));
                bw.Write((uint)2); // version

                bw.Write(this.Dependencies.Count);
                foreach(string dependency in this.Dependencies)
                {
                    bw.Write((ushort)dependency.Length);
                    bw.Write(Encoding.UTF8.GetBytes(dependency));
                }

                bw.Write(this.Objects.Count);
                foreach (BinTreeObject treeObject in this.Objects)
                {
                    treeObject.WriteHeader(bw);
                }
                foreach (BinTreeObject treeObject in this.Objects)
                {
                    treeObject.WriteContent(bw);
                }
            }
        }
    }

    public interface IBinTreeParent { }
}
