using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Globalization;
using System.IO;

namespace Fantome.Libraries.League.IO.LightEnvironment
{
    public class LightEnvironmentLight
    {
        public int[] Position { get; private set; } //Decimal Integer position, not float
        public ColorRGBVector3Byte Color { get; private set; } //*(v6 + 67) = (v38 << 16) | (v37 << 8) | v36 | 0xFF000000;
        public ColorRGBVector3Byte Color2 { get; private set; } //Not so sure about this one, seems to be a color *(v6 + 67) = (v38 << 16) | (v37 << 8) | v36 | 0xFF000000;
        public int Unknown1 { get; private set; } //Seems to be a flag
        public int Unknown2 { get; private set; }
        public bool Unknown3 { get; private set; }
        public float Opacity { get; private set; }//No idea what this might be but probably Opacity of the light since there is no alpha channel in color

        public LightEnvironmentLight(int[] position, ColorRGBVector3Byte color, ColorRGBVector3Byte color2, int unknown1, int unknown2, bool unknown3, float opacity)
        {
            this.Position = position;
            this.Color = color;
            this.Color2 = color2;
            this.Unknown1 = unknown1;
            this.Unknown2 = unknown2;
            this.Unknown3 = unknown3;
            this.Opacity = opacity;
        }

        public LightEnvironmentLight(StreamReader sr)
        {
            string[] line = sr.ReadLine().Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries);

            this.Position = new int[] { int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]) };
            this.Color = new ColorRGBVector3Byte(byte.Parse(line[3]), byte.Parse(line[4]), byte.Parse(line[5]));
            this.Color2 = new ColorRGBVector3Byte(byte.Parse(line[6]), byte.Parse(line[7]), byte.Parse(line[8]));
            this.Unknown1 = int.Parse(line[9]);
            this.Unknown2 = int.Parse(line[10]);
            this.Unknown3 = line[11] == "1";
            this.Opacity = float.Parse(line[12], CultureInfo.InvariantCulture);
        }

        public void Write(StreamWriter sw)
        {
            sw.Write("{0} {1} {2} ", this.Position[0], this.Position[1], this.Position[2]);
            this.Color.Write(sw, "{0} {1} {2} ");
            this.Color2.Write(sw, "{0} {1} {2} ");
            sw.Write("{0} {1} {2} {3}" + Environment.NewLine, this.Unknown1, this.Unknown2, Convert.ToUInt16(this.Unknown3), this.Opacity);
        }
    }
    /*
    [Flags]
    public enum LightFlags : UInt32
    {
        R3D_LIGHT_ON = 2,
        R3D_LIGHT_STATIC = 4,
        R3D_LIGHT_DYNAMIC = 8,
        R3D_LIGHT_HEAP = 16,
        R3D_LIGHT_AUTOREMOVE = 32,
        R3D_LIGHT_ALWAYSVISIBLE = 64
    }

    public enum LightType : UInt32
    {
        R3D_OMNI_LIGHT = 0,
        R3D_DIRECT_LIGHT = 1,
        R3D_SPOT_LIGHT = 2,
        R3D_PROJECTOR_LIGHT = 3,
        R3D_CUBE_LIGHT = 4
    }
    */
}
