using System;
using System.Collections.Generic;
using System.Linq;

namespace Geometry
{
    /// <summary>
    /// Represents a point in 2D Euclidean space with double precision coordinates.
    /// </summary>
    public readonly struct Point2D : IEquatable<Point2D>, IComparable<Point2D>
    {
        private const double Epsilon = 1e-10;

        /// <summary>
        /// Gets the X coordinate of the point.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Gets the Y coordinate of the point.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> struct.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Computes the 2D cross product of vectors (b - a) and (c - a).
        /// Returns a positive value for counter-clockwise turn, negative for clockwise turn, and zero for collinear points.
        /// </summary>
        /// <param name="a">The starting common point.</param>
        /// <param name="b">The end point of the first vector.</param>
        /// <param name="c">The end point of the second vector.</param>
        /// <returns>The signed cross product value.</returns>
        public static double CrossProduct(Point2D a, Point2D b, Point2D c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        /// <summary>
        /// Determines the orientation of the triplet (a, b, c).
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second point.</param>
        /// <param name="c">Third point.</param>
        /// <returns>1 if counter-clockwise (left turn), -1 if clockwise (right turn), 0 if collinear.</returns>
        public static int Orientation(Point2D a, Point2D b, Point2D c)
        {
            double cross = CrossProduct(a, b, c);
            if (cross > Epsilon) return 1;
            if (cross < -Epsilon) return -1;
            return 0;
        }

        /// <summary>
        /// Calculates the squared Euclidean distance between two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The squared distance.</returns>
        public static double DistanceSquared(Point2D a, Point2D b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        /// <inheritdoc />
        public int CompareTo(Point2D other)
        {
            int cmpX = X.CompareTo(other.X);
            if (cmpX != 0) return cmpX;
            return Y.CompareTo(other.Y);
        }

        /// <inheritdoc />
        public bool Equals(Point2D other)
        {
            return Math.Abs(X - other.X) < Epsilon && Math.Abs(Y - other.Y) < Epsilon;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is Point2D other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(Math.Round(X, 8), Math.Round(Y, 8));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static bool operator ==(Point2D left, Point2D right) => left.Equals(right);
        public static bool operator !=(Point2D left, Point2D right) => !left.Equals(right);
    }

    /// <summary>
    /// Provides an implementation of Chan's Convex Hull Algorithm for 2D points.
    /// Computes the convex hull in optimal O(n log h) time, where n is point count and h is hull vertex count.
    /// </summary>
    public static class ChansConvexHull
    {
        private const double Epsilon = 1e-10;

        /// <summary>
        /// Computes the 2D convex hull of a given set of points in counter-clockwise order.
        /// </summary>
        /// <param name="points">The collection of input points.</param>
        /// <returns>A read-only list containing the vertices of the convex hull in counter-clockwise order.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is null.</exception>
        public static IReadOnlyList<Point2D> ComputeConvexHull(IEnumerable<Point2D> points)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            // Remove duplicates and sort for baseline processing
            List<Point2D> uniquePoints = points.Distinct().ToList();
            int n = uniquePoints.Count;

            if (n <= 2)
            {
                return uniquePoints;
            }

            // Chan's parameter doubling/squaring loop: m = 2^(2^t)
            for (int t = 1; ; t++)
            {
                double power = Math.Pow(2, Math.Min(30, Math.Pow(2, t)));
                int m = (power >= n || power > int.MaxValue) ? n : (int)power;

                IReadOnlyList<Point2D>? hull = TryComputeHullWithM(uniquePoints, m);
                if (hull != null)
                {
                    return hull;
                }

                if (m >= n)
                {
                    // Fallback to standard Monotone Chain if parameter covers all points
                    return ComputeMonotoneChainHull(uniquePoints);
                }
            }
        }

        /// <summary>
        /// Tries to compute the convex hull assuming hull size <= m.
        /// Returns null if the hull has more than m vertices.
        /// </summary>
        private static IReadOnlyList<Point2D>? TryComputeHullWithM(List<Point2D> points, int m)
        {
            int n = points.Count;
            int r = (int)Math.Ceiling((double)n / m);

            // 1. Partition points into r subsets of size at most m and compute mini-hulls using Monotone Chain
            List<Point2D>[] miniHulls = new List<Point2D>[r];
            for (int i = 0; i < r; i++)
            {
                int start = i * m;
                int count = Math.Min(m, n - start);
                List<Point2D> subset = points.GetRange(start, count);
                miniHulls[i] = ComputeMonotoneChainHull(subset);
            }

            // 2. Find the starting point (lexicographically smallest point / leftmost-lowest)
            Point2D startPoint = points[0];
            for (int i = 1; i < n; i++)
            {
                if (points[i].CompareTo(startPoint) < 0)
                {
                    startPoint = points[i];
                }
            }

            List<Point2D> hull = new List<Point2D>();
            hull.Add(startPoint);

            // 3. Jarvis march step using binary search on each mini-hull
            for (int step = 0; step < m; step++)
            {
                Point2D current = hull[step];
                Point2D? bestCandidate = null;

                for (int i = 0; i < r; i++)
                {
                    List<Point2D> miniHull = miniHulls[i];
                    if (miniHull.Count == 0)
                        continue;

                    Point2D candidate = FindTangent(miniHull, current);
                    if (candidate.Equals(current))
                        continue;

                    if (bestCandidate == null)
                    {
                        bestCandidate = candidate;
                    }
                    else
                    {
                        int orient = Point2D.Orientation(current, bestCandidate.Value, candidate);
                        if (orient > 0)
                        {
                            // candidate is strictly to the left of (current -> bestCandidate)
                            bestCandidate = candidate;
                        }
                        else if (orient == 0)
                        {
                            // Collinear: pick the furthest point
                            if (Point2D.DistanceSquared(current, candidate) > Point2D.DistanceSquared(current, bestCandidate.Value))
                            {
                                bestCandidate = candidate;
                            }
                        }
                    }
                }

                if (bestCandidate == null)
                {
                    break;
                }

                // If we wrapped back to start, the convex hull is complete
                if (bestCandidate.Value.Equals(startPoint))
                {
                    return hull;
                }

                hull.Add(bestCandidate.Value);
            }

            // Hull not closed within m steps: m was too small
            return null;
        }

        /// <summary>
        /// Computes the convex hull of a point set using Andrew's Monotone Chain algorithm in O(k log k) time.
        /// Points returned are strictly in counter-clockwise order without collinear internal edge points.
        /// </summary>
        public static List<Point2D> ComputeMonotoneChainHull(List<Point2D> points)
        {
            List<Point2D> sorted = points.Distinct().OrderBy(p => p).ToList();
            int n = sorted.Count;

            if (n <= 2)
            {
                return sorted;
            }

            List<Point2D> lower = new List<Point2D>(n);
            for (int i = 0; i < n; i++)
            {
                while (lower.Count >= 2 && Point2D.Orientation(lower[lower.Count - 2], lower[lower.Count - 1], sorted[i]) <= 0)
                {
                    lower.RemoveAt(lower.Count - 1);
                }
                lower.Add(sorted[i]);
            }

            List<Point2D> upper = new List<Point2D>(n);
            for (int i = n - 1; i >= 0; i--)
            {
                while (upper.Count >= 2 && Point2D.Orientation(upper[upper.Count - 2], upper[upper.Count - 1], sorted[i]) <= 0)
                {
                    upper.RemoveAt(upper.Count - 1);
                }
                upper.Add(sorted[i]);
            }

            // Remove duplicate endpoints
            lower.RemoveAt(lower.Count - 1);
            upper.RemoveAt(upper.Count - 1);

            lower.AddRange(upper);
            return lower;
        }

        /// <summary>
        /// Finds the counter-clockwise tangent point from an external point to a convex polygon in O(log k) time.
        /// </summary>
        private static Point2D FindTangent(List<Point2D> polygon, Point2D point)
        {
            int n = polygon.Count;
            if (n == 1)
                return polygon[0];
            if (n == 2)
            {
                int ori = Point2D.Orientation(point, polygon[0], polygon[1]);
                if (ori > 0) return polygon[1];
                if (ori < 0) return polygon[0];
                return Point2D.DistanceSquared(point, polygon[0]) > Point2D.DistanceSquared(point, polygon[1])
                    ? polygon[0] : polygon[1];
            }

            // Binary search over the vertices of the convex polygon
            int low = 0;
            int high = n;
            while (low < high)
            {
                int mid = low + (high - low) / 2;
                Point2D midP = polygon[mid];
                Point2D nextP = polygon[(mid + 1) % n];
                Point2D prevP = polygon[(mid - 1 + n) % n];

                int midNext = Point2D.Orientation(point, midP, nextP);
                int midPrev = Point2D.Orientation(point, midP, prevP);

                // Tangent condition: neither neighbor is strictly to the left of ray (point -> midP)
                if (midNext <= 0 && midPrev <= 0)
                {
                    return midP;
                }

                int lowP = Point2D.Orientation(point, polygon[low], polygon[(low + 1) % n]);
                int lowMid = Point2D.Orientation(point, polygon[low], midP);

                if (lowMid > 0)
                {
                    if (midNext > 0 && (lowP <= 0 || Point2D.Orientation(point, polygon[low], polygon[high % n]) > 0))
                        high = mid;
                    else
                        low = mid + 1;
                }
                else
                {
                    if (midNext <= 0 && (lowP > 0 || Point2D.Orientation(point, polygon[low], polygon[high % n]) <= 0))
                        low = mid + 1;
                    else
                        high = mid;
                }
            }

            return polygon[low % n];
        }
    }
}