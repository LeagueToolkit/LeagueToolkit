using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.StaticObject
{
    public class StaticObjectVertex
    {
        public Vector3 Position { get; set; }
        public Vector2 UV { get; set; }
        public ColorRGBAVector4Byte Color { get; set; }

        public StaticObjectVertex(Vector3 position, Vector2 uv)
        {
            this.Position = position;
            this.UV = uv;
        }

        public StaticObjectVertex(Vector3 position, Vector2 uv, ColorRGBAVector4Byte color) : this(position, uv)
        {
            this.Color = color;
        }
    }
}
