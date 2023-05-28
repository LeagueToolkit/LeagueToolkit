namespace LeagueToolkit.Hashing
{
    public static class Elf
    {
        /// <summary>
        /// Hashes a string
        /// </summary>
        /// <param name="toHash">The string to hash</param>
        /// <returns>A hash generated from <paramref name="toHash"/></returns>
        /// <remarks>Used in RAF, SKL and ANM</remarks>
        public static uint Hash(string toHash)
        {
            uint hash = 0;
            uint high = 0;
            for (int i = 0; i < toHash.Length; i++)
            {
                hash = (hash << 4) + (byte)toHash[i];

                if ((high = hash & 0xF0000000) != 0)
                {
                    hash ^= high >> 24;
                }

                hash &= ~high;
            }

            return hash;
        }

        public static uint HashLower(string value) => Hash(value.ToLowerInvariant());
    }
}
