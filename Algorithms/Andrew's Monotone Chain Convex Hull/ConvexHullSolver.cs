using System;
using System.Collections.Generic;
using System.Linq;

public struct Point : IEquatable<Point>, IComparable<Point>
{
    public double X { get; }
    public double Y { get; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Point other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (X.GetHashCode() * 397) ^ Y.GetHashCode();
        }
    }

    public int CompareTo(Point other)
    {
        int compareX = X.CompareTo(other.X);
        if (compareX != 0) return compareX;
        return Y.CompareTo(other.Y);
    }
}

public static class ConvexHullSolver
{
    public static IList<Point> ComputeVal(IEnumerable<Point> points)
    {
        if (points == null)
        {
            throw new ArgumentNullException(nameof(points));
        }

        List<Point> sortedPoints = points.Distinct().ToList();
        sortedPoints.Sort();

        if (sortedPoints.Count <= 1)
        {
            return sortedPoints;
        }

        int n = sortedPoints.Count;
        List<Point> lower = new List<Point>();
        for (int i = 0; i < n; i++)
        {
            while (lower.Count >= 2 && CrossProduct(lower[lower.Count - 2], lower[lower.Count - 1], sortedPoints[i]) <= 0)
            {
                lower.RemoveAt(lower.Count - 1);
            }
            lower.Add(sortedPoints[i]);
        }

        List<Point> upper = new List<Point>();
        for (int i = n - 1; i >= 0; i--)
        {
            while (upper.Count >= 2 && CrossProduct(upper[upper.Count - 2], upper[upper.Count - 1], sortedPoints[i]) <= 0)
            {
                upper.RemoveAt(upper.Count - 1);
            }
            upper.Add(sortedPoints[i]);
        }

        if (lower.Count > 0) lower.RemoveAt(lower.Count - 1);
        if (upper.Count > 0) upper.RemoveAt(upper.Count - 1);

        List<Point> hull = new List<Point>();
        hull.AddRange(lower);
        hull.AddRange(upper);

        return hull;
    }

    private static double CrossProduct(Point o, Point a, Point b)
    {
        return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
    }
}