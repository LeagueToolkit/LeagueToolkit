using System.Numerics;

namespace LeagueToolkit.Core.Animation;

public static class Interpolators
{
    public const float SLERP_EPSILON = 0.000001f;

    public static readonly IInterpolator<Vector3> Vector3 = new Vector3Interpolator();
    public static readonly IInterpolator<Quaternion> Quaternion = new QuaternionInterpolator();
}

public interface IInterpolator<T>
{
    public T InterpolateLinear(T p0, T p1, float amount);
    public T InterpolateCatmull(float time, float tau20, float tau31, T p0, T p1, T p2, T p3);
}

internal struct Vector3Interpolator : IInterpolator<Vector3>
{
    public Vector3 InterpolateLinear(Vector3 p0, Vector3 p1, float amount) => Vector3.Lerp(p0, p1, amount);

    public Vector3 InterpolateCatmull(
        float time,
        float tau20,
        float tau31,
        Vector3 p0,
        Vector3 p1,
        Vector3 p2,
        Vector3 p3
    )
    {
        var (m0, m1, m2, m3) = CurveSampler.CreateCatmullRomWeights(time, tau20, tau31);

        return new()
        {
            X = (m1 * p1.X) + (m0 * p0.X) + (m3 * p3.X) + (m2 * p2.X),
            Y = (m1 * p1.Y) + (m0 * p0.Y) + (m3 * p3.Y) + (m2 * p2.Y),
            Z = (m1 * p1.Z) + (m0 * p0.Z) + (m3 * p3.Z) + (m2 * p2.Z)
        };
    }
}

internal struct QuaternionInterpolator : IInterpolator<Quaternion>
{
    public Quaternion InterpolateLinear(Quaternion p0, Quaternion p1, float amount) => Quaternion.Lerp(p0, p1, amount);

    public Quaternion InterpolateCatmull(
        float time,
        float tau20,
        float tau31,
        Quaternion p0,
        Quaternion p1,
        Quaternion p2,
        Quaternion p3
    )
    {
        var (m0, m1, m2, m3) = CurveSampler.CreateCatmullRomWeights(time, tau20, tau31);

        return Quaternion.Normalize(
            new()
            {
                X = (m1 * p1.X) + (m0 * p0.X) + (m3 * p3.X) + (m2 * p2.X),
                Y = (m1 * p1.Y) + (m0 * p0.Y) + (m3 * p3.Y) + (m2 * p2.Y),
                Z = (m1 * p1.Z) + (m0 * p0.Z) + (m3 * p3.Z) + (m2 * p2.Z),
                W = (m1 * p1.W) + (m0 * p0.W) + (m3 * p3.W) + (m2 * p2.W)
            }
        );
    }
}
