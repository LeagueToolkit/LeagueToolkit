using Fantome.Libraries.League.Helpers.Extensions;
using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Quaternion = Fantome.Libraries.League.Helpers.Structures.Quaternion;
using Vector3 = Fantome.Libraries.League.Helpers.Structures.Vector3;

namespace LeagueFileTranslator.FileTranslators.SKL.IO
{
    public class SkeletonJoint
    {
        public bool IsLegacy { get; private set; }

        public ushort Flags { get; private set; }
        public short ID { get; private set; }
        public short ParentID { get; private set; }
        public uint Hash { get; private set; }
        public float Radius { get; private set; } = 2.1f;
        public string Name { get; private set; }
        public Matrix4x4 Local { get; private set; }
        public Matrix4x4 Global { get; private set; }
        public Matrix4x4 InverseGlobal { get; private set; }

        public SkeletonJoint(BinaryReader br, bool isLegacy, short id = 0)
        {
            this.IsLegacy = isLegacy;

            if (isLegacy)
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

                this.Global = new Matrix4x4()
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
            else
            {
                this.Flags = br.ReadUInt16();
                this.ID = br.ReadInt16();
                this.ParentID = br.ReadInt16();
                br.ReadInt16(); //padding
                this.Hash = br.ReadUInt32();
                this.Radius = br.ReadSingle();

                Vector3 localTranslation = br.ReadVector3();
                Vector3 localScale = br.ReadVector3();
                Quaternion localRotation = br.ReadQuaternion();
                ComposeLocal(localTranslation, localScale, localRotation);

                Vector3 inverseGlobalTranslation = br.ReadVector3();
                Vector3 inverseGlobalScale = br.ReadVector3();
                Quaternion inverseGlobalRotation = br.ReadQuaternion();
                ComposeInverseGlobal(inverseGlobalTranslation, inverseGlobalScale, inverseGlobalRotation);

                int nameOffset = br.ReadInt32();
                long returnOffset = br.BaseStream.Position;

                br.BaseStream.Seek(returnOffset - 4 + nameOffset, SeekOrigin.Begin);
                this.Name = br.ReadZeroTerminatedString();
                br.BaseStream.Seek(returnOffset, SeekOrigin.Begin);
            }
        }

        private void ComposeLocal(Vector3 translation, Vector3 scale, Quaternion rotation)
        {
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(translation);
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);

            this.Local = scaleMatrix * rotationMatrix * translationMatrix;
        }
        private void ComposeInverseGlobal(Vector3 translation, Vector3 scale, Quaternion rotation)
        {
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(translation);
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);

            this.InverseGlobal = translationMatrix * rotationMatrix * scaleMatrix;
        }
    }
}
