using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOVertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 UV1 { get; set; }
        public Vector2 UV2 { get; set; }

        public MGEOVertex(Vector3 position, Vector3 normal, Vector2 uv1, Vector2 uv2)
        {
            this.Position = position;
            this.Normal = normal;
            this.UV1 = uv1;
            this.UV2 = uv2;
        }

        public MGEOVertex(BinaryReader br)
        {
            this.Position = new Vector3(br);
            this.Normal = new Vector3(br);
        }

        public void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            this.Normal.Write(bw);
            this.UV1.Write(bw);
            this.UV2.Write(bw);
        }
    }
}
