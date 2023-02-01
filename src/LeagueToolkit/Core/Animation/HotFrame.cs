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

    #region Parametrized Catmull Rom
    public Quaternion SampleRotationParametrized(ushort time)
    {
        var (amount, scaleIn, scaleOut) = CurveSampler.CreateCatmullRomKeyframeWeights(
            time,
            this.RotationP0.Time,
            this.RotationP1.Time,
            this.RotationP2.Time,
            this.RotationP3.Time
        );

        return Interpolators.Quaternion.InterpolateCatmull(
            amount,
            scaleIn,
            scaleOut,
            this.RotationP0.Value,
            this.RotationP1.Value,
            this.RotationP2.Value,
            this.RotationP3.Value
        );
    }

    public Vector3 SampleTranslationParametrized(ushort time)
    {
        var (amount, scaleIn, scaleOut) = CurveSampler.CreateCatmullRomKeyframeWeights(
            time,
            this.TranslationP0.Time,
            this.TranslationP1.Time,
            this.TranslationP2.Time,
            this.TranslationP3.Time
        );

        return Interpolators.Vector3.InterpolateCatmull(
            amount,
            scaleIn,
            scaleOut,
            this.TranslationP0.Value,
            this.TranslationP1.Value,
            this.TranslationP2.Value,
            this.TranslationP3.Value
        );
    }

    public Vector3 SampleScaleParametrized(ushort time)
    {
        var (amount, scaleIn, scaleOut) = CurveSampler.CreateCatmullRomKeyframeWeights(
            time,
            this.ScaleP0.Time,
            this.ScaleP1.Time,
            this.ScaleP2.Time,
            this.ScaleP3.Time
        );

        return Interpolators.Vector3.InterpolateCatmull(
            amount,
            scaleIn,
            scaleOut,
            this.ScaleP0.Value,
            this.ScaleP1.Value,
            this.ScaleP2.Value,
            this.ScaleP3.Value
        );
    }
    #endregion

    #region Uniform Catmull Rom
    public Quaternion SampleRotationUniform(ushort time)
    {
        float t_d = this.RotationP2.Time - this.RotationP1.Time;
        float amount = (time - this.RotationP1.Time) / t_d;

        return Interpolators.Quaternion.InterpolateCatmull(
            amount,
            0.5f,
            0.5f,
            this.RotationP0.Value,
            this.RotationP1.Value,
            this.RotationP2.Value,
            this.RotationP3.Value
        );
    }

    public Vector3 SampleTranslationUniform(ushort time)
    {
        float t_d = this.TranslationP2.Time - this.TranslationP1.Time;
        float amount = (time - this.TranslationP1.Time) / t_d;

        return Interpolators.Vector3.InterpolateCatmull(
            amount,
            0.5f,
            0.5f,
            this.TranslationP0.Value,
            this.TranslationP1.Value,
            this.TranslationP2.Value,
            this.TranslationP3.Value
        );
    }

    public Vector3 SampleScaleUniform(ushort time)
    {
        float t_d = this.ScaleP2.Time - this.ScaleP1.Time;
        float amount = (time - this.ScaleP1.Time) / t_d;

        return Interpolators.Vector3.InterpolateCatmull(
            amount,
            0.5f,
            0.5f,
            this.ScaleP0.Value,
            this.ScaleP1.Value,
            this.ScaleP2.Value,
            this.ScaleP3.Value
        );
    }
    #endregion
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
