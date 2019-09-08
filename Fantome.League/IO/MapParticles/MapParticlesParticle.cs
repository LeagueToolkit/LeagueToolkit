using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Fantome.Libraries.League.IO.MapParticles
{
    /// <summary>
    /// Represents a particle inside a <see cref="MapParticlesFile"/>
    /// </summary>
    public class MapParticlesParticle
    {
        /// <summary>
        /// Name of this <see cref="MapParticlesParticle"/>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Position of this <see cref="MapParticlesParticle"/>
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// World Quality of this <see cref="MapParticlesParticle"/>
        /// </summary>
        public int Quality { get; set; }
        /// <summary>
        /// Roatation of this <see cref="MapParticlesParticle"/>
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// Special Tags of this <see cref="MapParticlesParticle"/>
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Initializes a blank <see cref="MapParticlesParticle"/>
        /// </summary>
        public MapParticlesParticle() { }

        /// <summary>
        /// Initializes a new <see cref="MapParticlesParticle"/>
        /// </summary>
        /// <param name="name">Name of the particle</param>
        /// <param name="position">Position of the particle</param>
        /// <param name="quality">World Quality of the particle</param>
        /// <param name="rotation">Rotation of the particle</param>
        /// <param name="tags">Special Tags of the particle</param>
        public MapParticlesParticle(string name, Vector3 position, int quality, Vector3 rotation, List<string> tags)
        {
            this.Name = name;
            this.Position = position;
            this.Quality = quality;
            this.Rotation = rotation;
            this.Tags = tags;
        }

        /// <summary>
        /// Initializes a new <see cref="MapParticlesParticle"/> from a <see cref="StreamReader"/>
        /// </summary>
        /// <param name="sr">The <see cref="StreamReader"/> to read from</param>
        public MapParticlesParticle(StreamReader sr)
        {
            string[] input = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            this.Name = input[0];

            this.Position = new Vector3(float.Parse(input[1], CultureInfo.InvariantCulture),
                float.Parse(input[2], CultureInfo.InvariantCulture),
                float.Parse(input[3], CultureInfo.InvariantCulture));

            this.Quality = Int32.Parse(input[4]);

            this.Rotation = new Vector3(float.Parse(input[5], CultureInfo.InvariantCulture),
                float.Parse(input[6], CultureInfo.InvariantCulture),
                float.Parse(input[7], CultureInfo.InvariantCulture));

            this.Tags.AddRange(input.ToList().GetRange(8, input.Length - 8));
        }

        /// <summary>
        /// Writes this <see cref="MapParticlesParticle"/> into a <see cref="StreamWriter"/>
        /// </summary>
        /// <param name="sw">The <see cref="StreamWriter"/> to write to</param>
        public void Write(StreamWriter sw)
        {
            string write = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}",
                this.Name,
                this.Position.X,
                this.Position.Y,
                this.Position.Z,
                this.Quality,
                this.Rotation.X,
                this.Rotation.Y,
                this.Rotation.Z
                );

            foreach (string tag in this.Tags)
            {
                write += " " + tag;
            }

            sw.WriteLine(write);
        }
    }
}
