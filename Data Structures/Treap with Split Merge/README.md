# Treap with Split and Merge

A **Treap** (tree + heap) is a randomized balanced binary search tree. Each node contains a search key and an independently and uniformly generated random priority. The tree maintains:
1. **Binary Search Tree (BST) Property** on keys (`Left.Key < Node.Key < Right.Key`).
2. **Max-Heap Property** on priorities (`Node.Priority >= Child.Priority`).

Unlike traditional balanced trees (AVL, Red-Black) or rotation-based Treaps, this implementation is built entirely on the powerful **Split** and **Merge** primitives. This makes tree manipulation modular, simple to implement, and directly extensible to advanced applications such as implicit treaps, range queries, and rope data structures.

## Usage

```csharp
using System;
using DataStructures.Trees;

public class Example
{
    public static void Run()
    {
        // Initialize Treap with a fixed seed for deterministic behavior
        var treap = new Treap<int>(seed: 42);

        // Insertion
        treap.Insert(50);
        treap.Insert(20);
        treap.Insert(70);
        treap.Insert(10);
        treap.Insert(30);

        // Membership
        Console.WriteLine(treap.Contains(20)); // True
        Console.WriteLine(treap.Contains(99)); // False

        // Order-statistic queries
        Console.WriteLine(treap.GetKthSmallest(1)); // 10
        Console.WriteLine(treap.GetKthSmallest(3)); // 30
        Console.WriteLine(treap.RankOf(30));         // 2 (elements 10 and 20 are strictly less)

        // Deletion via Split/Merge
        treap.Remove(20);
        Console.WriteLine(treap.Contains(20)); // False
        Console.WriteLine(treap.Count);        // 4

        // Sorted traversal
        foreach (int key in treap)
        {
            Console.Write(key + " "); // 10 30 50 70
        }
        Console.WriteLine();
    }
}
```

## Detailed Explanation

### Core Primitives

1. **`Split(root, key)`**:
   Divides the treap rooted at `root` into two disjoint treaps:
   - `left`: Contains all nodes with `Key < key`.
   - `right`: Contains all nodes with `Key >= key`.
   This is done recursively by inspecting the root's key against the pivot key and recursing into the corresponding child subtree.

2. **`Merge(left, right)`**:
   Combines two valid treaps where every key in `left` is strictly smaller than every key in `right`.
   The root of the merged tree is selected based on whichever root has the higher heap priority. The other tree is merged recursively into the corresponding child subtree.

3. **`Insert(key)`**:
   Splits the tree at `key` into `(L, R)`. A new single-node treap `N` is created with random priority. The result is `Merge(Merge(L, N), R)`.

4. **`Remove(key)`**:
   Splits the tree at `key` into `(L, R)`. Because `R` contains keys `>= key`, splitting the smallest node off `R` extracts the target node. The remaining halves are merged back: `Merge(L, greater)`.

5. **Subtree Size Augmentation**:
   Every node maintains `Size = 1 + Size(Left) + Size(Right)`. This enables $O(\log n)$ order statistics: finding the $k$-th smallest element (`GetKthSmallest`) and the rank of a key (`RankOf`).

## Complexity Analysis

| Operation | Average Time Complexity | Worst-Case Time Complexity | Space Complexity |
| :--- | :--- | :--- | :--- |
| **Insert** | $O(\log n)$ | $O(n)$ | $O(\log n)$ stack space |
| **Remove** | $O(\log n)$ | $O(n)$ | $O(\log n)$ stack space |
| **Contains** | $O(\log n)$ | $O(n)$ | $O(1)$ |
| **GetKthSmallest** | $O(\log n)$ | $O(n)$ | $O(1)$ |
| **RankOf** | $O(\log n)$ | $O(n)$ | $O(1)$ |
| **InOrderTraversal** | $O(n)$ | $O(n)$ | $O(h)$ auxiliary stack |
| **Total Storage** | $O(n)$ | $O(n)$ | $O(n)$ |

*Note:* The randomized priorities guarantee that the expected tree height $h$ is $O(\log n)$ with high probability, preventing pathological worst-case performance under non-adversarial priority generation.