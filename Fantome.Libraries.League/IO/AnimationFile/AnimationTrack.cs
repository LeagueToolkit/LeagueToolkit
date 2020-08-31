using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantome.Libraries.League.IO.AnimationFile
{
    public class AnimationTrack
    {
        public string Name { get; private set; }
        public uint JointHash { get; private set; }

        public List<Vector3> Translations { get; private set; } = new List<Vector3>();
        public List<Vector3> Scales { get; private set; } = new List<Vector3>();
        public List<Quaternion> Rotations { get; private set; } = new List<Quaternion>();

        public AnimationTrack(uint jointHash)
        {
            this.JointHash = jointHash;
        }
    }
}
