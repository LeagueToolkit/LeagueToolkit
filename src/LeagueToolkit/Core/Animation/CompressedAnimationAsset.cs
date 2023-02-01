using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace LeagueToolkit.Core.Animation;

public sealed class CompressedAnimationAsset : IAnimationAsset
{
    public CompressedAnimationFlags Flags { get; private set; }

    public float Duration { get; private set; }
    public float Fps { get; private set; }

    private Vector3 _translationMin;
    private Vector3 _translationMax;

    private Vector3 _scaleMin;
    private Vector3 _scaleMax;

    private int _jumpCacheCount;

    private MemoryOwner<CompressedFrame> _frames;
    private MemoryOwner<byte> _jumpCaches;
    private MemoryOwner<uint> _joints;

    private HotFrameEvaluator _evaluator;

    /// <inheritdoc/>
    public bool IsDisposed { get; private set; }

    public CompressedAnimationAsset(string fileLocation) : this(File.OpenRead(fileLocation)) { }

    public CompressedAnimationAsset(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, false);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
        uint version = br.ReadUInt32();

        if (magic is "r3d2anmd")
            ThrowHelper.ThrowInvalidOperationException("Cannot read an uncompressed animation asset as compressed");
        else if (magic is not "r3d2canm")
            ThrowHelper.ThrowInvalidOperationException($"Invalid file signature: {magic}");

        if (version is not (1 or 2 or 3))
            ThrowHelper.ThrowInvalidOperationException($"Invalid version: {version}");

