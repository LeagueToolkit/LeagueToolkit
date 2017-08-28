using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Helpers
{
    public static class Utilities
    {
        public static string ByteArrayToHex(byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "");
        }
    }
}
