using LeagueToolkit.Helpers.Hashing;

namespace LeagueToolkit.Meta
{
    public struct MetaHash
    {
        public uint Hash { get; private set; }
        public string Value { get; private set; }

        public MetaHash(uint hash)
        {
            this.Hash = hash;
            this.Value = string.Empty;
        }
        public MetaHash(string value)
        {
            this.Hash = Fnv1a.HashLower(value);
            this.Value = value;
        }

        public override int GetHashCode()
        {
            return (int)this.Hash;
        }

        public override bool Equals(object obj)
        {
            return obj is MetaHash other && other.Hash == this.Hash;
        }

        public static implicit operator uint(MetaHash metaHash)
        {
            return metaHash.Hash;
        }
        public static implicit operator MetaHash(uint hash)
        {
            return new MetaHash(hash);
        }
    }
}
