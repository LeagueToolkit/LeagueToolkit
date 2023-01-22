using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation
{
    [DebuggerDisplay("{Name}")]
    public sealed class Joint
    {
        public string Name { get; }
        public ushort Flags { get; }
        public short Id { get; }
        public short ParentId { get; }

        public float Radius { get; } = 2.1f;
        public Matrix4x4 LocalTransform { get; }
        public Matrix4x4 InverseBindTransform { get; }

        internal Joint(
            string name,
            ushort flags,
            short id,
            short parentId,
            float radius,
            Matrix4x4 localTransform,
            Matrix4x4 inverseBindTransform
        )
        {
            this.Name = name;
            this.Flags = flags;
            this.Id = id;
            this.ParentId = parentId;
            this.Radius = radius;
            this.LocalTransform = localTransform;
            this.InverseBindTransform = inverseBindTransform;
        }

        internal void Write(BinaryWriter bw, int nameOffset)
        {
            bw.Write(this.Flags);
            bw.Write(this.Id);
            bw.Write(this.ParentId);
            bw.Write((ushort)0); // pad
            bw.Write(Elf.HashLower(this.Name));
            bw.Write(this.Radius);

            // Write local transform
            Matrix4x4.Decompose(
                this.LocalTransform,
                out Vector3 localScale,
                out Quaternion localRotation,
                out Vector3 localTranslation
            );
            bw.WriteVector3(localTranslation);
            bw.WriteVector3(localScale);
            bw.WriteQuaternion(localRotation);

            // Write inverse bind transform
            Matrix4x4.Decompose(
                this.InverseBindTransform,
                out Vector3 inverseBindScale,
                out Quaternion inverseBindRotation,
                out Vector3 inverseBindTranslation
            );
            bw.WriteVector3(inverseBindTranslation);
            bw.WriteVector3(inverseBindScale);
            bw.WriteQuaternion(inverseBindRotation);

            bw.Write(nameOffset - (int)bw.BaseStream.Position);
        }
    }
}
