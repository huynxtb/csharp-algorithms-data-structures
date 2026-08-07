# Tarjan's Strongly Connected Components (SCC) Algorithm

## 1. Introduction
Tarjan's algorithm is an efficient method for finding the Strongly Connected Components (SCCs) of a directed graph. An SCC is a maximal subgraph such that for every pair of vertices (u, v) in the subgraph, there is a path from u to v and a path from v to u. This algorithm is widely used in various applications, including:
*   **Dependency analysis**: Identifying cyclic dependencies in systems (e.g., build systems, software modules).
*   **Network analysis**: Understanding the structure of directed networks.
*   **Compiler design**: Analyzing control flow graphs.
*   **Topological sorting**: After finding SCCs, the condensation graph (where each SCC is a single node) is a Directed Acyclic Graph (DAG) that can be topologically sorted.

## 2. Usage
To use the `TarjansSccSolver`, you need to provide your directed graph as an adjacency list (`Dictionary<int, List<int>>`). The `Solve` method will return a list of lists, where each inner list represents an SCC.

```csharp
using System;
using System.Collections.Generic;

public class Example
{
    public static void Main(string[] args)
    {
        // Example graph:
        // 1 -> 2
        // 2 -> 3
        // 3 -> 1
        // 3 -> 4
        // 4 -> 5
        // 5 -> 4
        // 6 -> 3
        // 6 -> 7
        // 7 -> 8
        // 8 -> 6
        // 9 (isolated vertex)
        // 10 -> 11 (disconnected component)
        // 11 -> 10

        Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>
        {
            { 1, new List<int> { 2 } },
            { 2, new List<int> { 3 } },
            { 3, new List<int> { 1, 4 } },
            { 4, new List<int> { 5 } },
            { 5, new List<int> { 4 } },
            { 6, new List<int> { 3, 7 } },
            { 7, new List<int> { 8 } },
            { 8, new List<int> { 6 } },
            { 9, new List<int>() }, // Isolated vertex
            { 10, new List<int> { 11 } }, // Disconnected component
            { 11, new List<int> { 10 } }
        };

        TarjansSccSolver solver = new TarjansSccSolver();
        IList<IList<int>> sccs = solver.Solve(graph);

        Console.WriteLine("Strongly Connected Components:");
        for (int i = 0; i < sccs.Count; i++)
        {
            Console.WriteLine($"SCC {i + 1}: [{string.Join(", ", sccs[i])}]");
        }

        // Expected output (order of SCCs and elements within SCCs may vary):
        // SCC 1: [1, 3, 2]
        // SCC 2: [4, 5]
        // SCC 3: [6, 8, 7]
        // SCC 4: [9]
        // SCC 5: [10, 11]
    }
}
```

## 3. Detailed Explanation
Tarjan's algorithm uses a single Depth-First Search (DFS) traversal to find SCCs. It maintains several key pieces of information for each vertex `u`:

1.  **`_discoveryTime[u]`**: The order in which `u` was visited during the DFS. This is a unique identifier for each vertex's discovery.
2.  **`_lowLink[u]`**: The smallest `_discoveryTime` reachable from `u` (including `u` itself) through the DFS tree edges and at most one back-edge. This value is crucial for identifying SCC roots.
3.  **`_stack`**: A stack that keeps track of the vertices currently in the recursion path of the DFS. When an SCC is found, its vertices are popped from this stack.
4.  **`_onStack`**: A `HashSet` used for `O(1)` lookup to quickly determine if a vertex is currently on the `_stack`.

The algorithm proceeds as follows:

*   **Initialization**: All `_discoveryTime` and `_lowLink` values are uninitialized (or set to a sentinel value), the `_stack` and `_onStack` are empty, and a global `_index` counter is set to 0.
*   **Main Loop**: The `Solve` method iterates through all unique vertices in the graph. If a vertex `u` has not been visited (`_discoveryTime` is not set for `u`), a DFS is initiated from `u`. This ensures that all parts of a disconnected graph are processed.
*   **DFS Traversal (`Dfs(u)`)**:
    1.  When `u` is first visited, its `_discoveryTime[u]` and `_lowLink[u]` are set to the current `_index`, which is then incremented. `u` is pushed onto `_stack` and added to `_onStack`.
    2.  For each neighbor `v` of `u`:
        *   If `v` has not been visited (`_discoveryTime` is not set for `v`), a recursive `Dfs(v)` call is made. After the call returns, `_lowLink[u]` is updated to `min(_lowLink[u], _lowLink[v])`. This propagates the lowest reachable `_discoveryTime` from `v` back to `u`.
        *   If `v` has been visited and is currently `_onStack` (meaning it's an ancestor of `u` in the DFS tree, indicating a back-edge), `_lowLink[u]` is updated to `min(_lowLink[u], _discoveryTime[v])`. This signifies that `u` can reach `v` (an ancestor) and thus potentially other vertices reachable from `v`.
    3.  **SCC Identification**: After visiting all neighbors of `u`, if `_lowLink[u] == _discoveryTime[u]`, it means `u` is the root of an SCC. All vertices currently on the `_stack` from `u` upwards (until `u` itself is popped) form this SCC. These vertices are popped from the `_stack`, removed from `_onStack`, and collected into a new SCC list.

This process guarantees that all vertices within an SCC are processed together and correctly identified.

## 4. Complexity Analysis

*   **Time Complexity**: O(V + E)
    *   Each vertex is visited exactly once by the DFS.
    *   Each edge is traversed exactly once.
    *   Stack operations (push, pop) and `HashSet` operations (add, remove, contains) take O(1) on average.
    *   The initial collection of all vertices takes O(V+E).
    *   Therefore, the total time complexity is linear with respect to the number of vertices (V) and edges (E).

*   **Space Complexity**: O(V) auxiliary space
    *   `_discoveryTime` and `_lowLink` dictionaries: O(V)
    *   `_stack`: In the worst case (a path graph), it can hold all V vertices: O(V)
    *   `_onStack` hash set: O(V)
    *   `_sccs` list: In the worst case (V SCCs of size 1), it stores V vertices: O(V)
    *   The input `adjacencyList` itself can take O(V + E) space, but this is considered input space, not auxiliary space used by the algorithm's internal data structures.