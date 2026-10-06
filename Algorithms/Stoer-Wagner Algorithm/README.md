# Stoer-Wagner Algorithm for Global Minimum Cut

## Introduction

The Stoer-Wagner algorithm is an efficient algorithm for finding the global minimum cut in an undirected, weighted graph. A minimum cut is a partition of the graph's vertices into two disjoint sets such that the sum of the weights of the edges connecting vertices in different sets is minimized. This algorithm is particularly useful in applications like image segmentation, network reliability analysis, and clustering.

It works by repeatedly finding the most tightly connected pair of vertices in the current graph, performing a cut-of-the-phase, and then merging these two vertices. This process continues until only one vertex remains, and the minimum cut found across all phases is the global minimum cut.

## Usage

To use the Stoer-Wagner algorithm, you need to provide the number of vertices and a collection of edges with their weights. The vertices are assumed to be labeled from 0 to `numVertices - 1`.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class Example
{
    public static void Main(string[] args)
    {
        // Example graph with 6 vertices and weighted edges
        int numVertices = 6;
        var edges = new List<(int u, int v, double weight)>
        {
            (0, 1, 2.0), (0, 2, 3.0), (1, 2, 1.0), (1, 3, 4.0),
            (2, 4, 5.0), (3, 4, 1.0), (3, 5, 2.0), (4, 5, 3.0)
        };

        try
        {
            // Find the global minimum cut
            GlobalMinCutResult result = StoerWagner.FindGlobalMinCut(numVertices, edges);

            Console.WriteLine($"Minimum Cut Weight: {result.MinCutWeight}");
            Console.WriteLine("Partition A:");
            Console.WriteLine(string.Join(", ", result.PartitionA.OrderBy(v => v)));
            Console.WriteLine("Partition B:");
            Console.WriteLine(string.Join(", ", result.PartitionB.OrderBy(v => v)));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Assume StoerWagner class and GlobalMinCutResult record are defined here or in a separate file.
// For a single-file implementation, they would be in the same file.

/*
// Placeholder for the StoerWagner class and GlobalMinCutResult record if not included in the same scope:
public record GlobalMinCutResult
{
    public double MinCutWeight { get; init; }
    public IReadOnlyCollection<int> PartitionA { get; init; }
    public IReadOnlyCollection<int> PartitionB { get; init; }

    public GlobalMinCutResult(double minCutWeight, IReadOnlyCollection<int> partitionA, IReadOnlyCollection<int> partitionB)
    {
        MinCutWeight = minCutWeight;
        PartitionA = partitionA ?? throw new ArgumentNullException(nameof(partitionA));
        PartitionB = partitionB ?? throw new ArgumentNullException(nameof(partitionB));
    }
}

public static class StoerWagner
{
    public static GlobalMinCutResult FindGlobalMinCut(int numVertices, IEnumerable<(int u, int v, double weight)> edges)
    {
        // ... implementation details ...
        throw new NotImplementedException();
    }
    // ... other private methods ...
}
*/
```

## Detailed Explanation

The implementation consists of two main parts: the `StoerWagner` class for the overall algorithm and the `GlobalMinCutResult` record to hold the output.

1.  **`GlobalMinCutResult` Record**: This is a simple data structure to encapsulate the outcome: the `MinCutWeight` and the two sets of vertices forming the partition (`PartitionA` and `PartitionB`).

2.  **`StoerWagner` Class**: This static class contains the core logic.
    *   **`FindGlobalMinCut(int numVertices, IEnumerable<(int u, int v, double weight)> edges)`**: This is the public entry point.
        *   **Initialization**: It first validates input (at least 2 vertices, non-negative weights). It then initializes an adjacency matrix (`adjMatrix`) to store edge weights and a list of `HashSet<int>` (`vertexSets`) to keep track of which original vertices are merged into each current "super-vertex". Initially, each super-vertex represents a single original vertex.
        *   **Main Loop**: The algorithm iterates as long as there is more than one active vertex (`activeVertices.Count > 1`). In each iteration:
            *   **`MinimumCutPhase`**: It calls `MinimumCutPhase` to find the most tightly connected vertex pair in the current contracted graph. This phase uses a process similar to Prim's algorithm for MST or Dijkstra's algorithm for shortest paths, but instead of minimizing distance, it maximizes the connection weight to the growing set of vertices.
            *   **Update Global Min Cut**: The cut weight found in the `MinimumCutPhase` is compared with the current `minCutWeight`. If it's smaller, the global minimum is updated, and the corresponding partitions are stored.
            *   **Graph Contraction**: The last two vertices identified in the `MinimumCutPhase` (let's call them `u` and `v`) are merged. Vertex `v` is merged into `u`. This involves updating the `adjMatrix` by adding `v`'s edge weights to `u`'s, effectively consolidating `v`'s connections into `u`. Vertex `v` is then removed from the `activeVertices` set, and its corresponding `vertexSets` entry is merged into `u`'s.
        *   **Result**: After the loop finishes (when only one super-vertex remains), the `minCutWeight` and the corresponding partitions (`minCutPartitionA`, `minCutPartitionB`) are returned.
        *   **Disconnected Graphs**: The implementation includes a basic check for disconnected graphs where the min cut is 0. If `minCutWeight` remains `double.PositiveInfinity` after processing, it implies no edges were processed, suggesting a disconnected graph with no edges, and a 0-weight cut is returned.

    *   **`MinimumCutPhaseResult` Record**: A helper record to return the results of a single phase: the cut weight of the phase and the last two vertices added.

    *   **`MinimumCutPhase(List<List<double>> adjMatrix, List<HashSet<int>> vertexSets, HashSet<int> activeVertices)`**: This private method performs the Maximum Adjacency Search.
        *   It initializes `weights` to track the sum of edge weights connecting each vertex to the growing set and `added` to mark vertices already included in the set.
        *   It starts with an arbitrary active vertex and iteratively adds the vertex that has the maximum total weight connection to the current set of added vertices. This process continues until all active vertices (except one) have been added.
        *   The weight of the cut-of-the-phase is the sum of weights connecting the *last* vertex added to the rest of the set.

## Complexity Analysis

*   **Time Complexity**: The Stoer-Wagner algorithm performs $N-1$ phases, where $N$ is the number of vertices.
    *   Each `MinimumCutPhase` involves a process similar to Prim's algorithm. In each phase, we iterate through the active vertices to find the one with the maximum connection weight. This takes $O(V^2)$ time in the adjacency matrix representation, where $V$ is the number of *currently active* vertices. Since $V$ decreases from $N$ to 2, the total time for all phases is approximately $O(N^3)$.
    *   Graph contraction (merging vertices) involves updating the adjacency matrix, which takes $O(N)$ time per phase.
    *   Therefore, the overall time complexity is dominated by the phases, resulting in **$O(N^3)$**. Using a Fibonacci heap for the priority queue in `MinimumCutPhase` can improve this to $O(NM + N^2 \log N)$, where $M$ is the number of edges, but the adjacency matrix implementation is simpler and often sufficient for dense graphs.

*   **Space Complexity**: The algorithm requires space for:
    *   The adjacency matrix: $O(N^2)$.
    *   `vertexSets`: $O(N^2)$ in the worst case if many vertices are merged, but typically $O(N)$ if we consider the total number of original vertices stored.
    *   Auxiliary arrays within `MinimumCutPhase`: $O(N)$.
    *   The result: $O(N)$ for storing the partitions.
    *   Overall space complexity is **$O(N^2)$** due to the adjacency matrix.
