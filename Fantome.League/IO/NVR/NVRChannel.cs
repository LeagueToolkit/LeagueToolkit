using System.Text;
using System.IO;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.NVR
{
    public class NVRChannel
    {
        public ColorRGBAVector4 Color { get; private set; }
        public string Name { get; private set; }
        public R3DMatrix44 Matrix { get; private set; }

        public NVRChannel(BinaryReader br)
        {
            this.Color = new ColorRGBAVector4(br);
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(260)).Replace("\0", "");
            this.Matrix = new R3DMatrix44(br);
        }

        public NVRChannel(string name, ColorRGBAVector4 color, R3DMatrix44 matrix)
        {
            this.Name = name;
            this.Color = color;
            this.Matrix = matrix;
        }

        public void Write(BinaryWriter bw)
        {
            this.Color.Write(bw);
            bw.Write(this.Name.PadRight(260, '\u0000').ToCharArray());
            this.Matrix.Write(bw);
        }
    }
}