        ReadCompressed(br);
    }

    private void ReadCompressed(BinaryReader br)
    {
        uint resourceSize = br.ReadUInt32();
        uint formatToken = br.ReadUInt32();
        this.Flags = (CompressedAnimationFlags)br.ReadUInt32();

        int jointCount = br.ReadInt32();
        int frameCount = br.ReadInt32();
        this._jumpCacheCount = br.ReadInt32();

        this.Duration = br.ReadSingle();
        this.Fps = br.ReadSingle();

        TransformOptimizationSettings rotationOptimization = TransformOptimizationSettings.Read(br);
        TransformOptimizationSettings translationOptimization = TransformOptimizationSettings.Read(br);
        TransformOptimizationSettings scaleOptimization = TransformOptimizationSettings.Read(br);

        this._translationMin = br.ReadVector3();
        this._translationMax = br.ReadVector3();

        this._scaleMin = br.ReadVector3();
        this._scaleMax = br.ReadVector3();

        int framesOffset = br.ReadInt32();
        int jumpCachesOffset = br.ReadInt32();
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
    }

    public void Evaluate(float time, IDictionary<uint, (Quaternion Rotation, Vector3 Translation, Vector3 Scale)> pose)
    {
        ThrowIfDisposed();

        Evaluate(time);

        ushort compressedTime = Animation.CompressTime(time, this.Duration);

        // If this flag is set, league uses scaled tangents
        if (this.Flags.HasFlag(CompressedAnimationFlags.UseCurveParametrization))
        {
            for (int jointId = 0; jointId < this._joints.Length; jointId++)
            {
                uint jointHash = this._joints.Span[jointId];
                JointHotFrame hotFrame = this._evaluator.HotFrames[jointId];

                Quaternion rotation = hotFrame.SampleRotationParametrized(compressedTime);
                Vector3 translation = hotFrame.SampleTranslationParametrized(compressedTime);
                Vector3 scale = hotFrame.SampleScaleParametrized(compressedTime);

                if (!pose.TryAdd(jointHash, (rotation, translation, scale)))
                    pose[jointHash] = (rotation, translation, scale);
            }
        }
        else
        {
            for (int jointId = 0; jointId < this._joints.Length; jointId++)
            {
                uint jointHash = this._joints.Span[jointId];
                JointHotFrame hotFrame = this._evaluator.HotFrames[jointId];

                Quaternion rotation = hotFrame.SampleRotationUniform(compressedTime);
                Vector3 translation = hotFrame.SampleTranslationUniform(compressedTime);
                Vector3 scale = hotFrame.SampleScaleUniform(compressedTime);

                if (!pose.TryAdd(jointHash, (rotation, translation, scale)))
                    pose[jointHash] = (rotation, translation, scale);
            }
        }
    }

    private unsafe void Evaluate(float time)
    {
        time = Math.Clamp(time, 0.0f, this.Duration);

        ushort compressedEvaluationTime = Animation.CompressTime(time, this.Duration);

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

        while (this._evaluator.Cursor < this._frames.Length)
        {
            CompressedFrame frame = this._frames.Span[this._evaluator.Cursor];
            ushort jointId = frame.GetJointId();
            CompressedTransformType transformType = frame.GetTransformType();

            // We need to update the curves only if the evaluation requires new data
            if (transformType == CompressedTransformType.Rotation)
            {
                if (compressedEvaluationTime < this._evaluator.HotFrames[jointId].RotationP2.Time)
                    break;

                FetchRotationFrame(jointId, frame.Time, new Span<ushort>(frame.Value, 3));
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
        frames[0].Value = Animation.DecompressVector3(compressedValue, this._translationMin, this._translationMax);

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
        frames[0].Value = Animation.DecompressVector3(compressedValue, this._scaleMin, this._scaleMax);

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
        int jumpCacheId = (int)(this._jumpCacheCount * (evaluationTime / this.Duration));
        jumpCacheId = Math.Max(jumpCacheId, this._jumpCacheCount - 1);

        // Reset cursor
        this._evaluator.Cursor = 0;

        Span<int> rotationFrameKeys = stackalloc int[4];
        Span<int> translationFrameKeys = stackalloc int[4];
        Span<int> scaleFrameKeys = stackalloc int[4];

        if (this._frames.Length < 0x10001)
        {
            // Frame IDs are 16-bit
            int jumpCacheSize = 24 * this._joints.Length;
            ReadOnlySpan<JumpFrameU16> jumpFrames = MemoryMarshal.Cast<byte, JumpFrameU16>(
                this._jumpCaches.Span.Slice(jumpCacheId * jumpCacheSize, jumpCacheSize)
            );

            for (int jointId = 0; jointId < this._joints.Length; jointId++)
            {
                JumpFrameU16 jumpFrame = jumpFrames[jointId];

                // Initialize rotations
                for (int i = 0; i < 4; i++)
                    rotationFrameKeys[i] = jumpFrame.RotationKeys[i];

                // Initialize translations
                for (int i = 0; i < 4; i++)
                    translationFrameKeys[i] = jumpFrame.TranslationKeys[i];

                // Initialize scales
                for (int i = 0; i < 4; i++)
                    scaleFrameKeys[i] = jumpFrame.ScaleKeys[i];

                InitializeJointHotFrame(jointId, rotationFrameKeys, translationFrameKeys, scaleFrameKeys);
            }
        }
        else
        {
            // Frame IDs are 32-bit
            int jumpCacheSize = 48 * this._joints.Length;
            ReadOnlySpan<JumpFrameU32> jumpFrames = MemoryMarshal.Cast<byte, JumpFrameU32>(
                this._jumpCaches.Span.Slice(jumpCacheId * jumpCacheSize, jumpCacheSize)
            );

            for (int jointId = 0; jointId < this._joints.Length; jointId++)
            {
                JumpFrameU32 jumpFrame = jumpFrames[jointId];

                // Initialize rotations
                new Span<int>(jumpFrame.RotationKeys, 4).CopyTo(rotationFrameKeys);

                // Initialize translations
                new Span<int>(jumpFrame.TranslationKeys, 4).CopyTo(translationFrameKeys);

                // Initialize scales
                new Span<int>(jumpFrame.ScaleKeys, 4).CopyTo(scaleFrameKeys);

                InitializeJointHotFrame(jointId, rotationFrameKeys, translationFrameKeys, scaleFrameKeys);
            }
        }

        this._evaluator.Cursor++;
    }

    private void InitializeJointHotFrame(
        int jointId,
        ReadOnlySpan<int> rotationFrameKeys,
        ReadOnlySpan<int> translationFrameKeys,
        ReadOnlySpan<int> scaleFrameKeys
    )
    {
        this._evaluator.InitializeRotationJointHotFrames(jointId, rotationFrameKeys, this._frames.Span);

        this._evaluator.InitializeTranslationJointHotFrames(
            jointId,
            translationFrameKeys,
            this._frames.Span,
            this._translationMin,
            this._translationMax
        );

        this._evaluator.InitializeScaleJointHotFrames(
            jointId,
            scaleFrameKeys,
            this._frames.Span,
            this._scaleMin,
            this._scaleMax
        );
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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

    private void ThrowIfDisposed() =>
        ThrowHelper.ThrowObjectDisposedException(
            nameof(CompressedAnimationAsset),
            "Cannot use a disposed animation asset"
        );
}

[Flags]
public enum CompressedAnimationFlags : uint
{
    Unk1 = 1 << 0,
    Unk2 = 1 << 1,

    UseCurveParametrization = 1 << 2,
}
