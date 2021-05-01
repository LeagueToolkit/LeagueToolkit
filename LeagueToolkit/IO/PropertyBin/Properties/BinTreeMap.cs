using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeMap : BinTreeProperty, IBinTreeParent
    {
        public override BinPropertyType Type => BinPropertyType.Map;
        public BinPropertyType KeyType { get; private set; }
        public BinPropertyType ValueType { get; private set; }

        public ReadOnlyDictionary<BinTreeProperty, BinTreeProperty> Map { get; }
        private Dictionary<BinTreeProperty, BinTreeProperty> _map = new();

        public BinTreeMap(IBinTreeParent parent, uint nameHash,
            BinPropertyType keyType, BinPropertyType valueType, IEnumerable<KeyValuePair<BinTreeProperty, BinTreeProperty>> map)
            : base(parent, nameHash)
        {
            this.KeyType = keyType;
            this.ValueType = valueType;

            // Verify property types
            foreach (var pair in map)
            {
                if (pair.Key.Type != keyType) throw new ArgumentException("Found a key that does not match the specified key type", nameof(map));
                if (pair.Value.Type != valueType) throw new ArgumentException("Found a value that does not match the specified value type", nameof(map));

                pair.Key.Parent = this;
                pair.Value.Parent = this;
            }

            this._map = new Dictionary<BinTreeProperty, BinTreeProperty>(map);
            this.Map = new ReadOnlyDictionary<BinTreeProperty, BinTreeProperty>(this._map);
        }
        internal BinTreeMap(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.KeyType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
            this.ValueType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
            uint size = br.ReadUInt32();
            uint valueCount = br.ReadUInt32();

            for (int i = 0; i < valueCount; i++)
            {
                this._map.Add(BinTreeProperty.Read(br, this, this.KeyType), BinTreeProperty.Read(br, this, this.ValueType));
            }

            this.Map = new ReadOnlyDictionary<BinTreeProperty, BinTreeProperty>(this._map);
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write((byte)this.KeyType);
            bw.Write((byte)BinUtilities.PackType(this.ValueType));
            bw.Write(GetContentSize());
            bw.Write(this._map.Count);

            foreach (var pair in this._map)
            {
                pair.Key.Write(bw, false);
                pair.Value.Write(bw, false);
            }
        }

        internal override int GetSize(bool includeHeader)
        {
            return 1 + 1 + 4 + GetContentSize() + (includeHeader ? 5 : 0);
        }
        private int GetContentSize()
        {
            int size = 4;
            foreach (var pair in this._map)
            {
                size += pair.Key.GetSize(false) + pair.Value.GetSize(false);
            }
            return size;
        }

        public void Add(BinTreeProperty key, BinTreeProperty value)
        {
            if (key.Type != this.KeyType) throw new ArgumentException($"Key type ({key.Type}) does not match dictionary key type ({this.KeyType})", nameof(key));
            if (value.Type != this.ValueType) throw new ArgumentException($"Value type ({value.Type}) does not match dictionary value type ({this.ValueType})", nameof(value));

            if (this._map.TryAdd(key, value) is false)
            {
                throw new InvalidOperationException("Failed to add the specified key-value pair because the specified key already exists");
            }
        }
        public bool Remove(BinTreeProperty key)
        {
            if (key.Type != this.KeyType) throw new ArgumentException($"Key type ({key.Type}) does not match dictionary key type ({this.KeyType})", nameof(key));

            return this._map.Remove(key);
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeMap otherProperty)
            {
                if (this.KeyType != otherProperty.KeyType) return false;
                if (this.ValueType != otherProperty.ValueType) return false;
                if (this._map.Count != otherProperty._map.Count) return false;

                foreach (var entry in this._map)
                {
                    if (otherProperty._map.TryGetValue(entry.Key, out BinTreeProperty value))
                    {
                        if (!entry.Value.Equals(value)) return false;
                    }
                    else return false;
                }
            }

            return true;
        }

        public static implicit operator ReadOnlyDictionary<BinTreeProperty, BinTreeProperty>(BinTreeMap property) => property.Map;
    }
}
