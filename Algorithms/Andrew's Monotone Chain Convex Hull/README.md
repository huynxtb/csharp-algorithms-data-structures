# Andrew's Monotone Chain Convex Hull Algorithm

## 1. Introduction
Andrew's Monotone Chain algorithm computes the convex hull of a set of 2D points. The convex hull is the smallest convex polygon containing all points in the set. Use this algorithm in computational geometry, collision detection, and image processing.

## 2. Usage
```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var points = new List<Point>
        {
            new Point(0, 0),
            new Point(3, 0),
            new Point(3, 3),
            new Point(0, 3),
            new Point(1, 1),
            new Point(2, 2)
        };

        IList<Point> hull = ConvexHullSolver.ComputeVal(points);

        foreach (var point in hull)
        {
            Console.WriteLine($"({point.X}, {point.Y})");
        }
    }
}
```

## 3. Detailed Explanation
The algorithm constructs the convex hull by splitting it into two chains: the lower hull and the upper hull.
1. **Sorting**: Sort the input points lexicographically by their X-coordinate, resolving ties by their Y-coordinate.
2. **Lower Hull**: Iterate through the sorted points from left to right. For each point, append it to the lower hull. If the last three points do not form a counter-clockwise turn (determined by a non-positive cross product), remove the middle point. Repeat this check until a counter-clockwise turn is established or fewer than two points remain in the hull.
3. **Upper Hull**: Iterate through the sorted points in reverse order (right to left) and repeat the same process to build the upper hull.
4. **Combination**: Combine the lower and upper hulls, omitting the duplicate boundary points at the ends.

## 4. Complexity Analysis
- **Time Complexity**:
  - Sorting: O(N log N) where N is the number of unique points.
  - Hull Construction: O(N) since each point is pushed and popped from the hull at most once.
  - Total Time Complexity: O(N log N).
- **Space Complexity**: O(N) to store the sorted points and the resulting hull vertices.