using Fantome.Libraries.League.IO.BIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Diffing.BIN
{
    public static class BINDiffer
    {
        public static BINDiffInfo CreateDiff(BINFile oldBin, BINFile newBin)
        {
            List<BINEntry> added = new List<BINEntry>();
            List<BINEntry> removed = new List<BINEntry>();
            List<BINEntry> edited = new List<BINEntry>();

            foreach (BINEntry entry in newBin.Entries)
            {
                //Scan for new entries
                if (!oldBin.Entries.Contains(entry))
                {
                    //Since this is a completely new entry we need to store all its data
                    added.Add(entry);
                }
            }

            foreach (BINEntry entry in oldBin.Entries)
            {
                //Scan if it's removed
                if(!newBin.Entries.Any(x => x.Class == entry.Class && x.Property == entry.Property))
                {
                    //If it's removed we need to add it to the Removed Entry list
                    //Store the Entry without any values as we just need the relevant data
                    BINEntry removedEntry = entry;
                    removedEntry.Values.RemoveAll(x => true);

                    removed.Add(removedEntry);
                }
                //If it's not removed we need to scan if it's unedited, if it isn't then we need to look for the edited values
                else if (!newBin.Entries.Contains(entry))
                {
                    BINEntry newEntry = newBin.Entries.Find(x => x.Property == entry.Property && x.Class == entry.Class);
                    //edited.Add(ScanForChangedValues(entry, newEntry));
                }
            }

            return new BINDiffInfo(added, removed, edited);
        }

        /*public static BINFile ApplyDiff(BINFile bin, BINDiffInfo diffInfo)
        {

        }

        private static BINEntry ScanForChangedValues(BINEntry oldEntry, BINEntry newEntry)
        {

        }*/

        private static List<string> GenerateEntryValuePaths(BINEntry entry)
        {

        }

        private static IEnumerable<string> ProcessBINEntry(BINEntry entry)
        {
            List<string> strings = new List<string>();

            foreach (BINValue value in entry.Values)
            {
                strings.AddRange(ProcessBINValue(value));
            }

            return strings;
        }
        private static IEnumerable<string> ProcessBINValue(BINValue value)
        {
            List<string> strings = new List<string>();

            if (value.Type == BINFileValueType.OptionalData)
            {
                strings.AddRange(ProcessBINAdditionalData(value.Value as BINOptionalData));
            }
            else if (value.Type == BINFileValueType.Container)
            {
                strings.Add(value.GetPath());
                strings.AddRange(ProcessBINContainer(value.Value as BINContainer));
            }
            else if (value.Type == BINFileValueType.Embedded || value.Type == BINFileValueType.Structure)
            {
                strings.AddRange(ProcessBINStructure(value.Value as BINStructure));
            }
            else if (value.Type == BINFileValueType.Map)
            {
                strings.AddRange(ProcessBINMap(value.Value as BINMap));
            }
            else
            {
                strings.Add(value.GetPath(false));
            }

            return strings;
        }
        private static IEnumerable<string> ProcessBINAdditionalData(BINOptionalData additionalData)
        {
            List<string> strings = new List<string>();

            foreach (BINValue value in additionalData.Values)
            {
                strings.AddRange(ProcessBINValue(value));
            }

            return strings;
        }
        private static IEnumerable<string> ProcessBINContainer(BINContainer container)
        {
            List<string> strings = new List<string>();

            foreach (BINValue value in container.Values)
            {
                strings.AddRange(ProcessBINValue(value));
            }

            return strings;
        }
        private static IEnumerable<string> ProcessBINStructure(BINStructure structure)
        {
            List<string> strings = new List<string>();

            foreach (BINValue value in structure.Values)
            {
                strings.AddRange(ProcessBINValue(value));
            }

            return strings;
        }
        private static IEnumerable<string> ProcessBINMap(BINMap map)
        {
            List<string> strings = new List<string>();

            foreach (KeyValuePair<BINValue, BINValue> valuePair in map.Values)
            {
                strings.AddRange(ProcessBINValue(valuePair.Key));
            }

            return strings;
        }
    }
}
