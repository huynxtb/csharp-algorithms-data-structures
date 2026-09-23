# Link-Cut Tree

## 1. Introduction
The **Link-Cut Tree** (introduced by Daniel Sleator and Robert Tarjan in 1983) is a dynamic data structure that maintains a forest of disjoint rooted trees under dynamic edge insertions (links) and edge deletions (cuts).

It is ideally suited for:
- **Dynamic Connectivity in Forests:** Determining whether two nodes are connected while edges are continuously added and removed.
- **Dynamic Tree Path Queries:** Computing associative path aggregates (such as sum, min, max, XOR, GCD) between any pair of vertices in $O(\log N)$ amortized time.
- **Network Flow / Dynamic Trees:** Dynamic maximum flow algorithms, finding least common ancestors (LCA) online, and maintaining dynamic minimum spanning forests (e.g., using with ETT or for offline/online cycle maintenance).

---

## 2. Usage

```csharp
using System;
using AdvancedDataStructures;

public class Program
{
    public static void Main()
    {
        // Initialize a forest of 5 nodes (0 to 4) with initial values
        long[] values = { 10, 20, 30, 40, 50 };
        var lct = new LinkCutTree(values);

        // Link nodes to form edges: (0-1), (1-2), (2-3)
        lct.Link(0, 1);
        lct.Link(1, 2);
        lct.Link(2, 3);

        // Check connectivity
        bool connected = lct.IsConnected(0, 3); // True
        bool connectedIsolated = lct.IsConnected(0, 4); // False

        // Query path aggregations along path 0 -> 3 (nodes: 0, 1, 2, 3)
        var (sum, min, max, count) = lct.QueryPath(0, 3);
        // sum = 10 + 20 + 30 + 40 = 100, min = 10, max = 40, count = 4

        // Cut edge between 1 and 2
        lct.Cut(1, 2);
        bool stillConnected = lct.IsConnected(0, 3); // False

        // Update a node's value
        lct.SetValue(0, 100);
    }
}
```

---

## 3. Detailed Explanation

A Link-Cut Tree partitions each tree in the represented forest into disjoint vertex paths called **preferred paths**. Each preferred path is represented internally by an auxiliary **Splay Tree** ordered by node depths in the represented tree:

1. **Auxiliary Splay Trees:**
   - Nodes in a splay tree correspond to a preferred path.
   - In-order traversal of a splay tree visits vertices in increasing order of their depth in the represented tree.
   - Subtree aggregates (sum, minimum, maximum, count) are maintained at each node via `PushUp`.
2. **Path-Parent Pointers:**
   - The root of an auxiliary splay tree points to the parent of the topmost node in the represented tree via a `Parent` reference, while the parent does not point back to this auxiliary tree.
3. **Access(u):**
   - The core primitive. Splays $u$, disconnects its deeper preferred child, and attaches the previously accessed splay tree to make $u$ the deepest node on the preferred path containing the represented tree root.
4. **MakeRoot(u):**
   - Invokes `Access(u)`, followed by reversing the path direction in the splay tree via lazy tag propagation (`Reversed` flag and `PushDown`).
5. **Link(u, v) & Cut(u, v):**
   - `Link(u, v)`: Re-roots $u$ using `MakeRoot(u)` and attaches $u$'s parent pointer to $v$.
   - `Cut(u, v)`: Re-roots $u$, splays $v$, verifies direct adjacency, and severs the parent-child relationship.

---

## 4. Complexity Analysis

- **Amortized Time Complexity:**
  - `Access(u)`: $O(\log N)$
  - `MakeRoot(u)`: $O(\log N)$
  - `FindRoot(u)`: $O(\log N)$
  - `Link(u, v)`: $O(\log N)$
  - `Cut(u, v)`: $O(\log N)$
  - `IsConnected(u, v)`: $O(\log N)$
  - `QueryPath(u, v)`: $O(\log N)$
  - `SetValue(u, val)`: $O(\log N)$
- **Space Complexity:** $O(N)$ linear space to store node structures, parent/child pointers, and aggregate data.