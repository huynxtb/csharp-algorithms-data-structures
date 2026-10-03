# Chan's Convex Hull Algorithm (2D)

## 1. Introduction

Chan's Algorithm is an optimal output-sensitive algorithm designed to compute the convex hull of a set of $n$ points in 2D Euclidean space. Discovered by Timothy M. Chan in 1996, it achieves an optimal running time of $\mathcal{O}(n \log h)$, where $h$ is the number of vertices on the output convex hull.

It brilliantly combines two foundational algorithms:
1. **Graham Scan / Andrew's Monotone Chain** (efficient $\mathcal{O}(m \log m)$ algorithm for small sub-problems).
2. **Jarvis March (Gift Wrapping)** (output-sensitive $\mathcal{O}(n h)$ algorithm that iteratively finds tangents).

Use Chan's algorithm when dealing with very large point sets where the output convex hull size $h$ is expected to be significantly smaller than $n$ ($h \ll n$), achieving true theoretical and practical optimality.

---

## 2. Usage

```csharp
using System;
using System.Collections.Generic;
using Geometry;

public class Example
{
    public static void Run()
    {
        List<Point2D> points = new List<Point2D>
        {
            new Point2D(0.0, 0.0),
            new Point2D(5.0, 0.0),
            new Point2D(5.0, 5.0),
            new Point2D(0.0, 5.0),
            new Point2D(2.5, 2.5),
            new Point2D(1.0, 1.0),
            new Point2D(3.0, 4.0)
        };

        IReadOnlyList<Point2D> hull = ChansConvexHull.ComputeConvexHull(points);

        Console.WriteLine($"Hull contains {hull.Count} vertices:");
        foreach (Point2D vertex in hull)
        {
            Console.WriteLine(vertex);
        }
    }
}
```

---

## 3. Detailed Explanation

Chan's Algorithm proceeds as follows:

1. **Parameter Estimation ($m$):**
   - The algorithm guesses the hull size $m$ using doubly exponential steps: $m = 2^{2^t}$ for $t = 1, 2, 3, \dots$.
2. **Partitioning & Sub-hulls:**
   - Points are divided into $r = \lceil n / m \rceil$ disjoint subsets of size at most $m$.
   - For each subset, its 2D convex mini-hull is computed in $\mathcal{O}(m \log m)$ time using Andrew's Monotone Chain algorithm.
   - Across all $r$ subsets, computing mini-hulls takes $r \cdot \mathcal{O}(m \log m) = \mathcal{O}(n \log m)$ time.
3. **Jarvis March with Tangent Queries:**
   - Starting from the leftmost point, the algorithm executes Jarvis March steps.
   - At each step, instead of examining all $n$ points, it computes the counter-clockwise tangent from the current vertex to each of the $r$ mini-hulls in $\mathcal{O}(\log m)$ time using binary search.
   - The global next hull point is the best tangent among all $r$ mini-hulls, taking $\mathcal{O}(r \log m) = \mathcal{O}((n/m) \log m)$ time per step.
   - In $m$ steps, this phase takes at most $m \cdot \mathcal{O}((n/m) \log m) = \mathcal{O}(n \log m)$ time.
4. **Termination & Iteration:**
   - If the hull wraps back to the start point within $m$ steps, the complete convex hull is returned.
   - If not, $m < h$, so $t$ is incremented ($m \gets 2^{2^{t+1}}$) and the process repeats.

---

## 4. Complexity Analysis

- **Time Complexity:**
  - For a given parameter $m$, the step takes $\mathcal{O}(n \log m)$ time.
  - Total time across iterations: $\sum_{t=1}^{\lceil \log_2 \log_2 h \rceil} \mathcal{O}(n \log 2^{2^t}) = \mathcal{O}(n) \sum_{t=1}^{\lceil \log_2 \log_2 h \rceil} 2^t = \mathcal{O}(n \cdot 2^{\lceil \log_2 \log_2 h \rceil + 1}) = \mathcal{O}(n \log h)$.
  - Worst-Case: $\mathcal{O}(n \log n)$ when $h = \Theta(n)$.
  - Best-Case: $\mathcal{O}(n)$ when $h = \mathcal{O}(1)$.
- **Space Complexity:** $\mathcal{O}(n)$ auxiliary space to store partitions, mini-hulls, and the resulting convex hull points.