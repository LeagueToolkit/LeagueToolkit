using System;
using System.Collections.Generic;
using System.Numerics;

namespace LeagueToolkit.Core.Primitives
{
    /// <summary>
    /// Represents an Axis-Aligned Bounding Box
    /// </summary>
    public struct Box
    {
        public Vector3 Min;
        public Vector3 Max;

        public const int VERTEX_COUNT = 8;

        public Box()
        {
            this.Min = new(float.MaxValue, float.MaxValue, float.MaxValue);
            this.Max = new(float.MinValue, float.MinValue, float.MinValue);
        }

        /// <summary>
        /// Initializes a new <see cref="Box"/> instance
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Box(Vector3 min, Vector3 max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Creates a clone of a <see cref="Box"/> object
        /// </summary>
        /// <param name="box">The <see cref="Box"/> to clone</param>
        public Box(Box box)
        {
            this.Min = box.Min;
            this.Max = box.Max;
        }

        public static Box FromVertices(IEnumerable<Vector3> vertices)
        {
            Vector3 min = new(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new(float.MinValue, float.MinValue, float.MinValue);

            foreach (Vector3 vertex in vertices)
            {
                if (min.X > vertex.X)
                {
                    min.X = vertex.X;
                }
                if (min.Y > vertex.Y)
                {
                    min.Y = vertex.Y;
                }
                if (min.Z > vertex.Z)
                {
                    min.Z = vertex.Z;
                }
                if (max.X < vertex.X)
                {
                    max.X = vertex.X;
                }
                if (max.Y < vertex.Y)
                {
                    max.Y = vertex.Y;
                }
                if (max.Z < vertex.Z)
                {
                    max.Z = vertex.Z;
                }
            }

            return new(min, max);
        }

        public static Box ExpandByPoint(Box box, Vector3 point)
        {
            return new(Vector3.Min(box.Min, point), Vector3.Max(box.Max, point));
        }

        /// <summary>
        /// Calculates the size of this <see cref="Box"/>
        /// </summary>
        public Vector3 GetSize() => this.Max - this.Min;

        public Vector3 GetCentralPoint()
        {
            return new Vector3(
                0.5f * (this.Min.X + this.Max.X),
                0.5f * (this.Min.Y + this.Max.Y),
                0.5f * (this.Min.Z + this.Max.Z)
            );
        }

        /// <summary>
        /// Get vertex using same way League does
        /// <br>xyz - 0-Min 1-Max</br>
        /// <br>000 - 0</br>
        /// <br>010 - 1</br>
        /// <br>100 - 2</br>
        /// <br>110 - 3</br>
        /// <br>001 - 4</br>
        /// <br>011 - 5</br>
        /// <br>101 - 6</br>
        /// <br>111 - 7</br>
        /// </summary>
        public Vector3 GetVertex(int index)
        {
            return index switch
            {
                0 => this.Min,
                1 => new(this.Min.X, this.Max.Y, this.Min.Z),
                2 => new(this.Max.X, this.Min.Y, this.Min.Z),
                3 => new(this.Max.X, this.Max.Y, this.Min.Z),

                4 => new(this.Min.X, this.Min.Y, this.Max.Z),
                5 => new(this.Min.X, this.Max.Y, this.Max.Z),
                6 => new(this.Max.X, this.Min.Y, this.Max.Z),
                7 => this.Max,
                _
                    => throw new ArgumentOutOfRangeException(
                        $"The provided index: {index} is outside of the allowed range (0-7)"
                    ),
            };
        }

        public Sphere GetBoundingSphere()
        {
            Vector3 centralPoint = GetCentralPoint();

            return new(centralPoint, Vector3.Distance(centralPoint, this.Max));
        }

        /// <summary>
        /// Determines wheter this <see cref="Box"/> contains the <see cref="Vector3"/> <paramref name="point"/>
        /// </summary>
        /// <param name="point">The containing point</param>
        /// <returns>Wheter this <see cref="Box"/> contains the <see cref="Vector3"/> <paramref name="point"/></returns>
        public bool ContainsPoint(Vector3 point)
        {
            return point.X >= this.Min.X
                && point.X <= this.Max.X
                && point.Y >= this.Min.Y
                && point.Y <= this.Max.Y
                && point.Z >= this.Min.Z
                && point.Z <= this.Max.Z;
        }
    }
}
