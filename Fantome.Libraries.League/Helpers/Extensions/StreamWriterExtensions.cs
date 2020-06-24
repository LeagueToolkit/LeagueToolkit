using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.Helpers.Extensions
{
    public static class StreamWriterExtensions
    {
        public static void WriteColor(this StreamWriter writer, Color color, ColorFormat format)
        {
            writer.Write(color.ToString(format));
        }
    }
}
