using System;
using System.Collections.Generic; 

public record struct Point(double X, double Y);

public static class JarvisMarchSolver
{
    public static List<Point> FindConvexHull(List<Point> points)
    {
        if (points == null)
        {
            throw new ArgumentNullException(nameof(points));
        }

        int n = points.Count;
        if (n < 3)
        {
            return new List<Point>(points);
        }

        int leftmostIdx = 0;
        for (int i = 1; i < n; i++)
        {
            if (points[i].X < points[leftmostIdx].X ||
                (points[i].X == points[leftmostIdx].X && points[i].Y < points[leftmostIdx].Y))
            {
                leftmostIdx = i;
            }
        }

        List<Point> hull = new List<Point>();
        int p = leftmostIdx;
        int q;

        do
        {
            hull.Add(points[p]);
            q = (p + 1) % n;

            for (int i = 0; i < n; i++)
            {
                if (i == p) continue;

                double val = Orientation(points[p], points[q], points[i]);

                if (val < 0)
                {
                    q = i;
                }
                else if (val == 0)
                {
                    if (DistanceSquared(points[p], points[i]) > DistanceSquared(points[p], points[q]))
                    {
                        q = i;
                    }
                }
            }

            p = q;

        } while (p != leftmostIdx);

        return hull;
    }

    private static double Orientation(Point p, Point q, Point r)
    {
        return (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
    }

    private static double DistanceSquared(Point p1, Point p2)
    {
        double dx = p1.X - p2.X;
        double dy = p1.Y - p2.Y;
        return dx * dx + dy * dy;
    }
}