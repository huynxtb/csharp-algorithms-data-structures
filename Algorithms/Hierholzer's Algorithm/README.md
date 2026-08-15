# Hierholzer's Algorithm

## Introduction
Hierholzer's Algorithm finds an Eulerian Path or Eulerian Circuit in a directed graph. An Eulerian Path visits every edge in a graph exactly once. An Eulerian Circuit is an Eulerian Path that starts and ends at the same vertex. This algorithm is useful in DNA fragment assembly, routing problems, and network analysis.

## Usage
```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var finder = new EulerianPathFinder();
        int vertices = 4;
        var edges = new List<Tuple<int, int>>
        {
            Tuple.Create(0, 1),
            Tuple.Create(1, 2),
            Tuple.Create(2, 3),
            Tuple.Create(3, 0)
        };

        List<int> path = finder.FindEulerianPath(vertices, edges);
        Console.WriteLine(string.Join(" -> ", path)); // Outputs: 0 -> 1 -> 2 -> 3 -> 0
    }
}
```

## Detailed Explanation
1. **Degree Validation**: The algorithm first calculates the in-degree and out-degree of all vertices. For a directed graph to contain an Eulerian Path, it must satisfy:
   - At most one vertex has `outDegree - inDegree = 1` (start vertex).
   - At most one vertex has `inDegree - outDegree = 1` (end vertex).
   - All other vertices have `inDegree == outDegree`.
2. **Start Node Selection**: If a unique start vertex exists, the traversal begins there. Otherwise, it starts at the first vertex with an out-degree greater than 0.
3. **Hierholzer's Traversal**: The algorithm performs a modified Depth-First Search (DFS) using a stack. To achieve $O(V + E)$ time complexity, it tracks the current edge index being traversed for each vertex (`outEdgeIndex`), ensuring no edge is scanned or traversed twice.
4. **Connectivity Check**: After generating the path, the algorithm verifies if the path length equals the number of edges plus one. If not, the graph contains disconnected components with edges, and no Eulerian path exists.

## Complexity Analysis
- **Time Complexity**: $O(V + E)$ where $V$ is the number of vertices and $E$ is the number of edges. Each vertex and edge is processed a constant number of times.
- **Space Complexity**: $O(V + E)$ to store the adjacency list, degree arrays, recursion stack, and the final path.