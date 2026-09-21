# Van Emde Boas Tree

## 1. Introduction
A **Van Emde Boas (vEB) Tree** is an advanced tree data structure that supports dynamic set operations on non-negative integers bounded by a known universe size $U = \{0, 1, \ldots, U - 1\}$. Unlike balanced binary search trees (e.g., Red-Black trees, AVL trees) which require $O(\log n)$ time per operation where $n$ is the number of items stored, a vEB tree performs search, insertion, deletion, minimum/maximum lookup, successor, and predecessor queries in **$O(\log \log U)$** time, regardless of how many elements are currently stored.

Use cases include:
- Fast priority queues over bounded integer universes (e.g., packet routing, hardware schedulers).
- Successor and predecessor predecessor lookups in graphics, geometric algorithms, or index structures.
- Highly repeated lookups where $U$ is known in advance and $O(\log \log U)$ outperforms $O(\log N)$.

---

## 2. Usage

```csharp
using System;
using DataStructures.VanEmdeBoas;

public class Example
{
    public static void Run()
    {
        // Initialize a vEB tree with universe size 65536 (rounded to power of 2)
        var veb = new VanEmdeBoasTree(65536);

        // Insertion
        veb.Insert(10);
        veb.Insert(100);
        veb.Insert(5);
        veb.Insert(42);

        // Min / Max queries (O(1))
        Console.WriteLine($"Min: {veb.Min}"); // 5
        Console.WriteLine($"Max: {veb.Max}"); // 100

        // Membership query (O(log log U))
        Console.WriteLine($"Contains 42: {veb.Contains(42)}"); // True
        Console.WriteLine($"Contains 15: {veb.Contains(15)}"); // False

        // Successor / Predecessor queries (O(log log U))
        Console.WriteLine($"Successor of 10: {veb.Successor(10)}");     // 42
        Console.WriteLine($"Predecessor of 42: {veb.Predecessor(42)}"); // 10

        // Deletion
        veb.Delete(10);
        Console.WriteLine($"Successor of 5: {veb.Successor(5)}");       // 42
    }
}
```

---

## 3. Detailed Explanation

A Van Emde Boas tree achieves its $O(\log \log U)$ bound through divide-and-conquer on the bits of the universe size:

1. **Recursive Universe Decomposition**:
   - Let the universe size be $U = 2^k$. The higher square root is $\lceil\sqrt{U}\rceil = 2^{\lceil k/2 \rceil}$ and lower square root is $\lfloor\sqrt{U}\rfloor = 2^{\lfloor k/2 \rfloor}$.
   - High bits $\text{high}(x) = \lfloor x / \lfloor\sqrt{U}\rfloor \rfloor$ select which child cluster $x$ belongs to.
   - Low bits $\text{low}(x) = x \bmod \lfloor\sqrt{U}\rfloor$ select the position within that cluster.
   - Key reconstruction: $\text{index}(h, l) = h \times \lfloor\sqrt{U}\rfloor + l$.

2. **Min/Max Optimization**:
   - Each vEB node maintains `_min` and `_max`.
   - **Crucially, `_min` is NOT stored recursively inside the child clusters.**
   - Because `_min` is kept outside child clusters, inserting an element into an empty cluster takes only $O(1)$ recursive steps (we update min/max and recursively insert into `summary` only once).
   - This prevents branching into multiple subproblems per operation, ensuring the recurrence $T(U) = T(\sqrt{U}) + O(1)$, which solves to $O(\log \log U)$.

3. **Summary Tree and Clusters**:
   - An array of cluster trees holds sub-universes.
   - A single `summary` tree tracks which cluster trees are non-empty.
   - Successor and predecessor queries use the summary to skip entire empty clusters in $O(\log \log U)$ time.

4. **Lazy Allocation**:
   - Clusters and summary trees are allocated on-demand to conserve memory.

---

## 4. Complexity Analysis

| Operation | Time Complexity | Space Complexity |
| :--- | :--- | :--- |
| **Min / Max** | $O(1)$ | $O(1)$ |
| **Contains** | $O(\log \log U)$ | $O(1)$ |
| **Insert** | $O(\log \log U)$ | $O(\sqrt{U})$ worst-case allocated per level |
| **Delete** | $O(\log \log U)$ | $O(1)$ |
| **Successor** | $O(\log \log U)$ | $O(1)$ |
| **Predecessor** | $O(\log \log U)$ | $O(1)$ |

- **Total Auxiliary Space**: $O(U)$ worst-case if fully populated, significantly reduced via lazy allocation when sparsely populated.