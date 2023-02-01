namespace LeagueToolkit.Core.Animation;

public static class CurveSampler
{
    public static (float m0, float m1, float m2, float m3) CreateCatmullRomWeights(
        float amount,
        float easeIn, /* tau20 */
        float easeOut /* tau31 */
    )
    {
        //float t_sq = amount * amount;
        //float t_cu = t_sq * amount;

        float m0 = (((2.0f - amount) * amount) - 1.0f) * (amount * easeIn);
        float m1 = ((((2.0f - easeOut) * amount) + (easeOut - 3.0f)) * (amount * amount)) + 1.0f;
        float m2 = ((((3.0f - easeIn * 2) + ((easeIn - 2.0f) * amount)) * amount) + easeIn) * amount;
        float m3 = ((amount - 1.0f) * amount) * (amount * easeOut);

        //float m0 = (-easeIn * amount) + (2.0f * amount * t_sq) - (easeIn * t_cu);
        //float m1 = 1.0f + (easeOut - 3.0f) * t_sq + (2.0f - easeOut) * t_cu;
        //float m2 = easeIn * amount + (3.0f - easeIn * easeIn) * t_sq + (easeIn - 2.0f) * t_cu;
        //float m3 = (-easeOut * t_sq) + (easeOut * t_cu);

        return (m0, m1, m2, m3);
    }

    public static (float amount, float scaleIn, float scaleOut) CreateCatmullRomKeyframeWeights(
        ushort time,
        float time0,
        float time1,
        float time2,
        float time3
    )
    {
        float t_d = time2 - time1;
        float amount = (time - time1) / t_d;
        float scaleIn = t_d / (time2 - time0);
        float scaleOut = t_d / (time3 - time1);

        return (amount, scaleIn, scaleOut);
    }
}
