using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.IO.Atmosphere
{
    /// <summary>
    /// Represents an Atmosphere file (Atmosphere.dat)
    /// </summary>
    public class AtmosphereFile
    {
        /// <summary>
        /// The color gradient of the Sun
        /// </summary>
        public TimeGradient SunColor { get; set; }

        /// <summary>
        /// The color gradient of the Sky
        /// </summary>
        public TimeGradient SkyColor { get; set; }

        /// <summary>
        /// Initializes an empty <see cref="AtmosphereFile"/>
        /// </summary>
        public AtmosphereFile() { }

        /// <summary>
        /// Initializes a new <see cref="AtmosphereFile"/>
        /// </summary>
        /// <param name="sunColor">The color gradient of the Sun</param>
        /// <param name="skyColor">The color gradient of the Sky</param>
        public AtmosphereFile(TimeGradient sunColor, TimeGradient skyColor)
        {
            this.SunColor = sunColor;
            this.SkyColor = skyColor;
        }

        /// <summary>
        /// Initialiazes a new <see cref="AtmosphereFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">The location to read from</param>
        public AtmosphereFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initializes a new <see cref="AtmosphereFile"/> from the specified <see cref="Stream"/>
        /// </summary>
        /// <param name="stream"><The <see cref="Stream"/> to read from</param>
        public AtmosphereFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                this.SunColor = new TimeGradient(br);
                this.SkyColor = new TimeGradient(br);
            }
        }

        /// <summary>
        /// Writes this <see cref="AtmosphereFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">The location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="AtmosphereFile"/> into the specified <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                this.SunColor.Write(bw);
                this.SkyColor.Write(bw);
            }
        }
    }
}
