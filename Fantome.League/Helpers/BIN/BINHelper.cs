using System.Collections.Generic;

namespace Fantome.Libraries.League.Helpers.BIN
{
    public static class BINHelper
    {
        private static Dictionary<uint, string> _entryNames;
        private static Dictionary<uint, string> _classNames;
        private static Dictionary<uint, string> _fieldNames;

        public static void SetHashmap(Dictionary<uint, string> entryNames, Dictionary<uint, string> classNames, Dictionary<uint, string> fieldNames)
        {
            SetEntryNames(entryNames);
            SetClassNames(classNames);
            SetFieldNames(fieldNames);
        }

        public static void SetEntryNames(Dictionary<uint, string> entryNames)
        {
            _entryNames = entryNames;
        }

        public static void SetClassNames(Dictionary<uint, string> classNames)
        {
            _classNames = classNames;
        }

        public static void SetFieldNames(Dictionary<uint, string> fieldNames)
        {
            _fieldNames = fieldNames;
        }

        public static string GetEntry(uint hash)
        {
            if(_entryNames.TryGetValue(hash, out string entryName))
            {
                return entryName;
            }
            else
            {
                return hash.ToString();
            }
        }

        public static string GetClass(uint hash)
        {
            if (_classNames.TryGetValue(hash, out string className))
            {
                return className;
            }
            else
            {
                return hash.ToString();
            }
        }

        public static string GetField(uint hash)
        {
            if (_fieldNames.TryGetValue(hash, out string fieldName))
            {
                return fieldName;
            }
            else
            {
                return hash.ToString();
            }
        }
    }
}
