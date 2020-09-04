using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.StaticObjectFile
{
    public class StaticObjectVertex
    {
        public Vector3 Position { get; set; }
        public Vector2 UV { get; set; }
        public Color? Color { get; set; }

        public StaticObjectVertex(Vector3 position, Vector2 uv)
        {
            this.Position = position;
            this.UV = uv;
        }

        public StaticObjectVertex(Vector3 position, Vector2 uv, Color color) : this(position, uv)
        {
            this.Color = color;
        }
    }
}
