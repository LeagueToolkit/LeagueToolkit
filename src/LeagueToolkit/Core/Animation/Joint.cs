using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Extensions;
using System.Diagnostics;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.Core.Animation
{
    /// <summary>
    /// Represents a skeleton joint for a <see cref="RigResource"/>
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public sealed class Joint
    {
        /// <summary>
        /// The name of the joint
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The flags of the joint
        /// </summary>
        public ushort Flags { get; }

        /// <summary>
        /// The ID of the joint
        /// </summary>
        public short Id { get; }

        /// <summary>
        /// The Parent ID of the joint
        /// </summary>
        /// <remarks>
        /// Set to -1 if joint has no parent
        /// </remarks>
        public short ParentId { get; }

        /// <summary>
        /// The radius of the joint
        /// </summary>
        public float Radius { get; } = 2.1f;

        /// <summary>
        /// The local transform of the joint
        /// </summary>
        public Matrix4x4 LocalTransform { get; }

        /// <summary>
        /// The inverse-bind transform of the joint
        /// </summary>
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
