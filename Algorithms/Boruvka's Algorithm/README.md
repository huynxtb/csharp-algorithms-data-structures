# Borůvka's Algorithm

### 1. Introduction
Borůvka's algorithm is a greedy algorithm used to find the Minimum Spanning Tree (MST) of a connected, weighted, undirected graph. It is particularly well-suited for parallel processing because components independently search for their cheapest outgoing edges during each step.

### 2. Usage
```csharp
using System;
using Algorithms.Graphs;

class Program
{
    static void Main()
    {
        var graph = new Graph<string, int>();
        graph.AddEdge("A", "B", 4);
        graph.AddEdge("A", "C", 2);
        graph.AddEdge("B", "C", 1);
        graph.AddEdge("B", "D", 5);
        graph.AddEdge("C", "D", 8);
        graph.AddEdge("C", "E", 10);
        graph.AddEdge("D", "E", 2);
        graph.AddEdge("D", "F", 6);
        graph.AddEdge("E", "F", 3);

        var result = BoruvkaSolver.FindMinSpanningTree(graph);

        Console.WriteLine($"Total MST Weight: {result.TotalWeight}");
        foreach (var edge in result.MstEdges)
        {
            Console.WriteLine($"{edge.Source} - {edge.Target}: {edge.Weight}");
        }
    }
}
```

### 3. Detailed Explanation
The algorithm maintains a forest of trees (initially, each vertex is a tree). In each iteration, it finds the cheapest edge leaving each tree to another tree. These edges are added to the MST, merging the trees. The process repeats until only one tree remains containing all vertices.
- **Union-Find (DSU)**: Tracks connected components efficiently.
- **Cheapest Array**: Stores the minimum weight edge leaving each component in the current phase.
- **Generic Support**: Works with any vertex type and any weight type implementing `IComparable<T>`.

### 4. Complexity Analysis
- **Time Complexity**: $O(E \log V)$ where $V$ is the number of vertices and $E$ is the number of edges. The number of components is halved in each step, resulting in at most $O(\log V)$ phases. Each phase inspects all $E$ edges.
- **Space Complexity**: $O(V + E)$ to store the graph representation, Union-Find parent/rank arrays, and the cheapest edge tracking array.