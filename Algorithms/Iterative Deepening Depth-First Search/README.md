# Iterative Deepening Depth-First Search (IDDFS)

## 1. Introduction
Iterative Deepening Depth-First Search (IDDFS) is a state-space / graph search algorithm that combines the space-efficiency of Depth-First Search (DFS) with the optimality guarantees of Breadth-First Search (BFS).

In unweighted graphs or uniform step-cost domains, IDDFS visits nodes in breadth-first order while executing bounded depth-first traversals. By progressively increasing the depth limit ($0, 1, 2, \dots$), it ensures finding the shortest path (fewest edges) to the goal state using only linear memory with respect to the search depth.

### When to Use IDDFS:
- When searching large or infinite search spaces where memory is constrained.
- When the shortest/optimal path in terms of edge count is required.
- In game tree exploration (e.g., Chess, Checkers) combined with iterative evaluation.
- When BFS would exhaust available RAM due to large branching factors.

---

## 2. Usage

```csharp
using System;
using System.Collections.Generic;

public class Example
{
    public static void Run()
    {
        // Construct an unweighted graph using an adjacency list
        var graph = new Dictionary<string, List<string>>
        {
            { "A", new List<string> { "B", "C" } },
            { "B", new List<string> { "D", "E" } },
            { "C", new List<string> { "F", "G" } },
            { "D", new List<string>() },
            { "E", new List<string> { "H" } },
            { "F", new List<string>() },
            { "G", new List<string>() },
            { "H", new List<string>() }
        };

        var iddfs = new IterativeDeepeningDFS<string>(graph);

        // Search with a max depth limit of 3
        List<string>? path = iddfs.Search("A", "H", maxDepth: 3);

        if (path != null)
        {
            Console.WriteLine("Path found: " + string.Join(" -> ", path));
            // Output: Path found: A -> B -> E -> H
        }
        else
        {
            Console.WriteLine("Path not found within depth limit.");
        }
    }
}
```

---

## 3. Detailed Explanation

1. **Adjacency Representation**: The graph is ingested as a generic `Dictionary<T, List<T>>` along with an optional custom equality comparer to handle custom structs or class instances.
2. **Iterative Outer Loop**: The `Search` method controls depth expansion from level `0` up to `maxDepth`. At each iteration `d`, a fresh Depth-Limited Search (DLS) is initiated from the `start` node with depth bound `d`.
3. **Depth-Limited Search (DLS)**: The recursive helper function checks if the target `goal` is found. If not and the remaining depth allows, it iterates through all unvisited neighbors along the current path branch.
4. **Cycle Prevention & Backtracking**: A `HashSet<T>` tracks nodes on the *current recursion path*. When exploring a neighbor, it is added to the set and current path. When the recursion unwinds (backtracks), the node is removed from the set and path list. This prevents getting stuck in cyclic graphs while allowing nodes to be revisited along different paths at varying depths.
5. **Path Reconstruction**: Because successful paths are accumulated directly in the recursion stack, the algorithm immediately returns the complete `List<T>` upon first encountering the target at the shallowest valid depth.

---

## 4. Complexity Analysis

Let $b$ be the branching factor and $d$ be the depth of the shallowest goal:

- **Time Complexity**: $\mathcal{O}(b^d)$
  - While nodes at upper levels are visited multiple times across iterations, the geometric progression means the bottom level dominates the runtime: $\sum_{i=0}^d (d - i + 1) b^i = \mathcal{O}(b^d)$. The overhead compared to standard BFS is negligible (typically a factor of $\frac{b}{b-1}$).
- **Space Complexity**: $\mathcal{O}(d)$
  - Only the current path from the root to the frontier is stored in memory, requiring linear auxiliary space proportional to the search depth.