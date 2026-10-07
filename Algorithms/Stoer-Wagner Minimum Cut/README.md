# Stoer-Wagner Minimum Cut Algorithm

### 1. Introduction
The Stoer-Wagner algorithm is a deterministic algorithm designed to find the **global minimum cut** in an undirected, weighted graph with non-negative edge weights. 

Unlike traditional s-t min-cut algorithms (such as Edmonds-Karp or Push-Relabel) that require computing maximum flows between all pairs of source-sink vertices ($O(V^4)$ or $O(V \cdot \text{Flow})$), the Stoer-Wagner algorithm calculates the global minimum cut in $O(V^3)$ (or $O(V E + V^2 \log V)$ using Fibonacci Heaps) without computing max-flow.

### 2. Usage

```csharp
using System;
using System.Collections.Generic;
using StoerWagnerAlgorithm;

class Program
{
    static void RunDemo()
    {
        int vertexCount = 4;
        var edges = new List<(int u, int v, double weight)>
        {
            (0, 1, 2.0),
            (1, 2, 3.0),
            (2, 3, 2.0),
            (3, 0, 3.0),
            (0, 2, 1.0),
            (1, 3, 1.0)
        };

        MinCutResult result = StoerWagnerMinCut.ComputeMinCut(vertexCount, edges);

        Console.WriteLine($"Min-Cut Weight: {result.CutWeight}");
        Console.WriteLine($"Partition A: {string.Join(", ", result.PartitionA)}");
        Console.WriteLine($"Partition B: {string.Join(", ", result.PartitionB)}");
    }
}
```

### 3. Detailed Explanation
The algorithm operates in $|V| - 1$ phases. In each phase:
1. **Maximum Adjacency Search (MAS)**: Starting from an arbitrary vertex, vertices are added to an ordered set $A$ one by one based on which vertex outside $A$ is most tightly connected (has the highest sum of edge weights) to vertices already in $A$.
2. **Cut of the Phase**: The cut of the phase is the weight of the edges separating the last added vertex $t$ from all other vertices in the current contracted graph.
3. **Contraction / Merge**: The algorithm records the cut of the phase, compares it to the running minimum, and merges the last two vertices ($s$ and $t$) into a single super-vertex, updating the adjacency matrix.

By keeping track of the merged original vertices for each super-vertex, the algorithm reconstructs the exact subset of original vertices forming partition $A$ and partition $B$.

### 4. Complexity Analysis
- **Time Complexity**:
  - In this dense matrix representation, each Maximum Adjacency Search phase scans all remaining vertices in $O(V^2)$ time.
  - With $V - 1$ phases, the total time complexity is **$O(V^3)$**.
- **Space Complexity**:
  - Storing the adjacency matrix and active vertex sets requires **$O(V^2)$** memory.