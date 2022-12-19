using System.Collections.Generic;

namespace LeagueToolkit.Helpers
{
    public static class FileVersionProvider
    {
        private static readonly Dictionary<LeagueFileType, uint[]> SUPPORTED_VERSIONS = new()
        {
            { LeagueFileType.Animation, new uint[] { 3, 4, 5 } },
            { LeagueFileType.MapGeometry, new uint[] { 5, 6, 7, 9, 11, 12, 13 } },
            { LeagueFileType.PropertyBin, new uint[] { 1, 2, 3 } }
        };

        public static uint[] GetSupportedVersions(LeagueFileType fileType) => SUPPORTED_VERSIONS[fileType];
    }
}
