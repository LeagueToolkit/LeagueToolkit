using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.IO;

namespace Fantome.Libraries.League.IO.LightDat
{
    public class LightDatLight
    {
        public int[] Position { get; private set; }
        public ColorRGBVector3Byte Color { get; private set; }
        public int Radius { get; private set; }

        public LightDatLight(int[] position, ColorRGBVector3Byte color, int radius)
        {
            this.Position = position;
            this.Color = color;
            this.Radius = radius;
        }

        public LightDatLight(StreamReader sr)
        {
            string[] line = sr.ReadLine().Split(' ');
            this.Position = new int[] { int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]) };
            this.Color = new ColorRGBVector3Byte(byte.Parse(line[3]), byte.Parse(line[4]), byte.Parse(line[5]));
            this.Radius = int.Parse(line[6]);
        }

        public void Write(StreamWriter sw)
        {
            sw.Write("{0} {1} {2} ", this.Position[0], this.Position[1], this.Position[2]);
            this.Color.Write(sw, "{0} {1} {2} ");
            sw.Write(this.Radius + Environment.NewLine);
        }
    }
}
