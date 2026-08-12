# Dinic's Algorithm

## Introduction
Dinic's algorithm is a strongly polynomial algorithm for computing the maximum flow in a flow network. It improves upon the Edmonds-Karp algorithm by using level graphs and finding blocking flows. It is highly efficient and widely used in network routing, bipartite matching, and image segmentation.

## Usage
```csharp
// Create a flow network with 6 vertices (0 to 5)
var dinic = new DinicMaxFlow(6);

// Add edges: AddEdge(from, to, capacity)
dinic.AddEdge(0, 1, 10);
dinic.AddEdge(0, 2, 10);
dinic.AddEdge(1, 2, 2);
dinic.AddEdge(1, 3, 4);
dinic.AddEdge(1, 4, 8);
dinic.AddEdge(2, 4, 9);
dinic.AddEdge(3, 5, 10);
dinic.AddEdge(4, 3, 6);
dinic.AddEdge(4, 5, 10);

// Compute max flow from source (0) to sink (5)
long maxFlow = dinic.ComputeMaxFlow(0, 5);
Console.WriteLine($"Maximum Flow: {maxFlow}"); // Output: 19
```

## Detailed Explanation
The algorithm operates in phases:
1. **Level Graph Construction (BFS):** A Breadth-First Search is run from the source to assign levels to all reachable vertices. The level of a vertex is its shortest distance from the source in terms of edge count. If the sink cannot be reached, the algorithm terminates.
2. **Blocking Flow Finding (DFS):** A Depth-First Search is run from the source to the sink, sending flow only along edges that go from level `i` to level `i + 1`. 
3. **Current Arc Heuristic:** To avoid scanning edges that cannot carry more flow, a pointer array `ptr` tracks the next edge to explore for each vertex. This prevents re-evaluating saturated or dead-end paths within the same phase.

## Complexity Analysis
- **Time Complexity:** 
  - General networks: $O(V^2 E)$ where $V$ is the number of vertices and $E$ is the number of edges.
  - Unit networks (e.g., bipartite matching): $O(E \sqrt{V})$.
- **Space Complexity:** $O(V + E)$ to store the adjacency list, levels, and work pointers.