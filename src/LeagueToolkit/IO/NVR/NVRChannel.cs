using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.NVR
{
    public class NVRChannel
    {
        public Color Color { get; private set; }
        public string Name { get; private set; }
        public Matrix4x4 Transform { get; private set; }

        public NVRChannel(BinaryReader br)
        {
            this.Color = br.ReadColor(ColorFormat.RgbaF32);
            this.Name = br.ReadPaddedString(260);
            this.Transform = br.ReadMatrix4x4RowMajor();
        }

        public NVRChannel(string name, Color color, Matrix4x4 matrix)
        {
            this.Name = name;
            this.Color = color;
            this.Transform = matrix;
        }

        public void Write(BinaryWriter bw)
        {
            bw.WriteColor(this.Color, ColorFormat.RgbaF32);
            bw.WritePaddedString(this.Name, 260);
            bw.WriteMatrix4x4RowMajor(this.Transform);
        }
    }
}
