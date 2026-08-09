# Kosaraju's Algorithm

## 1. Introduction
Kosaraju's Algorithm is a linear-time algorithm used to find the Strongly Connected Components (SCCs) of a directed graph. A strongly connected component is a maximal subgraph where every vertex is reachable from any other vertex in the same subgraph. This algorithm is useful in social network analysis, cycle detection, and resolving dependencies in software packaging.

## 2. Usage
Below is an example of how to use the `KosarajuSccSolver` class:

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Create a directed graph with 5 vertices
        List<int>[] graph = new List<int>[5];
        for (int i = 0; i < 5; i++)
        {
            graph[i] = new List<int>();
        }

        // Add edges
        graph[0].Add(2);
        graph[2].Add(1);
        graph[1].Add(0);
        graph[0].Add(3);
        graph[3].Add(4);

        KosarajuSccSolver solver = new KosarajuSccSolver();
        List<List<int>> sccs = solver.GetSccs(graph);

        // Print the components
        foreach (var component in sccs)
        {
            Console.WriteLine("Component: " + string.Join(", ", component));
        }
    }
}
```

## 3. Detailed Explanation
Kosaraju's algorithm operates in three main phases:
1. **First DFS Pass**: Perform a Depth First Search (DFS) on the original graph. As vertices finish processing (i.e., all their neighbors are visited), push them onto a stack. This stack tracks the vertices ordered by their completion times.
2. **Graph Transposition**: Create a transposed version of the graph by reversing the direction of all directed edges.
3. **Second DFS Pass**: Pop vertices from the stack one by one. If a vertex has not been visited, perform a DFS on the transposed graph starting from that vertex. All vertices visited during this DFS form a single Strongly Connected Component.

## 4. Complexity Analysis
- **Time Complexity**: `O(V + E)` where `V` is the number of vertices and `E` is the number of edges. The algorithm performs two complete DFS traversals and one graph transposition, each taking linear time.
- **Space Complexity**: `O(V + E)` to store the transposed graph, the recursion stack for DFS, and the visited tracking array.