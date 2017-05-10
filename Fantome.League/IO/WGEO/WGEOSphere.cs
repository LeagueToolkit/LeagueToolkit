using Fantome.League.Helpers.Structures;
using System.IO;

namespace Fantome.League.IO.WGEO
{
    public struct WGEOSphere
    {
        public Vector3 Position;
        public float Radius;
        public WGEOSphere(Vector3 Position, float Radius)
        {
            this.Position = Position;
            this.Radius = Radius;
        }
        public WGEOSphere(BinaryReader br)
        {
            this.Position = new Vector3(br);
            this.Radius = br.ReadSingle();
        }
        public void Write(BinaryWriter bw)
        {
            Position.Write(bw);
            bw.Write(Radius);
        }
    }
}
