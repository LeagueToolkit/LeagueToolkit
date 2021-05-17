using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace LeagueToolkit.IO.PropertyBin
{
    public class BinTree
    {
        public bool IsOverride { get; private set; }

        public List<string> Dependencies { get; private set; } = new();

        public ReadOnlyCollection<BinTreeObject> Objects { get; }
        private List<BinTreeObject> _objects = new();

        public BinTree()
        {
            this.Objects = this._objects.AsReadOnly();
        }
        public BinTree(string fileLocation) : this(File.OpenRead(fileLocation))
        {

        }
        public BinTree(Stream stream) : this()
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "PROP" && magic != "PTCH")
                {
                    throw new InvalidFileSignatureException();
                }

                if (magic == "PTCH")
                {
                    this.IsOverride = true;

                    ulong unknown = br.ReadUInt64();
                    magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                    if (magic != "PROP") throw new InvalidFileSignatureException("Expected PROP section after PTCH, got: " + magic);
                }

                uint version = br.ReadUInt32();
                if (version != 1 && version != 2 && version != 3)
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
                    uint objectMetaClass = br.ReadUInt32();
                    this._objects.Add(new BinTreeObject(objectMetaClass));
                }

                foreach (BinTreeObject treeObject in this._objects)
                {
                    treeObject.ReadData(br);
                }
            }
        }

        public void Write(string fileLocation, Version version)
        {
            Write(File.OpenWrite(fileLocation), version);
        }
        public void Write(Stream stream, Version version, bool leaveOpen = false)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen))
            {
                bw.Write(Encoding.ASCII.GetBytes("PROP"));
                bw.Write(version.PackToInt()); // version

                if(version.Major >= 2)
                {
                    bw.Write(this.Dependencies.Count);
                    foreach (string dependency in this.Dependencies)
                    {
                        bw.Write((ushort)dependency.Length);
                        bw.Write(Encoding.UTF8.GetBytes(dependency));
                    }
                }

                bw.Write(this._objects.Count);
                foreach (BinTreeObject treeObject in this._objects)
                {
                    bw.Write(treeObject.MetaClassHash);
                }
                foreach (BinTreeObject treeObject in this._objects)
                {
                    treeObject.WriteContent(bw);
                }
            }
        }

        public void AddObject(BinTreeObject treeObject)
        {
            if (this._objects.Any(x => x.PathHash == treeObject.PathHash))
            {
                throw new InvalidOperationException("An object with the same path already exists");
            }
            else
            {
                this._objects.Add(treeObject);
            }
        }
        public void RemoveObject(uint pathHash)
        {
            if (this._objects.FirstOrDefault(x => x.PathHash == pathHash) is BinTreeObject treeObject)
            {
                this._objects.Remove(treeObject);
            }
            else throw new ArgumentException("Failed to find an object with the specified path hash", nameof(pathHash));
        }
    }

    public interface IBinTreeParent { }
}
