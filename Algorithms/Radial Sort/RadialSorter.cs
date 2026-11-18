using System;
using System.Collections.Generic;

public static class RadialSorter
{
    /// <summary>
    /// Sorts an array of 2D points based on their radial distance from the origin (0,0).
    /// </summary>
    /// <param name="points">An array of tuples representing 2D points (x, y).</param>
    public static void RadialSort(Span<ValueTuple<double, double>> points)
    {
        // Use the built-in Array.Sort with a custom comparer for efficiency.
        // The comparer calculates the Euclidean distance for each point.
        points.Sort(Comparer<ValueTuple<double, double>>.Create((p1, p2) =>
        {
            double dist1 = Math.Sqrt(p1.Item1 * p1.Item1 + p1.Item2 * p1.Item2);
            double dist2 = Math.Sqrt(p2.Item1 * p2.Item1 + p2.Item2 * p2.Item2);
            return dist1.CompareTo(dist2);
        }));
    }

    /// <summary>
    /// Sorts a list of 2D points based on their radial distance from the origin (0,0).
    /// </summary>
    /// <param name="points">A list of tuples representing 2D points (x, y).</param>
    public static void RadialSort(List<ValueTuple<double, double>> points)
    {
        // Use the built-in List.Sort with a custom comparer for efficiency.
        // The comparer calculates the Euclidean distance for each point.
        points.Sort((p1, p2) =>
        {
            double dist1 = Math.Sqrt(p1.Item1 * p1.Item1 + p1.Item2 * p1.Item2);
            double dist2 = Math.Sqrt(p2.Item1 * p2.Item1 + p2.Item2 * p2.Item2);
            return dist1.CompareTo(dist2);
        });
    }
}