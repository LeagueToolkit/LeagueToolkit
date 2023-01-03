using System.Collections.Generic;
using System.Numerics;

namespace LeagueToolkit.IO.AnimationFile
{
    public class QuantizedAnimationTrack
    {
        public uint JointHash { get; private set; }

        public Dictionary<float, Vector3> Translations { get; internal set; } = new();
        public Dictionary<float, Vector3> Scales { get; internal set; } = new();
        public Dictionary<float, Quaternion> Rotations { get; internal set; } = new();

        internal QuantizedAnimationTrack(uint jointHash)
        {
            this.JointHash = jointHash;
        }
    }
}
