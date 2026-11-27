# Introduction
This implementation provides a Depth-First Search (DFS) algorithm specifically designed for traversing a Directed Acyclic Graph (DAG). It allows for the exploration of all vertices in a DAG starting from a specified vertex, ensuring that vertices are not revisited.

# Usage
To use the DAG class, create an instance, add edges, and call the DFS method:
```csharp
DAG graph = new DAG();
graph.AddEdge(1, 2);
graph.AddEdge(1, 3);
graph.AddEdge(2, 4);
graph.AddEdge(3, 4);
List<int> visitedVertices = graph.DFS(1);
// visitedVertices will contain the order of visited vertices starting from vertex 1.
```

# Detailed Explanation
The DAG class encapsulates the graph's functionality using an adjacency list representation stored in a Dictionary. The `AddEdge` method allows adding directed edges between vertices. The `DFS` method initiates the depth-first search from a specified vertex, utilizing a helper method `DFSUtil` to recursively visit each vertex and its neighbors, marking them as visited to prevent cycles.

# Complexity Analysis
- **Time Complexity:** O(V + E), where V is the number of vertices and E is the number of edges, as each vertex and edge is processed once.
- **Space Complexity:** O(V), for the storage of the adjacency list and the visited set.