using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueToolkit.Helpers.Extensions
{
    public static class VersionExtensions
    {
        public static int PackToInt(this Version version)
        {
            return ((byte)version.Major << 0) | ((byte)version.Minor << 8) | ((byte)version.Build << 16) | ((byte)version.Revision << 24);
        }
    }
}
