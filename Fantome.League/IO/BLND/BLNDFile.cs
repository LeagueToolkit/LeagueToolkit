using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.League.IO.BLND
{
    public class BLNDFile
    {
        public List<BLNDBlend> Blends { get; private set; } = new List<BLNDBlend>();
        public List<BLNDCategory> Categories { get; private set; } = new List<BLNDCategory>();
        public List<BLNDEvent> Entries { get; private set; } = new List<BLNDEvent>();
        public List<BLNDUnknown> Unknowns { get; private set; } = new List<BLNDUnknown>();
        public List<int> Negatives { get; private set; } = new List<int>();
        public List<uint> Nulls { get; private set; } = new List<uint>();
        public List<BLNDAnimation> Animations { get; private set; } = new List<BLNDAnimation>();
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

                uint zero1 = br.ReadUInt32();
                uint creationID = br.ReadUInt32();
                uint zero2 = br.ReadUInt32();
                uint entriesCount = br.ReadUInt32();
                uint blendCount = br.ReadUInt32();
                uint unknown = br.ReadUInt32();
                uint categoryCount = br.ReadUInt32();
                uint animationCount = br.ReadUInt32();
                uint unknownSectorEntryCount = br.ReadUInt32();
                uint negativesCount = br.ReadUInt32();
                uint zero4 = br.ReadUInt32();
                uint zero5 = br.ReadUInt32();
                uint offsetBlend = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint zero6 = br.ReadUInt32();
                uint offsetCategory = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetEvents = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetUnknownSector = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetNegatives = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint offsetNulls = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint nullsCount = br.ReadUInt32();
                uint offsetAnimation = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint zero7 = br.ReadUInt32();
                uint offsetSkeleton = (uint)br.BaseStream.Position + br.ReadUInt32();
                uint zero8 = br.ReadUInt32();

                br.Seek(offsetBlend, SeekOrigin.Begin);
                for (int i = 0; i < blendCount; i++)
                {
                    this.Blends.Add(new BLNDBlend(br));
                }

                br.Seek(offsetCategory, SeekOrigin.Begin);
                for (int i = 0; i < categoryCount; i++)
                {
                    this.Categories.Add(new BLNDCategory(br));
                }

                br.Seek(offsetEvents, SeekOrigin.Begin);
                for (int i = 0; i < entriesCount; i++)
                {
                    UInt32 returnOffset = (UInt32)br.BaseStream.Position;
                    this.Entries.Add(new BLNDEvent(br.ReadUInt32() + returnOffset, returnOffset, i, br));
                }

                br.Seek(offsetUnknownSector, SeekOrigin.Begin);
                for (int i = 0; i < unknownSectorEntryCount; i++)
                {
                    UInt32 returnOffset = (UInt32)br.BaseStream.Position;
                    this.Unknowns.Add(new BLNDUnknown(br, br.ReadUInt32(), returnOffset));
                }

                br.Seek(offsetNegatives, SeekOrigin.Begin);
                for (int i = 0; i < negativesCount; i++)
                {
                    this.Negatives.Add(br.ReadInt32());
                }

                br.Seek(offsetNulls, SeekOrigin.Begin);
                for (int i = 0; i < nullsCount; i++)
                {
                    this.Nulls.Add(br.ReadUInt32());
                }

                br.Seek(offsetAnimation, SeekOrigin.Begin);
                for (int i = 0; i < animationCount; i++)
                {
                    this.Animations.Add(new BLNDAnimation(br));
                }

                br.Seek(offsetSkeleton, SeekOrigin.Begin);
                this.Skeleton = br.ReadString(4);
            }
        }
    }
}
