namespace LeagueToolkit.Meta
{
    public struct MetaObjectLink
    {
        public uint ObjectPathHash { get; private set; }
    
        public MetaObjectLink(uint objectPathHash)
        {
            this.ObjectPathHash = objectPathHash;
        }

        public override int GetHashCode()
        {
            return (int)this.ObjectPathHash;
        }
        public override bool Equals(object obj)
        {
            return obj is MetaObjectLink other && this.ObjectPathHash == other.ObjectPathHash;
        }

        public static implicit operator uint(MetaObjectLink objectLink)
        {
            return objectLink.ObjectPathHash;
        }
        public static implicit operator MetaObjectLink(uint objectPathHash)
        {
            return new MetaObjectLink(objectPathHash);
        }
    }
}
