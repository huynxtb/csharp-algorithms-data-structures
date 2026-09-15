# Johnson's Algorithm for All-Pairs Shortest Paths

## 1. Introduction
Johnson's Algorithm is an efficient algorithm used to find the shortest paths between all pairs of vertices in a directed, weighted graph. It is particularly useful for sparse graphs that may contain negative edge weights, provided there are no negative-weight cycles. Unlike running Bellman-Ford V times (which would be `O(V^2 * E)`), Johnson's algorithm leverages the strengths of both Bellman-Ford and Dijkstra's algorithms to achieve better performance on sparse graphs.

## 2. Usage
To use the `JohnsonAlgorithm` class, instantiate it with the number of vertices in your graph. Then, add edges using the `AddEdge` method, specifying the source vertex, destination vertex, and weight. Finally, call `ComputeAllPairsShortestPaths` to get a 2D array representing the shortest path distances between all pairs of vertices. If the graph contains a negative-weight cycle reachable from the virtual source, an `InvalidOperationException` will be thrown.

```csharp
using System;
using System.Collections.Generic;

// Assume Edge, MinHeap, and JohnsonAlgorithm classes are defined as provided.

public class Example
{
    public static void Main(string[] args)
    {
        int numVertices = 4;
        JohnsonAlgorithm graph = new JohnsonAlgorithm(numVertices);

        // Add edges: u, v, weight
        graph.AddEdge(0, 1, 3);
        graph.AddEdge(0, 2, 8);
        graph.AddEdge(0, 3, -4);
        graph.AddEdge(1, 3, 1);
        graph.AddEdge(1, 2, 4);
        graph.AddEdge(2, 1, -5); // Negative edge
        graph.AddEdge(3, 2, 2);

        try
        {
            double[,] shortestPaths = graph.ComputeAllPairsShortestPaths();

            Console.WriteLine("All-Pairs Shortest Paths:");
            for (int i = 0; i < numVertices; i++)
            {
                for (int j = 0; j < numVertices; j++)
                {
                    if (shortestPaths[i, j] == double.PositiveInfinity)
                    {
                        Console.Write("INF\t");
                    }
                    else
                    {
                        Console.Write($"{shortestPaths[i, j]:F2}\t");
                    }
                }
                Console.WriteLine();
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Example with a negative cycle (uncomment to test)
        // Console.WriteLine("\nTesting with a negative cycle:");
        // JohnsonAlgorithm graphWithCycle = new JohnsonAlgorithm(3);
        // graphWithCycle.AddEdge(0, 1, 1);
        // graphWithCycle.AddEdge(1, 2, 1);
        // graphWithCycle.AddEdge(2, 0, -3); // Negative cycle: 0 -> 1 -> 2 -> 0 with total weight -1
        // try
        // {
        //     graphWithCycle.ComputeAllPairsShortestPaths();
        // }
        // catch (InvalidOperationException ex)
        // {
        //     Console.WriteLine($"Error: {ex.Message}");
        // }
    }
}
```

## 3. Detailed Explanation
Johnson's Algorithm works in four main steps:

1.  **Bellman-Ford for Reweighting Potentials (`h`)**: A new virtual source vertex `s` is added to the graph, with zero-weight edges connecting `s` to all original vertices. The Bellman-Ford algorithm is then run from `s` to compute the shortest path distances `h(v)` from `s` to every vertex `v` in the original graph. If Bellman-Ford detects a negative-weight cycle reachable from `s`, it indicates that the original graph contains a negative cycle, and an `InvalidOperationException` is thrown. The `h(v)` values serve as vertex potentials.

2.  **Reweighting Edges**: All edges `(u, v)` in the original graph are reweighted using the formula: `w'(u, v) = w(u, v) + h(u) - h(v)`. This reweighting ensures that all edge weights `w'(u, v)` become non-negative, while preserving the shortest path property. Specifically, if `P` is a path from `x` to `y`, its reweighted length `w'(P)` will be `w(P) + h(x) - h(y)`. This means that the shortest path in the reweighted graph corresponds to the shortest path in the original graph.

3.  **Dijkstra's Algorithm from Each Vertex**: Since the reweighted graph `G'` has no negative edge weights, Dijkstra's algorithm can be safely applied. Dijkstra's is run from each original vertex `u` as a source on `G'` to compute the shortest path distances `d'(u, v)` for all `v`.

4.  **Distance Conversion**: The shortest path distances `d'(u, v)` computed by Dijkstra's on the reweighted graph are converted back to the original graph's distances using the formula: `d(u, v) = d'(u, v) - h(u) + h(v)`. This restores the true shortest path distances in the original graph.

**Data Structures**: The graph is represented using an adjacency list (`List<List<Edge>>`). Dijkstra's algorithm utilizes a binary min-heap (`MinHeap`) to efficiently extract the vertex with the smallest distance.

## 4. Complexity Analysis
Let `V` be the number of vertices and `E` be the number of edges in the graph.

*   **Time Complexity**:
    *   **Bellman-Ford**: `O(V * E)` for computing potentials `h` and detecting negative cycles.
    *   **Reweighting**: `O(E)` to iterate through all edges and update their weights.
    *   **Dijkstra's (V times)**: Each run of Dijkstra's algorithm with a binary min-heap takes `O(E log V)` time. Since it's run `V` times (once for each source vertex), this step takes `O(V * E log V)`.
    *   **Total Time Complexity**: `O(V * E + V * E log V) = O(V * E log V)`. This is efficient for sparse graphs where `E` is close to `V`. For dense graphs where `E` is closer to `V^2`, the complexity approaches `O(V^3 log V)`, which can be less efficient than the Floyd-Warshall algorithm (`O(V^3)`).

*   **Space Complexity**:
    *   **Graph Representation**: `O(V + E)` for storing the adjacency list of the original graph and the reweighted graph.
    *   **Bellman-Ford Potentials (`h`)**: `O(V)` for the array of potentials.
    *   **Dijkstra's Priority Queue**: `O(V)` in the worst case (all vertices in the queue).
    *   **Result Matrix**: `O(V^2)` for storing the final all-pairs shortest path distances.
    *   **Total Space Complexity**: `O(V^2)` due to the output matrix, assuming the graph itself is not denser than `V^2` edges.