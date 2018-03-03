using System.IO;

namespace Fantome.Libraries.League.IO.NavigationGrid
{
    public class NavigationGridHintNode
    {
        public float[] Distances { get; private set; } = new float[900];
        public ushort X { get; private set; }
        public ushort Y { get; private set; }

        public NavigationGridHintNode(BinaryReader br)
        {
            for (int i = 0; i < 900; i++)
            {
                this.Distances[i] = br.ReadSingle();
            }

            this.X = br.ReadUInt16();
            this.Y = br.ReadUInt16();
        }
    }
}
