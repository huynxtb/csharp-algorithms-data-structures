# Dijkstra's Shortest Path Algorithm

## 1. Introduction
Dijkstra's algorithm is a greedy algorithm that finds the shortest paths from a single source vertex to all other vertices in a weighted graph. It is applicable only to graphs where all edge weights are non-negative. It works by iteratively selecting the unvisited vertex with the smallest known distance from the source and updating the distances of its neighbors. This algorithm is fundamental in various applications, including network routing protocols, mapping and navigation systems, and resource allocation problems where finding optimal paths is crucial.

## 2. Usage
To use the Dijkstra's Shortest Path algorithm, first, create an instance of `WeightedGraph<TVertex>` and populate it with vertices and edges. Then, instantiate `DijkstraShortestPath<TVertex>` and call its `FindShortestPaths` method with your graph and a source vertex. The result will be a `DijkstraResult<TVertex>` object containing shortest distances and a method to reconstruct paths.

```csharp
// 1. Create a graph
var graph = new WeightedGraph<string>();

// 2. Add vertices and edges
graph.AddEdge("A", "B", 4);
graph.AddEdge("A", "C", 2);
graph.AddEdge("B", "E", 3);
graph.AddEdge("C", "D", 2);
graph.AddEdge("C", "F", 4);
graph.AddEdge("D", "E", 3);
graph.AddEdge("D", "F", 1);
graph.AddEdge("E", "Z", 1);
graph.AddEdge("F", "Z", 1);

// 3. Instantiate Dijkstra's algorithm
var dijkstra = new DijkstraShortestPath<string>();

// 4. Find shortest paths from a source vertex
string sourceVertex = "A";
DijkstraResult<string> result = dijkstra.FindShortestPaths(graph, sourceVertex);

// 5. Access shortest distances
// Example: Get distance to 'Z'
double distanceToZ = result.ShortestDistances["Z"]; // Expected: 6

// 6. Reconstruct a specific path
string targetVertex = "Z";
IEnumerable<string> pathToZ = result.GetPath(targetVertex);
// Example: Path to Z: A -> C -> D -> F -> Z
string pathString = string.Join(" -> ", pathToZ);

// Example of an unreachable vertex
graph.AddVertex("X"); // Add an isolated vertex
DijkstraResult<string> result2 = dijkstra.FindShortestPaths(graph, sourceVertex);
double distanceToX = result2.ShortestDistances["X"]; // Expected: double.PositiveInfinity
bool pathToXExists = result2.GetPath("X").Any(); // Expected: false
```

## 3. Detailed Explanation
This implementation of Dijkstra's algorithm is designed for clarity, reusability, and adherence to modern C# practices. It consists of three main classes:

1.  **`Edge<TVertex>`**: A simple, generic class representing a directed connection between two vertices. It stores the `Destination` vertex and the `Weight` of the edge. A check ensures that edge weights are non-negative, which is a fundamental requirement for Dijkstra's algorithm.

2.  **`WeightedGraph<TVertex>`**: This class provides a flexible way to represent a graph using an adjacency list. The `AdjacencyList` is a `Dictionary<TVertex, List<Edge<TVertex>>>`, where each key is a vertex, and its associated value is a list of `Edge` objects originating from that vertex. Methods like `AddVertex` and `AddEdge` facilitate graph construction, automatically adding vertices if they don't exist when an edge is added.

3.  **`DijkstraResult<TVertex>`**: This class encapsulates the output of the `DijkstraShortestPath` algorithm. It holds two key pieces of information:
    *   `ShortestDistances`: A `Dictionary<TVertex, double>` mapping each vertex to its calculated shortest distance from the source. Unreachable vertices will have a distance of `double.PositiveInfinity`.
    *   `Predecessors`: An internal `Dictionary<TVertex, TVertex>` that stores the immediate predecessor of each vertex on its shortest path from the source. This dictionary is crucial for reconstructing the actual path sequence.
    *   `GetPath(TVertex targetVertex)`: A public method that uses the `Predecessors` dictionary to backtrack from a given `targetVertex` to the source, returning the shortest path as an ordered `IEnumerable<TVertex>`.

4.  **`DijkstraShortestPath<TVertex>`**: This is the core class that implements the algorithm. Its `FindShortestPaths` method performs the following steps:
    *   **Initialization**: It initializes `distances` (a dictionary to store the shortest distance found so far for each vertex, starting with `0` for the source and `PositiveInfinity` for others), `predecessors` (to track path reconstruction), and `visited` (a `HashSet` to mark vertices whose shortest path has been finalized). A `System.Collections.Generic.PriorityQueue<TVertex, double>` is used to efficiently retrieve the unvisited vertex with the smallest current distance.
    *   **Main Loop**: The algorithm iterates while the priority queue is not empty. In each iteration:
        *   It extracts the `currentVertex` with the minimum distance from the priority queue.
        *   If `currentVertex` has already been visited (meaning a shorter path to it was finalized earlier), it skips to the next iteration.
        *   It marks `currentVertex` as visited.
        *   For each `neighbor` of `currentVertex`, it calculates a `newDistance` from the source through `currentVertex`. If this `newDistance` is shorter than the `distances[neighbor]` currently recorded, it updates `distances[neighbor]`, sets `predecessors[neighbor]` to `currentVertex`, and enqueues the `neighbor` with its `newDistance` into the priority queue.

The generic type parameter `TVertex` is constrained to `notnull`, `IComparable<TVertex>`, and `IEquatable<TVertex>` to ensure proper functionality as dictionary keys and for comparisons.

## 4. Complexity Analysis

*   **Time Complexity**: O((V + E) log V)
    *   `V` represents the number of vertices in the graph.
    *   `E` represents the number of edges in the graph.
    *   Each vertex is extracted from the priority queue at most once, contributing `V * O(log V)` to the complexity (due to `Dequeue` operations).
    *   Each edge is processed at most once. For each edge, a relaxation step (distance update and potential `Enqueue` into the priority queue) takes `O(log V)` time in the worst case.
    *   Therefore, the total time complexity is dominated by the priority queue operations, resulting in `O((V + E) log V)`.

*   **Space Complexity**: O(V + E)
    *   The `WeightedGraph`'s `AdjacencyList` requires `O(V + E)` space to store all vertices and their associated edges.
    *   The `distances` dictionary stores an entry for each vertex, requiring `O(V)` space.
    *   The `predecessors` dictionary also stores an entry for each reachable vertex, requiring `O(V)` space.
    *   The `visited` hash set stores up to `V` vertices, taking `O(V)` space.
    *   The `priorityQueue` can hold up to `V` elements in the worst case, requiring `O(V)` space.
    *   Combining these, the overall space complexity is `O(V + E)`.