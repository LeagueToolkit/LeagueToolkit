using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Hashing
{
    public static class Sdbm
    {
        public static uint Hash(string value)
        {
            uint hash = 0;
            for (int i = 0; i < value.Length; i++)
            {
                hash = value[i] + 65599 * hash;
            }

            return hash;
        }

        public static uint Hash(string key, string value)
        {
            uint hash = 0;
            for (int i = 0; i < key.Length; i++)
            {
                hash = key[i] + 65599 * hash;
            }
            for (int i = 0; i < value.Length; i++)
            {
                hash = value[i] + 65599 * hash;
            }
            return hash;
        }

        public static uint HashWithDelimiter(string key, string value, char delimiter)
        {
            uint hash = 0;
            for (int i = 0; i < key.Length; i++)
            {
                hash = key[i] + 65599 * hash;
            }

            hash = 65599 * hash + delimiter;
            for (int i = 0; i < value.Length; i++)
            {
                hash = value[i] + 65599 * hash;
            }
            return hash;
        }

        public static uint HashLower(string value) => Hash(value.ToLowerInvariant());

        public static uint HashLower(string key, string value) => Hash(key.ToLowerInvariant(), value.ToLowerInvariant());

        public static uint HashLowerWithDelimiter(string key, string value, char delimiter) =>
            HashWithDelimiter(key.ToLowerInvariant(), value.ToLowerInvariant(), delimiter);
    }
}
