using LeagueToolkit.Core.Primitives;
using System;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Animation;

internal struct HotFrameEvaluator
{
    public float LastEvaluationTime { get; set; }
    public int Cursor { get; set; }
    public JointHotFrame[] HotFrames { get; set; }

    public HotFrameEvaluator(int jointCount)
    {
        this.HotFrames = new JointHotFrame[jointCount];
    }

    public unsafe void InitializeRotationJointHotFrames(
        int jointId,
        ReadOnlySpan<int> frameKeys,
        ReadOnlySpan<CompressedFrame> frames
    )
    {
        Span<QuaternionHotFrame> hotFrames = stackalloc QuaternionHotFrame[4];
        for (int i = 0; i < 4; i++)
        {
            CompressedFrame frame = frames[frameKeys[i]];

            this.Cursor = Math.Max(this.Cursor, frameKeys[i]);

            ReadOnlySpan<ushort> value = new Span<ushort>(frame.Value, 3);
            Quaternion rotation = QuantizedQuaternion.Decompress(value);

            hotFrames[i] = new(frame.Time, rotation);
        }

        // Rotate along shortest path
        for (int i = 1; i < 3; i++)
            if (Quaternion.Dot(hotFrames[i].Value, hotFrames[0].Value) < 0.0f)
                hotFrames[i].Value *= -1;

        this.HotFrames[jointId] = this.HotFrames[jointId] with
        {
            RotationP0 = hotFrames[0],
            RotationP1 = hotFrames[1],
            RotationP2 = hotFrames[2],
            RotationP3 = hotFrames[3]
        };
    }

    public unsafe void InitializeTranslationJointHotFrames(
        int jointId,
        ReadOnlySpan<int> frameKeys,
        ReadOnlySpan<CompressedFrame> frames,
        Vector3 min,
        Vector3 max
    )
    {
        Span<VectorHotFrame> hotFrames = stackalloc VectorHotFrame[4];
        for (int i = 0; i < 4; i++)
        {
            CompressedFrame frame = frames[frameKeys[i]];

            this.Cursor = Math.Max(this.Cursor, frameKeys[i]);

            ReadOnlySpan<ushort> compressedValue = new Span<ushort>(frame.Value, 3);
            hotFrames[i] = new(frame.Time, Animation.DecompressVector3(compressedValue, min, max));
        }

        this.HotFrames[jointId] = this.HotFrames[jointId] with
        {
            TranslationP0 = hotFrames[0],
            TranslationP1 = hotFrames[1],
            TranslationP2 = hotFrames[2],
            TranslationP3 = hotFrames[3]
        };
    }

    public unsafe void InitializeScaleJointHotFrames(
        int jointId,
        ReadOnlySpan<int> frameKeys,
        ReadOnlySpan<CompressedFrame> frames,
        Vector3 min,
        Vector3 max
    )
    {
        Span<VectorHotFrame> hotFrames = stackalloc VectorHotFrame[4];
        for (int i = 0; i < 4; i++)
        {
            CompressedFrame frame = frames[frameKeys[i]];

            this.Cursor = Math.Max(this.Cursor, frameKeys[i]);

            ReadOnlySpan<ushort> compressedValue = new Span<ushort>(frame.Value, 3);
            hotFrames[i] = new(frame.Time, Animation.DecompressVector3(compressedValue, min, max));
        }

        this.HotFrames[jointId] = this.HotFrames[jointId] with
        {
            ScaleP0 = hotFrames[0],
            ScaleP1 = hotFrames[1],
            ScaleP2 = hotFrames[2],
            ScaleP3 = hotFrames[3]
        };
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

[DebuggerDisplay("{GetDebuggerDisplay()}")]
internal struct QuaternionHotFrame
{
    public ushort Time;
    public Quaternion Value;

    public QuaternionHotFrame(ushort keyTime, Quaternion value)
    {
        this.Time = keyTime;
        this.Value = value;
    }

    private string GetDebuggerDisplay() => string.Format("T: {0, -5} {1}", this.Time, this.Value);
}

[DebuggerDisplay("{GetDebuggerDisplay()}")]
internal struct VectorHotFrame
{
    public ushort Time;
    public Vector3 Value;

    public VectorHotFrame(ushort keyTime, Vector3 value)
    {
        this.Time = keyTime;
        this.Value = value;
    }

    private string GetDebuggerDisplay() => string.Format("T: {0, -5} {1}", this.Time, this.Value);
}
