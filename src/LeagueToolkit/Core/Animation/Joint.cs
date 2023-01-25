using CommunityToolkit.Diagnostics;
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
        /// <remarks>Equivalent to <c>S * R * T</c></remarks>
        public Matrix4x4 LocalTransform { get; }

        /// <summary>The local translation of the joint</summary>
        public Vector3 LocalTranslation { get; }

        /// <summary>The local scale of the joint</summary>
        public Vector3 LocalScale { get; }

        /// <summary>The local rotation of the joint</summary>
        public Quaternion LocalRotation { get; }

        /// <summary>
        /// The inverse-bind transform of the joint
        /// </summary>
        /// <remarks>Equivalent to <c>S * R * T</c></remarks>
        public Matrix4x4 InverseBindTransform { get; }

        /// <summary>The inverse bind translation of the joint</summary>
        public Vector3 InverseBindTranslation { get; }

        /// <summary>The inverse bind scale of the joint</summary>
        public Vector3 InverseBindScale { get; }

        /// <summary>The inverse bind rotation of the joint</summary>
        public Quaternion InverseBindRotation { get; }

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
            Guard.IsNotNullOrEmpty(name, nameof(name));

            this.Name = name;
            this.Flags = flags;
            this.Id = id;
            this.ParentId = parentId;
            this.Radius = radius;

            // Set local transform
            Matrix4x4.Decompose(
                localTransform,
                out Vector3 localScale,
                out Quaternion localRotation,
                out Vector3 localTranslation
            );

            this.LocalTransform = localTransform;
            this.LocalTranslation = localTranslation;
            this.LocalScale = localScale;
            this.LocalRotation = localRotation;

            // Set inverse bind transform
            Matrix4x4.Decompose(
                inverseBindTransform,
                out Vector3 inverseBindScale,
                out Quaternion inverseBindRotation,
                out Vector3 inverseBindTranslation
            );

            this.InverseBindTransform = inverseBindTransform;
            this.InverseBindTranslation = inverseBindTranslation;
            this.InverseBindScale = inverseBindScale;
            this.InverseBindRotation = inverseBindRotation;
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
            bw.WriteVector3(this.LocalTranslation);
            bw.WriteVector3(this.LocalScale);
            bw.WriteQuaternion(this.LocalRotation);

            // Write inverse bind transform
            bw.WriteVector3(this.InverseBindTranslation);
            bw.WriteVector3(this.InverseBindScale);
            bw.WriteQuaternion(this.InverseBindRotation);

            // Write relative name offset
            bw.Write(nameOffset - (int)bw.BaseStream.Position);
        }
    }
}
