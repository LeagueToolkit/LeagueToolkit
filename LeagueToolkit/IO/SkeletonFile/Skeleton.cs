using LeagueToolkit.Helpers.Cryptography;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueToolkit.IO.SkeletonFile
{
    public class Skeleton
    {
        internal const int FORMAT_TOKEN = 0x22FD4FC3; // FNV hash of the format token string

        public bool IsLegacy { get; private set; }

        public List<SkeletonJoint> Joints { get; private set; } = new List<SkeletonJoint>();
        public List<short> Influences { get; private set; } = new List<short>();
        public string Name { get; private set; } = string.Empty;
        public string AssetName { get; private set; } = string.Empty;

        public Skeleton(List<SkeletonJoint> joints, List<short> influenceMap)
        {
            this.Joints = joints;
            this.Influences = influenceMap;

            foreach(SkeletonJoint joint in this.Joints)
            {
                if(joint.ParentID == -1)
                {
                    joint.GlobalTransform = joint.LocalTransform;
                }
                else
                {
                    joint.GlobalTransform = this.Joints[joint.ParentID].GlobalTransform * joint.LocalTransform;
                }
            }
        }
        public Skeleton(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public Skeleton(Stream stream, bool leaveOpen = false)
        {
            using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, leaveOpen))
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
                throw new UnsupportedFileVersionException();
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
                throw new InvalidFileSignatureException();
            }

            uint version = br.ReadUInt32();
            if (version != 1 && version != 2)
            {
                throw new UnsupportedFileVersionException();
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

            // Derive Local transformations
            foreach(SkeletonJoint joint in this.Joints)
            {
                if(joint.ParentID == -1)
                {
                    joint.LocalTransform = joint.GlobalTransform;
                }
                else
                {
                    SkeletonJoint parent = this.Joints[joint.ParentID];

                    joint.LocalTransform = joint.GlobalTransform * parent.InverseBindTransform;
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(0); //File Size, will Seek to start and write it at the end
                bw.Write(FORMAT_TOKEN);
                bw.Write(0); //Version
                bw.Write((ushort)0); //Flags
                bw.Write((ushort)this.Joints.Count);
                bw.Write(this.Influences.Count);

                int jointsSectionSize = this.Joints.Count * 100;
                int jointIndicesSectionSize = this.Joints.Count * 8;
                int influencesSectionSize = this.Influences.Count * 2;
                int jointsOffset = 64;
                int jointIndicesOffset = jointsOffset + jointsSectionSize;
                int influencesOffset = jointIndicesOffset + jointIndicesSectionSize;
                int jointNamesOffset = influencesOffset + influencesSectionSize;

                bw.Write(jointsOffset); //Joints Offset
                bw.Write(jointIndicesOffset);
                bw.Write(influencesOffset);

                long nameOffsetOffset = bw.BaseStream.Position;
                bw.Seek(4, SeekOrigin.Current); //Name offset

                long assetNameOffsetOffset = bw.BaseStream.Position;
                bw.Seek(4, SeekOrigin.Current); //Asset Name offset

                bw.Write(jointNamesOffset);
                bw.Write(0xFFFFFFFF); //Write reserved offset field
                bw.Write(0xFFFFFFFF); //Write reserved offset field
                bw.Write(0xFFFFFFFF); //Write reserved offset field
                bw.Write(0xFFFFFFFF); //Write reserved offset field
                bw.Write(0xFFFFFFFF); //Write reserved offset field

                Dictionary<int, int> jointNameOffsets = new Dictionary<int, int>();
                bw.Seek(jointNamesOffset, SeekOrigin.Begin);
                for (int i = 0; i < this.Joints.Count; i++)
                {
                    jointNameOffsets.Add(i, (int)bw.BaseStream.Position);

                    bw.Write(Encoding.ASCII.GetBytes(this.Joints[i].Name));
                    bw.Write((byte)0);
                }

                bw.Seek(jointsOffset, SeekOrigin.Begin);
                for (int i = 0; i < this.Joints.Count; i++)
                {
                    this.Joints[i].Write(bw, jointNameOffsets[i]);
                }

                bw.Seek(influencesOffset, SeekOrigin.Begin);
                foreach (short influence in this.Influences)
                {
                    bw.Write(influence);
                }

                bw.Seek(jointIndicesOffset, SeekOrigin.Begin);
                foreach (SkeletonJoint joint in this.Joints)
                {
                    bw.Write(Cryptography.ElfHash(joint.Name));
                    bw.Write((ushort)0);
                    bw.Write(joint.ID);
                }

                bw.BaseStream.Seek(0, SeekOrigin.End);
                // Write Name
                long nameOffset = bw.BaseStream.Position;
                bw.Write(Encoding.ASCII.GetBytes(this.Name));
                bw.Write((byte)0);

                // Write Asset Name
                long assetNameOffset = bw.BaseStream.Position;
                bw.Write(Encoding.ASCII.GetBytes(this.AssetName));
                bw.Write((byte)0);

                // Write Name offset to header
                bw.BaseStream.Seek(nameOffsetOffset, SeekOrigin.Begin);
                bw.Write((int)nameOffset);

                // Write Asset Name offset to header
                bw.BaseStream.Seek(assetNameOffsetOffset, SeekOrigin.Begin);
                bw.Write((int)assetNameOffset);

                uint fileSize = (uint)bw.BaseStream.Length;
                bw.BaseStream.Seek(0, SeekOrigin.Begin);
                bw.Write(fileSize);
            }
        }
    }
}
