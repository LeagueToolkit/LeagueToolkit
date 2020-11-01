using LeagueToolkit.Helpers.Hashing;
using LeagueToolkit.IO.PropertyBin;
using System;

namespace LeagueToolkit.Meta.Attributes
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
