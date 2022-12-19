using LeagueToolkit.Helpers.Hashing;
using System;

namespace LeagueToolkit.Meta.Attributes
{
    public sealed class MetaClassAttribute : Attribute
    {
        public string Name
        {
            get => this._name;
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));

                this._name = value;
                this._nameHash = Fnv1a.HashLower(value);
            }
        }
        public uint NameHash
        {
            get => this._nameHash;
            set
            {
                this._name = null;
                this._nameHash = value;
            }
        }

        public string Path
        {
            get => this._path;
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));

                this._path = value;
                this._pathHash = Fnv1a.HashLower(value);
            }
        }
        public uint PathHash
        {
            get => this._pathHash;
            set
            {
                this._path = null;
                this._pathHash = value;
            }
        }

        public string _name;
        public string _path;
        private uint _nameHash;
        private uint _pathHash;

        public MetaClassAttribute(string name)
        {
            this.Name = name;
            this._nameHash = Fnv1a.HashLower(name);
        }
        public MetaClassAttribute(uint nameHash)
        {
            this.Name = string.Empty;
            this._nameHash = nameHash;
        }
    }
}
