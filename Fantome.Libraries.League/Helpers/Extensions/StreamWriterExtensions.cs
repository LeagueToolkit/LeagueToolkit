using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.Helpers.Extensions
{
    internal static class StreamWriterExtensions
    {
        public static void WriteColor(this StreamWriter writer, Color color, ColorFormat format)
        {
            writer.Write(color.ToString(format));
        }

        public static void WriteLineIndented(this StreamWriter writer, int indentationLevel, string format, params object[] arg)
        {
            for(int i = 0; i < indentationLevel; i++)
            {
                writer.Write("    ");
            }

            writer.WriteLine(format, arg);
        }
        public static void WriteLineIndented(this StreamWriter writer, int indentationLevel, string value)
        {
            for (int i = 0; i < indentationLevel; i++)
            {
                writer.Write("    ");
            }

            writer.WriteLine(value);
        }
    }
}
