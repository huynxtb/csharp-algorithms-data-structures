# Introduction
The Hopcroft-Karp algorithm is an algorithm in computer science for finding a maximum cardinality matching in a bipartite graph. It runs in O(E * sqrt(V)) time, where E is the number of edges in the graph, and V is the number of vertices.

# Usage
```csharp
var graph = new int[][]
{
    new int[] { 1, 0, 1, 0 },
    new int[] { 0, 1, 1, 0 },
    new int[] { 0, 0, 1, 1 },
    new int[] { 0, 0, 0, 1 }
};
var hopcroftKarp = new HopcroftKarp(graph);
var (matching, matchingSize) = hopcroftKarp.ComputeMaximumMatching();
Console.WriteLine("Maximum matching size: " + matchingSize);
``` 
# Detailed Explanation
The algorithm works by using a breadth-first search (BFS) to find augmenting paths in the graph, and then using a depth-first search (DFS) to perform the actual augmentations. The BFS is used to find the shortest augmenting path, and the DFS is used to find the longest augmenting path.

# Complexity Analysis
* Time complexity: O(E * sqrt(V))
* Space complexity: O(V)
