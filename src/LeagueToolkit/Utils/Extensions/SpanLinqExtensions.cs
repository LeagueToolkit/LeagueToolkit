namespace LeagueToolkit.Utils.Extensions;

public static class SpanLinqExtensions
{
    /// <summary>
    /// Returns the element with the lowest value in <paramref name="span"/>
    /// </summary>
    public static ushort Min(this ReadOnlySpan<ushort> span)
    {
        ushort min = ushort.MaxValue;
        for (int i = 0; i < span.Length; i++)
        {
            ushort value = span[i];
            if (value < min)
            {
                min = value;
            }
        }

        return min;
    }

    /// <summary>
    /// Returns the element with the highest value in <paramref name="span"/>
    /// </summary>
    public static ushort Max(this ReadOnlySpan<ushort> span)
    {
        ushort max = ushort.MinValue;
        for (int i = 0; i < span.Length; i++)
        {
            ushort value = span[i];
            if (value > max)
            {
                max = value;
            }
        }

        return max;
    }
}
