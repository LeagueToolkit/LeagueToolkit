using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public class BinTreeContainer : BinTreeProperty, IBinTreeParent
    {
        public override BinPropertyType Type => BinPropertyType.Container;

        public BinPropertyType PropertiesType { get; private set; }

        public ReadOnlyCollection<BinTreeProperty> Properties { get; }
        protected List<BinTreeProperty> _properties = new();

        public BinTreeContainer(IBinTreeParent parent, uint nameHash, BinPropertyType propertiesType, IEnumerable<BinTreeProperty> properties)
            : base(parent, nameHash)
        {
            this.PropertiesType = propertiesType;

            // Verify properties
            foreach (BinTreeProperty property in properties)
            {
                if (property.Type != propertiesType)
                {
                    throw new ArgumentException($"Found a property with a different type ({property.Type}) than the specified one {propertiesType}");
                }

                property.Parent = this;
            }

            this._properties = new List<BinTreeProperty>(properties);
            this.Properties = this._properties.AsReadOnly();
        }
        internal BinTreeContainer(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.PropertiesType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
            uint size = br.ReadUInt32();

            uint valueCount = br.ReadUInt32();
            for (int i = 0; i < valueCount; i++)
            {
                this._properties.Add(BinTreeProperty.Read(br, this, this.PropertiesType));
            }

            this.Properties = this._properties.AsReadOnly();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            // Verify that all properties have the correct type and parent
            foreach (BinTreeProperty property in this._properties)
            {
                if (property.Type != this.PropertiesType) throw new InvalidOperationException("Found a Property with an invalid Type");
                if (property.Parent != this) throw new InvalidOperationException("Found a Property with an invalid Parent");
            }

            bw.Write((byte)BinUtilities.PackType(this.PropertiesType));
            bw.Write(GetContentSize());
            bw.Write(this._properties.Count);

            foreach (BinTreeProperty property in this._properties)
            {
                if (property.Type != this.PropertiesType) throw new InvalidOperationException("Found a Property with an invalid Type");
                if (property.Parent != this) throw new InvalidOperationException("Found a Property with an invalid Parent");

                property.Write(bw, false);
            }
        }

        public void Add(BinTreeProperty property)
        {
            if (property.Type != this.PropertiesType)
            {
                throw new ArgumentException($"Property type ({property.Type}) does not match container prooperties type ({this.Type})", nameof(property));
            }
            else if (this._properties.Any(x => x.NameHash == property.NameHash))
            {
                throw new ArgumentException("A Property with the same name hash already exists", nameof(property));
            }
            else
            {
                this._properties.Add(property);
            }
        }
        public bool Remove(BinTreeProperty property)
        {
            if (property.Type != this.PropertiesType) return false;
            else return this._properties.Remove(property);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4 + 1 + GetContentSize();
        }
        private int GetContentSize()
        {
            int size = 4;
            foreach (BinTreeProperty property in this._properties)
            {
                size += property.GetSize(false);
            }

            return size;
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeContainer otherProperty && other is not BinTreeUnorderedContainer)
            {
                if (this._properties.Count != otherProperty._properties.Count) return false;
                if (this.PropertiesType != otherProperty.PropertiesType) return false;

                for (int i = 0; i < this._properties.Count; i++)
                {
                    if (!this._properties[i].Equals(otherProperty._properties[i])) return false;
                }
            }

            return true;
        }
    }
}
