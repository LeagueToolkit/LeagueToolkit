namespace LeagueToolkit.Hashing
{
    public static class Fnv1a
    {
        public static uint HashLower(string input)
        {
            input = input.ToLowerInvariant();

            uint hash = 2166136261;
            for (int i = 0; i < input.Length; i++)
            {
                hash ^= input[i];
                hash *= 16777619;
            }

            return hash;
        }
    }
}
