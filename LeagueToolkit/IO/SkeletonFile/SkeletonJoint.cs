using LeagueToolkit.Helpers.Cryptography;
using LeagueToolkit.Helpers.Extensions;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.SkeletonFile
{
    public class SkeletonJoint
    {
        public bool IsLegacy { get; private set; }

        public ushort Flags { get; private set; }
        public short ID { get; private set; }
        public short ParentID { get; private set; }
        public float Radius { get; private set; } = 2.1f;
        public string Name { get; private set; }
        public Matrix4x4 LocalTransform { get; internal set; }
        public Matrix4x4 GlobalTransform { get; internal set; }
        public Matrix4x4 InverseBindTransform
        {
            get 
            {
                Matrix4x4.Invert(this.GlobalTransform, out Matrix4x4 inverted);
                return inverted;
            }
        }

        public SkeletonJoint(short id, short parentId, string name, Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
        {
            this.ID = id;
            this.ParentID = parentId;
            this.Name = name;
            this.LocalTransform = ComposeLocal(localPosition, localScale, localRotation);
        }

        internal SkeletonJoint(BinaryReader br, bool isLegacy, short id = 0)
        {
            this.IsLegacy = isLegacy;

            if (isLegacy)
            {
                ReadLegacy(br, id);
            }
            else
            {
                ReadNew(br);
            }
        }
        private void ReadLegacy(BinaryReader br, short id = 0)
        {
            this.ID = id;
            this.Name = br.ReadPaddedString(32);
            this.ParentID = (short)br.ReadInt32();
            float scale = br.ReadSingle();
            float[,] transform = new float[4, 4];
            transform[0, 3] = 0;
            transform[1, 3] = 0;
            transform[2, 3] = 0;
            transform[3, 3] = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    transform[j, i] = br.ReadSingle();
                }
            }

            this.GlobalTransform = new Matrix4x4()
            {
                M11 = transform[0, 0],
                M12 = transform[0, 1],
                M13 = transform[0, 2],
                M14 = transform[0, 3],

                M21 = transform[1, 0],
                M22 = transform[1, 1],
                M23 = transform[1, 2],
                M24 = transform[1, 3],

                M31 = transform[2, 0],
                M32 = transform[2, 1],
                M33 = transform[2, 2],
                M34 = transform[2, 3],

                M41 = transform[3, 0],
                M42 = transform[3, 1],
                M43 = transform[3, 2],
                M44 = transform[3, 3],
            };
        }
        private void ReadNew(BinaryReader br)
        {
            this.Flags = br.ReadUInt16();
            this.ID = br.ReadInt16();
            this.ParentID = br.ReadInt16();
            br.ReadInt16(); //padding
            uint nameHash = br.ReadUInt32();
            this.Radius = br.ReadSingle();

            Vector3 localTranslation = br.ReadVector3();
            Vector3 localScale = br.ReadVector3();
            Quaternion localRotation = br.ReadQuaternion();
            this.LocalTransform = ComposeLocal(localTranslation, localScale, localRotation);

            Vector3 inverseGlobalTranslation = br.ReadVector3();
            Vector3 inverseGlobalScale = br.ReadVector3();
            Quaternion inverseGlobalRotation = br.ReadQuaternion();
            this.GlobalTransform = ComposeGlobal(inverseGlobalTranslation, inverseGlobalScale, inverseGlobalRotation);

            int nameOffset = br.ReadInt32();
            long returnOffset = br.BaseStream.Position;

            br.BaseStream.Seek(returnOffset - 4 + nameOffset, SeekOrigin.Begin);
            this.Name = br.ReadZeroTerminatedString();
            br.BaseStream.Seek(returnOffset, SeekOrigin.Begin);
        }

        private Matrix4x4 ComposeLocal(Vector3 translation, Vector3 scale, Quaternion rotation)
        {
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(translation);
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);

            return scaleMatrix * rotationMatrix * translationMatrix;
        }
        private Matrix4x4 ComposeGlobal(Vector3 translation, Vector3 scale, Quaternion rotation)
        {
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(translation);
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);

            Matrix4x4.Invert(scaleMatrix * rotationMatrix * translationMatrix, out Matrix4x4 global);

            return global;
        }

        internal void Write(BinaryWriter bw, int nameOffset)
        {
            bw.Write(this.Flags);
            bw.Write(this.ID);
            bw.Write(this.ParentID);
            bw.Write((ushort)0); // pad
            bw.Write(Cryptography.ElfHash(this.Name));
            bw.Write(this.Radius);
            WriteLocalTransform(bw);
            WriteInverseGlobalTransform(bw);
            bw.Write(nameOffset - (int)bw.BaseStream.Position);
        }
        private void WriteLocalTransform(BinaryWriter bw)
        {
            bw.WriteVector3(this.LocalTransform.Translation);
            bw.WriteVector3(this.LocalTransform.GetScale());
            bw.WriteQuaternion(Quaternion.CreateFromRotationMatrix(this.LocalTransform));
        }
        private void WriteInverseGlobalTransform(BinaryWriter bw)
        {
            Matrix4x4 inverse = this.InverseBindTransform;

            bw.WriteVector3(inverse.Translation);
            bw.WriteVector3(inverse.GetScale());
            bw.WriteQuaternion(Quaternion.CreateFromRotationMatrix(inverse));
        }
    }
}
