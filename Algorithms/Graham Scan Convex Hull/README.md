# Graham Scan Convex Hull

### 1. Introduction
The Graham Scan algorithm finds the convex hull of a finite set of 2D points. The convex hull is the smallest convex polygon containing all points in the set. This algorithm is widely used in computational geometry, collision detection, and GIS applications.

### 2. Usage
```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
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

        IReadOnlyList<Point> hull = ConvexHull.Compute(points);
        foreach (var point in hull)
        {
            Console.WriteLine($"({point.X}, {point.Y})");
        }
    }
}
```

### 3. Detailed Explanation
1. **Pivot Selection**: The algorithm identifies the point with the lowest Y-coordinate (and lowest X-coordinate in case of ties) as the pivot. This point is guaranteed to be on the convex hull.
2. **Polar Angle Sorting**: The remaining points are sorted by their polar angle relative to the pivot. Instead of using trigonometric functions, the algorithm uses the cross product of vectors to determine orientation, preventing floating-point precision issues.
3. **Collinear Filtering**: If multiple points share the same polar angle relative to the pivot, only the furthest point is kept, and the closer ones are discarded.
4. **Stack-based Boundary Construction**: The algorithm iterates through the sorted points, maintaining a stack of the active hull boundary. For each point, it checks if the turn from the previous two points is counter-clockwise. If it is clockwise or collinear, the last point is popped from the stack until a counter-clockwise turn is established.

### 4. Complexity Analysis
- **Time Complexity**: 
  - **Sorting**: $O(N \log N)$ where $N$ is the number of unique points.
  - **Scan**: $O(N)$ since each point is pushed and popped from the stack at most once.
  - **Total Time Complexity**: $O(N \log N)$.
- **Space Complexity**: $O(N)$ to store the sorted points and the stack.