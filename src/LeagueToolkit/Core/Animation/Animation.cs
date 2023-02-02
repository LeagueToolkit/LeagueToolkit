using System.Numerics;

namespace LeagueToolkit.Core.Animation;

public static class Animation
{
    private const float ONE_OVER_USHORT_MAX = 0.000015259022f;

    internal static Vector3 DecompressVector3(ReadOnlySpan<ushort> value, Vector3 min, Vector3 max)
    {
        Vector3 uncompressed = max - min;

        uncompressed.X *= value[0] * ONE_OVER_USHORT_MAX;
        uncompressed.Y *= value[1] * ONE_OVER_USHORT_MAX;
        uncompressed.Z *= value[2] * ONE_OVER_USHORT_MAX;

        uncompressed += min;

        return uncompressed;
    }

    internal static float DecompressTime(ushort compressedTime, float duration) =>
        compressedTime / ushort.MaxValue * duration;

    internal static ushort CompressTime(float time, float duration) => (ushort)(time / duration * ushort.MaxValue);
}
