using System;
using System.Collections.Generic;
using System.Linq;

public readonly struct Point : IEquatable<Point>
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
        return Math.Abs(X - other.X) < 1e-9 && Math.Abs(Y - other.Y) < 1e-9;
    }

    public override bool Equals(object obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Math.Round(X, 9), Math.Round(Y, 9));
    }
}

public static class ConvexHull
{
    private const double Epsilon = 1e-9;

    public static IReadOnlyList<Point> Compute(IEnumerable<Point> points)
    {
        if (points == null)
        {
            throw new ArgumentNullException(nameof(points));
        }

        var uniquePoints = points.Distinct().ToList();
        if (uniquePoints.Count < 3)
        {
            throw new ArgumentException("Input must contain at least 3 unique points.", nameof(points));
        }

        Point pivot = uniquePoints[0];
        int pivotIndex = 0;
        for (int i = 1; i < uniquePoints.Count; i++)
        {
            Point p = uniquePoints[i];
            if (p.Y < pivot.Y || (Math.Abs(p.Y - pivot.Y) < Epsilon && p.X < pivot.X))
            { 
                pivot = p;
                pivotIndex = i;
            }
        }

        var sorted = uniquePoints.Where((_, idx) => idx != pivotIndex).ToList();
        sorted.Sort((a, b) =>
        {
            double order = (a.X - pivot.X) * (b.Y - pivot.Y) - (a.Y - pivot.Y) * (b.X - pivot.X);
            if (Math.Abs(order) < Epsilon)
            { 
                double distA = (a.X - pivot.X) * (a.X - pivot.X) + (a.Y - pivot.Y) * (a.Y - pivot.Y);
                double distB = (b.X - pivot.X) * (b.X - pivot.X) + (b.Y - pivot.Y) * (b.Y - pivot.Y);
                return distA.CompareTo(distB);
            }
            return order > 0 ? -1 : 1;
        });

        var filtered = new List<Point>();
        for (int i = 0; i < sorted.Count; i++)
        {
            while (i < sorted.Count - 1 && IsCollinear(pivot, sorted[i], sorted[i + 1]))
            {
                i++;
            }
            filtered.Add(sorted[i]);
        }

        if (filtered.Count < 2)
        {
            throw new ArgumentException("Points are collinear; a 2D convex hull cannot be formed.");
        }

        var hull = new List<Point> { pivot, filtered[0] };
        for (int i = 1; i < filtered.Count; i++)
        {
            while (hull.Count >= 2 && GetOrientation(hull[hull.Count - 2], hull[hull.Count - 1], filtered[i]) <= 0)
            {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(filtered[i]);
        }

        return hull.AsReadOnly();
    }

    private static bool IsCollinear(Point p, Point a, Point b)
    { 
        double order = (a.X - p.X) * (b.Y - p.Y) - (a.Y - p.Y) * (b.X - p.X);
        return Math.Abs(order) < Epsilon;
    }

    private static int GetOrientation(Point a, Point b, Point c)
    {
        double val = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        if (Math.Abs(val) < Epsilon) return 0;
        return val > 0 ? 1 : -1;
    }
}