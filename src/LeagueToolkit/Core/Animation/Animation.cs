using System.Numerics;

namespace LeagueToolkit.Core.Animation;

public static class Animation
{
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
}
