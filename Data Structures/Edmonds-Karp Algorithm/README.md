# Edmonds-Karp Algorithm

## Introduction

The Edmonds-Karp algorithm is an implementation of the Ford-Fulkerson method for computing the maximum flow in a flow network. It uses Breadth-First Search (BFS) to find augmenting paths in the residual graph. This algorithm is particularly useful for finding the maximum possible flow from a source node to a sink node in a directed graph where each edge has a capacity.

## Usage

```csharp
// Example Usage:
// Assuming you have a FlowNetwork instance and have added vertices and edges.

// var network = new FlowNetwork();
// network.AddVertex(0);
// network.AddVertex(1);
// network.AddVertex(2);
// network.AddVertex(3);
//
// network.AddEdge(0, 1, 10);
// network.AddEdge(0, 2, 5);
// network.AddEdge(1, 2, 15);
// network.AddEdge(1, 3, 5);
// network.AddEdge(2, 3, 10);
//
// int source = 0;
// int sink = 3;
//
// int maxFlow = EdmondsKarpSolver.ComputeMaxFlow(network, source, sink);
// Console.WriteLine($"The maximum flow is: {maxFlow}"); // Expected output: The maximum flow is: 15
```

## Detailed Explanation

The `FlowNetwork` class represents the directed graph. It uses a dictionary of dictionaries (`_adjacencyList`) to store the graph's structure, where the outer dictionary maps a vertex ID to an inner dictionary. The inner dictionary maps a neighbor vertex ID to the capacity of the edge connecting them. It also maintains a `HashSet` of all vertices.

Key methods in `FlowNetwork`:
- `AddVertex(int vertexId)`: Adds a vertex to the network.
- `AddEdge(int fromVertexId, int toVertexId, int capacity)`: Adds a directed edge with a given capacity. If an edge already exists, its capacity is increased.
- `GetVertices()`: Returns all vertex IDs in the network.
- `GetNeighbors(int vertexId)`: Returns a dictionary of neighbors and their edge capacities for a given vertex.
- `GetCapacity(int fromVertexId, int toVertexId)`: Returns the capacity of the edge from `fromVertexId` to `toVertexId`.
- `UpdateCapacity(int fromVertexId, int toVertexId, int newCapacity)`: Updates the capacity of an edge.

The `EdmondsKarpSolver` class contains the logic for computing the maximum flow.

- `ComputeMaxFlow(FlowNetwork network, int sourceId, int sinkId)`: This is the main public method. It initializes the residual network and then repeatedly finds augmenting paths using BFS until no more paths can be found.
  - It first creates a `residualNetwork` which is a copy of the original network, also including reverse edges with initial capacity 0.
  - It enters a loop that continues as long as an augmenting path from `sourceId` to `sinkId` is found in the `residualNetwork`.
  - `FindAugmentingPath` uses BFS to find a path with available capacity.
  - If a path is found, the bottleneck capacity (`pathFlow`) of that path is determined.
  - This `pathFlow` is added to the `maxFlow`.
  - The capacities in the `residualNetwork` are updated: forward edges on the path have their capacity decreased by `pathFlow`, and backward edges have their capacity increased by `pathFlow`.
  - Once no more augmenting paths can be found, the accumulated `maxFlow` is returned.

- `CreateResidualNetwork(FlowNetwork originalNetwork)`: Constructs the initial residual graph from the original flow network. For every edge `(u, v)` with capacity `c` in the original network, it adds an edge `(u, v)` with capacity `c` and a reverse edge `(v, u)` with capacity `0` to the residual network.

- `FindAugmentingPath(FlowNetwork residualNetwork, int sourceId, int sinkId)`: Performs a Breadth-First Search on the `residualNetwork` starting from `sourceId` to find a path to `sinkId` where all edges have a positive residual capacity. It returns a boolean indicating if a path was found and a dictionary (`parentMap`) that reconstructs the path.

## Complexity Analysis

- **Time Complexity:** O(V * E^2), where V is the number of vertices and E is the number of edges. In the worst case, each augmentation might increase the flow by only 1, and BFS takes O(E) time. The number of augmentations can be up to O(V*E).
- **Space Complexity:** O(V + E) for storing the residual graph and BFS data structures (queue, visited set, parent map).