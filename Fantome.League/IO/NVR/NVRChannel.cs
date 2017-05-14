using System.Text;
using System.IO;
using Fantome.League.Helpers.Structures;

namespace Fantome.League.IO.NVR
{
    public class NVRChannel
    {
        public Vector4 Color { get; private set; }
        public string Name { get; private set; }
        public D3DMATRIX Matrix { get; private set; }

        public NVRChannel(BinaryReader br)
        {
            this.Color = new Vector4(br);
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(260)).Replace("\0", "");
            this.Matrix = new D3DMATRIX(br);
        }

        public NVRChannel(string name, Vector4 color, D3DMATRIX matrix)
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
