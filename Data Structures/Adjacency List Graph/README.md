# Adjacency List Graph

### 1. Introduction
An Adjacency List Graph is a graph representation where each vertex stores a collection of its adjacent vertices (neighbors). This representation is highly efficient for sparse graphs, where the number of edges is much less than the maximum possible number of edges. It allows for fast neighbor lookups and iteration over all neighbors of a given vertex.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        // Create an undirected graph
        var graph = new AdjacencyListGraph<string>(isDirected: false);

        // Add vertices
        graph.AddVertex("A");
        graph.AddVertex("B");
        graph.AddVertex("C");

        // Add edges
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");

        // Query graph properties
        Console.WriteLine($"Vertices: {graph.VertexCount}"); // Output: 3
        Console.WriteLine($"Edges: {graph.EdgeCount}");       // Output: 2
        Console.WriteLine($"Has Edge A-B: {graph.HasEdge("A", "B")}"); // Output: True

        // Get neighbors of B
        foreach (var neighbor in graph.GetNeighbors("B"))
        {
            Console.WriteLine($"Neighbor of B: {neighbor}"); // Output: A, C
        }
    }
}
```

### 3. Detailed Explanation
This implementation uses a `Dictionary<TVertex, HashSet<TVertex>>` to store the graph structure. 
- **Vertices**: Each vertex is stored as a key in the dictionary. The `HashSet<TVertex>` value contains all vertices connected to it by an outgoing edge.
- **Directed vs. Undirected**: The graph's behavior is configured at instantiation. For undirected graphs, adding or removing an edge automatically updates the adjacency sets of both the source and destination vertices. For directed graphs, only the source vertex's adjacency set is modified.
- **Self-Loops**: The implementation supports self-loops (an edge from a vertex to itself) and ensures that edge counts remain accurate without causing infinite loops or duplicate modifications.

### 4. Complexity Analysis
- **Space Complexity**: $O(V + E)$ where $V$ is the number of vertices and $E$ is the number of edges.
- **Time Complexity**:
  - **Add Vertex**: $O(1)$ average time.
  - **Remove Vertex**: $O(V + E)$ for directed graphs (must search all adjacency lists to remove incoming edges) and $O(d)$ for undirected graphs where $d$ is the degree of the vertex.
  - **Add Edge**: $O(1)$ average time.
  - **Remove Edge**: $O(1)$ average time.
  - **Has Vertex**: $O(1)$ average time.
  - **Has Edge**: $O(1)$ average time.
  - **Get Neighbors**: $O(1)$ to retrieve the collection.