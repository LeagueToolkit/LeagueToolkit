using Fantome.League.Helpers.Structures;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Fantome.League.IO.SCO
{
    [DebuggerDisplay("[ {Material} ]")]
    public class SCOFace
    {
        public UInt16[] Indices { get; private set; } = new UInt16[3];
        public string Material { get; private set; }
        public Vector2[] UV { get; private set; } = new Vector2[3];

        public SCOFace(UInt16[] Indices, string Material, Vector2[] UV)
        {
            this.Indices = Indices;
            this.Material = Material;
            this.UV = UV;
        }
        public SCOFace(StreamReader sr)
        {
            string[] input = sr.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < 3; i++)
            {
                this.Indices[i] = UInt16.Parse(input[i + 1]);
            }

            this.Material = input[4];

            for (int i = 0; i < 3; i++)
            {
                this.UV[i].X = float.Parse(input[i + 5], CultureInfo.InvariantCulture);
                this.UV[i].Y = float.Parse(input[i + 8], CultureInfo.InvariantCulture);
            }
        }
    }
}
