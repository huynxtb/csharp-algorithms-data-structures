# Depth-First Search (DFS) for Graph Traversal

## 1. Introduction
Depth-First Search (DFS) is an algorithm for traversing or searching tree or graph data structures. The algorithm starts at a designated source node and explores as deep as possible along each branch before backtracking. DFS is commonly used for topological sorting, finding connected components, solving puzzles (like mazes), and detecting cycles in graphs.

## 2. Usage
```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var graph = new Dictionary<int, List<int>>
        {
            { 1, new List<int> { 2, 3 } },
            { 2, new List<int> { 4, 5 } },
            { 3, new List<int> { 6 } },
            { 4, new List<int>() },
            { 5, new List<int>() },
            { 6, new List<int>() }
        };

        List<int> result = GraphDfs.Traverse(1, graph);
        Console.WriteLine(string.Join(", ", result)); // Output: 1, 2, 4, 5, 3, 6
    }
}
```

## 3. Detailed Explanation
This implementation uses an iterative approach with an explicit `Stack<int>` to prevent stack overflow issues associated with recursion on deep graphs.
- **Initialization**: A `visited` set tracks visited nodes to prevent infinite loops in cyclic graphs. A `visitedOrder` list records the traversal sequence.
- **Traversal Loop**: The algorithm pops a node from the stack. If it has not been visited, it is marked as visited and added to the output list.
- **Neighbor Processing**: Neighbors of the current node are pushed onto the stack in reverse order. Pushing in reverse order ensures that the first neighbor listed in the adjacency list is processed first, maintaining standard left-to-right DFS traversal order.

## 4. Complexity Analysis
- **Time Complexity**: `O(V + E)` where `V` is the number of vertices (nodes) and `E` is the number of edges. Every vertex and edge is explored at most once.
- **Space Complexity**: `O(V)` to store the visited set, the stack, and the output list.