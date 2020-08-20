using Fantome.Libraries.League.Helpers.Extensions;
using LeagueFileTranslator.FileTranslators.SKL.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.SkeletonFile
{
    public class Skeleton
    {
        public bool IsLegacy { get; private set; }

        public List<SkeletonJoint> Joints { get; set; } = new List<SkeletonJoint>();
        public List<short> Influences { get; set; } = new List<short>();
        public Dictionary<uint, short> JointIndices { get; set; } = new Dictionary<uint, short>();
        public string Name { get; private set; }
        public string AssetName { get; private set; }

        public Skeleton(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public Skeleton(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                br.BaseStream.Seek(4, SeekOrigin.Begin);
                uint magic = br.ReadUInt32();
                br.BaseStream.Seek(0, SeekOrigin.Begin);

                if (magic == 0x22FD4FC3)
                {
                    this.IsLegacy = false;
                    ReadNew(br);
                }
                else
                {
                    this.IsLegacy = true;
                    ReadLegacy(br);
                }
            }
        }
        private void ReadNew(BinaryReader br)
        {
            uint fileSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint version = br.ReadUInt32();
            if (version != 0)
            {
                throw new Exception("Unsupported SKL version: " + version);
            }

            ushort flags = br.ReadUInt16();
            ushort jointCount = br.ReadUInt16();
            uint influencesCount = br.ReadUInt32();
            int jointsOffset = br.ReadInt32();
            int jointIndicesOffset = br.ReadInt32();
            int influencesOffset = br.ReadInt32();
            int nameOffset = br.ReadInt32();
            int assetNameOffset = br.ReadInt32();
            int boneNamesOffset = br.ReadInt32();
            int reservedOffset1 = br.ReadInt32();
            int reservedOffset2 = br.ReadInt32();
            int reservedOffset3 = br.ReadInt32();
            int reservedOffset4 = br.ReadInt32();
            int reservedOffset5 = br.ReadInt32();

            if (jointsOffset > 0 && jointCount != 0) //wesmart
            {
                br.BaseStream.Seek(jointsOffset, SeekOrigin.Begin);

                for (int i = 0; i < jointCount; i++)
                {
                    this.Joints.Add(new SkeletonJoint(br, false));
                }
            }
            if (influencesOffset > 0 && influencesCount != 0)
            {
                br.BaseStream.Seek(influencesOffset, SeekOrigin.Begin);

                for (int i = 0; i < influencesCount; i++)
                {
                    this.Influences.Add(br.ReadInt16());
                }
            }
            if (jointIndicesOffset > 0 && jointCount != 0)
            {
                br.BaseStream.Seek(jointIndicesOffset, SeekOrigin.Begin);

                for (int i = 0; i < jointCount; i++)
                {
                    short index = br.ReadInt16();
                    br.ReadInt16(); //pad
                    uint hash = br.ReadUInt32();
                    this.JointIndices.Add(hash, index);
                }
            }
            if (nameOffset > 0)
            {
                br.BaseStream.Seek(nameOffset, SeekOrigin.Begin);
                this.Name = br.ReadZeroTerminatedString();
            }
            if (assetNameOffset > 0)
            {
                br.BaseStream.Seek(assetNameOffset, SeekOrigin.Begin);
                this.AssetName = br.ReadZeroTerminatedString();
            }
        }
        private void ReadLegacy(BinaryReader br)
        {
            string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
            if (magic != "r3d2sklt")
            {
                throw new Exception("Invalid File Magic: " + magic);
            }

            uint version = br.ReadUInt32();
            if (version != 1 && version != 2)
            {
                throw new Exception("Unsupported File Version: " + version);
            }

            uint skeletonID = br.ReadUInt32();

            uint jointCount = br.ReadUInt32();
            for (int i = 0; i < jointCount; i++)
            {
                this.Joints.Add(new SkeletonJoint(br, true, (short)i));
            }

            if (version == 2)
            {
                uint influencesCount = br.ReadUInt32();
                for (int i = 0; i < influencesCount; i++)
                {
                    this.Influences.Add((short)br.ReadUInt32());
                }
            }
            else if (version == 1)
            {
                for (int i = 0; i < this.Joints.Count; i++)
                {
                    this.Influences.Add((short)i);
                }
            }
        }
    }
}
