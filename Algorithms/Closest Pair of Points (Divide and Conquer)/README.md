# Closest Pair of Points Algorithm

## 1. Introduction
The **Closest Pair of Points** problem is a classic computational geometry problem where given a set of $n$ points in a 2D Euclidean space, the objective is to find the two points with the smallest Euclidean distance between them.

While a naive brute-force approach compares all possible pairs in $O(N^2)$ time, the divide-and-conquer approach achieves an optimal $O(N \log N)$ time complexity. This algorithm is widely utilized in:
- Geographic Information Systems (GIS) and spatial indexing.
- Collision detection in computer graphics and robotics.
- Clustering analysis in data science and machine learning.
- Air traffic control systems to detect near-miss conditions.

## 2. Usage

```csharp
using System;
using ClosestPairOfPoints;

public class Program
{
    public static void Main()
    {
        Point2D[] points = new[]
        {
            new Point2D(2.0, 3.0),
            new Point2D(12.0, 30.0),
            new Point2D(40.0, 50.0),
            new Point2D(5.0, 1.0),
            new Point2D(12.0, 10.0),
            new Point2D(3.0, 4.0)
        };

        ClosestPairResult result = ClosestPair.FindClosestPair(points);

        Console.WriteLine($"Closest Pair: {result.PointA} and {result.PointB}");
        Console.WriteLine($"Minimum Distance: {result.Distance:F4}");
    }
}
```

## 3. Detailed Explanation
The divide-and-conquer algorithm operates as follows:

1. **Pre-sorting**: Points are pre-sorted by X-coordinates and Y-coordinates in $O(N \log N)$ time.
2. **Divide**: The point set is divided into two halves (left and right) along the median X-coordinate.
3. **Conquer**: The algorithm recursively finds the closest pair in the left subset and right subset, yielding a minimum distance $\delta = \min(\delta_{left}, \delta_{right})$.
4. **Combine (Strip Processing)**:
   - A vertical strip centered at the dividing line of width $2\delta$ is constructed.
   - Only points lying within the strip ($|x - x_{mid}| < \delta$) are considered.
   - The points within the strip are ordered by their Y-coordinates.
   - For each point in the strip, the algorithm compares it against subsequent points with vertical difference less than $\delta$.
   - Geometrically, within a $\delta \times 2\delta$ rectangle, at most 7-8 points can exist without violating the property that no two points in the same partition are closer than $\delta$. Therefore, the inner loop executes in $O(1)$ operations per point.

## 4. Complexity Analysis

- **Time Complexity**:
  - **Sorting**: $O(N \log N)$ to pre-sort points by X and Y coordinates.
  - **Recurrence**: $T(N) = 2T(N / 2) + O(N)$, which by the Master Theorem resolves to $O(N \log N)$.
  - **Total Time Complexity**: $O(N \log N)$ in all cases.

- **Space Complexity**:
  - **Auxiliary Space**: $O(N)$ auxiliary space for storing sorted point arrays and intermediate merge buffers.
  - **Recursion Stack**: $O(\log N)$ call stack depth.