# Adjacency Matrix Graph

### 1. Introduction
An `AdjacencyMatrixGraph<TVertex>` is a generic, undirected graph implementation that uses a two-dimensional boolean matrix to represent connections between vertices. This structure is ideal for dense graphs where the number of edges is close to the maximum possible number of edges, and for scenarios requiring fast edge lookups.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        var graph = new AdjacencyMatrixGraph<string>(5);

        graph.AddVertex("A");
        graph.AddVertex("B");
        graph.AddVertex("C");

        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");

        Console.WriteLine($"Has edge A-B: {graph.HasEdge("A", "B")}"); // True
        Console.WriteLine($"Has edge A-C: {graph.HasEdge("A", "C")}"); // False

        Console.WriteLine("Neighbors of B:");
        foreach (var neighbor in graph.GetNeighbors("B"))
        { 
            Console.WriteLine(neighbor); // A, C
        }
    }
}
```

### 3. Detailed Explanation
* **Internal Storage**: The graph uses a 2D boolean array `bool[,]` where `_adjacencyMatrix[i, j] = true` indicates an edge between vertex `i` and vertex `j`.
* **Vertex Mapping**: A `Dictionary<TVertex, int>` maps generic vertex identifiers to internal integer indices. A corresponding array `TVertex[]` maps indices back to vertex identifiers.
* **Undirected Nature**: When an edge is added or removed, both `_adjacencyMatrix[i, j]` and `_adjacencyMatrix[j, i]` are updated to maintain symmetry.

### 4. Complexity Analysis
* **Time Complexity**:
  * `AddVertex`: $O(1)$ amortized lookup and assignment.
  * `AddEdge` / `RemoveEdge` / `HasEdge`: $O(1)$ direct array access.
  * `GetNeighbors`: $O(V)$ where $V$ is the current number of vertices, as it must scan the row corresponding to the vertex.
* **Space Complexity**: $O(V^2)$ where $V$ is the capacity of the graph, due to the 2D matrix allocation.