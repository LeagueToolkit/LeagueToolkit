using System.IO;

namespace Fantome.League.IO.BLND
{
    public class BLNDBlend
    {
        public uint FromBlend { get; private set; }
        public uint ToBlend { get; private set; }
        public uint Flags { get; private set; }
        public float TimeToBlend { get; private set; }
        public BLNDBlend(BinaryReader br)
        {
            this.FromBlend = br.ReadUInt32();
            this.ToBlend = br.ReadUInt32();
            this.Flags = br.ReadUInt32();
            this.TimeToBlend = br.ReadSingle();
        }
    }
}
