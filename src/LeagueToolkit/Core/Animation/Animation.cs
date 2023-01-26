﻿using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Animation
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
            using BinaryReader br = new(stream);

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

        private void ReadCompressed(BinaryReader br)
        {
            uint resourceSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint flags = br.ReadUInt32(); // 7 ?

            int jointCount = br.ReadInt32();
            int frameCount = br.ReadInt32();
            int jumpCacheCount = br.ReadInt32();

            float duration = br.ReadSingle();
            float fps = br.ReadSingle();
            this.FrameDuration = 1 / fps;

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

            if (framesOffset <= 0)
                throw new Exception("Animation does not contain Frame data");
            //if (jumpCachesOffset <= 0) throw new Exception("Animation does not contain jump cache data");
            if (jointNameHashesOffset <= 0)
                throw new Exception("Animation does not contain joint data");

            // Read joint hashes
            br.BaseStream.Seek(jointNameHashesOffset + 12, SeekOrigin.Begin);
            List<uint> jointHashes = new(jointCount);
            for (int i = 0; i < jointCount; i++)
            {
                jointHashes.Add(br.ReadUInt32());
            }

            // Read frames
            br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
            Dictionary<uint, Dictionary<float, Vector3>> translations = new(jointCount);
            Dictionary<uint, Dictionary<float, Vector3>> scales = new(jointCount);
            Dictionary<uint, Dictionary<float, Quaternion>> rotations = new(jointCount);

            for (int i = 0; i < jointCount; i++)
            {
                uint jointHash = jointHashes[i];

                translations.Add(jointHash, new Dictionary<float, Vector3>());
                scales.Add(jointHash, new Dictionary<float, Vector3>());
                rotations.Add(jointHash, new Dictionary<float, Quaternion>());

                this.Tracks.Add(new AnimationTrack(jointHash));
            }

            Span<byte> compressedTransform = stackalloc byte[6];
            for (int i = 0; i < frameCount; i++)
            {
                ushort compressedTime = br.ReadUInt16();

                ushort bits = br.ReadUInt16(); // JointHashIndex | TransformType
                byte jointHashIndex = (byte)(bits & 0x3FFF);
                CompressedTransformType transformType = (CompressedTransformType)(bits >> 14);

                br.Read(compressedTransform);

                uint jointHash = jointHashes[jointHashIndex];
                float uncompressedTime = DecompressFrameTime(compressedTime, duration);

                if (transformType == CompressedTransformType.Rotation)
                {
                    Quaternion rotation = QuantizedQuaternion.Decompress(compressedTransform);

                    if (!rotations[jointHash].ContainsKey(uncompressedTime))
                    {
                        rotations[jointHash].Add(uncompressedTime, rotation);
                    }
                }
                else if (transformType == CompressedTransformType.Translation)
                {
                    Vector3 translation = DecompressVector3(translationMin, translationMax, compressedTransform);

                    if (!translations[jointHash].ContainsKey(uncompressedTime))
                    {
                        translations[jointHash].Add(uncompressedTime, translation);
                    }
                }
                else if (transformType == CompressedTransformType.Scale)
                {
                    Vector3 scale = DecompressVector3(scaleMin, scaleMax, compressedTransform);

                    if (!scales[jointHash].ContainsKey(uncompressedTime))
                    {
                        scales[jointHash].Add(uncompressedTime, scale);
                    }
                }
                else
                {
                    throw new Exception("Encountered unknown transform type: " + transformType);
                }
            }

            // Build quantized tracks
            for (int i = 0; i < jointCount; i++)
            {
                uint jointHash = jointHashes[i];
                AnimationTrack track = this.Tracks.First(x => x.JointHash == jointHash);

                track.Translations = translations[jointHash];
                track.Scales = scales[jointHash];
                track.Rotations = rotations[jointHash];
            }

            // jumpCaches data size = 24|48 * jointCount * jumpCacheCount

            // Read jump caches
            JumpFrameFormat jumpFrameFormat = frameCount < 0x10001 ? JumpFrameFormat.U16 : JumpFrameFormat.U32;

            br.BaseStream.Seek(jumpCachesOffset + 12, SeekOrigin.Begin);
            for (int jumpCacheId = 0; jumpCacheId < jumpCacheCount; jumpCacheId++)
            {
                JumpFrame[] jumpFrames = new JumpFrame[jointCount];

                for (int jointId = 0; jointId < jointCount; jointId++)
                {
                    JumpFrame jumpFrame = new();

                    if (jumpFrameFormat == JumpFrameFormat.U16)
                    {
                        ReadJumpFramesU16(jumpFrame.RotationKeys, br);
                        ReadJumpFramesU16(jumpFrame.TranslationKeys, br);
                        ReadJumpFramesU16(jumpFrame.ScaleKeys, br);
                    }
                    else
                    {
                        ReadJumpFramesU32(jumpFrame.RotationKeys, br);
                        ReadJumpFramesU32(jumpFrame.TranslationKeys, br);
                        ReadJumpFramesU32(jumpFrame.ScaleKeys, br);
                    }

                    jumpFrames[jointId] = jumpFrame;
                }
            }

            DequantizeAnimationChannels(
                rotationQuantizationProperties,
                translationQuantizationProperties,
                scaleQuantizationProperties
            );
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

            if (vectorsOffset <= 0)
                throw new Exception("Animation does not contain Vector data");
            if (rotationsOffset <= 0)
                throw new Exception("Animation does not contain Rotation data");
            if (framesOffset <= 0)
                throw new Exception("Animation does not contain Frame data");

            int vectorsCount = (rotationsOffset - vectorsOffset) / 12;
            int rotationsCount = (framesOffset - rotationsOffset) / 16;

            br.BaseStream.Seek(vectorsOffset + 12, SeekOrigin.Begin);
            List<Vector3> vectors = new();
            for (int i = 0; i < vectorsCount; i++)
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
            for (int i = 0; i < frameCount * trackCount; i++)
            {
                frames.Add((br.ReadUInt32(), br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
                br.ReadUInt16(); // padding
            }

            foreach ((uint jointHash, ushort translationIndex, ushort scaleIndex, ushort rotationIndex) in frames)
            {
                if (!this.Tracks.Any(x => x.JointHash == jointHash))
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

            if (jointHashesOffset <= 0)
                throw new Exception("Animation does not contain Joint hashes");
            if (vectorsOffset <= 0)
                throw new Exception("Animation does not contain Vector data");
            if (rotationsOffset <= 0)
                throw new Exception("Animation does not contain Rotation data");
            if (framesOffset <= 0)
                throw new Exception("Animation does not contain Frame data");

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
            Span<byte> quantizedRotation = stackalloc byte[6];
            br.BaseStream.Seek(rotationsOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < rotationsCount; i++)
            {
                br.Read(quantizedRotation);
                rotations.Add(Quaternion.Normalize(QuantizedQuaternion.Decompress(quantizedRotation)));
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

            for (int t = 0; t < trackCount; t++)
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

            for (int i = 0; i < trackCount; i++)
            {
                string trackName = br.ReadPaddedString(32);
                uint flags = br.ReadUInt32();

                AnimationTrack track = new(Elf.HashLower(trackName));

                float frameTime = 0f;
                for (int j = 0; j < frameCount; j++)
                {
                    track.Rotations.Add(frameTime, br.ReadQuaternion());
                    track.Translations.Add(frameTime, br.ReadVector3());
                    track.Scales.Add(frameTime, new Vector3(1, 1, 1));

                    frameTime += this.FrameDuration;
                }

                this.Tracks.Add(track);
            }
        }

        private Vector3 DecompressVector3(Vector3 min, Vector3 max, ReadOnlySpan<byte> data)
        {
            Vector3 uncompressed = max - min;
            ushort cX = (ushort)(data[0] | data[1] << 8);
            ushort cY = (ushort)(data[2] | data[3] << 8);
            ushort cZ = (ushort)(data[4] | data[5] << 8);

            uncompressed.X *= cX / 65535.0f;
            uncompressed.Y *= cY / 65535.0f;
            uncompressed.Z *= cZ / 65535.0f;

            uncompressed += min;

            return uncompressed;
        }

        private float DecompressFrameTime(ushort compressedTime, float animationLength)
        {
            return compressedTime / 65535.0f * animationLength;
        }

        private static void ReadJumpFramesU16(Span<int> frames, BinaryReader br)
        {
            for (int j = 0; j < 4; j++)
                frames[j] = br.ReadUInt16();
        }

        private static void ReadJumpFramesU32(Span<int> frames, BinaryReader br)
        {
            for (int j = 0; j < 4; j++)
                frames[j] = br.ReadInt32();
        }

        // ------------ DEQUANTIZATION ------------ \\
        private void DequantizeAnimationChannels(
            TransformQuantizationProperties rotationQuantizationProperties,
            TransformQuantizationProperties translationQuantizationProperties,
            TransformQuantizationProperties scaleQuantizationProperties
        )
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
                    if (i + 1 >= track.Translations.Count)
                        return;

                    var frameA = track.Translations.ElementAt(i + 0);
                    var frameB = track.Translations.ElementAt(i + 1);

                    // Check if interpolation is needed

                    interpolatedFrames.Add(
                        InterpolateTranslationFrames((frameA.Key, frameA.Value), (frameB.Key, frameB.Value))
                    );
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

        public bool IsCompatibleWithSkeleton(RigResource skeleton)
        {
            foreach (AnimationTrack track in this.Tracks)
            {
                if (!skeleton.Joints.Any(x => Elf.HashLower(x.Name) == track.JointHash))
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

        private enum JumpFrameFormat
        {
            U16,
            U32
        }

        [DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
        private struct JumpFrame
        {
            public int[] RotationKeys;
            public int[] TranslationKeys;
            public int[] ScaleKeys;

            public JumpFrame()
            {
                this.RotationKeys = new int[4];
                this.TranslationKeys = new int[4];
                this.ScaleKeys = new int[4];
            }

            private string GetDebuggerDisplay()
            {
                return $"R:[{string.Join(',', this.RotationKeys)}] T:[{string.Join(',', this.TranslationKeys)}] S:[{string.Join(',', this.ScaleKeys)}]";
            }
        }
    }
}
