using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Helpers.Exceptions
{
    public class InvalidFileMagicException : Exception
    {
        public InvalidFileMagicException() : base("Invalid file Magic") { }
    }

    public class UnsupportedFileVersionException : Exception
    {
        public UnsupportedFileVersionException() : base("Unsupported file Version") { }
    }
}
