using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation;

public static class CurveSampler
{
    public static (float m0, float m1, float m2, float m3) CreateCatmullRomWeights(
        float amount,
        float tau20,
        float tau31
    )
    {
        float t_sq = amount * amount;
        float t_cu = t_sq * amount;

        //float m0 = (((2.0f - time) * time) - 1.0f) * (time * tau20);
        //float m1 = ((((2.0f - tau31) * time) + (tau31 - 3.0f)) * (time * time)) + 1.0f;
        //float m2 = ((((3.0f - tau20 * 2) + ((tau20 - 2.0f) * time)) * time) + tau20) * time;
        //float m3 = ((time - 1.0f) * time) * (time * tau31);

        float m0 = (-tau20 * amount) + (2.0f * amount * t_sq) - (tau20 * t_cu);
        float m1 = 1.0f + (tau31 - 3.0f) * t_sq + (2.0f - tau31) * t_cu;
        float m2 = tau20 * amount + (3.0f - tau20 * tau20) * t_sq + (tau20 - 2.0f) * t_cu;
        float m3 = (-tau31 * t_sq) + (tau31 * t_cu);

        return (m0, m1, m2, m3);
    }
}
