using System.Collections.Generic;
using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.Skeleton
{
    public class SKLRawBone : SKLBone
    {
        public short ID { get; set; }
        public short ParentID { get; set; }
        public ushort Unknown1 { get; private set; }
        public ushort Unknown2 { get; private set; }
        public uint Hash { get; private set; }
        public float CollisionRadius { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Scale { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector3 CT { get; private set; }
        public float[] Extra { get; private set; }

        public SKLRawBone(BinaryReader br)
        {
            this.Unknown1 = br.ReadUInt16();
            this.ID = br.ReadInt16();
            this.ParentID = br.ReadInt16();
            this.Unknown2 = br.ReadUInt16();
            this.Hash = br.ReadUInt32();
            this.CollisionRadius = br.ReadSingle();
            this.Position = new Vector3(br);
            this.Scale = new Vector3(br);
            this.Rotation = new Quaternion(br);
            this.CT = new Vector3(br);
            this.Extra = new float[]
            {
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle()
            };
        }
    }
}
