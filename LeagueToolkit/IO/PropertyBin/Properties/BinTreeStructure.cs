using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public class BinTreeStructure : BinTreeProperty, IBinTreeParent
    {
        public override BinPropertyType Type => BinPropertyType.Structure;
        public uint MetaClassHash { get; private set; }

        public ReadOnlyCollection<BinTreeProperty> Properties { get; }
        protected List<BinTreeProperty> _properties = new();

        public BinTreeStructure(IBinTreeParent parent, uint nameHash, uint metaClassHash, IEnumerable<BinTreeProperty> properties)
            : base(parent, nameHash)
        {
            this.MetaClassHash = metaClassHash;

            // Verify properties
            foreach (BinTreeProperty property in properties)
            {
                if (properties.Any(x => x.NameHash == property.NameHash && x != property))
                {
                    throw new ArgumentException($"Found two properties with the same name hash: {property.NameHash}");
                }

                property.Parent = this;
            }

            this._properties = new List<BinTreeProperty>(properties);
            this.Properties = this._properties.AsReadOnly();
        }
        internal BinTreeStructure(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.MetaClassHash = br.ReadUInt32();
            if (this.MetaClassHash == 0) {
                this.Properties = this._properties.AsReadOnly();
                return; // Empty structure
            }

            uint size = br.ReadUInt32();
            ushort propertyCount = br.ReadUInt16();
            for (int i = 0; i < propertyCount; i++)
            {
                this._properties.Add(BinTreeProperty.Read(br, this));
            }

            this.Properties = this._properties.AsReadOnly();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.MetaClassHash);
            if (this.MetaClassHash == 0) return; // empty

            bw.Write(GetContentSize());
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

            this._properties.Add(property);
        }
        public bool RemoveProperty(BinTreeProperty property)
        {
            if (property is not null)
            {
                return this._properties.Remove(property);
            }
            else return false;
        }
        public bool RemoveProperty(uint nameHash)
        {
            return RemoveProperty(this._properties.FirstOrDefault(x => x.NameHash == nameHash));
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? HEADER_SIZE : 0;
            if (this.MetaClassHash == 0) // empty struct
            {
                return size + 4;
            }
            else
            {
                return size + 4 + 4 + GetContentSize();
            }
        }
        private int GetContentSize()
        {
            int size = 2;
            foreach (BinTreeProperty property in this._properties)
            {
                size += property.GetSize(true);
            }
            return size;
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeStructure otherProperty && other is not BinTreeEmbedded)
            {
                if (this.MetaClassHash != otherProperty.MetaClassHash) return false;
                if (this._properties.Count != otherProperty._properties.Count) return false;

                for (int i = 0; i < this._properties.Count; i++)
                {
                    if (!this._properties[i].Equals(otherProperty._properties[i])) return false;
                }
            }

            return true;
        }
    }
}
