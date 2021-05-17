using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueToolkit.IO.WorldDescription
{
    /// <summary>
    /// Represents a World Description file (room.dsc) which specifies the world quality of map objects
    /// </summary>
    public class WorldDescriptionFile
    {
        /// <summary>
        /// Objects of this <see cref="WorldDescriptionFile"/>
        /// </summary>
        public List<WorldDescriptionObject> Objects { get; set; } = new List<WorldDescriptionObject>();

        /// <summary>
        /// 
        /// </summary>
        public WorldDescriptionFile() { }

        /// <summary>
        /// Initializes a new <see cref="WorldDescriptionFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">The location to read from</param>
        public WorldDescriptionFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initializes a new <see cref="WorldDescriptionFile"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        public WorldDescriptionFile(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    this.Objects.Add(new WorldDescriptionObject(sr));
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="WorldDescriptionFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">The location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.OpenWrite(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="WorldDescriptionFile"/> into a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream, bool leaveOpen = false)
        {
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8, 1024, leaveOpen))
            {
                foreach (WorldDescriptionObject worldObject in this.Objects)
                {
                    worldObject.Write(sw);
                }
            }
        }
    }
}