using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.SCB;
using System;
using System.Globalization;
using System.IO;

namespace Fantome.Libraries.League.IO.SCO
{
    public class SCOFace
    {
        public uint[] Indices { get; private set; }
        public string Material { get; private set; }
        public Vector2[] UVs { get; private set; }

        public SCOFace(SCBFace face)
        {
            this.Indices = face.Indices;
            this.Material = face.Material;
            this.UVs = face.UVs;
        }

        public SCOFace(uint[] indices, string material, Vector2[] uvs)
        {
            this.Indices = indices;
            this.Material = material;
            this.UVs = uvs;
        }

        public SCOFace(StreamReader sr)
        {
            string[] input = sr.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            this.Indices = new uint[] { uint.Parse(input[1]), uint.Parse(input[2]), uint.Parse(input[3]) };
            this.Material = input[4];
            this.UVs = new Vector2[]
            {
                new Vector2(float.Parse(input[5], CultureInfo.InvariantCulture), float.Parse(input[8], CultureInfo.InvariantCulture)),
                new Vector2(float.Parse(input[6], CultureInfo.InvariantCulture), float.Parse(input[9], CultureInfo.InvariantCulture)),
                new Vector2(float.Parse(input[7], CultureInfo.InvariantCulture), float.Parse(input[10], CultureInfo.InvariantCulture)),
            };
        }

        public void Write(StreamWriter sw)
        {
            string indices = string.Format("{0} {1} {2}", this.Indices[0], this.Indices[1], this.Indices[2]);
            string uvs = string.Format("{0} {1} {2} {3} {4} {5}", this.UVs[0].X, this.UVs[1].X, this.UVs[2].X, this.UVs[0].Y, this.UVs[1].Y, this.UVs[2].Y);
            sw.WriteLine(string.Format("3 {0} {1} {2}", indices, this.Material, uvs));
        }
    }
}
