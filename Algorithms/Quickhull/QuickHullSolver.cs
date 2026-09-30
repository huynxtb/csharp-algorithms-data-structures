using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickHullAlgorithm
{
    /// <summary>
    /// Represents a 2D point with double-precision Cartesian coordinates.
    /// </summary>
    public readonly struct Point2D : IEquatable<Point2D>
    {
        private const double Epsilon = 1e-9;

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
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns the squared Euclidean distance to another point.
        /// </summary>
        /// <param name="other">The target point.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceSquared(Point2D other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return (dx * dx) + (dy * dy);
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
            // Round to grid to align with epsilon-based equality
            long hx = (long)Math.Round(X / Epsilon);
            long hy = (long)Math.Round(Y / Epsilon);
            return HashCode.Combine(hx, hy);
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(Point2D left, Point2D right) => left.Equals(right);

        /// <summary>
        /// Inequality operator.
        /// </summary>
        public static bool operator !=(Point2D left, Point2D right) => !left.Equals(right);

        /// <inheritdoc />
        public override string ToString() => $"({X}, {Y})";
    }

    /// <summary>
    /// Provides methods to compute the convex hull of 2D point sets using the Quickhull algorithm.
    /// </summary>
    public static class QuickHullSolver
    {
        private const double Epsilon = 1e-9;

        /// <summary>
        /// Computes the convex hull of the given collection of 2D points in counter-clockwise order.
        /// </summary>
        /// <param name="points">The sequence of input points.</param>
        /// <returns>A read-only list containing the ordered vertices of the convex hull.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="points"/> is null.</exception>
        public static IReadOnlyList<Point2D> FindConvexHull(IEnumerable<Point2D> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            // Remove duplicates within epsilon tolerance
            List<Point2D> uniquePoints = new();
            foreach (Point2D pt in points)
            {
                bool exists = false;
                for (int i = 0; i < uniquePoints.Count; i++)
                {
                    if (uniquePoints[i].Equals(pt))
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    uniquePoints.Add(pt);
                }
            }

            int count = uniquePoints.Count;
            if (count <= 2)
            {
                return uniquePoints.AsReadOnly();
            }

            // Find extreme points along the X axis (break ties with Y)
            Point2D minXPoint = uniquePoints[0];
            Point2D maxXPoint = uniquePoints[0];

            for (int i = 1; i < count; i++)
            {
                Point2D p = uniquePoints[i];
                if (p.X < minXPoint.X || (Math.Abs(p.X - minXPoint.X) < Epsilon && p.Y < minXPoint.Y))
                {
                    minXPoint = p;
                }

                if (p.X > maxXPoint.X || (Math.Abs(p.X - maxXPoint.X) < Epsilon && p.Y > maxXPoint.Y))
                {
                    maxXPoint = p;
                }
            }

            // If all points are identical (min == max)
            if (minXPoint.Equals(maxXPoint))
            {
                return new List<Point2D> { minXPoint }.AsReadOnly();
            }

            // Partition points into left and right of the directed baseline (minX -> maxX)
            List<Point2D> leftSet = new();
            List<Point2D> rightSet = new();

            for (int i = 0; i < count; i++)
            {
                Point2D p = uniquePoints[i];
                if (p.Equals(minXPoint) || p.Equals(maxXPoint))
                {
                    continue;
                }

                double location = CrossProductLocation(minXPoint, maxXPoint, p);
                if (location > Epsilon)
                {
                    leftSet.Add(p);
                }
                else if (location < -Epsilon)
                {
                    rightSet.Add(p);
                }
            }

            // Hull construction: Upper hull (left of min->max) then Lower hull (left of max->min)
            List<Point2D> hull = new();
            hull.Add(minXPoint);
            FindHullSet(minXPoint, maxXPoint, leftSet, hull);
            hull.Add(maxXPoint);
            FindHullSet(maxXPoint, minXPoint, rightSet, hull);

            return hull.AsReadOnly();
        }

        /// <summary>
        /// Recursively computes hull vertices to the left of directed segment (a -> b).
        /// Vertices are appended in counter-clockwise order.
        /// </summary>
        private static void FindHullSet(Point2D a, Point2D b, List<Point2D> points, List<Point2D> hull)
        {
            if (points.Count == 0)
            {
                return;
            }

            if (points.Count == 1)
            {
                hull.Add(points[0]);
                return;
            }

            // Find the point with maximum positive distance from line AB
            double maxDist = -1.0;
            int furthestIndex = -1;

            for (int i = 0; i < points.Count; i++)
            {
                double dist = CrossProductLocation(a, b, points[i]);
                if (dist > maxDist)
                {
                    maxDist = dist;
                    furthestIndex = i;
                }
                else if (Math.Abs(dist - maxDist) < Epsilon && furthestIndex != -1)
                {
                    // Tie-breaker: prefer point furthest from segment endpoints to maximize hull area
                    double currentMaxDist = Math.Max(a.DistanceSquared(points[i]), b.DistanceSquared(points[i]));
                    double prevMaxDist = Math.Max(a.DistanceSquared(points[furthestIndex]), b.DistanceSquared(points[furthestIndex]));
                    if (currentMaxDist > prevMaxDist)
                    {
                        furthestIndex = i;
                    }
                }
            }

            if (furthestIndex == -1 || maxDist <= Epsilon)
            {
                return;
            }

            Point2D c = points[furthestIndex];

            // Partition remaining points into: left of (A -> C) and left of (C -> B)
            List<Point2D> leftOfAC = new();
            List<Point2D> leftOfCB = new();

            for (int i = 0; i < points.Count; i++)
            {
                if (i == furthestIndex)
                {
                    continue;
                }

                Point2D p = points[i];
                if (CrossProductLocation(a, c, p) > Epsilon)
                {
                    leftOfAC.Add(p);
                }
                else if (CrossProductLocation(c, b, p) > Epsilon)
                {
                    leftOfCB.Add(p);
                }
            }

            // Recurse on left of AC, add vertex C, recurse on left of CB
            FindHullSet(a, c, leftOfAC, hull);
            hull.Add(c);
            FindHullSet(c, b, leftOfCB, hull);
        }

        /// <summary>
        /// Computes the 2D cross product of vector AB and AP.
        /// Returns positive if P is strictly to the left of ray AB,
        /// negative if P is to the right, and ~0 if collinear.
        /// </summary>
        private static double CrossProductLocation(Point2D a, Point2D b, Point2D p)
        {
            return ((b.X - a.X) * (p.Y - a.Y)) - ((b.Y - a.Y) * (p.X - a.X));
        }
    }
}