# Tarjan's Bridge and Articulation Point Finding Algorithm

### 1. Introduction
Tarjan's algorithm identifies critical connections (bridges) and critical nodes (articulation points) in an undirected graph. A bridge is an edge whose removal increases the number of connected components. An articulation point (or cut vertex) is a vertex whose removal increases the number of connected components. This algorithm is widely used in network reliability analysis and vulnerability assessment.

### 2. Usage
```csharp
using System;
using System.Collections.Generic;
using GraphAlgorithms;

class Program
{
    static void Main()
    {
        var graph = new Dictionary<int, List<int>>
        {
            { 0, new List<int> { 1, 2 } },
            { 1, new List<int> { 0, 2 } },
            { 2, new List<int> { 0, 1, 3 } },
            { 3, new List<int> { 2, 4 } },
            { 4, new List<int> { 3 } }
        };

        AnalysisResult result = UndirectedGraphAnalyzer.Analyze(graph);

        Console.WriteLine("Bridges:");
        foreach (var bridge in result.Bridges)
        {
            Console.WriteLine($"({bridge.Item1}, {bridge.Item2})");
        }

        Console.WriteLine("Articulation Points:");
        foreach (var point in result.ArticulationPoints)
        {
            Console.WriteLine(point);
        }
    }
}
```

### 3. Detailed Explanation
The algorithm performs a single Depth-First Search (DFS) traversal. It tracks two main values for each vertex:
- `tin[u]`: The discovery time of vertex `u`.
- `low[u]`: The lowest discovery time reachable from `u` using at most one back-edge.

During DFS backtracking:
- **Bridge Condition**: An edge `(u, v)` is a bridge if and only if `low[v] > tin[u]`. This means there is no back-edge from the subtree rooted at `v` to `u` or any ancestor of `u`.
- **Articulation Point Condition**:
  - If `u` is not the root of the DFS tree, it is an articulation point if it has a child `v` such that `low[v] >= tin[u]`.
  - If `u` is the root of the DFS tree, it is an articulation point if it has more than one child in the DFS tree.

To handle parallel edges correctly, the algorithm tracks whether the edge back to the parent has already been traversed. Self-loops are ignored.

### 4. Complexity Analysis
- **Time Complexity**: $O(V + E)$ where $V$ is the number of vertices and $E$ is the number of edges. Each vertex and edge is visited a constant number of times.
- **Space Complexity**: $O(V + E)$ to store the graph representation, recursion stack, and tracking structures (`tin`, `low`, `visited`).