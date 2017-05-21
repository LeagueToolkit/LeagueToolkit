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
        public BLNDHeader          Header;
        public List<BLNDBlend>     Blends     { get; private set; } = new List<BLNDBlend>();
        public List<BLNDCategory>  Categories { get; private set; } = new List<BLNDCategory>();
        public List<BLNDEvent>     Entries    { get; private set; } = new List<BLNDEvent>();
        public List<BLNDUnknown>   Unknowns   { get; private set; } = new List<BLNDUnknown>();
        public List<Int32>         Negatives  { get; private set; } = new List<Int32>();
        public List<UInt32>        Nulls      { get; private set; } = new List<UInt32>();
        public List<BLNDAnimation> Animations { get; private set; } = new List<BLNDAnimation>();
        public string              Skeleton   { get; private set; }

        public BLNDFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                Header = new BLNDHeader(br);

                br.Seek(Header.OffsetBlend, SeekOrigin.Begin);
                for(int i = 0; i < Header.BlendCount; i++)
                {
                    Blends.Add(new BLNDBlend(br));
                }

                br.Seek(Header.OffsetCategory, SeekOrigin.Begin);
                for(int i = 0; i < Header.CategoryCount; i++)
                {
                    Categories.Add(new BLNDCategory(br));
                }

                br.Seek(Header.OffsetEvents, SeekOrigin.Begin);
                for(int i = 0; i < Header.EntriesCount; i++)
                {
                    UInt32 returnOffset = (UInt32)br.BaseStream.Position;
                    Entries.Add(new BLNDEvent(br.ReadUInt32() + returnOffset, returnOffset, i, br));
                }

                br.Seek(Header.OffsetUnknownSector, SeekOrigin.Begin);
                for(int i = 0; i < Header.UnkSectorEntryCount; i++)
                {
                    UInt32 returnOffset = (UInt32)br.BaseStream.Position;
                    Unknowns.Add(new BLNDUnknown(br, br.ReadUInt32(), returnOffset));
                }

                br.Seek(Header.OffsetNegatives, SeekOrigin.Begin);
                for(int i = 0; i < Header.NegativesCount; i++)
                {
                    Negatives.Add(br.ReadInt32());
                }

                br.Seek(Header.OffsetNulls, SeekOrigin.Begin);
                for(int i = 0; i < Header.NullsCount; i++)
                {
                    Nulls.Add(br.ReadUInt32());
                }

                br.Seek(Header.OffsetAnimation, SeekOrigin.Begin);
                for(int i = 0; i < Header.AnimationCount; i++)
                {
                    Animations.Add(new BLNDAnimation(br));
                }

                br.Seek(Header.OffsetSKL, SeekOrigin.Begin);
                Skeleton = br.ReadString(4);

                foreach(BLNDBlend blend in Blends)
                {
                    blend.AssignEntries(Entries[(int)blend.FromBlend], Entries[(int)blend.ToBlend]);
                }
            }
        }
    }
}
