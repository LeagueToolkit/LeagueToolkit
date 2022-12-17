using System;

namespace LeagueToolkit.Helpers.Extensions
{
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
    }
}
