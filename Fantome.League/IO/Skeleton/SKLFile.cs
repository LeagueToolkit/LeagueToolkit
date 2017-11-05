using Fantome.Libraries.League.Helpers.Cryptography;
using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fantome.Libraries.League.IO.Skeleton
{
    /// <summary>
    /// Represents a Skeleton file
    /// </summary>
    public class SKLFile
    {
        public SKLType Type { get; private set; }
        private uint _id;
        public List<ISKLBone> Bones { get; private set; } = new List<ISKLBone>();
        public List<ushort> BoneIDs { get; private set; } = new List<ushort>();

        public SKLFile() { }

        public SKLFile(List<ISKLBone> bones, SKLType type)
        {
            this.Type = type;
            this.Bones = bones;
            this._id = 0x84211248;
        }

        public SKLFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                br.BaseStream.Seek(8, SeekOrigin.Begin);
                uint version = br.ReadUInt32();
                if (version == 1 || version == 2)
                {
                    this.Type = SKLType.Legacy;
                }
                else if (version == 0)
                {
                    this.Type = SKLType.Raw;
                }
                else
                {
                    throw new Exception("This is not a valid SKL file");
                }

                if (this.Type == SKLType.Raw)
                {
                    br.BaseStream.Seek(0, SeekOrigin.Begin);
                    uint fileLength = br.ReadUInt32();
                    uint unknown = br.ReadUInt32();
                    if (fileLength != br.BaseStream.Length)
                    {
                        throw new Exception("This SKL file is corrupted");
                    }
                    br.ReadUInt32();

                    ushort zero = br.ReadUInt16();
                    ushort boneCount = br.ReadUInt16();
                    uint boneIDCount = br.ReadUInt32();
                    uint offsetBoneData = br.ReadUInt32();
                    uint offsetBoneIDMap = br.ReadUInt32();
                    uint offsetBoneIDs = br.ReadUInt32();
                    uint offsetUnknown1 = br.ReadUInt32();
                    uint offsetUnknown2 = br.ReadUInt32();
                    uint offsetBoneNames = br.ReadUInt32();

                    br.BaseStream.Seek(offsetBoneData, SeekOrigin.Begin);
                    for (int i = 0; i < boneCount; i++)
                    {
                        this.Bones.Add(new SKLRawBone(br));
                    }

                    br.BaseStream.Seek(offsetBoneIDs, SeekOrigin.Begin);
                    for (int i = 0; i < boneIDCount; i++)
                    {
                        this.BoneIDs.Add(br.ReadUInt16());
                    }

                    br.BaseStream.Seek(offsetBoneNames, SeekOrigin.Begin);
                    string[] boneNames = Encoding.ASCII.GetString(br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position)))
                        .Split(new char[] { '\u0000' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string boneName in boneNames)
                    {
                        this.Bones.Find(x => (x as SKLRawBone).Hash == Cryptography.LeagueHash(boneName)).Name = boneName;
                    }
                }
                else
                {
                    br.BaseStream.Seek(0, SeekOrigin.Begin);
                    string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                    if (magic != "r3d2sklt")
                    {
                        throw new Exception("This is not a valid SKL file");
                    }

                    br.ReadUInt32();
                    this._id = br.ReadUInt32();

                    uint boneCount = br.ReadUInt32();
                    for (int i = 0; i < boneCount; i++)
                    {
                        SKLLegacyBone bone  = new SKLLegacyBone(br, i);

                    }

                    uint boneIndexCount = br.ReadUInt32();
                    for(int i = 0; i < boneIndexCount; i++)
                    {
                        this.BoneIDs.Add((ushort)br.ReadUInt32());
                    }

                    foreach (SKLLegacyBone bone in bones)
                    {
                        bone.LocalMatrix = bone._parentID != -1 ? bones[bone._parentID].GlobalMatrix.Inverse() * bone.GlobalMatrix : bone.GlobalMatrix;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Types of SKL Files
    /// </summary>
    public enum SKLType
    {
        /// <summary>
        /// Represents a Legacy SKL File whose version is either 1 or 2
        /// </summary>
        Legacy,
        /// <summary>
        /// Represents a Raw SKL File whose version is 0
        /// </summary>
        Raw
    }
}
