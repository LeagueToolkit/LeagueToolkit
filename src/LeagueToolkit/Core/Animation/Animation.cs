using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace LeagueToolkit.Core.Animation
{
    public sealed class Animation : IDisposable
    {
        public float Duration { get; private set; }
        public float FrameDuration { get; private set; }
        public float FPS => 1 / this.FrameDuration;

        public List<AnimationTrack> Tracks { get; private set; } = new();

        private Vector3 _translationMin;
        private Vector3 _translationMax;

        private Vector3 _scaleMin;
        private Vector3 _scaleMax;

        private int _jumpCacheCount;

        private MemoryOwner<CompressedFrame> _frames;
        private MemoryOwner<uint> _joints;
        private MemoryOwner<byte> _jumpCaches;

        private HotFrameEvaluator _evaluator;

        public bool IsDisposed { get; private set; }

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
            this._jumpCacheCount = br.ReadInt32();

            this.Duration = br.ReadSingle();
            float fps = br.ReadSingle();
            this.FrameDuration = 1 / fps;

            TransformOptimizationSettings rotationOptimization = TransformOptimizationSettings.Read(br);
            TransformOptimizationSettings translationOptimization = TransformOptimizationSettings.Read(br);
            TransformOptimizationSettings scaleOptimization = TransformOptimizationSettings.Read(br);

            this._translationMin = br.ReadVector3();
            this._translationMax = br.ReadVector3();

            this._scaleMin = br.ReadVector3();
            this._scaleMax = br.ReadVector3();

            int framesOffset = br.ReadInt32();
            int jumpCachesOffset = br.ReadInt32(); // 5328
            int jointNameHashesOffset = br.ReadInt32();

            if (framesOffset <= 0)
                throw new InvalidOperationException("Animation does not contain frame data");
            if (jumpCachesOffset <= 0)
                throw new InvalidOperationException("Animation does not contain jump cache data");
            if (jointNameHashesOffset <= 0)
                throw new InvalidOperationException("Animation does not contain joint data");

            // Read joint hashes
            br.BaseStream.Seek(jointNameHashesOffset + 12, SeekOrigin.Begin);
            this._joints = MemoryOwner<uint>.Allocate(jointCount);
            Span<byte> jointsRawBuffer = MemoryMarshal.Cast<uint, byte>(this._joints.Span);
            br.Read(jointsRawBuffer);

            this._evaluator = new(this._joints.Length);

            // Read frames
            br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
            this._frames = MemoryOwner<CompressedFrame>.Allocate(frameCount);
            Span<byte> framesRawBuffer = MemoryMarshal.Cast<CompressedFrame, byte>(this._frames.Span);
            br.Read(framesRawBuffer);

            // Read jump caches
            int jumpFrameSize = frameCount < 0x10001 ? 24 : 48;
            br.BaseStream.Seek(jumpCachesOffset + 12, SeekOrigin.Begin);
            this._jumpCaches = MemoryOwner<byte>.Allocate(jumpFrameSize * jointCount * this._jumpCacheCount);
            br.Read(this._jumpCaches.Span);

            Evaluate(this.FrameDuration * 0);
            Evaluate(this.FrameDuration * 1);
            Evaluate(this.FrameDuration * 2);
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
                rotations.Add(Quaternion.Normalize(br.ReadQuaternion()));
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
                for (int f = 0; f < framesPerTrack; f++)
                {
                    (int translationIndex, int scaleIndex, int rotationIndex) = frames[f * trackCount + t];
                    float currentTime = this.FrameDuration * f;

                    track.Translations.Add(currentTime, vectors[translationIndex]);
                    track.Scales.Add(currentTime, vectors[scaleIndex]);
                    track.Rotations.Add(currentTime, rotations[rotationIndex]);
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

        public unsafe void Evaluate(float time)
        {
            time = Math.Min(this.Duration, time);

            ushort compressedEvaluationTime = CompressTime(time, this.Duration);

            // Check if we need to reset hot frames
            // Jump cache is split by duration
            float evaluationDelta = time - this._evaluator.LastEvaluationTime;
            if (
                this._evaluator.LastEvaluationTime < 0.0f
                || this._evaluator.LastEvaluationTime > time
                || evaluationDelta > this.Duration / this._jumpCacheCount
            )
            {
                // Initialize starting hot frame
                InitializeHotFrameEvaluator(time);
            }

            for (int i = this._evaluator.Cursor; i < this._frames.Length; i++)
            {
                CompressedFrame frame = this._frames.Span[i];
                ushort jointId = frame.GetJointId();
                CompressedTransformType transformType = frame.GetTransformType();

                // We need to update the curves only if the evaluation requires new data
                //
                if (transformType == CompressedTransformType.Rotation)
                {
                    if (compressedEvaluationTime < this._evaluator.HotFrames[jointId].RotationP2.Time)
                        break;

                    FetchRotationFrame(jointId, frame.Time, new Span<ushort>(frame.Value, 4));
                }
                else if (transformType == CompressedTransformType.Translation)
                {
                    if (compressedEvaluationTime < this._evaluator.HotFrames[jointId].TranslationP2.Time)
                        break;

                    FetchTranslationFrame(jointId, frame.Time, new Span<ushort>(frame.Value, 3));
                }
                else if (transformType == CompressedTransformType.Scale)
                {
                    if (compressedEvaluationTime < this._evaluator.HotFrames[jointId].ScaleP2.Time)
                        break;

                    FetchScaleFrame(jointId, frame.Time, new Span<ushort>(frame.Value, 3));
                }

                this._evaluator.Cursor++;
            }

            this._evaluator.LastEvaluationTime = time;
        }

        private void FetchRotationFrame(int jointId, ushort time, ReadOnlySpan<ushort> compressedValue)
        {
            JointHotFrame hotFrame = this._evaluator.HotFrames[jointId];
            Span<QuaternionHotFrame> frames =
                stackalloc QuaternionHotFrame[4] {
                    hotFrame.RotationP0,
                    hotFrame.RotationP1,
                    hotFrame.RotationP2,
                    hotFrame.RotationP3
                };

            frames[0].Time = time;
            frames[0].Value = QuantizedQuaternion.Decompress(compressedValue);

            //frames[0..1].Reverse();
            frames[1..4].Reverse();
            frames[0..4].Reverse();

            // Rotate along shortest path
            for (int i = 1; i < 3; i++)
                if (Quaternion.Dot(frames[i].Value, frames[0].Value) < 0.0f)
                    frames[i].Value *= -1;

            this._evaluator.HotFrames[jointId] = hotFrame with
            {
                RotationP0 = frames[0],
                RotationP1 = frames[1],
                RotationP2 = frames[2],
                RotationP3 = frames[3]
            };
        }

        private void FetchTranslationFrame(int jointId, ushort time, ReadOnlySpan<ushort> compressedValue)
        {
            JointHotFrame hotFrame = this._evaluator.HotFrames[jointId];
            Span<VectorHotFrame> frames =
                stackalloc VectorHotFrame[4] {
                    hotFrame.TranslationP0,
                    hotFrame.TranslationP1,
                    hotFrame.TranslationP2,
                    hotFrame.TranslationP3
                };

            frames[0].Time = time;
            frames[0].Value = DecompressVector3(compressedValue, this._translationMin, this._translationMax);

            //frames[0..1].Reverse();
            frames[1..4].Reverse();
            frames[0..4].Reverse();

            this._evaluator.HotFrames[jointId] = hotFrame with
            {
                TranslationP0 = frames[0],
                TranslationP1 = frames[1],
                TranslationP2 = frames[2],
                TranslationP3 = frames[3]
            };
        }

        private void FetchScaleFrame(int jointId, ushort time, ReadOnlySpan<ushort> compressedValue)
        {
            JointHotFrame hotFrame = this._evaluator.HotFrames[jointId];
            Span<VectorHotFrame> frames =
                stackalloc VectorHotFrame[4] { hotFrame.ScaleP0, hotFrame.ScaleP1, hotFrame.ScaleP2, hotFrame.ScaleP3 };

            frames[0].Time = time;
            frames[0].Value = DecompressVector3(compressedValue, this._scaleMin, this._scaleMax);

            //frames[0..1].Reverse();
            frames[1..4].Reverse();
            frames[0..4].Reverse();

            this._evaluator.HotFrames[jointId] = hotFrame with
            {
                ScaleP0 = frames[0],
                ScaleP1 = frames[1],
                ScaleP2 = frames[2],
                ScaleP3 = frames[3]
            };
        }

        private unsafe void InitializeHotFrameEvaluator(float evaluationTime)
        {
            // Get cache id
            int jumpCacheId = (int)(this._jumpCacheCount * (this._evaluator.LastEvaluationTime / this.Duration));

            // Reset cursor
            this._evaluator.Cursor = 0;

            Span<int> frameKeys = stackalloc int[4];
            if (this._frames.Length < 0x10001)
            {
                int jumpCacheSize = 24 * this._joints.Length;
                ReadOnlySpan<JumpFrameU16> jumpFrames = MemoryMarshal.Cast<byte, JumpFrameU16>(
                    this._jumpCaches.Slice(jumpCacheId * jumpCacheSize, jumpCacheSize).Span
                );

                for (int jointId = 0; jointId < this._joints.Length; jointId++)
                {
                    JumpFrameU16 jumpFrame = jumpFrames[jointId];

                    // Initialize rotations
                    for (int i = 0; i < 4; i++)
                        frameKeys[i] = jumpFrame.RotationKeys[i];
                    this._evaluator.InitializeRotationJointHotFrames(jointId, frameKeys, this._frames.Span);

                    // Initialize translations
                    for (int i = 0; i < 4; i++)
                        frameKeys[i] = jumpFrame.TranslationKeys[i];
                    this._evaluator.InitializeTranslationJointHotFrames(
                        jointId,
                        frameKeys,
                        this._frames.Span,
                        this._translationMin,
                        this._translationMax
                    );

                    // Initialize scales
                    for (int i = 0; i < 4; i++)
                        frameKeys[i] = jumpFrame.ScaleKeys[i];
                    this._evaluator.InitializeScaleJointHotFrames(
                        jointId,
                        frameKeys,
                        this._frames.Span,
                        this._scaleMin,
                        this._scaleMax
                    );
                }
            }
            else
            {
                int jumpCacheSize = 48 * this._joints.Length;
                ReadOnlySpan<JumpFrameU32> jumpFrames = MemoryMarshal.Cast<byte, JumpFrameU32>(
                    this._jumpCaches.Slice(jumpCacheId * jumpCacheSize, jumpCacheSize).Span
                );
            }

            this._evaluator.Cursor++;
        }

        internal static Vector3 DecompressVector3(ReadOnlySpan<ushort> value, Vector3 min, Vector3 max)
        {
            Vector3 uncompressed = max - min;

            uncompressed.X *= value[0] / 65535.0f;
            uncompressed.Y *= value[1] / 65535.0f;
            uncompressed.Z *= value[2] / 65535.0f;

            uncompressed += min;

            return uncompressed;
        }

        internal static float DecompressTime(ushort compressedTime, float duration) =>
            compressedTime / ushort.MaxValue * duration;

        internal static ushort CompressTime(float time, float duration) => (ushort)(time / duration * ushort.MaxValue);

        private void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing)
            {
                this._frames?.Dispose();
                this._joints?.Dispose();
                this._jumpCaches?.Dispose();
            }

            this.IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public enum CompressedAnimationFlags
    {
        Unk1 = 1 << 0,
        Unk2 = 1 << 1,

        UseContinuousHermite = 1 << 2,
    }

    internal enum CompressedTransformType : byte
    {
        Rotation = 0,
        Translation = 1,
        Scale = 2
    }
}
