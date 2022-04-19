﻿using LeagueToolkit.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using LeagueToolkit.Helpers.Hashing;
using LeagueToolkit.IO.PropertyBin.Properties;

namespace LeagueToolkit.IO.PropertyBin
{
    public class BinTree
    {
        public bool IsOverride { get; private set; }

        public List<string> Dependencies { get; private set; } = new();

        public ReadOnlyCollection<BinTreeObject> Objects { get; }
        private readonly List<BinTreeObject> _objects = new();

        public ReadOnlyCollection<BinTreePatchObject> PatchObjects { get; }
        private readonly List<BinTreePatchObject> _patchObjects = new();
        private uint _patchObjectCount;

        public uint Version { get; }

        public BinTree()
        {
            this.Objects = this._objects.AsReadOnly();
            this.PatchObjects = this._patchObjects.AsReadOnly();
        }
        public BinTree(string fileLocation) : this(File.OpenRead(fileLocation))
        {

        }
        public BinTree(Stream stream) : this()
        {
            using BinaryReader br = new(stream);

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

            this.Version = br.ReadUInt32();
            if (this.Version != 1 && this.Version != 2 && this.Version != 3)
            {
                throw new UnsupportedFileVersionException();
            }

            if (this.Version >= 2)
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

            if (this.Version >= 3 && this.IsOverride)
            {
                ReadPatchSection(br);
            }
        }

        private void ReadPatchSection(BinaryReader br)
        {
            this._patchObjectCount = br.ReadUInt32();
            for (int i = 0; i < this._patchObjectCount; i++)
            {
                uint pathHash = br.ReadUInt32();
                uint size = br.ReadUInt32();
                BinPropertyType type = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
                string objectPath = Encoding.ASCII.GetString(br.ReadBytes(br.ReadUInt16()));

                if (!this._patchObjects.Exists(o => o.PathHash == pathHash))
                {
                    this._patchObjects.Add(new BinTreePatchObject(pathHash));
                }

                IBinNestedProvider currentObject = this._patchObjects.Find(o => o.PathHash == pathHash);

                string[] parts = objectPath.Split('.');
                string valueName = parts[^1];
                foreach (string part in parts)
                {
                    if (part == valueName) break; // don't handle the value part
                    uint nameHash = Fnv1a.HashLower(part);
                    if (currentObject.Properties.Find(((BinTreeProperty property, string) p) => p.property.NameHash == nameHash) == (null, null))
                    {
                        currentObject.Properties.Add((new BinTreeNested(currentObject, nameHash), part));
                    }

                    (BinTreeProperty property, _) = currentObject.Properties.Find(((BinTreeProperty property, string) p) => p.property.NameHash == nameHash);
                    currentObject = (IBinNestedProvider)property;
                }

                // set the actual value to the deepest BinNested
                currentObject.Properties.Add((BinTreeProperty.Read(br, currentObject, type, Fnv1a.HashLower(valueName)), valueName));
            }
        }

        public void Write(string fileLocation) => Write(fileLocation, this.Version);
        public void Write(string fileLocation, uint version) => Write(File.Create(fileLocation), version);
        public void Write(Stream stream, bool leaveOpen = false) => Write(stream, this.Version, leaveOpen);
        public void Write(Stream stream, uint version, bool leaveOpen = false)
        {
            using BinaryWriter bw = new(stream, Encoding.UTF8, leaveOpen);

            if (this.IsOverride)
            {
                bw.Write(Encoding.ASCII.GetBytes("PTCH"));
                bw.Write(1); // unknown
                bw.Write(0); // unknown
            }

            bw.Write(Encoding.ASCII.GetBytes("PROP"));
            bw.Write(version); // version

            if (version >= 2)
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

            if (version >= 3 && this.IsOverride)
            {
                bw.Write(this._patchObjectCount);
                foreach (BinTreePatchObject patchObject in this._patchObjects)
                {
                    patchObject.Write(bw);
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
