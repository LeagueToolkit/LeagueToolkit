using Fantome.Libraries.League.IO.INI;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.ObjectConfig
{
    /// <summary>
    /// Contains serialized content of ObjectCFG
    /// </summary>
    public class ObjectConfigFile
    {
        /// <summary>
        /// The Objects of this <see cref="ObjectConfigFile"/>
        /// </summary>
        public Dictionary<string, ObjectConfigObject> Objects { get; set; } = new Dictionary<string, ObjectConfigObject>();

        /// <summary>
        /// Initializes an empty <see cref="ObjectConfigFile"/>
        /// </summary>
        public ObjectConfigFile() { }

        /// <summary>
        /// Initializes a new <see cref="ObjectConfigFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">Location to read from</param>
        /// <remarks>The file located at <paramref name="fileLocation"/> must be an INI file</remarks>
        public ObjectConfigFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initializes a new <see cref="ObjectConfigFile"/> from the specified <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        /// <remarks>The file located inside <paramref name="stream"/> must be an INI file</remarks>
        public ObjectConfigFile(Stream stream) : this(new IniFile(stream)) { }

        /// <summary>
        /// Initializes a new <see cref="ObjectConfigFile"/> from the specified <see cref="IniFile"/>
        /// </summary>
        /// <param name="ini">The <see cref="IniFile"/> from which to parse the content</param>
        public ObjectConfigFile(IniFile ini)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> objectDefinition in ini.Sections)
            {
                this.Objects.Add(objectDefinition.Key, new ObjectConfigObject(objectDefinition.Value));
            }
        }

        /// <summary>
        /// Writes this <see cref="ObjectConfigFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">Location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="ObjectConfigFile"/> into a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream)
        {
            Dictionary<string, Dictionary<string, string>> sections = new Dictionary<string, Dictionary<string, string>>();

            foreach (KeyValuePair<string, ObjectConfigObject> section in this.Objects)
            {
                sections.Add(section.Key, section.Value.ConvertToSection());
            }

            new IniFile(sections).Write(stream);
        }
    }
}
