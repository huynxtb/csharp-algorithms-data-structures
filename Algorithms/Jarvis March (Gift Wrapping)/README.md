# Jarvis March (Gift Wrapping) Algorithm

### 1. Introduction
The Jarvis March algorithm computes the convex hull of a set of 2D points. The convex hull is the smallest convex polygon that encloses all the points in the set. This algorithm simulates wrapping a string around the point set, making it highly intuitive. It is ideal for scenarios with a small number of vertices on the output hull.

### 2. Usage
```csharp
using System;
using System.Collections.Generic; 

class Program
{
    static void Main()
    {
        List<Point> points = new List<Point>
        {
            new Point(0, 3),
            new Point(2, 2),
            new Point(1, 1),
            new Point(2, 1),
            new Point(3, 0),
            new Point(0, 0),
            new Point(3, 3)
        };

        List<Point> hull = JarvisMarchSolver.FindConvexHull(points);

        foreach (var point in hull)
        {
            Console.WriteLine($"({point.X}, {point.Y})");
        }
    }
}
```

### 3. Detailed Explanation
1. **Find Start Point**: The algorithm begins by identifying the leftmost point (with the minimum X coordinate, using the minimum Y coordinate as a tie-breaker). This point is guaranteed to be on the convex hull.
2. **Wrap the Hull**: Starting from the current hull point $p$, the algorithm searches for a point $q$ such that all other points $r$ lie to the left of the directed line segment $pq$. 
3. **Orientation Check**: The cross product formula `(q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y)` determines the turn direction. A negative value indicates a counter-clockwise turn, meaning $r$ is further to the left than $q$.
4. **Collinear Handling**: If a point $r$ is collinear with $p$ and $q$, the algorithm selects the point furthest from $p$ to ensure only the extreme vertices are included in the hull.
5. **Termination**: The process repeats, setting the next point $p = q$, until the algorithm wraps back to the starting point.

### 4. Complexity Analysis
- **Time Complexity**: $O(nh)$, where $n$ is the total number of input points and $h$ is the number of points on the convex hull. In the worst case (where all points lie on the hull), the complexity is $O(n^2)$.
- **Space Complexity**: $O(h)$ auxiliary space to store the output vertices of the convex hull.