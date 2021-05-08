using LeagueToolkit.Helpers.Hashing;
using LeagueToolkit.IO.PropertyBin;
using System;

namespace LeagueToolkit.Meta.Attributes
{
    public sealed class MetaPropertyAttribute : Attribute
    {
        public string Name { get; private set; }
        public uint NameHash { get; private set; }

        public string OtherClass { get; private set; }

        public BinPropertyType ValueType { get; private set; }
        public BinPropertyType PrimaryType { get; private set; }
        public BinPropertyType SecondaryType { get; private set; }

        public MetaPropertyAttribute(string name, BinPropertyType type, string otherClass, BinPropertyType primaryType, BinPropertyType secondaryType)
        {
            this.Name = name;
            this.NameHash = Fnv1a.HashLower(name);

            this.OtherClass = otherClass;
            
            this.ValueType = type;
            this.PrimaryType = primaryType;
            this.SecondaryType = secondaryType;
        }
        public MetaPropertyAttribute(uint nameHash, BinPropertyType type, string otherClass, BinPropertyType primaryType, BinPropertyType secondaryType)
        {
            this.Name = string.Empty;
            this.NameHash = nameHash;

            this.OtherClass = otherClass;

            this.ValueType = type;
            this.PrimaryType = primaryType;
            this.SecondaryType = secondaryType;
        }
    }
}
