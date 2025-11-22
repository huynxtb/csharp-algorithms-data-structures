# Bellman-Ford Algorithm

## 1. Introduction

The Bellman-Ford algorithm computes shortest paths from a single source vertex to all other vertices in a weighted graph. Unlike Dijkstra's algorithm, Bellman-Ford handles graphs with negative weight edges and can detect negative weight cycles reachable from the source, which makes it particularly useful for graphs that may contain such edges. If a negative cycle exists and is reachable from the source, the algorithm identifies this condition, as shortest path distances would be undefined.

Use this algorithm when you need to find shortest paths in graphs where edges might have negative weights and detecting negative cycles is important.

## 2. Usage

// Create edges of the graph
var edges = new List<Edge>
{
    new Edge(0, 1, 6),
    new Edge(0, 2, 7),
    new Edge(1, 2, 8),
    new Edge(1, 3, 5),
    new Edge(1, 4, -4),
    new Edge(2, 3, -3),
    new Edge(2, 4, 9),
    new Edge(3, 1, -2),
    new Edge(4, 0, 2),
    new Edge(4, 3, 7)
};

// Instantiate BellmanFord
var bellmanFord = new BellmanFord();

try
{
    int[] distances = bellmanFord.FindShortestPaths(5, edges, 0);
    // distances now hold the shortest paths from vertex 0
}
catch (NegativeWeightCycleException ex)
{
    // Handle the negative weight cycle case
}

## 3. Detailed Explanation

- Graph Representation:
  The graph is represented using an edge list, where each edge contains a source vertex, a destination vertex, and a weight.

- Distance Initialization:
  An array stores the shortest known distance to each vertex from the source. It's initialized with `int.MaxValue` (to represent infinity) except the source which is set to 0.

- Relaxation:
  The algorithm performs |V| - 1 iterations, relaxing all edges each time. Relaxation means checking if the current best distance to a vertex can be improved by going through another vertex.

- Negative Cycle Check:
  After the main iterations, the algorithm checks one more time for any edge that can be relaxed further. If any can, it indicates a reachable negative weight cycle, and an exception is thrown.

- Optimization:
  Early termination is applied if in any iteration no distance is updated, indicating no further improvements are possible.

## 4. Complexity Analysis

- Time Complexity: O(V * E) where V is the number of vertices and E is the number of edges. This is because the algorithm relaxes all edges up to |V| - 1 times and performs one additional check for negative cycles.

- Space Complexity: O(V + E) for storing distances and the edge list.

This implementation is clean, reusable, and suited for integration within larger projects requiring shortest path computations and cycle detection in weighted graphs with potential negative edges.