# Cycle Detection in a Directed Graph using Depth-First Search (DFS)

## Introduction

Cycle detection in directed graphs is a fundamental algorithmic problem where the goal is to determine whether a cycle exists within a given directed graph. A cycle occurs when following the directed edges results in returning back to a previously visited node, indicating circular dependencies.

This is critical in various domains including task scheduling (to detect circular task dependencies), operating systems (for deadlock detection), and compiler optimizations (to verify program correctness).

The algorithm discussed here uses Depth-First Search (DFS) to efficiently detect cycles by tracking the recursion stack during traversal.

## Usage

The `DirectedGraph` class provides a clean interface for creating a directed graph and checking for cycles.

Here's how you can use the class:

// Create an instance of the graph
DirectedGraph graph = new DirectedGraph();

// Add directed edges: graph.AddEdge(source, destination);
graph.AddEdge(0, 1);
graph.AddEdge(1, 2);
graph.AddEdge(2, 0); // This introduces a cycle

// Check if cycle exists
bool hasCycle = graph.ContainsCycle();

if (hasCycle)
{
    // Cycle detected
}
else
{
    // No cycle
}

## Detailed Explanation

- The graph is represented using an adjacency list stored in a `Dictionary<int, List<int>>`, mapping each node to its list of direct successors.
- The `AddEdge` method adds a directed edge from the source node to the destination node and ensures both nodes exist in the graph.
- The `ContainsCycle` method performs DFS on each unvisited node to detect cycles:
  - It maintains two sets: `visited` (all nodes visited so far), and `recursionStack` (nodes currently in the DFS recursion call stack).
  - During DFS (`DetectCycleDFS`), when visiting a neighbor:
    - If the neighbor has not been visited, recursively visit it.
    - If the neighbor is in the recursion stack, a cycle is detected (back edge found).
  - After processing all neighbors, the current node is removed from the recursion stack.

This approach effectively detects back edges that form cycles in directed graphs.

## Complexity Analysis

- **Time Complexity:**
  - Adding edges is O(1) per edge.
  - DFS traversal visits each node and edge once, resulting in O(V + E), where V is the number of vertices and E is the number of edges.

- **Space Complexity:**
  - The adjacency list requires O(V + E) space.
  - The visited and recursion stack sets each require O(V) space.
  
Overall, the algorithm is efficient and suitable for integration into larger systems requiring cycle detection.