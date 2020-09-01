using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Fantome.Libraries.League.IO.AnimationFile
{
    public class AnimationTrack
    {
        public uint JointHash { get; private set; }

        public Dictionary<float, Vector3> Translations { get; internal set; } = new();
        public Dictionary<float, Vector3> Scales { get; internal set; } = new();
        public Dictionary<float, Quaternion> Rotations { get; internal set; } = new();

        public AnimationTrack(uint jointHash)
        {
            this.JointHash = jointHash;
        }
    }
}
