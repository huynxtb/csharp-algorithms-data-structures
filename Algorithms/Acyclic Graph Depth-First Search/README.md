# Introduction
The Acyclic Graph Depth-First Search (DFS) algorithm is used to traverse or search through an acyclic graph. It allows the programmer to explore all vertices and edges without revisiting any vertex. This algorithm is particularly useful in scenarios where you need to explore all possible paths in a directed acyclic graph (DAG).

# Usage
```csharp
Graph graph = new Graph();
graph.AddEdge(1, 2);
graph.AddEdge(1, 3);
graph.AddEdge(2, 4);
graph.AddEdge(3, 4);
List<int> result = graph.DepthFirstSearch(1);
// result will contain the vertices in the order they were visited.
```

# Detailed Explanation
The `Graph` class maintains an adjacency list to represent the directed edges of the graph. The `AddEdge` method allows adding directed edges between vertices. The `DepthFirstSearch` method initiates the DFS traversal from a specified starting vertex. It uses a helper method `DFSUtil` to recursively visit each vertex, marking them as visited to avoid cycles. If a vertex does not exist in the graph, it handles the situation gracefully by simply returning without any action.

# Complexity Analysis
- **Time Complexity:** O(V + E), where V is the number of vertices and E is the number of edges. Each vertex and edge is processed once.
- **Space Complexity:** O(V), for the visited list and the recursion stack in the worst case.