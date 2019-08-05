using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BLND
{
    public class BLNDTransitionClip
    {
        public uint FromAnimationID { get; set; }
        public uint TransitionToCount { get; set; }
        public List<TransitionToData> TransitionTo { get; set; } = new List<TransitionToData>();

        public BLNDTransitionClip(BinaryReader br)
        {
            this.FromAnimationID = br.ReadUInt32();
            this.TransitionToCount = br.ReadUInt32();

            for(int i = 0; i < this.TransitionToCount; i++)
            {
                uint offset = (uint)br.BaseStream.Position + br.ReadUInt32();
                long returnOffset = br.BaseStream.Position;

                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                this.TransitionTo.Add(new TransitionToData(br));
                br.BaseStream.Seek(returnOffset, SeekOrigin.Begin);
            }
        }

        public class TransitionToData
        {
            public uint ToAnimationID { get; set; }
            public uint TransitionAnimationID { get; set; }

            public TransitionToData(BinaryReader br)
            {
                this.ToAnimationID = br.ReadUInt32();
                this.TransitionAnimationID = br.ReadUInt32();
            }
        }
    }
}
