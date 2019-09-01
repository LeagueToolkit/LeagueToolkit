using Fantome.Libraries.League.IO.BIN;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fantome.Libraries.League.Diffing.BIN
{
    public class BINDiffInfo
    {
        public ReadOnlyCollection<BINEntry> Added { get; private set; }
        public ReadOnlyCollection<BINEntry> Removed { get; private set; }
        public ReadOnlyCollection<BINEntry> Changed { get; private set; }

        internal BINDiffInfo(List<BINEntry> added, List<BINEntry> removed, List<BINEntry> changed)
        {
            this.Added = added.AsReadOnly();
            this.Removed = removed.AsReadOnly();
            this.Changed = changed.AsReadOnly();
        }
    }
}
