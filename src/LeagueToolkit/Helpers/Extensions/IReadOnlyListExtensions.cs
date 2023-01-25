using System.Collections.Generic;

namespace LeagueToolkit.Helpers.Extensions
{
    public static class IReadOnlyListExtensions
    {
        public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
        {
            for (int i = 0; i < self.Count; i++)
                if (elementToFind.Equals(self[i]))
                    return i;

            return -1;
        }
    }
}
