using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation
{
    public static class Interpolators
    {
        public static readonly IInterpolator<Vector3> Vector3 = new Vector3Interpolator();
    }

    internal struct Vector3Interpolator : IInterpolator<Vector3>
    {
        public Vector3 InterpolateCatmull(
            float time,
            float tau20,
            float tau31,
            Vector3 p0,
            Vector3 p1,
            Vector3 p2,
            Vector3 p3
        ) => throw new NotImplementedException();
    }

    internal struct QuaternionInterpolator : IInterpolator<Quaternion>
    {
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
            float t_sq = time * time;
            float t_cu = t_sq * time;

            //float m0 = (((2.0f - time) * time) - 1.0f) * (time * tau20);
            //float m1 = ((((2.0f - tau31) * time) + (tau31 - 3.0f)) * (time * time)) + 1.0f;
            //float m2 = ((((3.0f - tau20 * 2) + ((tau20 - 2.0f) * time)) * time) + tau20) * time;
            //float m3 = ((time - 1.0f) * time) * (time * tau31);

            float m0 = (-tau20 * time) + (2.0f * time * t_sq) - (tau20 * t_cu);
            float m1 = 1.0f + (tau31 - 3.0f) * t_sq + (2.0f - tau31) * t_cu;
            float m2 = tau20 * time + (3.0f - tau20 * tau20) * t_sq + (tau20 - 2.0f) * t_cu;
            float m3 = (-tau31 * t_sq) + (tau31 * t_cu);

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
}
