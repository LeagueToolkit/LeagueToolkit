using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.Helpers.Structures
{
    /// <summary>
    /// Represents a Sphere
    /// </summary>
    public class R3DSphere
    {
        public static readonly R3DSphere Infinite = new R3DSphere(Vector3.Zero, float.MaxValue);

        public Vector3 Position;
        public float Radius;

        /// <summary>
        /// Initializes a new <see cref="R3DSphere"/> instance
        /// </summary>
        /// <param name="position">Position of the sphere</param>
        /// <param name="radius">Radius of the sphere</param>
        public R3DSphere(Vector3 position, float radius)
        {
            this.Position = position;
            this.Radius = radius;
        }

        /// <summary>
        /// Initializes a new <see cref="R3DSphere"/> instance from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public R3DSphere(BinaryReader br)
        {
            this.Position = br.ReadVector3();
            this.Radius = br.ReadSingle();
        }

        /// <summary>
        /// Creates a clone of a <see cref="R3DSphere"/> object
        /// </summary>
        /// <param name="r3dSphere">The <see cref="R3DSphere"/> to clone</param>
        public R3DSphere(R3DSphere r3dSphere)
        {
            this.Position = r3dSphere.Position;
            this.Radius = r3dSphere.Radius;
        }

        /// <summary>
        /// Writes this <see cref="R3DSphere"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.WriteVector3(this.Position);
            bw.Write(this.Radius);
        }

        public bool Contains(Vector3 point)
        {
            const double tolerance = 0.001; // need to give a bit of leniency here
            return Math.Pow(Position.X - point.X, 2) + Math.Pow(Position.Y - point.Y, 2) + Math.Pow(Position.Z - point.Z, 2) <=
                   Math.Pow(Radius, 2) * (1 + tolerance) + tolerance;
        }

        public static R3DSphere CalculateBoundingSphere(IEnumerable<Vector3> points)
        {
            Random random = new();
            Vector3[] shuffledPoints = points.OrderBy(_ => random.Next()).ToArray();

            return Welzl(shuffledPoints);
        }

        // see https://en.wikipedia.org/wiki/Smallest-circle_problem#Welzl's_algorithm
        // non-recursive implementation taken from https://gist.github.com/Darkyenus/c0b31a79e6115508822ce2128ab42cbf
        private static R3DSphere Welzl(IReadOnlyList<Vector3> points)
        {
            R3DSphere currentBoundingSphere = null!;
            Vector3[] boundaryPoints = new Vector3[4];

            int currentPointCount = 0;
            int boundaryPointCount = 0;

            // true at index x if a virtual recursion was started from point count x
            bool[] virtualRecursionStart = new bool[points.Count];
            bool descending = true;
            do
            {
                if (descending)
                {
                    currentBoundingSphere = BoundingSphereFromPoints(boundaryPoints[..boundaryPointCount]);
                    if (boundaryPointCount == 4)
                        currentPointCount++;
                    else
                        currentPointCount = 1;

                    descending = false;
                }
                else if (virtualRecursionStart[currentPointCount])
                {
                    virtualRecursionStart[currentPointCount] = false;
                    boundaryPointCount--;
                    currentPointCount++;
                }
                else
                {
                    Vector3 lastPoint = points[currentPointCount-1];
                    if (currentBoundingSphere.Contains(lastPoint))
                    {
                        currentPointCount++;
                    }
                    else
                    {
                        boundaryPoints[boundaryPointCount++] = lastPoint;
                        virtualRecursionStart[currentPointCount] = true;
                        currentPointCount--;
                        descending = true;
                    }
                }
            } while (currentPointCount < points.Count);

            return currentBoundingSphere;
        }

        private static R3DSphere BoundingSphereFromPoints(IReadOnlyList<Vector3> boundaryPoints)
        {
            switch (boundaryPoints.Count)
            {
                case 0:
                    return new R3DSphere(Vector3.Zero, 0);
                case 1:
                    return new R3DSphere(boundaryPoints[0], 0);
                case 2:
                {
                    Vector3 centralPoint = (boundaryPoints[0] + boundaryPoints[1]) / 2;
                    return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, boundaryPoints[0]));
                }
                case 3:
                {
                    Vector3 originA = boundaryPoints[0] - boundaryPoints.Last();
                    Vector3 originB = boundaryPoints[1] - boundaryPoints.Last();
                    // logic from wikipedia https://en.wikipedia.org/wiki/Circumscribed_circle#Higher_dimensions
                    Vector3 centralPoint = boundaryPoints.Last() +
                                           Vector3.Cross(originA.LengthSquared() * originB - originB.LengthSquared() * originA,
                                               Vector3.Cross(originA, originB)) / (2 * Vector3.Cross(originA, originB).LengthSquared());
                    float radius = Vector3.Distance(centralPoint, boundaryPoints.Last());
                    return new R3DSphere(centralPoint, radius);
                }
                case 4:
                {
                    Vector3 originA = boundaryPoints[0] - boundaryPoints.Last();
                    Vector3 originB = boundaryPoints[1] - boundaryPoints.Last();
                    Vector3 originC = boundaryPoints[2] - boundaryPoints.Last();
                    Vector3 crossAB = Vector3.Cross(originA, originB);
                    Vector3 crossCA = Vector3.Cross(originC, originA);
                    Vector3 crossBC = Vector3.Cross(originB, originC);
                    // logic taken from https://gist.github.com/Darkyenus/c0b31a79e6115508822ce2128ab42cbf
                    float determinant = crossBC.X * originA.X + crossCA.X * originB.X + crossAB.X * originC.X;

                    // logic from https://math.stackexchange.com/questions/2414640/circumsphere-of-a-tetrahedron
                    Vector3 centralPoint =
                        (originA.LengthSquared() * crossBC +
                         originB.LengthSquared() * crossCA +
                         originC.LengthSquared() * crossAB)
                        / (2 * determinant);

                    float radius = centralPoint.Length();
                    centralPoint += boundaryPoints.Last();
                    return new R3DSphere(centralPoint, radius);
                }
                default: // impossible
                    throw new InvalidOperationException($"Invalid amount of boundary points: {boundaryPoints.Count} (must be between 0 and 4).");
            }
        }
    }
}
