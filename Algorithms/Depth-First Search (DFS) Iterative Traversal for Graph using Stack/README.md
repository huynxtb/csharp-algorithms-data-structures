# Depth-First Search (DFS) Iterative Traversal for Graph using Stack

## Introduction

Depth-First Search (DFS) is a fundamental graph traversal algorithm that explores as far as possible along each branch before backtracking. Unlike recursive implementations, this iterative approach uses an explicit `Stack<int>` data structure to manage the traversal process. This implementation is particularly useful for:

- Avoiding stack overflow errors on very large graphs
- Improving performance in scenarios where recursion overhead is significant
- Detecting cycles in graphs
- Finding connected components
- Topological sorting
- Path finding in graphs

The iterative approach provides the same time complexity as the recursive version while offering better control over the traversal order and memory usage.

## Usage

```csharp
using Algorithms.Graph;

// Create a graph with 5 vertices
Graph graph = new Graph(5);

// Add edges to the graph
graph.AddEdge(0, 1);
graph.AddEdge(0, 2);
graph.AddEdge(1, 3);
graph.AddEdge(2, 3);
graph.AddEdge(3, 4);

// Perform DFS from vertex 0
List<int> dfsTraversal = graph.DFSIterative(0);
// Result: [0, 2, 3, 4, 1]

// Perform DFS on all vertices (handles disconnected components)
List<int> allVertices = graph.DFSIterativeAll();
// Result: [0, 2, 3, 4, 1]

// Check if path exists between two vertices
bool pathExists = graph.HasPath(0, 4);
// Result: true

bool noPath = graph.HasPath(1, 5); // 5 is out of range - throws exception
```

## Detailed Explanation

### Graph Representation

The implementation uses an **adjacency list** representation:
- `_adjacencyList` is an array of lists, where each index represents a vertex
- Each list contains the neighbors of that vertex
- For undirected graphs, edges are added in both directions

### DFSIterative Method

The iterative DFS algorithm works as follows:

1. **Initialize**: Create a visited array to track which vertices have been explored, initialize a stack with the start vertex, and mark the start vertex as visited
2. **Main Loop**: While the stack is not empty:
   - Pop a vertex from the stack and add it to the result list
   - Iterate through all its unvisited neighbors (in reverse order to maintain left-to-right exploration)
   - Mark each unvisited neighbor as visited and push it onto the stack
3. **Return**: Return the result list containing all visited vertices in DFS order

The **reverse iteration** (`for (int i = _adjacencyList[vertex].Count - 1; i >= 0; i--)`) ensures that neighbors are explored in the same order as the recursive version would explore them.

### DFSIterativeAll Method

This method extends basic DFS to handle disconnected graphs:

- Iterates through all vertices from 0 to _vertexCount - 1
- For each unvisited vertex, initiates a DFS traversal
- Collects all visited vertices from all connected components
- Ensures no vertex is visited more than once

### HasPath Method

The path-finding method is an optimized version of DFS:

- Performs DFS from the source vertex
- Returns `true` immediately when the destination is found
- Returns `false` if the stack becomes empty without finding the destination
- Handles the special case where source equals destination

### Error Handling

All public methods validate input parameters and throw `ArgumentOutOfRangeException` for invalid vertex indices, ensuring robust and predictable behavior.

## Complexity Analysis

### Time Complexity

- **DFSIterative(int startVertex)**: **O(V + E)**
  - V = number of vertices, E = number of edges
  - Each vertex is visited once and pushed/popped from the stack once
  - Each edge is examined once (for undirected graphs, once from each direction)

- **DFSIterativeAll()**: **O(V + E)**
  - Performs DFS on all vertices, visiting each vertex and edge exactly once
  - Guarantees traversal of all connected components

- **HasPath(int source, int destination)**: **O(V + E)**
  - Worst case: explores the entire connected component before determining no path exists
  - Best case: **O(V)** if destination is found quickly

### Space Complexity

- **DFSIterative(int startVertex)**: **O(V)**
  - Visited array: O(V)
  - Result list: O(V)
  - Stack: O(V) in the worst case (all vertices on stack)
  - Total: O(V)

- **DFSIterativeAll()**: **O(V)**
  - Similar to DFSIterative but includes all vertices
  - Visited array: O(V)
  - Result list: O(V)
  - Stack: O(V)
  - Total: O(V)

- **HasPath(int source, int destination)**: **O(V)**
  - Visited array: O(V)
  - Stack: O(V) in worst case
  - Total: O(V)

### Adjacency List Storage

- **O(V + E)** space for the entire graph structure, which is optimal for sparse graphs
- More efficient than adjacency matrix for sparse graphs, which would require **O(V²)** space