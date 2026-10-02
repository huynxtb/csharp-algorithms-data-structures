# Dijkstra's Shortest Path Algorithm with Priority Queue

## 1. Introduction
Dijkstra's algorithm is an optimal single-source shortest path algorithm designed for weighted graphs where edge weights are non-negative. It operates similarly to an optimized Breadth-First Search (BFS) by expanding outward based on accumulated path weight rather than edge count (hop count).

Key applications include:
- Routing protocols in computer networking (e.g., OSPF, IS-IS)
- Mapping and navigation services for finding the shortest transit routes
- Resource allocation and dependency resolution pipelines

## 2. Usage

```csharp
// Instantiate the graph
var graph = new Graph();

// Add weighted directed edges (vertices are registered automatically)
graph.AddEdge(1, 2, 4);
graph.AddEdge(1, 3, 2);
graph.AddEdge(3, 2, 1);
graph.AddEdge(2, 4, 5);
graph.AddEdge(3, 4, 8);

// Create pathfinder
var pathFinder = new DijkstraPathFinder(graph);

// Get shortest distances from source node 1
Dictionary<int, int> distances = pathFinder.FindShortestDistances(1);
// distances[4] will be 8 (1 -> 3 -> 2 -> 4 with total cost 2 + 1 + 5 = 8)

// Retrieve shortest path from 1 to 4
List<int> path = pathFinder.GetShortestPath(1, 4);
// path contains: [1, 3, 2, 4]
```

## 3. Detailed Explanation
1. **Graph Representation**: The graph uses an adjacency list where each vertex maps to a collection of outgoing weighted edges.
2. **Min-Priority Queue**: A custom binary min-heap tracks vertices ordered by their currently discovered shortest distance from the source.
3. **Relaxation**: For the extracted vertex $u$ with minimum tentative distance, all outgoing edges $(u, v)$ with weight $w$ are inspected. If `dist[u] + w < dist[v]`, the distance to $v$ is updated, the predecessor for $v$ is set to $u$, and $(v, \text{newDistance})$ is inserted into the min-heap.
4. **Path Reconstruction**: Backtracks through recorded predecessor vertices starting from the destination up to the source vertex, then reverses the collected vertices.

## 4. Complexity Analysis

- **Time Complexity**:
  - **Graph Construction**: $O(1)$ per edge insertion.
  - **Shortest Path Computation**: $O((V + E) \log V)$ where $V$ is the number of vertices and $E$ is the number of edges, as each edge triggers at most one enqueue operation into a binary heap of size at most $V$.
  - **Path Reconstruction**: $O(L)$ where $L \le V$ is the number of vertices in the final path.
- **Space Complexity**:
  - **Adjacency List**: $O(V + E)$ to store graph vertices and edges.
  - **Priority Queue & Auxiliary State**: $O(V)$ auxiliary memory for tracking distances, predecessors, and heap elements.