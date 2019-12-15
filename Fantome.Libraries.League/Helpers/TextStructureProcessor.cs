using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Globalization;

namespace Fantome.Libraries.League.Helpers
{
    /// <summary>
    /// Helper class used to convert structures between strings
    /// </summary>
    public static class TextStructureProcessor
    {
        /// <summary>
        /// Parses a <see cref="Vector2"/> structure from the provided string assuming the vector components are separated by a space
        /// </summary>
        /// <param name="value">The string which contains a <see cref="Vector2"/></param>
        public static Vector2 ParseVector2(string value)
        {
            return ParseVector2(value, ' ');
        }

        /// <summary>
        /// Parses a <see cref="Vector2"/> structure from the provided string assuming the vector components are separated by <paramref name="separator"/>
        /// </summary>
        /// <param name="value">The string which contains a <see cref="Vector2"/></param>
        /// <param name="separator">The character which separates the components of the vector</param>
        public static Vector2 ParseVector2(string value, char separator)
        {
            string[] values = value.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 2)
            {
                throw new ArgumentException("Value is not a Vector2 or is not correctly formatted");
            }

            return ParseVector2(values);
        }

        /// <summary>
        /// Parses a <see cref="Vector2"/> structure from the provided string array assuming each item is a separate vector component
        /// </summary>
        /// <param name="values">The string array which contains the components of a <see cref="Vector2"/></param>
        public static Vector2 ParseVector2(string[] values)
        {
            return new Vector2(float.Parse(values[0], CultureInfo.InvariantCulture),
                float.Parse(values[1], CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Parses a <see cref="Vector3"/> structure from the provided string assuming the vector components are separated by a space
        /// </summary>
        /// <param name="value">The string which contains a <see cref="Vector3"/></param>
        public static Vector3 ParseVector3(string value)
        {
            return ParseVector3(value, ' ');
        }

        /// <summary>
        /// Parses a <see cref="Vector3"/> structure from the provided string assuming the vector components are separated by <paramref name="separator"/>
        /// </summary>
        /// <param name="value">The string which contains a <see cref="Vector3"/></param>
        /// <param name="separator">The character which separates the components of the vector</param>
        public static Vector3 ParseVector3(string value, char separator)
        {
            string[] values = value.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 3)
            {
                throw new ArgumentException("Value is either not a Vector3 or is not correctly formatted");
            }

            return ParseVector3(values);
        }

        /// <summary>
        /// Parses a <see cref="Vector3"/> structure from the provided string array assuming each item is a separate vector component
        /// </summary>
        /// <param name="values">The string array which contains the components of a <see cref="Vector3"/></param>
        public static Vector3 ParseVector3(string[] values)
        {
            return new Vector3(float.Parse(values[0], CultureInfo.InvariantCulture),
                float.Parse(values[1], CultureInfo.InvariantCulture),
                float.Parse(values[2], CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts a <see cref="Vector2"/> to string with the components being separated by a space
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to convert</param>
        public static string ConvertVector2(Vector2 vector)
        {
            return ConvertVector2(vector, ' ');
        }

        /// <summary>
        /// Converts a <see cref="Vector2"/> to string with the components being separated by a space and formatted by <paramref name="format"/>
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to convert</param>
        /// <param name="format">Format of the vector components</param>
        public static string ConvertVector2(Vector2 vector, string format)
        {
            return ConvertVector2(vector, ' ', format);
        }

        /// <summary>
        /// Converts a <see cref="Vector2"/> to string with the components being separated by <paramref name="separator"/>
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to convert</param>
        /// <param name="separator">The character to separate the vector components</param>
        public static string ConvertVector2(Vector2 vector, char separator)
        {
            return ConvertVector2(vector, separator, "");
        }

        /// <summary>
        /// Converts a <see cref="Vector2"/> to string with the components being separated by <paramref name="separator"/> and formatted by <paramref name="format"/>
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to convert</param>
        /// <param name="separator">The character to separate the vector components</param>
        /// <param name="format">Format of the vector components</param>
        public static string ConvertVector2(Vector2 vector, char separator, string format)
        {
            return string.Format("{0}{1}{2}", vector.X.ToString(format, CultureInfo.InvariantCulture), separator, vector.Y.ToString(format, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts a <see cref="Vector3"/> to string with the components being separated by a space
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to convert</param>
        public static string ConvertVector3(Vector3 vector)
        {
            return ConvertVector3(vector, ' ');
        }

        /// <summary>
        /// Converts a <see cref="Vector3"/> to string with the components being separated by <paramref name="separator"/>
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to convert</param>
        /// <param name="separator">The character to separate the vector components</param>
        public static string ConvertVector3(Vector3 vector, char separator)
        {
            return ConvertVector3(vector, separator, "");
        }

        /// <summary>
        /// Converts a <see cref="Vector3"/> to string with the components being separated by a space and formatted by <paramref name="format"/>
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to convert</param>
        /// <param name="format">Format of the vector components</param>
        public static string ConvertVector3(Vector3 vector, string format)
        {
            return ConvertVector3(vector, ' ', format);
        }

        /// <summary>
        /// Converts a <see cref="Vector3"/> to string with the components being separated by <paramref name="separator"/> and formatted by <paramref name="format"/>
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to convert</param>
        /// <param name="separator">The character to separate the vector components</param>
        /// <param name="format">Format of the vector components</param>
        public static string ConvertVector3(Vector3 vector, char separator, string format)
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, vector.X.ToString(format, CultureInfo.InvariantCulture), vector.Y.ToString(format, CultureInfo.InvariantCulture), vector.Z.ToString(format, CultureInfo.InvariantCulture));
        }
    }
}