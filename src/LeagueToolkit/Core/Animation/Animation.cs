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

            TransformQuantizationProperties rotationQuantizationProperties = new(br);
            TransformQuantizationProperties translationQuantizationProperties = new(br);
            TransformQuantizationProperties scaleQuantizationProperties = new(br);

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

            Evaluate(0.4f);
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

        public void Evaluate(float time)
        {
            float clampedEvaluationTime = Math.Min(this.Duration, time);

            // Check if we need to reset hot frames
            // Jump cache is split by duration
            float evaluationDelta = clampedEvaluationTime - this._evaluator.LastEvaluationTime;
            if (
                this._evaluator.LastEvaluationTime < 0.0f
                || this._evaluator.LastEvaluationTime > clampedEvaluationTime
                || evaluationDelta > this.Duration / this._jumpCacheCount
            )
            {
                // Initialize starting hot frame
                InitializeHotFrameEvaluator(clampedEvaluationTime);
            }
        }

        private void InitializeHotFrameEvaluator(float evaluationTime)
        {
            // Get cache id
            int jumpCacheId = (int)(this._jumpCacheCount * (this._evaluator.LastEvaluationTime / this.Duration));

            // Reset cursor
            this._evaluator.Cursor = 0;

            if (this._frames.Length < 0x10001)
            {
                int jumpCacheSize = 24 * this._joints.Length;
                ReadOnlySpan<JumpFrameU16> jumpFrames = MemoryMarshal.Cast<byte, JumpFrameU16>(
                    this._jumpCaches.Slice(jumpCacheId * jumpCacheSize, jumpCacheSize).Span
                );

                for (int jointId = 0; jointId < this._joints.Length; jointId++)
                {
                    JumpFrameU16 jumpFrame = jumpFrames[jointId];

                    jumpFrame.FetchRotations(jointId, this._frames.Span, ref this._evaluator);
                    jumpFrame.FetchTranslations(
                        jointId,
                        this._frames.Span,
                        ref this._evaluator,
                        this._translationMin,
                        this._translationMax
                    );
                    jumpFrame.FetchScales(
                        jointId,
                        this._frames.Span,
                        ref this._evaluator,
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
        }

        private static Vector3 DecompressVector3(Vector3 min, Vector3 max, ReadOnlySpan<byte> data)
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

        private static Vector3 DecompressVector3(ReadOnlySpan<ushort> value, Vector3 min, Vector3 max)
        {
            Vector3 uncompressed = max - min;

            uncompressed.X *= value[0] / 65535.0f;
            uncompressed.Y *= value[1] / 65535.0f;
            uncompressed.Z *= value[2] / 65535.0f;

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

        private enum JumpFrameFormat
        {
            U16,
            U32
        }

        [DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
        private unsafe struct JumpFrameU16
        {
            public fixed ushort RotationKeys[4];
            public fixed ushort TranslationKeys[4];
            public fixed ushort ScaleKeys[4];

            public void FetchRotations(
                int jointId,
                ReadOnlySpan<CompressedFrame> frames,
                ref HotFrameEvaluator evaluator
            )
            {
                Span<QuaternionHotFrame> hotFrames = stackalloc QuaternionHotFrame[4];
                fixed (ushort* rotationKeys = this.RotationKeys)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        CompressedFrame frame = frames[rotationKeys[i]];

                        evaluator.Cursor = Math.Max(evaluator.Cursor, rotationKeys[i]);

                        ReadOnlySpan<ushort> value = new Span<ushort>(frame.Value, 3);
                        Quaternion rotation = QuantizedQuaternion.Decompress(value);

                        hotFrames[i] = new(frame.KeyTime, rotation);
                    }
                }

                // TODO: Re-order rotations by their dot product so they occur along the shortest path
                JointHotFrame jointHotFrame = evaluator.HotFrames[jointId];

                jointHotFrame.RotationP0 = hotFrames[0];
                jointHotFrame.RotationP1 = hotFrames[1];
                jointHotFrame.RotationP2 = hotFrames[2];
                jointHotFrame.RotationP3 = hotFrames[3];

                evaluator.HotFrames[jointId] = jointHotFrame;
            }

            public void FetchTranslations(
                int jointId,
                ReadOnlySpan<CompressedFrame> frames,
                ref HotFrameEvaluator evaluator,
                Vector3 min,
                Vector3 max
            )
            {
                Span<VectorHotFrame> hotFrames = stackalloc VectorHotFrame[4];
                fixed (ushort* translationKeys = this.TranslationKeys)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        CompressedFrame frame = frames[translationKeys[i]];

                        evaluator.Cursor = Math.Max(evaluator.Cursor, translationKeys[i]);

                        ReadOnlySpan<ushort> compressedValue = new Span<ushort>(frame.Value, 3);
                        hotFrames[i] = new(frame.KeyTime, DecompressVector3(compressedValue, min, max));
                    }
                }

                JointHotFrame jointHotFrame = evaluator.HotFrames[jointId];

                jointHotFrame.TranslationP0 = hotFrames[0];
                jointHotFrame.TranslationP1 = hotFrames[1];
                jointHotFrame.TranslationP2 = hotFrames[2];
                jointHotFrame.TranslationP3 = hotFrames[3];

                evaluator.HotFrames[jointId] = jointHotFrame;
            }

            public void FetchScales(
                int jointId,
                ReadOnlySpan<CompressedFrame> frames,
                ref HotFrameEvaluator evaluator,
                Vector3 min,
                Vector3 max
            )
            {
                Span<VectorHotFrame> hotFrames = stackalloc VectorHotFrame[4];
                fixed (ushort* scaleKeys = this.ScaleKeys)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        CompressedFrame frame = frames[scaleKeys[i]];

                        evaluator.Cursor = Math.Max(evaluator.Cursor, scaleKeys[i]);

                        ReadOnlySpan<ushort> compressedValue = new Span<ushort>(frame.Value, 3);
                        hotFrames[i] = new(frame.KeyTime, DecompressVector3(compressedValue, min, max));
                    }
                }

                JointHotFrame jointHotFrame = evaluator.HotFrames[jointId];

                jointHotFrame.ScaleP0 = hotFrames[0];
                jointHotFrame.ScaleP1 = hotFrames[1];
                jointHotFrame.ScaleP2 = hotFrames[2];
                jointHotFrame.ScaleP3 = hotFrames[3];

                evaluator.HotFrames[jointId] = jointHotFrame;
            }

            private string GetDebuggerDisplay()
            {
                fixed (ushort* rotationKeys = this.TranslationKeys)
                fixed (ushort* translationKeys = this.TranslationKeys)
                fixed (ushort* scaleKeys = this.ScaleKeys)
                {
                    return $"R:[{string.Join(',', rotationKeys[0], rotationKeys[1], rotationKeys[2], rotationKeys[3])}] "
                        + $"T:[{string.Join(',', translationKeys[0], translationKeys[1], translationKeys[2], translationKeys[3])}] "
                        + $"S:[{string.Join(',', scaleKeys[0], scaleKeys[1], scaleKeys[2], scaleKeys[3])}]";
                }
            }
        }

        [DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
        private unsafe struct JumpFrameU32
        {
            public fixed int RotationKeys[4];
            public fixed int TranslationKeys[4];
            public fixed int ScaleKeys[4];

            private string GetDebuggerDisplay()
            {
                fixed (int* rotationKeys = this.RotationKeys)
                fixed (int* translationKeys = this.TranslationKeys)
                fixed (int* scaleKeys = this.ScaleKeys)
                {
                    return $"R:[{string.Join(',', rotationKeys[0], rotationKeys[1], rotationKeys[2], rotationKeys[3])}] "
                        + $"T:[{string.Join(',', translationKeys[0], translationKeys[1], translationKeys[2], translationKeys[3])}] "
                        + $"S:[{string.Join(',', scaleKeys[0], scaleKeys[1], scaleKeys[2], scaleKeys[3])}]";
                }
            }
        }

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
        DoSomeWeirdInterpolationShit = 1 << 2,
    }

    [DebuggerDisplay("{GetJointId()} | {GetTransformType()}")]
    internal unsafe struct CompressedFrame
    {
        public readonly ushort KeyTime;
        public readonly ushort JointId;
        public fixed ushort Value[3];

        public ushort GetJointId() => (ushort)(this.JointId & 0x3FFF);

        public CompressedTransformType GetTransformType() => (CompressedTransformType)(this.JointId >> 14);
    }

    internal enum CompressedTransformType : byte
    {
        Rotation = 0,
        Translation = 1,
        Scale = 2
    }

    internal struct HotFrameEvaluator
    {
        public float LastEvaluationTime { get; set; }
        public int Cursor { get; set; }
        public JointHotFrame[] HotFrames { get; set; }

        public HotFrameEvaluator(int jointCount)
        {
            this.HotFrames = new JointHotFrame[jointCount];
        }
    }

    internal struct JointHotFrame
    {
        public QuaternionHotFrame RotationP0;
        public QuaternionHotFrame RotationP1;
        public QuaternionHotFrame RotationP2;
        public QuaternionHotFrame RotationP3;

        public VectorHotFrame TranslationP0;
        public VectorHotFrame TranslationP1;
        public VectorHotFrame TranslationP2;
        public VectorHotFrame TranslationP3;

        public VectorHotFrame ScaleP0;
        public VectorHotFrame ScaleP1;
        public VectorHotFrame ScaleP2;
        public VectorHotFrame ScaleP3;
    }

    [DebuggerDisplay("Time: {KeyTime} Value: {Value}")]
    internal struct QuaternionHotFrame
    {
        public ushort KeyTime;
        public Quaternion Value;

        public QuaternionHotFrame(ushort keyTime, Quaternion value)
        {
            this.KeyTime = keyTime;
            this.Value = value;
        }
    }

    [DebuggerDisplay("Time: {KeyTime} Value: {Value}")]
    internal struct VectorHotFrame
    {
        public ushort KeyTime;
        public Vector3 Value;

        public VectorHotFrame(ushort keyTime, Vector3 value)
        {
            this.KeyTime = keyTime;
            this.Value = value;
        }
    }
}
