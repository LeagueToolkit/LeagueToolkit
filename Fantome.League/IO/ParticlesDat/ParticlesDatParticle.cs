using Fantome.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Fantome.League.IO.ParticlesDat
{
    [DebuggerDisplay("[ {Name} ]")]
    public class ParticlesDatParticle
    {
        public string Name { get; private set; }
        public Vector3 Position { get; private set; }
        public Int32 Quality { get; private set; }
        public Vector3 Rotation { get; private set; }
        public List<string> Tags { get; private set; } = new List<string>();

        public ParticlesDatParticle(StreamReader sr)
        {
            string[] input = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            this.Name = input[0];
            this.Position = new Vector3(
                float.Parse(input[1], CultureInfo.InvariantCulture),
                float.Parse(input[2], CultureInfo.InvariantCulture),
                float.Parse(input[3], CultureInfo.InvariantCulture));
            this.Quality = Int32.Parse(input[4]);
            this.Rotation = new Vector3(
                float.Parse(input[5], CultureInfo.InvariantCulture),
                float.Parse(input[6], CultureInfo.InvariantCulture),
                float.Parse(input[7], CultureInfo.InvariantCulture));
            this.Tags.AddRange(input.ToList().GetRange(8, input.Length - 8));
        }

        public void Write(StreamWriter sw)
        {
            string write = string.Format
                ("{0} {1} {2} {3} {4} {5} {6} {7}",
                this.Name,
                this.Position.X,
                this.Position.Y,
                this.Position.Z,
                this.Quality,
                this.Rotation.X,
                this.Rotation.Y,
                this.Rotation.Z
                );

            foreach (string Tag in this.Tags)
            {
                write += " " + Tag;
            }

            sw.WriteLine(write);
        }
    }
}
