using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueToolkit.Helpers.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> collection, IEnumerable<T> sequence)
        {
            int collectionCount = collection.Count();
            int sequenceCount = sequence.Count();

            if (sequenceCount > collectionCount)
                return -1;

            if (collection.Take(sequenceCount).SequenceEqual(sequence))
                return 0;

            int index = Enumerable
                .Range(1, collectionCount - sequenceCount + 1)
                .FirstOrDefault(i => collection.Skip(i).Take(sequenceCount).SequenceEqual(sequence));

            return index is 0 ? -1 : index;
        }
    }
}
