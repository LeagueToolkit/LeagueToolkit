using System.IO;
using System.Text;

namespace Fantome.League.IO.BLND
{
    public class BLNDTrack
    {
        public uint Size { get; private set; }
        public float Value { get; private set; }
        public uint Flag { get; private set; }
        public uint Index { get; private set; }
        public string Name { get; private set; }
        public BLNDTrack(BinaryReader br)
        {
            this.Size = br.ReadUInt32();
            this.Value = br.ReadSingle();
            this.Flag = br.ReadUInt32();
            this.Index = br.ReadUInt32();
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(32)).Replace("\0", "");
        }
    }
}
