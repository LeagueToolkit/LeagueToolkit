using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.FX
{
    [DebuggerDisplay("[ Flag: {Flag}, Particle: {Particle}, Bone: {Bone}, Frames: [ {StartFrame}, {EndFrame} ] ]")]
    public class FXTrack
    {
        public UInt32 Flag { get; private set; }
        public TrackType Type { get; private set; }
        public float StartFrame { get; private set; }
        public float EndFrame { get; private set; }
        public string Particle { get; private set; }
        public string Bone { get; private set; }
        public Vector3 SpawnOffset { get; private set; }
        public FXWeaponStreakInfo StreakInfo { get; private set; }

        public FXTrack(BinaryReader br)
        {
            this.Flag = br.ReadUInt32();
            this.Type = (TrackType)br.ReadUInt32();
            this.StartFrame = br.ReadSingle();
            this.EndFrame = br.ReadSingle();

            this.Particle = Encoding.ASCII.GetString(br.ReadBytes(64));
            this.Bone = Encoding.ASCII.GetString(br.ReadBytes(64));

            char ParticleIndexOf = this.Particle.Contains("\0") ? '\u0000' : '?';
            char BoneIndexOf = this.Bone.Contains("\0") ? '\u0000' : '?';

            this.Particle = this.Particle.Remove(this.Particle.IndexOf(ParticleIndexOf));
            this.Bone = this.Bone.Remove(this.Bone.IndexOf(BoneIndexOf));

            this.SpawnOffset = new Vector3(br);
            this.StreakInfo = new FXWeaponStreakInfo(br);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Flag);
            bw.Write((UInt32)this.Type);
            bw.Write(this.StartFrame);
            bw.Write(this.EndFrame);
            bw.Write(this.Particle.PadRight(64, '\u0000').ToCharArray());
            bw.Write(this.Bone.PadRight(64, '\u0000').ToCharArray());
            SpawnOffset.Write(bw);
            StreakInfo.Write(bw);
        }
    }

    public enum TrackType : UInt32
    {
        None,
        EffPosition,
        EffBone,
        WStreak
    }
}
