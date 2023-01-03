using LeagueToolkit.Helpers.Cryptography;
using System.Collections.Generic;
using System.Numerics;

namespace LeagueToolkit.IO.AnimationFile
{
    public class AnimationTrack
    {
        public uint JointHash { get; }
        public string JointName { get; }
        internal uint V3Flag { get; set; }

        public List<Vector3> Translations { get; } = new();
        public List<Vector3> Scales { get; } = new();
        public List<Quaternion> Rotations { get; } = new();

        internal AnimationTrack(uint jointHash)
        {
            this.JointHash = jointHash;
        }

        internal AnimationTrack(string jointName) : this(Cryptography.ElfHash(jointName))
        {
            this.JointName = jointName;
        }
    }
}
