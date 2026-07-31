using LeagueToolkit.Core.Animation;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Tests.Core.Animation;

public class CompressedAnimationAssetTests
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void Should_Evaluate_Missing_And_Constant_Frame_Channels(
        bool use32BitFrameKeys,
        bool useKeyframeParametrization
    )
    {
        using MemoryStream stream = CreateAnimation(use32BitFrameKeys, useKeyframeParametrization);
        using IAnimationAsset animation = AnimationAsset.Load(stream);
        Dictionary<uint, (Quaternion Rotation, Vector3 Translation, Vector3 Scale)> pose = [];

        animation.Evaluate(animation.Duration * 0.5f, pose);

        Assert.True(
            MathF.Abs(Quaternion.Dot(Quaternion.Identity, pose[1].Rotation)) > 0.99999f,
            $"Expected identity rotation, got {pose[1].Rotation}"
        );
        Assert.Equal(Vector3.Zero, pose[2].Translation);
        Assert.Equal(Vector3.One, pose[3].Scale);
        Assert.All(
            pose.Values,
            transform =>
            {
                Assert.True(IsFinite(transform.Rotation));
                Assert.True(IsFinite(transform.Translation));
                Assert.True(IsFinite(transform.Scale));
            }
        );
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Should_Initialize_Hot_Frames_On_First_Evaluation(bool use32BitFrameKeys)
    {
        using MemoryStream stream = CreateAnimation(use32BitFrameKeys, false);
        using IAnimationAsset animation = AnimationAsset.Load(stream);
        Dictionary<uint, (Quaternion Rotation, Vector3 Translation, Vector3 Scale)> pose = [];

        animation.Evaluate(0.001f, pose);

        Assert.Equal(Vector3.Zero, pose[1].Scale);
    }

    private static MemoryStream CreateAnimation(bool use32BitFrameKeys, bool useKeyframeParametrization)
    {
        const int headerSize = 128;
        const int jointCount = 3;
        const int jumpCacheCount = 1;

        int frameCount = use32BitFrameKeys ? 65_537 : 1;
        int frameDataSize = frameCount * 10;
        int jumpFrameSize = use32BitFrameKeys ? 48 : 24;
        int jumpCacheDataSize = jumpFrameSize * jointCount * jumpCacheCount;
        int framesOffset = headerSize;
        int jumpCachesOffset = framesOffset + frameDataSize;
        int jointHashesOffset = jumpCachesOffset + jumpCacheDataSize;

        MemoryStream stream = new(jointHashesOffset + jointCount * sizeof(uint));
        using (BinaryWriter writer = new(stream, Encoding.UTF8, true))
        {
            writer.Write("r3d2canm"u8);
            writer.Write(3u);
            writer.Write((uint)(stream.Capacity - 12));
            writer.Write(0u);
            writer.Write(useKeyframeParametrization ? 4u : 0u);
            writer.Write(jointCount);
            writer.Write(frameCount);
            writer.Write(jumpCacheCount);
            writer.Write(1f);
            writer.Write(30f);

            for (int i = 0; i < 6; i++)
                writer.Write(0f);
            for (int i = 0; i < 12; i++)
                writer.Write(0f);

            writer.Write(framesOffset - 12);
            writer.Write(jumpCachesOffset - 12);
            writer.Write(jointHashesOffset - 12);

            for (int i = 0; i < frameCount; i++)
            {
                writer.Write((ushort)0);
                writer.Write((ushort)0xC000);
                writer.Write(0u);
                writer.Write((ushort)0);
            }

            for (int jointId = 0; jointId < jointCount; jointId++)
                for (int channel = 0; channel < 3; channel++)
                    for (int key = 0; key < 4; key++)
                    {
                        bool isMissing = jointId == channel;
                        if (use32BitFrameKeys)
                            writer.Write(isMissing ? -1 : 0);
                        else
                            writer.Write(isMissing ? ushort.MaxValue : (ushort)0);
                    }

            writer.Write(1u);
            writer.Write(2u);
            writer.Write(3u);
        }

        stream.Position = 0;
        return stream;
    }

    private static bool IsFinite(Quaternion value) =>
        float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z) && float.IsFinite(value.W);

    private static bool IsFinite(Vector3 value) =>
        float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z);
}
