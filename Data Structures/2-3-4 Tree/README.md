# 2-3-4 Tree (2-4 Tree)

## 1. Introduction
A **2-3-4 Tree** (also known as a **2-4 Tree**) is a self-balancing search tree data structure where every internal node has either 2, 3, or 4 child pointers, and stores 1, 2, or 3 sorted keys respectively:
- **2-node**: contains 1 key and 2 children.
- **3-node**: contains 2 keys and 3 children.
- **4-node**: contains 3 keys and 4 children.

All leaf nodes reside at the exact same depth, guaranteeing perfectly balanced searches. It is structurally equivalent to a Red-Black tree (order-4 B-tree), but supports single-pass top-down insertion and deletion through preemptive node splits and merges.

### When to Use:
- When predictable $O(\log n)$ search, insertion, and deletion times are needed.
- In scenarios where single-pass preemptive restructuring is preferred to avoid backtracking or cascading tree rotations.
- In memory or storage structures where multi-way indexing reduces the overall height of the tree compared to binary search trees.

---

## 2. Usage

```csharp
using System;
using AdvancedDataStructures;

class Program
{
    static void Main()
    {
        var tree = new TwoThreeFourTree<int, string>();

        // Insert elements
        tree.Insert(50, "Fifty");
        tree.Insert(20, "Twenty");
        tree.Insert(70, "Seventy");
        tree.Insert(10, "Ten");
        tree.Insert(30, "Thirty");
        tree.Insert(60, "Sixty");
        tree.Insert(80, "Eighty");

        // Indexer access & updates
        tree[25] = "Twenty-Five";
        Console.WriteLine($"Count: {tree.Count}, Height: {tree.Height}");

        // Search
        if (tree.Search(30, out string? val))
        {
            Console.WriteLine($"Found: {val}");
        }

        // Iteration (in ascending order)
        foreach (var pair in tree)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }

        // Deletion
        tree.Delete(20);
        Console.WriteLine($"Contains 20? {tree.ContainsKey(20)}");

        // Min / Max
        if (tree.TryGetMin(out var min))
        {
            Console.WriteLine($"Min: {min.Key} => {min.Value}");
        }
    }
}
```

---

## 3. Detailed Explanation

### Insertion (Top-Down Preemptive Split)
1. Starting at the root, the algorithm traverses down to find the insertion position.
2. **Preemptive Split**: If any 4-node (containing 3 keys) is encountered along the descent path, it is immediately split:
   - The middle key is promoted to its parent node.
   - The left and right keys become two independent 2-nodes.
   - Because the parent was already verified not to be a 4-node, the parent always has room to receive the promoted key.
3. If the root itself is a 4-node, a new root is created, increasing tree height by 1.
4. When a leaf is reached, the new key is inserted directly into the leaf (which is guaranteed to have room).

### Deletion (Top-Down Transformation)
1. During top-down traversal for deletion, every encountered 2-node (node with only 1 key) is proactively transformed into a 3-node or 4-node:
   - **Borrowing (Rotation)**: If an immediate sibling has more than 1 key, rotate a key through the parent.
   - **Merging**: If both siblings are 2-nodes, merge the current node with a sibling and pull down the separator key from the parent.
2. When the key to be deleted is located:
   - If it is in a leaf, remove it directly.
   - If it is in an internal node, swap it with its in-order predecessor or successor and remove that key recursively.
3. The root shrinks in height if all its keys are merged into its only child.

---

## 4. Complexity Analysis

| Operation | Time Complexity (Average) | Time Complexity (Worst-case) | Space Complexity |
| :--- | :--- | :--- | :--- |
| **Search** | $O(\log n)$ | $O(\log n)$ | $O(1)$ iterative |
| **Insert** | $O(\log n)$ | $O(\log n)$ | $O(1)$ iterative |
| **Delete** | $O(\log n)$ | $O(\log n)$ | $O(\log n)$ recursive call stack |
| **In-Order Traversal** | $O(n)$ | $O(n)$ | $O(\log n)$ stack depth |
| **Min / Max** | $O(\log n)$ | $O(\log n)$ | $O(1)$ |

- **Height Bound**: The tree height $h$ satisfies $\log_4(n+1) \le h \le \log_2(n+1)$, ensuring strictly logarithmic depth.