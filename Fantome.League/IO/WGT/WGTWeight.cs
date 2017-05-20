using Fantome.League.Helpers.Structures;
using System.IO;

namespace Fantome.League.IO.WGT
{
    public class WGTWeight
    {
        public Vector4Byte Indices { get; private set; }
        public Vector4 Weights { get; private set; }

        public WGTWeight(BinaryReader br)
        {
            this.Indices = new Vector4Byte(br);
            this.Weights = new Vector4(br);
        }

        public void Write(BinaryWriter bw)
        {
            this.Indices.Write(bw);
            this.Weights.Write(bw);
        }
    }
}
