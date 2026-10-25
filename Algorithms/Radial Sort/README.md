# Radial Sort Algorithm

## 1. Introduction

Radial Sort is a specialized sorting algorithm designed to arrange a collection of 2D points (represented as `(x, y)` integer tuples) based on their spatial relationship to a central origin point (0,0). The primary sorting criterion is the Euclidean distance of each point from the origin. For points that share the exact same distance from the origin, a secondary sorting criterion is applied: their angle with respect to the positive x-axis, sorted in ascending order. This results in points being arranged in concentric "circles" around the origin, with points on the same circle ordered angularly.

This algorithm is particularly useful in scenarios involving spatial data, computational geometry, graphics, or any application where ordering points by their proximity and angular position relative to a central reference is required.

## 2. Usage

To use the `RadialSort` algorithm, simply call the static `Sort` method on the `RadialSort` class, passing an array of `(int x, int y)` tuples. The method will return a new array containing the sorted points.

```csharp
using System;

public class Example
{
    public static void Main(string[] args)
    {
        // Example usage of RadialSort
        (int x, int y)[] points = new (int x, int y)[]
        {
            (1, 1),    // Distance sqrt(2), Angle 45 deg
            (0, 2),    // Distance 2, Angle 90 deg
            (2, 0),    // Distance 2, Angle 0 deg
            (-1, 1),   // Distance sqrt(2), Angle 135 deg
            (0, 0),    // Distance 0, Angle 0 deg (or undefined, handled by Atan2)
            (-2, 0),   // Distance 2, Angle 180 deg
            (1, -1),   // Distance sqrt(2), Angle -45 deg
            (3, 3)     // Distance sqrt(18), Angle 45 deg
        };

        Console.WriteLine("Original Points:");
        foreach (var p in points)
        {
            Console.WriteLine($"({p.x}, {p.y})");
        }

        (int x, int y)[] sortedPoints = RadialSort.Sort(points);

        Console.WriteLine("\nSorted Points (Radial Sort):");
        foreach (var p in sortedPoints)
        {
            Console.WriteLine($"({p.x}, {p.y})");
        }

        // Expected Output Order (distances and angles):
        // (0, 0) - Dist: 0, Angle: 0
        // (2, 0) - Dist: 2, Angle: 0
        // (0, 2) - Dist: 2, Angle: 90
        // (-2, 0) - Dist: 2, Angle: 180
        // (1, -1) - Dist: 1.414, Angle: -45
        // (1, 1) - Dist: 1.414, Angle: 45
        // (-1, 1) - Dist: 1.414, Angle: 135
        // (3, 3) - Dist: 4.242, Angle: 45
    }
}
```

## 3. Detailed Explanation

The `RadialSort` algorithm works in three main steps:

1.  **Data Transformation**: For each input `(x, y)` point, a temporary `PointData` object is created. This object stores the original `(x, y)` tuple, and crucially, calculates two derived properties:
    *   **Distance**: The Euclidean distance from the origin `(0,0)` is calculated using the formula `sqrt(x^2 + y^2)`. To prevent potential integer overflow during `x*x` or `y*y` for large integer coordinates, the coordinates are cast to `double` before multiplication.
    *   **Angle**: The angle with respect to the positive x-axis is calculated using `Math.Atan2(y, x)`. This function is preferred over `Math.Atan` because it correctly handles all four quadrants and returns angles in radians within the range `(-PI, PI]`, which simplifies comparison.

2.  **Sorting**: All the `PointData` objects are collected into a `List<PointData>`. This list is then sorted using a custom comparison logic. The comparison prioritizes distance: points with smaller distances come first. If two points have the exact same distance, their angles are then used as a secondary sorting key, with smaller angles coming first. This ensures that points on the same "concentric circle" are ordered clockwise or counter-clockwise (depending on the angle range and interpretation, `Atan2` typically gives counter-clockwise from positive x-axis).

3.  **Result Extraction**: After the `List<PointData>` is sorted, a new array of `(int x, int y)` tuples is created. The original `(x, y)` tuples are extracted from the sorted `PointData` objects and placed into this new array, which is then returned as the final sorted output.

## 4. Complexity Analysis

*   **Time Complexity**: O(N log N)
    *   **Calculation of Distance and Angle**: Iterating through N points and performing `Math.Sqrt` and `Math.Atan2` for each takes O(N) time, as each calculation is a constant-time operation.
    *   **Sorting**: The `List<T>.Sort` method in C# typically uses an introsort algorithm, which has an average and worst-case time complexity of O(N log N), where N is the number of points.
    *   **Result Extraction**: Iterating through the sorted list to populate the result array takes O(N) time.
    *   Combining these steps, the dominant factor is the sorting, leading to an overall time complexity of O(N log N).

*   **Space Complexity**: O(N)
    *   **`PointData` List**: A `List<PointData>` is created to store N `PointData` objects. Each `PointData` object stores the original tuple (two integers) and two `double` values (distance and angle). This requires O(N) space.
    *   **Result Array**: A new array of `(int x, int y)` tuples is created to hold the sorted results, also requiring O(N) space.
    *   Therefore, the total space complexity is O(N).
