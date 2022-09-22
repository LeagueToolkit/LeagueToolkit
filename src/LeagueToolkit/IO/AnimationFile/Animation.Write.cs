using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.AnimationFile
{
    public partial class Animation
    {
        public void Write(string fileLocation, uint version) => Write(File.Create(fileLocation), version);
        public void Write(Stream stream, uint version)
        {
            using BinaryWriter bw = new BinaryWriter(stream);
            switch (version)
            {
                case 3:
                    WriteV3(bw);
                    break;
                case 4:
                    WriteV4(bw);
                    break;
                case 5:
                    WriteV5(bw);
                    break;
                default:
                    throw new Exception($"Unsupported version: {version}");
            }
        }

        private void WriteV3(BinaryWriter bw)
        {
            const uint nameSize = 32;

            bw.Write(Encoding.ASCII.GetBytes("r3d2anmd")); // v3-5 magic
            bw.Write(3); // version 3

            bw.Write(0x84211248); // format token
            bw.Write((uint)this.Tracks.Count); // joint count
            bw.Write(this.FramesPerTrack);
            bw.Write((uint)this.FPS);

            byte[] nameFile = new byte[nameSize];

            foreach (AnimationTrack joint in this.Tracks)
            {
                string name = joint.JointName;
                if (!string.IsNullOrEmpty(name))
                {
                    Array.Clear(nameFile, 0, nameFile.Length);
                    Encoding.UTF8.GetBytes(name, 0, name.Length, nameFile, 0);
                    bw.Write(nameFile);
                    bw.Write(joint.V3Flag);
                    for (int frame = 0; frame < this.FramesPerTrack; frame++)
                    {
                        bw.WriteQuaternion(joint.Rotations[frame]);
                        bw.WriteVector3(joint.Translations[frame]);
                    }
                }
            }
        }

        private void WriteV4(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes("r3d2anmd")); // v3-5 magic
            bw.Write(4); // version 4

            bw.Seek(4, SeekOrigin.Current); // File Size, will Seek to start and write it at the end
            bw.Seek(4, SeekOrigin.Current); // Format token, unused
            bw.Seek(4, SeekOrigin.Current); // version, unused
            bw.Seek(4, SeekOrigin.Current); // flags, unused

            bw.Write(this.Tracks.Count);
            bw.Write(this.FramesPerTrack);
            bw.Write(this.FrameDuration);

            const int vectorsOffset = 64;

            bw.Seek(4, SeekOrigin.Current); // jointHashesOffset, unused
            bw.Seek(4, SeekOrigin.Current); // assetNameOffset, unused
            bw.Seek(4, SeekOrigin.Current); // timeOffset, unused
            bw.Write(vectorsOffset);
            bw.Seek(4, SeekOrigin.Current); // rotations offset
            bw.Seek(4, SeekOrigin.Current); // frames offset

            bw.Seek(12, SeekOrigin.Current); // padding

            // write all vectors
            List<Vector3> allVectors = this.Tracks.SelectMany(joint => joint.Scales.Concat(joint.Translations)).Distinct().ToList();
            allVectors.Sort(Vector3Extensions.Vector3Comparer.Comparer);
            foreach (Vector3 vector in allVectors)
            {
                bw.WriteVector3(vector);
            }

            // write all rotation data
            int rotationsOffset = (int)bw.BaseStream.Position;
            List<Quaternion> allRotations = this.Tracks.SelectMany(joint => joint.Rotations).Distinct().ToList();
            allRotations.Sort(QuaternionExtensions.QuaternionComparer.Comparer);
            foreach (Quaternion rotation in allRotations)
            {
                bw.WriteQuaternion(rotation);
            }

            // write array indices
            int framesOffset = (int)bw.BaseStream.Position;
            foreach (AnimationTrack joint in this.Tracks)
            {
                for (int frame = 0; frame < this.FramesPerTrack; frame++)
                {
                    bw.Write(joint.JointHash);
                    bw.Write((ushort)allVectors.BinarySearch(joint.Translations[frame], Vector3Extensions.Vector3Comparer.Comparer));
                    bw.Write((ushort)allVectors.BinarySearch(joint.Scales[frame], Vector3Extensions.Vector3Comparer.Comparer));
                    bw.Write((ushort)allRotations.BinarySearch(joint.Rotations[frame], QuaternionExtensions.QuaternionComparer.Comparer));
                    bw.Write((ushort)0); // padding
                }
            }

            // write file size
            uint fileSize = (uint)bw.BaseStream.Length;
            bw.BaseStream.Seek(12, SeekOrigin.Begin);
            bw.Write(fileSize - 12);

            bw.Seek(56, SeekOrigin.Begin);
            bw.Write(rotationsOffset - 12);
            bw.Write(framesOffset - 12);
        }

        private void WriteV5(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes("r3d2anmd")); // v3-5 magic
            bw.Write(5); // version 5

            bw.Seek(4, SeekOrigin.Current); // File Size, will Seek to start and write it at the end
            bw.Seek(4, SeekOrigin.Current); // Format token, unused
            bw.Seek(4, SeekOrigin.Current); // version, unused
            bw.Seek(4, SeekOrigin.Current); // flags, unused

            bw.Write(this.Tracks.Count);
            bw.Write(this.FramesPerTrack);
            bw.Write(this.FrameDuration);

            const int vectorsOffset = 64;

            bw.Seek(4, SeekOrigin.Current); // jointHashesOffset
            bw.Seek(4, SeekOrigin.Current); // assetNameOffset, unused
            bw.Seek(4, SeekOrigin.Current); // timeOffset, unused
            bw.Write(vectorsOffset);
            bw.Seek(4, SeekOrigin.Current); // rotations offset
            bw.Seek(4, SeekOrigin.Current); // frames offset

            bw.Seek(12, SeekOrigin.Current); // padding

            // write all vectors
            List<Vector3> allVectors = this.Tracks.SelectMany(joint => joint.Scales.Concat(joint.Translations))
                .Distinct()
                .ToList();
            allVectors.Sort(Vector3Extensions.Vector3Comparer.Comparer);
            foreach (Vector3 vector in allVectors)
            {
                bw.WriteVector3(vector);
            }

            // write all rotation data
            int rotationsOffset = (int)bw.BaseStream.Position;
            List<QuantizedQuaternion> allRotationsCompressed = this.Tracks
                .SelectMany(joint => joint.Rotations.Select(QuantizedQuaternion.Compress)).Distinct().ToList();
            allRotationsCompressed.Sort();
            foreach (QuantizedQuaternion compressedRotation in allRotationsCompressed)
            {
                bw.Write(compressedRotation.GetBytes());
            }

            // write joint hashes
            int jointHashesOffset = (int)bw.BaseStream.Position;
            foreach (AnimationTrack joint in this.Tracks)
            {
                bw.Write(joint.JointHash);
            }

            // write array indices
            int framesOffset = (int)bw.BaseStream.Position;
            for (int frame = 0; frame < this.FramesPerTrack; frame++)
            {
                foreach (AnimationTrack joint in this.Tracks)
                {
                    bw.Write((ushort)allVectors.BinarySearch(joint.Translations[frame], Vector3Extensions.Vector3Comparer.Comparer));
                    bw.Write((ushort)allVectors.BinarySearch(joint.Scales[frame], Vector3Extensions.Vector3Comparer.Comparer));
                    bw.Write((ushort)allRotationsCompressed.BinarySearch(QuantizedQuaternion.Compress(joint.Rotations[frame])));
                }
            }

            // write file size
            uint fileSize = (uint)bw.BaseStream.Length;
            bw.BaseStream.Seek(12, SeekOrigin.Begin);
            bw.Write(fileSize - 12);

            bw.Seek(40, SeekOrigin.Begin);
            bw.Write(jointHashesOffset - 12);
            bw.Seek(56, SeekOrigin.Begin);
            bw.Write(rotationsOffset - 12);
            bw.Write(framesOffset - 12);
        }
    }
}
