using Fantome.Libraries.League.Helpers.Hashing;
using Fantome.Libraries.League.IO.PropertyBin;
using System;

namespace Fantome.Libraries.League.Meta.Attributes
{
    public sealed class MetaPropertyAttribute : Attribute
    {
        public string Name { get; private set; }
        public uint NameHash { get; private set; }
        public BinPropertyType ValueType { get; private set; }

        public MetaPropertyAttribute(string name, BinPropertyType type)
        {
            this.Name = name;
            this.NameHash = Fnv1a.HashLower(name);
            this.ValueType = type;
        }
        public MetaPropertyAttribute(uint nameHash, BinPropertyType type)
        {
            this.Name = string.Empty;
            this.NameHash = nameHash;
            this.ValueType = type;
        }
    }
}
