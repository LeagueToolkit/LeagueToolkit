using LeagueToolkit.Helpers.Cryptography;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.IO.SkeletonFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.AnimationFile
{
    public class Animation
    {
        public float FrameDuration { get; private set; }
        public float FPS => 1 / this.FrameDuration;

        public List<AnimationTrack> Tracks { get; private set; } = new();

        private List<List<ushort>> _jumpCaches = new();

        public Animation(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public Animation(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                uint version = br.ReadUInt32();

                if (magic == "r3d2canm")
                {
                    ReadCompressed(br);
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
                    throw new InvalidFileSignatureException();
                }
            }
        }

        private void ReadCompressed(BinaryReader br)
        {
            uint resourceSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint flags = br.ReadUInt32(); // 7 ?

            int jointCount = br.ReadInt32();
            int frameCount = br.ReadInt32();
            int jumpCacheCount = br.ReadInt32();

            float duration = br.ReadSingle();
            this.FrameDuration = 1 / br.ReadSingle();

            TransformQuantizationProperties rotationQuantizationProperties = new(br);
            TransformQuantizationProperties translationQuantizationProperties = new(br);
            TransformQuantizationProperties scaleQuantizationProperties = new(br);

            Vector3 translationMin = br.ReadVector3();
            Vector3 translationMax = br.ReadVector3();

            Vector3 scaleMin = br.ReadVector3();
            Vector3 scaleMax = br.ReadVector3();

            int framesOffset = br.ReadInt32();
            int jumpCachesOffset = br.ReadInt32(); // 5328
            int jointNameHashesOffset = br.ReadInt32();

            if (framesOffset <= 0) throw new Exception("Animation does not contain Frame data");
            //if (jumpCachesOffset <= 0) throw new Exception("Animation does not contain jump cache data");
            if (jointNameHashesOffset <= 0) throw new Exception("Animation does not contain joint data");

            // Read joint hashes
            br.BaseStream.Seek(jointNameHashesOffset + 12, SeekOrigin.Begin);
            List<uint> jointHashes = new(jointCount);
            for(int i = 0; i < jointCount; i++)
            {
                jointHashes.Add(br.ReadUInt32());
            }

            // Read frames
            br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
            List<KeyValuePair<uint, Dictionary<float, Vector3>>> translations = new(jointCount);
            List<KeyValuePair<uint, Dictionary<float, Vector3>>> scales = new(jointCount);
            List<KeyValuePair<uint, Dictionary<float, Quaternion>>> rotations = new(jointCount);
            
            for(int i = 0; i < jointCount; i++)
            {
                uint jointHash = jointHashes[i];

                translations.Add(new KeyValuePair<uint, Dictionary<float, Vector3>>(jointHash, new Dictionary<float, Vector3>()));
                scales.Add(new KeyValuePair<uint, Dictionary<float, Vector3>>(jointHash, new Dictionary<float, Vector3>()));
                rotations.Add(new KeyValuePair<uint, Dictionary<float, Quaternion>>(jointHash, new Dictionary<float, Quaternion>()));

                this.Tracks.Add(new AnimationTrack(jointHash));
            }
            
            for(int i = 0; i < frameCount; i++)
            {
                ushort compressedTime = br.ReadUInt16();

                ushort bits = br.ReadUInt16(); // JointHashIndex | TransformType
                byte jointHashIndex = (byte)(bits & 0x3FFF);
                CompressedTransformType transformType = (CompressedTransformType)(bits >> 14);

                byte[] compressedTransform = br.ReadBytes(6);

                float uncompressedTime = UncompressFrameTime(compressedTime, duration);

                if (transformType == CompressedTransformType.Rotation)
                {
                    Quaternion rotation = new QuantizedQuaternion(compressedTransform).Decompress();

                    if(!rotations[jointHashIndex].Value.ContainsKey(uncompressedTime))
                    {
                        rotations[jointHashIndex].Value.Add(uncompressedTime, rotation);
                    }
                }
                else if(transformType == CompressedTransformType.Translation)
                {
                    Vector3 translation = UncompressVector3(translationMin, translationMax, compressedTransform);

                    if (!translations[jointHashIndex].Value.ContainsKey(uncompressedTime))
                    {
                        translations[jointHashIndex].Value.Add(uncompressedTime, translation);
                    }
                }
                else if(transformType == CompressedTransformType.Scale)
                {
                    Vector3 scale = UncompressVector3(scaleMin, scaleMax, compressedTransform);

                    if (!scales[jointHashIndex].Value.ContainsKey(uncompressedTime))
                    {
                        scales[jointHashIndex].Value.Add(uncompressedTime, scale);
                    }
                }
                else
                {
                    throw new Exception("Encountered unknown transform type: " + transformType);
                }
            }

            // Build quantized tracks
            for(int i = 0; i < jointCount; i++)
            {
                AnimationTrack track = this.Tracks[i];

                track.Translations = translations[i].Value;
                track.Scales = scales[i].Value;
                track.Rotations = rotations[i].Value;
            }

            // Read jump caches
            //br.BaseStream.Seek(jumpCachesOffset + 12, SeekOrigin.Begin);
            //for(int i = 0; i < jumpCacheCount; i++)
            //{
            //    int count = 1332;
            //
            //    this._jumpCaches.Add(new List<ushort>());
            //    for(int j = 0; j < count; j++)
            //    {
            //        this._jumpCaches[i].Add(br.ReadUInt16());
            //    }
            //}

            DequantizeAnimationChannels(rotationQuantizationProperties, translationQuantizationProperties, scaleQuantizationProperties);
        }

        private void ReadV4(BinaryReader br)
        {
            uint resourceSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint version = br.ReadUInt32();
            uint flags = br.ReadUInt32();

            int trackCount = br.ReadInt32();
            int frameCount = br.ReadInt32();
            this.FrameDuration = br.ReadSingle();

            int tracksOffset = br.ReadInt32();
            int assetNameOffset = br.ReadInt32();
            int timeOffset = br.ReadInt32();
            int vectorsOffset = br.ReadInt32();
            int rotationsOffset = br.ReadInt32();
            int framesOffset = br.ReadInt32();

            if (vectorsOffset <= 0) throw new Exception("Animation does not contain Vector data");
            if (rotationsOffset <= 0) throw new Exception("Animation does not contain Rotation data");
            if (framesOffset <= 0) throw new Exception("Animation does not contain Frame data");

            int vectorsCount = (rotationsOffset - vectorsOffset) / 12;
            int rotationsCount = (framesOffset - rotationsOffset) / 16;

            br.BaseStream.Seek(vectorsOffset + 12, SeekOrigin.Begin);
            List<Vector3> vectors = new();
            for(int i = 0; i < vectorsCount; i++)
            {
                vectors.Add(br.ReadVector3());
            }

            br.BaseStream.Seek(rotationsOffset + 12, SeekOrigin.Begin);
            List<Quaternion> rotations = new();
            for (int i = 0; i < rotationsCount; i++)
            {
                rotations.Add(br.ReadQuaternion());
            }

            br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
            List<(uint, ushort, ushort, ushort)> frames = new(frameCount * trackCount);
            for(int i = 0; i < frameCount * trackCount; i++)
            {
                frames.Add((br.ReadUInt32(), br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
                br.ReadUInt16(); // padding
            }

            foreach((uint jointHash, ushort translationIndex, ushort scaleIndex, ushort rotationIndex) in frames)
            {
                if(!this.Tracks.Any(x => x.JointHash == jointHash))
                {
                    this.Tracks.Add(new AnimationTrack(jointHash));
                }

                AnimationTrack track = this.Tracks.First(x => x.JointHash == jointHash);

                int trackFrameTranslationIndex = track.Translations.Count;
                int trackFrameScaleIndex = track.Scales.Count;
                int trackFrameRotationIndex = track.Rotations.Count;

                Vector3 translation = vectors[translationIndex];
                Vector3 scale = vectors[scaleIndex];
                Quaternion rotation = rotations[rotationIndex];

                track.Translations.Add(this.FrameDuration * trackFrameTranslationIndex, translation);
                track.Scales.Add(this.FrameDuration * trackFrameScaleIndex, scale);
                track.Rotations.Add(this.FrameDuration * trackFrameRotationIndex, rotation);
            }
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

            if (jointHashesOffset <= 0) throw new Exception("Animation does not contain Joint hashes");
            if (vectorsOffset <= 0) throw new Exception("Animation does not contain Vector data");
            if (rotationsOffset <= 0) throw new Exception("Animation does not contain Rotation data");
            if (framesOffset <= 0) throw new Exception("Animation does not contain Frame data");

            int jointHashesCount = (framesOffset - jointHashesOffset) / sizeof(uint);
            int vectorsCount = (rotationsOffset - vectorsOffset) / 12;
            int rotationsCount = (jointHashesOffset - rotationsOffset) / 6;

            List<uint> jointHashes = new(jointHashesCount);
            List<Vector3> vectors = new(vectorsCount);
            List<Quaternion> rotations = new(rotationsCount);
            var frames = new List<(ushort, ushort, ushort)>(framesPerTrack * trackCount);

            // Read Joint Hashes
            br.BaseStream.Seek(jointHashesOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < jointHashesCount; i++)
            {
                jointHashes.Add(br.ReadUInt32());
            }

            // Read Vectors
            br.BaseStream.Seek(vectorsOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < vectorsCount; i++)
            {
                vectors.Add(br.ReadVector3());
            }

            // Read Rotations
            br.BaseStream.Seek(rotationsOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < rotationsCount; i++)
            {
                Quaternion rotation = new QuantizedQuaternion(br.ReadBytes(6)).Decompress();

                rotations.Add(Quaternion.Normalize(rotation));
            }

            // Read Frames
            br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < framesPerTrack * trackCount; i++)
            {
                frames.Add((br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
            }

            // Create tracks
            for (int i = 0; i < trackCount; i++)
            {
                this.Tracks.Add(new AnimationTrack(jointHashes[i]));
            }

            for(int t = 0; t < trackCount; t++)
            {
                AnimationTrack track = this.Tracks[t];
                float currentTime = 0;
                for (int f = 0; f < framesPerTrack; f++)
                {
                    (int translationIndex, int scaleIndex, int rotationIndex) = frames[f * trackCount + t];

                    track.Translations.Add(currentTime, vectors[translationIndex]);
                    track.Scales.Add(currentTime, vectors[scaleIndex]);
                    track.Rotations.Add(currentTime, rotations[rotationIndex]);

                    currentTime += this.FrameDuration;
                }
            }
        }

        private void ReadLegacy(BinaryReader br)
        {
            uint skeletonId = br.ReadUInt32();
            
            int trackCount = br.ReadInt32();
            int frameCount = br.ReadInt32();
            
            this.FrameDuration = 1.0f / br.ReadInt32(); // FPS

            for(int i = 0; i < trackCount; i++)
            {
                string trackName = br.ReadPaddedString(32);
                uint flags = br.ReadUInt32();

                AnimationTrack track = new AnimationTrack(Cryptography.ElfHash(trackName));

                float frameTime = 0f;
                for(int j = 0; j < frameCount; j++)
                {
                    track.Rotations.Add(frameTime, br.ReadQuaternion());
                    track.Translations.Add(frameTime, br.ReadVector3());
                    track.Scales.Add(frameTime, new Vector3(1, 1, 1));

                    frameTime += this.FrameDuration;
                }

                this.Tracks.Add(track);
            }
        }

        private Vector3 UncompressVector3(Vector3 min, Vector3 max, byte[] compressedData)
        {
            Vector3 uncompressed = max - min;
            ushort cX = (ushort)(compressedData[0] | compressedData[1] << 8);
            ushort cY = (ushort)(compressedData[2] | compressedData[3] << 8);
            ushort cZ = (ushort)(compressedData[4] | compressedData[5] << 8);

            uncompressed.X *= (cX / 65535.0f);
            uncompressed.Y *= (cY / 65535.0f);
            uncompressed.Z *= (cZ / 65535.0f);

            uncompressed += min;

            return uncompressed;
        }
        private float UncompressFrameTime(ushort compressedTime, float animationLength)
        {
            return (compressedTime / 65535.0f) * animationLength;
        }

        // ------------ DEQUANTIZATION ------------ \\
        private void DequantizeAnimationChannels(
            TransformQuantizationProperties rotationQuantizationProperties,
            TransformQuantizationProperties translationQuantizationProperties,
            TransformQuantizationProperties scaleQuantizationProperties)
        {
            // TODO

            //TranslationDequantizationRound(translationQuantizationProperties);
        }
        private void TranslationDequantizationRound(TransformQuantizationProperties quantizationProperties)
        {
            foreach (AnimationTrack track in this.Tracks)
            {
                List<(float, Vector3)> interpolatedFrames = new();
                for (int i = 0; i < track.Translations.Count; i++)
                {
                    // Do not process last frame
                    if (i + 1 >= track.Translations.Count) return;

                    var frameA = track.Translations.ElementAt(i + 0);
                    var frameB = track.Translations.ElementAt(i + 1);

                    // Check if interpolation is needed

                    interpolatedFrames.Add(InterpolateTranslationFrames((frameA.Key, frameA.Value), (frameB.Key, frameB.Value)));
                }

                foreach ((float time, Vector3 value) in interpolatedFrames)
                {
                    track.Translations.Add(time, value);
                }
            }
        }

        private (float, Vector3) InterpolateTranslationFrames((float, Vector3) a, (float, Vector3) b)
        {
            float time = (a.Item1 + b.Item1) / 2;
            return (time, Vector3.Lerp(a.Item2, b.Item2, 0.5f));
        }

        public bool IsCompatibleWithSkeleton(Skeleton skeleton)
        {
            foreach(AnimationTrack track in this.Tracks)
            {
                if(!skeleton.Joints.Any(x => Cryptography.ElfHash(x.Name) == track.JointHash))
                {
                    return false;
                }
            }

            return true;
        }

        private struct TransformQuantizationProperties
        {
            internal float ErrorMargin { get; private set; }
            internal float DiscontinuityThreshold { get; private set; }

            internal TransformQuantizationProperties(BinaryReader br)
            {
                this.ErrorMargin = br.ReadSingle();
                this.DiscontinuityThreshold = br.ReadSingle();
            }
        }
        private enum CompressedTransformType : byte
        {
            Rotation = 0,
            Translation = 1,
            Scale = 2
        }
    }
}
