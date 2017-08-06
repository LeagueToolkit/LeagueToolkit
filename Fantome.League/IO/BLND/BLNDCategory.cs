using System.IO;
using System.Text;

namespace Fantome.League.IO.BLND
{
    public class BLNDCategory
    {
        public uint Size { get; private set; }
        public float Value { get; private set; }
        public uint Flag { get; private set; }
        public uint Index { get; private set; }
        public string Name { get; private set; }
        public BLNDCategory(BinaryReader br)
        {
            this.Size = br.ReadUInt32();
            this.Value = br.ReadSingle();
            this.Flag = br.ReadUInt32();
            this.Index = br.ReadUInt32();
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(32)).Replace("\0", "");
        }
    }
}
