# Persistent Segment Tree

## 1. Introduction
A **Persistent Segment Tree** (also referred to as a fully persistent segment tree or functional segment tree) is a specialized data structure that preserves all historical versions of itself across modifications. Rather than updating nodes in place, every update operation creates a new root and instantiates new nodes along the path from the root down to the modified leaf. Subtrees that remain unaffected by the update are shared structurally between the new version and previous versions.

### When to Use
- **Historical Queries:** When you need to query the state of an array at past points in time.
- **K-th Smallest / Range Order Statistics:** When used over frequency domains to find the k-th smallest element within an arbitrary range `[L, R]` in $O(\log N)$.
- **Branching States / Undo Operations:** Efficiently maintaining different versions of datasets with branching paths and rollbacks without copying entire arrays.

---

## 2. Usage

```csharp
using System;
using DataStructures;

class Program
{
    static void Main()
    {
        int[] initialArray = { 1, 3, 5, 7, 9, 11 };
        PersistentSegmentTree pst = new PersistentSegmentTree(initialArray);

        // Version 0: [1, 3, 5, 7, 9, 11]
        Console.WriteLine(pst.Query(0, 1, 3)); // Output: 3 + 5 + 7 = 15

        // Version 1: update index 2 to 10 on top of version 0 -> [1, 3, 10, 7, 9, 11]
        int v1 = pst.Update(0, 2, 10);
        Console.WriteLine(pst.Query(v1, 1, 3)); // Output: 3 + 10 + 7 = 20

        // Version 0 is unchanged
        Console.WriteLine(pst.Query(0, 1, 3)); // Output: 15

        // Version 2: update index 0 to 0 on top of version 1 -> [0, 3, 10, 7, 9, 11]
        int v2 = pst.Update(v1, 0, 0);
        Console.WriteLine(pst.Query(v2, 0, 2)); // Output: 0 + 3 + 10 = 13
    }
}
```

---

## 3. Detailed Explanation

1. **Structural Sharing / Path Copying:**
   - A standard segment tree over an array of size $N$ has a depth of $\approx \lceil \log_2 N \rceil$.
   - When an update occurs at index `i`, only the nodes along the direct path from the root to the leaf `i` change. There are at most $O(\log N)$ such nodes.
   - The algorithm creates copies of only these $O(\log N)$ nodes. The unaffected child pointers point directly to the existing nodes from previous versions.

2. **Version Management:**
   - Roots of each version are stored sequentially in a list (`List<Node> _roots`).
   - Creating an update creates a new root node, appends it to `_roots`, and returns its index representing the new version number.

3. **Queries:**
   - Range queries behave identically to standard segment tree queries: the tree is traversed starting from the specific root corresponding to the requested version number.

---

## 4. Complexity Analysis

| Operation | Time Complexity | Space Complexity |
| :--- | :--- | :--- |
| **Build (Version 0)** | $O(N)$ | $O(N)$ |
| **Point Update** | $O(\log N)$ | $O(\log N)$ new nodes |
| **Range Sum Query** | $O(\log N)$ | $O(1)$ auxiliary (ignoring stack) |
| **Total Space (after $M$ updates)** | — | $O(N + M \log N)$ |