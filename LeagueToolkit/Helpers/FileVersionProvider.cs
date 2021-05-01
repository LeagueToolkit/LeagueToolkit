using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueToolkit.Helpers
{
    public static class FileVersionProvider
    {
        private static readonly Dictionary<LeagueFileType, Version[]> SUPPORTED_VERSIONS = new() 
        {
            { 
                LeagueFileType.Animation, new Version[]
                { 
                    new Version(3, 0, 0, 0), 
                    new Version(4, 0, 0, 0),
                    new Version(5, 0, 0, 0) 
                } 
            },
            {
                LeagueFileType.MapGeometry, new Version[]
                {
                    new Version(5, 0, 0, 0),
                    new Version(6, 0, 0, 0),
                    new Version(7, 0, 0, 0),
                    new Version(9, 0, 0, 0),
                    new Version(11, 0, 0, 0)
                }
            },
            {
                LeagueFileType.PropertyBin, new Version[]
                {
                    new Version(1, 0, 0, 0),
                    new Version(2, 0, 0, 0),
                    new Version(3, 0, 0, 0)
                }
            }
        };

        public static Version[] GetSupportedVersions(LeagueFileType fileType) => SUPPORTED_VERSIONS[fileType];
    }
}
