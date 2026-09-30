# Quickhull Algorithm in C#

### 1. Introduction
Quickhull is a divide-and-conquer algorithm designed to compute the convex hull of a finite set of $n$ points in a Euclidean plane. Similar in spirit to Quicksort, Quickhull iteratively partitions points with respect to dividing lines and isolates extreme boundary points.

Quickhull is particularly well-suited for:
- Geometric computing and collision detection.
- Computer graphics, GIS shape analysis, and pattern recognition.
- Scenarios where points are uniformly distributed, yielding fast average-case execution.

---

### 2. Usage

```csharp
using System;
using System.Collections.Generic;
using QuickHullAlgorithm;

public class Example
{
    public static void Run()
    {
        var points = new List<Point2D>
        {
            new Point2D(0, 0),
            new Point2D(1, 1),
            new Point2D(2, 2),
            new Point2D(4, 0),
            new Point2D(4, 4),
            new Point2D(0, 4),
            new Point2D(2, 3)
        };

        IReadOnlyList<Point2D> hull = QuickHullSolver.FindConvexHull(points);

        foreach (Point2D vertex in hull)
        {
            Console.WriteLine(vertex);
        }
    }
}
```

---

### 3. Detailed Explanation

The algorithm proceeds as follows:
1. **Preprocessing**: Finds the minimum and maximum points along the X-axis (using Y as a tie-breaker). These two extreme points (`minX`, `maxX`) are guaranteed to belong to the convex hull.
2. **Partitioning**: The line connecting `minX` and `maxX` divides the remaining points into two subsets based on the 2D cross product (determinant orientation test):
   - Points to the left of $\vec{minX \to maxX}$ (upper hull).
   - Points to the left of $\vec{maxX \to minX}$ (lower hull).
3. **Recursive Step (`FindHullSet`)**:
   - Given a directed line segment $AB$ and candidate points to the left of $AB$, find point $C$ with maximum perpendicular distance from $AB$.
   - Point $C$ is a verified convex hull vertex.
   - Any points falling inside the triangle $\triangle ABC$ cannot be part of the convex hull and are discarded.
   - Recursively process points lying to the left of directed segment $AC$, output $C$, and recursively process points to the left of directed segment $CB$.
4. **Edge Cases & Stability**:
   - Duplicates and collinear points are handled cleanly using an epsilon tolerance ($10^{-9}$).
   - Single-point and two-point geometries return directly without recursive decomposition.

---

### 4. Complexity Analysis

| Case | Time Complexity | Space Complexity |
| :--- | :--- | :--- |
| **Best Case** | $\mathcal{O}(n \log n)$ | $\mathcal{O}(\log n)$ |
| **Average Case** | $\mathcal{O}(n \log n)$ | $\mathcal{O}(\log n)$ |
| **Worst Case** | $\mathcal{O}(n^2)$ | $\mathcal{O}(n)$ |

- **Time Complexity**:
  - In the average case where points are uniformly distributed, points inside triangles are discarded rapidly, leading to $\mathcal{O}(n \log n)$ runtime.
  - In the worst case (e.g., when all points lie on a circle), only one point is eliminated per recursive step, degrading runtime to $\mathcal{O}(n^2)$.
- **Space Complexity**:
  - $\mathcal{O}(\log n)$ auxiliary space for call recursion stack and partitioned lists in balanced scenarios, up to $\mathcal{O}(n)$ auxiliary memory in degenerate worst-case partitions.