# Depth-First Search (DFS)

## Introduction
Depth-First Search (DFS) is an algorithm for traversing or searching tree or graph data structures. The algorithm starts at the root (or an arbitrary node in the case of a graph) and explores as far as possible along each branch before backtracking. It is particularly useful for scenarios where you need to explore all possible paths in a graph or tree.

## Usage
```csharp
Graph graph = new Graph();
graph.AddEdge(1, 2);
graph.AddEdge(1, 3);
graph.AddEdge(2, 4);
graph.AddEdge(3, 5);
graph.AddEdge(4, 6);

graph.DFSIterative(1); // Output will be the nodes visited in DFS order.
```

## Detailed Explanation
The `Graph` class uses an adjacency list to represent the graph. The `AddEdge` method allows adding edges between nodes, with an option to specify whether the graph is directed or undirected. The `DFSIterative` method implements the DFS algorithm using a stack to keep track of nodes to visit. It marks nodes as visited to avoid cycles and prints each node as it is visited.

## Complexity Analysis
- **Time Complexity:** O(V + E), where V is the number of vertices and E is the number of edges. Each vertex and edge is processed once.
- **Space Complexity:** O(V), where V is the number of vertices. This is due to the storage of the visited set and the stack used for traversal.