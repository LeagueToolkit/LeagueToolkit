using System.Text;
using System.IO;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.Helpers.Extensions;

namespace LeagueToolkit.IO.NVR
{
    public class NVRChannel
    {
        public Color Color { get; private set; }
        public string Name { get; private set; }
        public R3DMatrix44 Matrix { get; private set; }

        public NVRChannel(BinaryReader br)
        {
            this.Color = br.ReadColor(ColorFormat.RgbaF32);
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(260)).Replace("\0", "");
            this.Matrix = new R3DMatrix44(br);
        }

        public NVRChannel(string name, Color color, R3DMatrix44 matrix)
        {
            this.Name = name;
            this.Color = color;
            this.Matrix = matrix;
        }

        public void Write(BinaryWriter bw)
        {
            bw.WriteColor(this.Color, ColorFormat.RgbaF32);
            bw.Write(this.Name.PadRight(260, '\u0000').ToCharArray());
            this.Matrix.Write(bw);
        }
    }
}
