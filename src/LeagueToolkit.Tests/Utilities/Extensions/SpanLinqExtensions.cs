using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Tests.Utilities.Extensions
{
    public class SpanLinqExtensions
    {
        [Theory()]
        [InlineData(new ushort[] { 1, 2, 3 }, 1)]
        [InlineData(new ushort[] { 953, 250, 830 }, 250)]
        public void Min_ushort_eq(ushort[] spanArray, ushort expected)
        {
            ReadOnlySpan<ushort> span = spanArray;
            Assert.Equal(expected, span.Min());
        }

        [Theory()]
        [InlineData(new ushort[] { 1, 2, 3 }, 3)]
        [InlineData(new ushort[] { 953, 250, 830 }, 953)]
        public void Max_ushort_eq(ushort[] spanArray, ushort expected)
        {
            ReadOnlySpan<ushort> span = spanArray;
            Assert.Equal(expected, span.Max());
        }
    }
}
