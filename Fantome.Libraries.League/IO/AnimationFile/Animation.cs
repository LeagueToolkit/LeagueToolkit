using Fantome.Libraries.League.Helpers.Extensions;
using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.AnimationFile
{
    public class Animation
    {
        public float FrameDuration { get; private set; }
        public float FPS => 1 / this.FrameDuration;

        public List<AnimationTrack> Tracks { get; private set; } = new List<AnimationTrack>();

        public Animation(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public Animation(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                uint version = br.ReadUInt32();

                if (magic == "r3d2canm")
                {
                    ReadNew(br);
                }
                else if (magic == "r3d2anmd")
                {
                    if (version == 5)
                    {
                        ReadV5(br);
                    }
                    else if (version == 4)
                    {
                        ReadV4(br);
                    }
                    else
                    {
                        ReadLegacy(br);
                    }
                }
                else
                {
                    throw new Exception("Invalid file signature: " + magic);
                }
            }
        }

        private void ReadNew(BinaryReader br)
        {

        }

        private void ReadV4(BinaryReader br)
        {
            uint resourceSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint version = br.ReadUInt32();
            uint flags = br.ReadUInt32();

            uint trackCount = br.ReadUInt32();
            uint frameCount = br.ReadUInt32();
            this.FrameDuration = br.ReadSingle();

            uint tracksOffset = br.ReadUInt32();
            uint assetNameOffset = br.ReadUInt32();
            uint timeOffset = br.ReadUInt32();
            uint vectorsOffset = br.ReadUInt32();
            uint rotationsOffset = br.ReadUInt32();
            uint framesOffset = br.ReadUInt32();


        }

        private void ReadV5(BinaryReader br)
        {
            uint resourceSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint version = br.ReadUInt32();
            uint flags = br.ReadUInt32();

            int trackCount = br.ReadInt32();
            int framesPerTrack = br.ReadInt32();
            this.FrameDuration = br.ReadSingle();

            int jointHashesOffset = br.ReadInt32();
            int assetNameOffset = br.ReadInt32();
            int timeOffset = br.ReadInt32();
            int vectorsOffset = br.ReadInt32();
            int rotationsOffset = br.ReadInt32();
            int framesOffset = br.ReadInt32();

            int jointHashesCount = (framesOffset - jointHashesOffset) / sizeof(uint);
            int vectorsCount = (rotationsOffset - vectorsOffset) / 12;
            int rotationsCount = (jointHashesOffset - rotationsOffset) / 6;

            List<uint> jointHashes = new List<uint>(jointHashesCount);
            List<Vector3> vectors = new List<Vector3>(vectorsCount);
            List<Quaternion> rotations = new List<Quaternion>(rotationsCount);
            var frames = new List<(ushort, ushort, ushort)>(framesPerTrack * trackCount);

            // Read Joint Hashes
            if (jointHashesOffset != 0)
            {
                br.BaseStream.Seek(jointHashesOffset + 12, SeekOrigin.Begin);

                for (int i = 0; i < jointHashesCount; i++)
                {
                    jointHashes.Add(br.ReadUInt32());
                }
            }
            else
            {
                throw new Exception("Animation does not contain joint data");
            }

            // Read Vectors
            if (vectorsOffset != 0)
            {
                br.BaseStream.Seek(vectorsOffset + 12, SeekOrigin.Begin);

                for (int i = 0; i < vectorsCount; i++)
                {
                    vectors.Add(br.ReadVector3());
                }
            }
            else
            {
                throw new Exception("Animation does not contain Translation/Scale data");
            }

            // Read Rotations
            if (rotationsOffset != 0)
            {
                br.BaseStream.Seek(rotationsOffset + 12, SeekOrigin.Begin);

                for (int i = 0; i < rotationsCount; i++)
                {
                    rotations.Add(new QuantizedQuaternion(br).Decompress().Normalized());
                }
            }
            else
            {
                throw new Exception("Animation does not contain Rotation data");
            }

            // Read Frames
            if (framesOffset != 0)
            {
                br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);

                for (int i = 0; i < framesPerTrack * trackCount; i++)
                {
                    frames.Add((br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
                }
            }
            else
            {
                throw new Exception("Animation does not contain frame data");
            }

            // Create tracks
            for(int i = 0; i < trackCount; i++)
            {
                this.Tracks.Add(new AnimationTrack(jointHashes[i]));
            }

            for(int t = 0; t < trackCount; t++)
            {
                AnimationTrack track = this.Tracks[t];
                for (int f = 0; f < framesPerTrack; f++)
                {
                    (int translationIndex, int scaleIndex, int rotationIndex) = frames[f * trackCount + t];

                    track.Translations.Add(vectors[translationIndex]);
                    track.Scales.Add(vectors[scaleIndex]);
                    track.Rotations.Add(rotations[rotationIndex]);
                }
            }
        }

        private void ReadLegacy(BinaryReader br)
        {

        }
    }
}
