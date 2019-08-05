using System.IO;

namespace Fantome.Libraries.League.IO.BLND
{
    public class BLNDBlendData
    {
        public uint FromAnimationID { get; set; }
        public uint ToAnimationID { get; set; }
        public uint BlendFlags { get; set; }
        public float BlendTime { get; set; }

        public BLNDBlendData(BinaryReader br)
        {
            this.FromAnimationID = br.ReadUInt32();
            this.ToAnimationID = br.ReadUInt32();
            this.BlendFlags = br.ReadUInt32();
            this.BlendTime = br.ReadSingle();
        }
    }
}
