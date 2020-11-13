using LeagueToolkit.Helpers.Hashing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace LeagueToolkit.IO.PropertyBin
{
    public class BinTreeObject : IBinTreeParent, IEquatable<BinTreeObject>
    {
        public uint MetaClassHash { get; private set; }
        public uint PathHash { get; private set; }

        public ReadOnlyCollection<BinTreeProperty> Properties { get; }
        private List<BinTreeProperty> _properties = new();

        internal BinTreeObject(uint metaClassHash)
        {
            this.MetaClassHash = metaClassHash;
            this.Properties = this._properties.AsReadOnly();
        }
        public BinTreeObject(string metaClass, string path, ICollection<BinTreeProperty> properties)
            : this(Fnv1a.HashLower(metaClass), Fnv1a.HashLower(path), properties)
        {

        }
        public BinTreeObject(string metaClass, uint pathHash, ICollection<BinTreeProperty> properties)
            : this(Fnv1a.HashLower(metaClass), pathHash, properties)
        {

        }
        public BinTreeObject(uint metaClassHash, string path, ICollection<BinTreeProperty> properties)
            : this(metaClassHash, Fnv1a.HashLower(path), properties)
        {

        }
        public BinTreeObject(uint metaClassHash, uint pathHash, ICollection<BinTreeProperty> properties)
        {
            this.MetaClassHash = metaClassHash;
            this.PathHash = pathHash;
            this._properties = new List<BinTreeProperty>(properties.Select(x => 
            {
                // Assign this as a parent of the properties
                x.Parent = this;
                return x;
            }));
            this.Properties = this._properties.AsReadOnly();
        }

        internal void ReadData(BinaryReader br)
        {
            uint size = br.ReadUInt32();
            this.PathHash = br.ReadUInt32();

            ushort propertyCount = br.ReadUInt16();
            for (int i = 0; i < propertyCount; i++)
            {
                this._properties.Add(BinTreeProperty.Read(br, this, null));
            }
        }

        internal void WriteHeader(BinaryWriter bw)
        {
            bw.Write(this.MetaClassHash);
        }
        internal void WriteContent(BinaryWriter bw)
        {
            bw.Write(GetSize());
            bw.Write(this.PathHash);

            bw.Write((ushort)this._properties.Count);
            foreach (BinTreeProperty property in this._properties)
            {
                property.Write(bw, true);
            }
        }

        public void AddProperty(BinTreeProperty property)
        {
            if (this._properties.Any(x => x.NameHash == property.NameHash))
            {
                throw new InvalidOperationException("A property with the same name already exists");
            }
            else
            {
                property.Parent = this;
                this._properties.Add(property);
            }
        }
        public void RemoveProperty(uint nameHash)
        {
            if (this._properties.FirstOrDefault(x => x.NameHash == nameHash) is BinTreeProperty property)
            {
                this._properties.Remove(property);
            }
            else throw new ArgumentException("Failed to find a property with the specified name hash", nameof(nameHash));
        }

        private int GetSize()
        {
            int size = 4 + 2;
            foreach (BinTreeProperty property in this._properties)
            {
                size += property.GetSize(true);
            }

            return size;
        }

        public bool Equals(BinTreeObject other)
        {
            return this.PathHash == other.PathHash &&
                this.MetaClassHash == other.MetaClassHash &&
                this._properties.SequenceEqual(other._properties);
        }
    }
}
