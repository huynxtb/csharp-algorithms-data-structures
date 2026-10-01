# Depth-First Search (DFS) Recursive Traversal

## 1. Introduction
Depth-First Search (DFS) is a fundamental graph traversal algorithm that explores as deep as possible along each branch before backtracking. It is widely used in algorithms for topological sorting, cycle detection, connected component identification, pathfinding, and solving mazes/puzzles.

This implementation provides a reusable C# `Graph` class represented via an adjacency list (`Dictionary<int, List<int>>`) supporting both directed and undirected graphs, self-loops, disconnected components, and isolated vertices.

## 2. Usage

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        // Create an undirected graph
        var graph = new Graph(isDirected: false);

        // Add edges
        graph.AddEdge(0, 1);
        graph.AddEdge(0, 2);
        graph.AddEdge(1, 3);
        graph.AddEdge(2, 4);

        // Add an isolated vertex (separate component)
        graph.AddVertex(5);

        // Perform DFS from a specific start vertex
        List<int> componentDfs = graph.DFSRecursive(0);
        Console.WriteLine("DFS starting from 0: " + string.Join(", ", componentDfs));

        // Perform DFS across all components
        List<int> allDfs = graph.DFSRecursiveAll();
        Console.WriteLine("Full DFS across all components: " + string.Join(", ", allDfs));
    }
}
```

## 3. Detailed Explanation

- **Adjacency List Representation**: The graph stores vertices as keys in a dictionary mapping to lists of adjacent neighbor vertices. This allows fast lookups and variable degree representations.
- **Configurable Directionality**: Specified via the constructor flag `isDirected`. When undirected, adding an edge `(u, v)` also automatically registers edge `(v, u)` unless it is a self-loop `u == v`.
- **Single Component Traversal (`DFSRecursive`)**: Validates the presence of the start vertex and recursively explores all unvisited neighbors depth-first.
- **Full Graph Traversal (`DFSRecursiveAll`)**: Iterates through all vertices in the graph, initiating a DFS from any vertex that has not yet been visited. This ensures full traversal coverage across disconnected graphs and isolated vertices.
- **Cycle & Duplicate Prevention**: A `HashSet<int>` maintains the set of visited vertices to avoid infinite recursion on cyclic structures and self-loops.

## 4. Complexity Analysis

Let $V$ be the number of vertices and $E$ be the number of edges in the graph:

- **Time Complexity**:
  - `AddVertex`: $\mathcal{O}(1)$ average time.
  - `AddEdge`: $\mathcal{O}(1)$ average time.
  - `DFSRecursive(startVertex)`: $\mathcal{O}(V' + E')$ where $V'$ and $E'$ are vertices and edges in the reachable component.
  - `DFSRecursiveAll()`: $\mathcal{O}(V + E)$ since every vertex and edge is processed at most once.

- **Space Complexity**:
  - Graph Storage: $\mathcal{O}(V + E)$ for vertices and adjacency lists.
  - `DFSRecursive` / `DFSRecursiveAll`: $\mathcal{O}(V)$ auxiliary space for the recursion call stack in the worst case (e.g., linear/path graphs) and the `HashSet<int>` tracking visited vertices.