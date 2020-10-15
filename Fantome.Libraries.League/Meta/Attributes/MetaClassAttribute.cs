using Fantome.Libraries.League.Helpers.Hashing;
using System;

namespace Fantome.Libraries.League.Meta.Attributes
{
    public sealed class MetaClassAttribute : Attribute
    {
        public string Name { get; private set; }
        public uint NameHash { get; private set; }

        public string Path { get; private set; }
        public uint PathHash { get; private set; }

        public MetaClassAttribute(string name)
        {
            this.Name = name;
            this.NameHash = Fnv1a.HashLower(name);
        }
        public MetaClassAttribute(uint nameHash)
        {
            this.Name = string.Empty;
            this.NameHash = nameHash;
        }

        internal void SetPath(string path)
        {
            this.Path = path;
            this.PathHash = Fnv1a.HashLower(path);
        }
        internal void SetPath(uint pathHash)
        {
            this.Path = string.Empty;
            this.PathHash = pathHash;
        }
    }
}
