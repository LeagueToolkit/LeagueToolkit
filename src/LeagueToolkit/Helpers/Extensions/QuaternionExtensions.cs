using System.Collections.Generic;
using System.Numerics;

namespace LeagueToolkit.Helpers.Extensions
{
    internal static class QuaternionExtensions
    {
        public class QuaternionComparer : IComparer<Quaternion>
        {
            public static readonly QuaternionComparer Comparer = new();
            private QuaternionComparer() { }

            public int Compare(Quaternion @this, Quaternion other)
            {
                int xComparison = @this.X.CompareTo(other.X);
                if (xComparison != 0) return xComparison;
                int yComparison = @this.Y.CompareTo(other.Y);
                if (yComparison != 0) return yComparison;
                int zComparison = @this.Z.CompareTo(other.Z);
                return zComparison != 0 ? zComparison : @this.W.CompareTo(other.W);
            }
        }
    }
}
