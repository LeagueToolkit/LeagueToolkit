using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Helpers.Exceptions
{
    public class InvalidFileSignatureException : Exception
    {
        public InvalidFileSignatureException() : base("Invalid file signature") { }
        public InvalidFileSignatureException(string message) : base("Invalid file signature: " + message) { }
    }

    public class UnsupportedFileVersionException : Exception
    {
        public UnsupportedFileVersionException() : base("Unsupported file Version") { }
    }
}
