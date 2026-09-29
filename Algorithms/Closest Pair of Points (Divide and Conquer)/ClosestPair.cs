using System;
using System.Collections.Generic;
using System.Linq;

namespace ClosestPairOfPoints
{
    public readonly struct Point2D : IEquatable<Point2D>
    {
        public double X { get; }
        public double Y { get; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Point2D other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public bool Equals(Point2D other) => X.Equals(other.X) && Y.Equals(other.Y);

        public override bool Equals(object? obj) => obj is Point2D other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Point2D left, Point2D right) => left.Equals(right);

        public static bool operator !=(Point2D left, Point2D right) => !left.Equals(right);

        public override string ToString() => $"({X}, {Y})";
    }

    public readonly struct ClosestPairResult
    {
        public Point2D PointA { get; }
        public Point2D PointB { get; }
        public double Distance { get; }

        public ClosestPairResult(Point2D pointA, Point2D pointB, double distance)
        {
            PointA = pointA;
            PointB = pointB;
            Distance = distance;
        }

        public override string ToString() => $"Distance: {Distance}, PointA: {PointA}, PointB: {PointB}";
    }

    public static class ClosestPair
    {
        public static ClosestPairResult FindClosestPair(IReadOnlyList<Point2D> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            if (points.Count < 2)
            {
                throw new ArgumentException("At least two points are required to find the closest pair.", nameof(points));
            }

            Point2D[] pointsSortedByX = points.ToArray();
            Array.Sort(pointsSortedByX, (a, b) =>
            {
                int cmp = a.X.CompareTo(b.X);
                return cmp != 0 ? cmp : a.Y.CompareTo(b.Y);
            });

            Point2D[] pointsSortedByY = points.ToArray();
            Array.Sort(pointsSortedByY, (a, b) =>
            {
                int cmp = a.Y.CompareTo(b.Y);
                return cmp != 0 ? cmp : a.X.CompareTo(b.X);
            });

            Point2D[] auxiliary = new Point2D[points.Count];
            return FindClosestPairRecursive(pointsSortedByX, pointsSortedByY, auxiliary, 0, points.Count - 1);
        }

        private static ClosestPairResult FindClosestPairRecursive(
            Point2D[] pointsX,
            Point2D[] pointsY,
            Point2D[] auxY,
            int low,
            int high)
        {
            int count = high - low + 1;

            if (count <= 3)
            {
                return BruteForce(pointsX, low, high);
            }

            int mid = low + (count / 2);
            Point2D midPoint = pointsX[mid];

            // Partition pointsY into left and right halves maintaining Y-order
            int leftIndex = low;
            int rightIndex = mid + 1;

            for (int i = low; i <= high; i++)
            {
                Point2D p = pointsY[i];
                if (p.X < midPoint.X || (p.X == midPoint.X && p.Y <= midPoint.Y && leftIndex <= mid))
                {
                    auxY[leftIndex++] = p;
                }
                else
                {
                    auxY[rightIndex++] = p;
                }
            }

            Array.Copy(auxY, low, pointsY, low, count);

            ClosestPairResult leftResult = FindClosestPairRecursive(pointsX, pointsY, auxY, low, mid);
            ClosestPairResult rightResult = FindClosestPairRecursive(pointsX, pointsY, auxY, mid + 1, high);

            ClosestPairResult bestResult = leftResult.Distance < rightResult.Distance ? leftResult : rightResult;
            double delta = bestResult.Distance;

            // Merge sorted left and right halves back into auxY to maintain overall Y-sorted order
            MergeByY(pointsY, auxY, low, mid, high);

            // Build strip of points within delta distance from the vertical dividing line
            int stripCount = 0;
            for (int i = low; i <= high; i++)
            {
                if (Math.Abs(auxY[i].X - midPoint.X) < delta)
                {
                    pointsY[low + stripCount] = auxY[i];
                    stripCount++;
                }
            }

            // Compare each point in the strip with subsequent points within delta along the Y-axis
            for (int i = 0; i < stripCount; i++)
            {
                Point2D p1 = pointsY[low + i];
                for (int j = i + 1; j < stripCount; j++)
                {
                    Point2D p2 = pointsY[low + j];
                    if (p2.Y - p1.Y >= delta)
                    {
                        break;
                    }

                    double dist = p1.DistanceTo(p2);
                    if (dist < delta)
                    {
                        delta = dist;
                        bestResult = new ClosestPairResult(p1, p2, dist);
                    }
                }
            }

            return bestResult;
        }

        private static void MergeByY(Point2D[] source, Point2D[] destination, int low, int mid, int high)
        {
            int i = low;
            int j = mid + 1;
            int k = low;

            while (i <= mid && j <= high)
            {
                if (source[i].Y <= source[j].Y)
                {
                    destination[k++] = source[i++];
                }
                else
                {
                    destination[k++] = source[j++];
                }
            }

            while (i <= mid)
            {
                destination[k++] = source[i++];
            }

            while (j <= high)
            {
                destination[k++] = source[j++];
            }
        }

        private static ClosestPairResult BruteForce(Point2D[] points, int low, int high)
        {
            double minDistance = double.PositiveInfinity;
            Point2D bestA = default;
            Point2D bestB = default;

            for (int i = low; i <= high; i++)
            {
                for (int j = i + 1; j <= high; j++)
                {
                    double dist = points[i].DistanceTo(points[j]);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        bestA = points[i];
                        bestB = points[j];
                    }
                }
            }

            return new ClosestPairResult(bestA, bestB, minDistance);
        }
    }
}