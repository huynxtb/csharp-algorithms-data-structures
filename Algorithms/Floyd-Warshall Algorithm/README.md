# Floyd-Warshall Algorithm

## Introduction
The Floyd-Warshall algorithm is a classic dynamic programming method used to find the shortest paths between all pairs of vertices in a weighted directed graph. It handles graphs that may include negative edge weights, as long as there are no negative weight cycles. This algorithm is useful in scenarios requiring the determination of shortest paths among all vertices, such as routing, network analysis, or evaluating path costs in transportation systems.

## Usage
int[][] graph =
{
    new int[] { 0, 3, int.MaxValue, 7 },
    new int[] { 8, 0, 2, int.MaxValue },
    new int[] { 5, int.MaxValue, 0, 1 },
    new int[] { 2, int.MaxValue, int.MaxValue, 0 }
};

// Compute shortest paths
int[][] shortestPaths = FloydWarshall.FindShortestPaths(graph);

// Optional: Check for negative cycles
bool hasNegativeCycle = FloydWarshall.HasNegativeCycle(shortestPaths);

// shortestPaths now contains the minimum distances between every pair of vertices.

## Detailed Explanation
The algorithm initializes a distance matrix `dist` as a copy of the input adjacency matrix. The key insight is to iteratively improve paths by allowing each vertex to act as an intermediate vertex in the paths between all pairs (i, j). For each triple of vertices `(i, j, k)`, the algorithm tests whether the path from `i` to `j` passing through `k` (`i -> k -> j`) is shorter than the current known path. If so, it updates the shortest distance.

Special care is taken to avoid arithmetic overflows by checking for unreachable edges represented by `int.MaxValue`. The diagonal entries of the matrix are initialized to zero, reflecting the zero distance from a vertex to itself.

After completion, the `dist` matrix contains the shortest distances between all vertex pairs. If any diagonal value in the matrix is negative, the graph contains a negative weight cycle.

## Complexity Analysis
- **Time Complexity:** O(V^3), where V is the number of vertices. The triple nested loops each run up to V times.
- **Space Complexity:** O(V^2) for storing the distance matrix.

This complexity makes the Floyd-Warshall algorithm suitable for graphs with moderate vertex counts but less efficient for very large graphs compared to other shortest path algorithms specialized for single-source or single-destination queries.
