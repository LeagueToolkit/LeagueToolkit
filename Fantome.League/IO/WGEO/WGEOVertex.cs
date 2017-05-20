using Fantome.League.Helpers.Structures;
using System.IO;

namespace Fantome.League.IO.WGEO
{
    public struct WGEOVertex
    {
        public Vector3 Position;
        public Vector2 UV;

        public WGEOVertex(Vector3 Position, Vector2 UV)
        {
            this.Position = Position;
            this.UV = UV;
        }

        public WGEOVertex(BinaryReader br)
        {
            this.Position = new Vector3(br);
            this.UV = new Vector2(br);
        }

        public void Write(BinaryWriter bw)
        {
            Position.Write(bw);
            UV.Write(bw);
        }
    }
}
