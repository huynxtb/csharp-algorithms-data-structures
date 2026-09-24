# Hungarian Algorithm (Kuhn-Munkres Algorithm)

## 1. Introduction
The **Hungarian Algorithm** (also known as the **Kuhn-Munkres Algorithm**) is a combinatorial optimization algorithm that solves the **assignment problem** (both Maximum Weight Bipartite Matching and Minimum Cost Bipartite Matching) in polynomial time.

It is used to find an optimal one-to-one assignment between two sets of entities (e.g., workers and tasks, agents and resources) such that the total cost is minimized or total profit/weight is maximized. The algorithm operates seamlessly on square ($N \times N$) and rectangular ($N \times M$ where $N \le M$) matrices, handling both dense and sparse (via sentinel values) bipartite graphs.

## 2. Usage

```csharp
using System;
using Algorithms.Graph;

public class Example
{
    public static void Run()
    {
        // Define a weight matrix (Left: 3 workers, Right: 4 jobs)
        double[,] weights = new double[,]
        {
            { 7, 5, 8, 2 },
            { 7, 8, 9, 4 },
            { 3, 1, 4, 6 }
        };

        // Compute Maximum Weight Matching
        HungarianResult maxResult = HungarianAlgorithm.FindMaximumWeightMatching(weights);

        Console.WriteLine($"Feasible: {maxResult.IsFeasible}");
        Console.WriteLine($"Total Max Weight: {maxResult.TotalWeight}");
        for (int i = 0; i < maxResult.LeftAssignment.Count; i++)
        {
            Console.WriteLine($"Worker {i} -> Job {maxResult.LeftAssignment[i]}");
        }

        // Compute Minimum Cost Matching
        double[,] costs = new double[,]
        {
            { 10, 19, 8, 15 },
            { 10, 18, 7, 17 },
            { 13, 16, 9, 14 }
        };

        HungarianResult minResult = HungarianAlgorithm.FindMinimumCostMatching(costs);
        Console.WriteLine($"Total Min Cost: {minResult.TotalWeight}");
    }
}
```

## 3. Detailed Explanation
The implementation uses the modernized, pot-based formulation of the Hungarian algorithm with potential dual vectors $u$ and $v$:
1. **Dual Potentials & Slack Tracking**: Dual potentials $u[i]$ and $v[j]$ maintain the invariant $u[i] + v[j] \le C_{i, j}$ where $C$ is the cost matrix. The reduced cost of an edge is defined as $\text{slack}(i, j) = C_{i, j} - u[i] - v[j]$.
2. **Augmenting Paths with Shortest Path Search**: For each row $i$, an alternating augmenting tree is grown in $O(M)$ steps. A 1D array `minv` maintains the minimum slack to unvisited columns, avoiding an $O(NM)$ rescan in each step.
3. **Potential Updates**: When reaching a tight edge, potentials are updated uniformly with $\delta = \min_{j \notin Z} \text{slack}(j)$, tightening the dual bounds and creating new admissible edges.
4. **Augmentation**: Once an unassigned column is reached, assignments along the augmenting path are reversed using the predecessor array `way`.

## 4. Complexity Analysis
- **Time Complexity**:
  - Outer loop runs $N$ times (one per row).
  - Inner path finding performs at most $M$ potential updates, each taking $O(M)$ time using the `minv` slack array.
  - Overall Time Complexity: **$O(N^2 M)$** (or **$O(V^3)$** for square matrices where $N = M$).
- **Space Complexity**:
  - Auxiliary 1D arrays for potentials and alternating trees: $u$, $v$, $p$, $way$, $minv$, $used$.
  - Overall Space Complexity: **$O(N + M)$** additional space beyond the input matrix.