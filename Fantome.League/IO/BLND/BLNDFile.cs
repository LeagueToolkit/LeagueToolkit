using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.IO.BLND
{
    public class BLNDFile
    {
        public uint FormatToken { get; set; }
        public bool UseCascadeBlend { get; set; }
        public float CascadeBlendValue { get; set; }

        public List<BLNDBlendData> BlendData { get; set; } = new List<BLNDBlendData>();
        public List<BLNDTransitionClip> TransitionClips { get; set; } = new List<BLNDTransitionClip>();

        public string Skeleton { get; private set; }

        public BLNDFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                if (magic != "r3d2blnd")
                {
                    throw new Exception("Not a valid BLND file");
                }

                uint version = br.ReadUInt32();
                if (version != 1)
                {
                    throw new Exception("Unsupported BLND version");
                }

                uint binaryBlockVersion = br.ReadUInt32();
                this.FormatToken = br.ReadUInt32();
                uint poolDataVersion = br.ReadUInt32();

                uint clipCount = br.ReadUInt32();
                uint blendCount = br.ReadUInt32();
                uint transitionClipCount = br.ReadUInt32();
                uint trackCount = br.ReadUInt32();
                uint animationDataCount = br.ReadUInt32();
                uint maskDataCount = br.ReadUInt32();
                uint eventDataCount = br.ReadUInt32();

                this.UseCascadeBlend = br.ReadUInt32() == 1;
                this.CascadeBlendValue = br.ReadSingle();

                uint offsetBlendData = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetTransitionClips = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetBlendTracks = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetClips = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetMasks = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetEvents = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetAnimationData = (uint)br.BaseStream.Position + br.ReadUInt32();

                uint animationNameCount = br.ReadUInt32();
                uint offsetAnimationNames = (uint)br.BaseStream.Position + br.ReadUInt32();

                uint skeletonHash = br.ReadUInt32();
                uint offsetSkeleton = (uint)br.BaseStream.Position + br.ReadUInt32();

                uint extBuffer = br.ReadUInt32();

                br.BaseStream.Seek(offsetBlendData, SeekOrigin.Begin);
                for(int i = 0; i < blendCount; i++)
                {
                    this.BlendData.Add(new BLNDBlendData(br));
                }

                br.BaseStream.Seek(offsetTransitionClips, SeekOrigin.Begin);
                for (int i = 0; i < blendCount; i++)
                {
                    this.TransitionClips.Add(new BLNDTransitionClip(br));
                }
            }
        }
    }
}
