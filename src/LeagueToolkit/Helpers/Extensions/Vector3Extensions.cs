using System.Collections.Generic;
using System.Numerics;

namespace LeagueToolkit.Helpers.Extensions
{
    internal static class Vector3Extensions
    {
        public class Vector3Comparer : IComparer<Vector3>
        {
            public static readonly Vector3Comparer Comparer = new();
            private Vector3Comparer() { }

            public int Compare(Vector3 @this, Vector3 other)
            {
                int xComparison = @this.X.CompareTo(other.X);
                if (xComparison != 0) return xComparison;
                int yComparison = @this.Y.CompareTo(other.Y);
                return yComparison != 0 ? yComparison : @this.Z.CompareTo(other.Z);
            }
        }
    }
}
