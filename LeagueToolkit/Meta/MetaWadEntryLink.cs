namespace LeagueToolkit.Meta
{
    public struct MetaWadEntryLink
    {
        public ulong EntryPathHash { get; private set; }

        public MetaWadEntryLink(ulong entryPathHash)
        {
            this.EntryPathHash = entryPathHash;
        }

        public override int GetHashCode()
        {
            return (int)this.EntryPathHash; // ://
        }

        public override bool Equals(object obj)
        {
            return obj is MetaWadEntryLink other && this.EntryPathHash == other.EntryPathHash;
        }

        public static implicit operator ulong(MetaWadEntryLink wadEntryLink)
        {
            return wadEntryLink.EntryPathHash;
        }
        public static implicit operator MetaWadEntryLink(ulong entryPathHash)
        {
            return new MetaWadEntryLink(entryPathHash);
        }
    }
}
