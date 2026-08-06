# Prim's Algorithm for Minimum Spanning Tree

## 1. Introduction
Prim's Algorithm is a greedy algorithm used to find a Minimum Spanning Tree (MST) for a connected, weighted, undirected graph. An MST is a subset of the edges of a connected, edge-weighted undirected graph that connects all the vertices together, without any cycles and with the minimum possible total edge weight. Prim's algorithm starts from an arbitrary vertex and grows the MST by iteratively adding the cheapest edge that connects a vertex already in the MST to a vertex outside the MST.

**When to use it:**
*   When you need to connect all components of a network with the minimum possible cost (e.g., laying cables, designing pipelines).
*   It is particularly efficient for dense graphs (graphs with many edges).
*   It can be used as a subroutine in other algorithms, such as approximating the Traveling Salesperson Problem.

## 2. Usage
To use the Prim's Algorithm implementation, you first need to create a `Graph` instance, add edges to it, and then call the `PrimsAlgorithm.FindMinimumSpanningTree` static method. The method returns a `PrimsResult` struct containing the MST edges, their total weight, and a flag indicating if the graph was connected.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

// Assume Edge, Graph, PrimsResult, and PrimsAlgorithm classes are defined as above.

public class Example
{
    public static void Run()
    {
        // Create a graph with 5 vertices
        Graph graph = new Graph(5);

        // Add edges (u, v, weight)
        graph.AddEdge(0, 1, 2);
        graph.AddEdge(0, 3, 6);
        graph.AddEdge(1, 2, 3);
        graph.AddEdge(1, 3, 8);
        graph.AddEdge(1, 4, 5);
        graph.AddEdge(2, 4, 7);
        graph.AddEdge(3, 4, 9);

        // Find the MST starting from vertex 0
        PrimsResult result = PrimsAlgorithm.FindMinimumSpanningTree(graph, 0);

        if (result.IsConnected)
        {
            Console.WriteLine("MST found:");
            foreach (Edge edge in result.MstEdges)
            {
                Console.WriteLine($"  {edge.Source} - {edge.Destination} (Weight: {edge.Weight})");
            }
            Console.WriteLine($"Total MST Weight: {result.TotalWeight}");
        }
        else
        {
            Console.WriteLine("Graph is disconnected. MST could not span all vertices.");
        }

        // Example of a disconnected graph
        Graph disconnectedGraph = new Graph(4);
        disconnectedGraph.AddEdge(0, 1, 1);
        disconnectedGraph.AddEdge(2, 3, 1);

        PrimsResult disconnectedResult = PrimsAlgorithm.FindMinimumSpanningTree(disconnectedGraph, 0);
        if (!disconnectedResult.IsConnected)
        {
            Console.WriteLine("\nDisconnected graph example:");
            Console.WriteLine("Graph is disconnected. MST could not span all vertices.");
        }
    }
}
```

## 3. Detailed Explanation

This implementation of Prim's Algorithm uses an adjacency list to represent the graph and a `System.Collections.Generic.PriorityQueue<TElement, TPriority>` to efficiently select the minimum weight edge.

1.  **Graph Representation (`Graph` class):**
    *   The graph is represented using an array of `List<Edge>`. `AdjacencyList[i]` contains all `Edge` objects connected to vertex `i`.
    *   The `AddEdge` method adds an edge in both directions (`u` to `v` and `v` to `u`) to correctly model an undirected graph.

2.  **Edge Representation (`Edge` class):**
    *   A simple class storing `Source`, `Destination`, and `Weight` of an edge.

3.  **Algorithm (`PrimsAlgorithm.FindMinimumSpanningTree` method):**
    *   **Initialization:**
        *   `numVertices`: Total number of vertices in the graph.
        *   `minHeap`: A `PriorityQueue` stores `Edge` objects, with their `Weight` as the priority. This ensures that the edge with the smallest weight is always at the top.
        *   `inMST`: A boolean array, `inMST[i]` is `true` if vertex `i` has been included in the MST, `false` otherwise. This prevents cycles and redundant processing.
        *   `mstEdges`: A `List<Edge>` to store the edges that form the MST.
        *   `totalWeight`: A `long` to accumulate the sum of weights of edges in the MST.
    *   **Starting Point:** The algorithm begins by marking the `startVertex` as `inMST` and adding all edges connected to it into the `minHeap`.
    *   **Main Loop:** The algorithm continues as long as there are edges in the `minHeap` and the `mstEdges` count is less than `numVertices - 1` (an MST for `V` vertices has `V-1` edges).
        1.  **Extract Minimum:** The edge with the smallest weight (`minEdge`) is extracted from the `minHeap`.
        2.  **Cycle Check:** If the `minEdge.Destination` vertex is already in `inMST`, this edge would form a cycle, so it's skipped.
        3.  **Add to MST:** If no cycle is formed, `minEdge` is added to `mstEdges`, its weight is added to `totalWeight`, and `minEdge.Destination` is marked as `inMST`.
        4.  **Explore New Vertex:** All edges connected to the newly added vertex (`minEdge.Destination`) are then added to the `minHeap`, but only if their other endpoint is *not* yet in `inMST`. This ensures that only valid candidate edges are considered for expansion.
    *   **Disconnected Graph Handling:** After the loop, if `mstEdges.Count` is not equal to `numVertices - 1` (for graphs with more than one vertex), it means the graph is disconnected, and an MST spanning all vertices could not be formed. The `PrimsResult.IsConnected` flag indicates this, and an empty `mstEdges` list is returned in such cases.

## 4. Complexity Analysis

*   **Time Complexity:** O(E log V)
    *   `V` is the number of vertices, and `E` is the number of edges.
    *   Each edge is enqueued into the `PriorityQueue` at most once. Enqueuing an edge takes O(log E) time. Since `E` can be at most `V^2`, `log E` is at most `2 log V`. Therefore, each `PriorityQueue` operation is effectively O(log V).
    *   In the worst case, we might enqueue and dequeue `E` edges. Thus, the total time complexity is O(E log V).

*   **Space Complexity:** O(V + E)
    *   `AdjacencyList`: Stores all vertices and edges, requiring O(V + E) space.
    *   `inMST` array: Requires O(V) space.
    *   `PriorityQueue`: In the worst case, it can hold up to O(E) edges.
    *   `mstEdges` list: Stores up to O(V) edges.
    *   The dominant factor is O(V + E) for storing the graph and the priority queue.