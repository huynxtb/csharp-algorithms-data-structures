using System;
using System.Collections.Generic;

// No namespace as per requirement for single file and no external dependencies unless necessary.

public static class RadialSort
{
    /// <summary>
    /// A private helper class to encapsulate a point's original coordinates along with its
    /// calculated distance from the origin and its angle.
    /// </summary>
    private class PointData
    {
        public (int x, int y) OriginalPoint { get; }
        public double Distance { get; }
        public double Angle { get; }

        /// <summary>
        /// Initializes a new instance of the PointData class, calculating distance and angle.
        /// </summary>
        /// <param name="point">The original (x, y) tuple representing the point.</param>
        public PointData((int x, int y) point)
        {
            OriginalPoint = point;
            // Calculate distance from origin (0,0)
            // Cast to double before multiplication to prevent potential integer overflow for large x, y
            Distance = Math.Sqrt((double)point.x * point.x + (double)point.y * point.y);
            // Calculate angle using Atan2, which handles all quadrants and returns radians in (-PI, PI]
            Angle = Math.Atan2(point.y, point.x);
        }
    }

    /// <summary>
    /// Sorts an array of 2D points based on their radial distance from the origin (0,0).
    /// Points at the same distance are further sorted by their angle with respect to the positive x-axis.
    /// </summary>
    /// <param name="points">An array of tuples, where each tuple represents (x, y) coordinates.</param>
    /// <returns>A new array of tuples sorted radially. Returns an empty array if the input is null or empty.</returns>
    public static (int x, int y)[] Sort((int x, int y)[] points)
    {
        if (points == null || points.Length == 0)
        {
            return Array.Empty<(int x, int y)>();
        }

        // Create a list to hold PointData objects, pre-allocating capacity for efficiency.
        List<PointData> pointDataList = new List<PointData>(points.Length);

        // Populate the list with PointData objects, calculating distance and angle for each point.
        foreach (var point in points)
        {
            pointDataList.Add(new PointData(point));
        }

        // Sort the list using a custom comparison logic:
        // 1. Primary sort key: Distance (ascending)
        // 2. Secondary sort key: Angle (ascending)
        pointDataList.Sort((a, b) =>
        {
            // Compare distances first
            int distanceComparison = a.Distance.CompareTo(b.Distance);
            if (distanceComparison != 0)
            {
                return distanceComparison; // If distances are different, return the comparison result
            }
            // If distances are the same, compare angles
            return a.Angle.CompareTo(b.Angle);
        });

        // Create the result array and populate it with the original points from the sorted list.
        (int x, int y)[] sortedPoints = new (int x, int y)[points.Length];
        for (int i = 0; i < pointDataList.Count; i++)
        {
            sortedPoints[i] = pointDataList[i].OriginalPoint;
        }

        return sortedPoints;
    }
}