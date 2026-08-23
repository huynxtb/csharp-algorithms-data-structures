# Lowest Common Ancestor (LCA) using Binary Lifting

### 1. Introduction
The Lowest Common Ancestor (LCA) of two nodes $u$ and $v$ in a rooted tree is the deepest node that is an ancestor of both $u$ and $v$. Binary Lifting is an efficient technique to find the LCA by precomputing the $2^j$-th ancestor for every node. This approach is highly optimized for scenarios requiring multiple LCA queries on static trees.

### 2. Usage

```csharp
using System;
using System.Collections.Generic;
using AdvancedAlgorithms.Graphs;

class Program
{
    static void Main()
    {
        int nodeCount = 7;
        var adjacencyList = new List<int>[nodeCount];
        for (int i = 0; i < nodeCount; i++)
        {
            adjacencyList[i] = new List<int>();
        }

        // Constructing a simple tree:
        //       0
        //      / \
        //     1   2
        //    / \   \
        //   3   4   5
        //          /
        //         6
        adjacencyList[0].AddRange(new[] { 1, 2 });
        adjacencyList[1].AddRange(new[] { 0, 3, 4 });
        adjacencyList[2].AddRange(new[] { 0, 5 });
        adjacencyList[3].AddRange(new[] { 1 });
        adjacencyList[4].AddRange(new[] { 1 });
        adjacencyList[5].AddRange(new[] { 2, 6 });
        adjacencyList[6].AddRange(new[] { 5 });

        var lcaFinder = new BinaryLiftingLca(nodeCount, adjacencyList, root: 0);

        int lca = lcaFinder.GetLca(3, 4); // Returns 1
        int distance = lcaFinder.GetDistance(3, 6); // Returns 5 (3 -> 1 -> 0 -> 2 -> 5 -> 6)

        Console.WriteLine($"LCA of 3 and 4: {lca}");
        Console.WriteLine($"Distance between 3 and 6: {distance}");
    }
}
```

### 3. Detailed Explanation
- **Preprocessing**: We perform a Breadth-First Search (BFS) starting from the root to compute the depth of each node and their direct parent ($2^0$-th ancestor). BFS is chosen over DFS to prevent stack overflow exceptions on deep trees.
- **DP Table Initialization**: We populate a 2D array `up[i, j]` representing the $2^j$-th ancestor of node `i` using the relation:
  $$up[i, j] = up[up[i, j - 1], j - 1]$$
- **LCA Querying**:
  1. Bring both nodes to the same depth by lifting the deeper node up using binary steps.
  2. If they are now the same node, that node is the LCA.
  3. Otherwise, lift both nodes simultaneously until they are direct children of the LCA.
  4. Return the parent of either node.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Preprocessing**: $O(N \log N)$ where $N$ is the number of nodes.
  - **LCA Query**: $O(\log N)$ per query.
  - **Distance Query**: $O(\log N)$ per query.
- **Space Complexity**: $O(N \log N)$ to store the binary lifting table.