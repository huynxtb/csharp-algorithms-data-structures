# Self-Balancing AVL List

## 1. Introduction

The **Self-Balancing AVL List** is a dynamic collection of integers that is always kept in sorted order. Internally it is implemented as an **AVL tree**, a type of self-balancing binary search tree named after its inventors Adelson-Velsky and Landis.

In an AVL tree, the heights of the two child subtrees of any node differ by at most one. Whenever an insertion or deletion violates this property, the tree performs **rotations** to restore balance. This guarantees that all core operations (insertion, deletion, and lookup) run in logarithmic time even in the worst case.

Use this structure when you need:
- A collection that is continuously maintained in sorted order.
- Fast membership tests (`Contains`).
- Efficient insertions and deletions without re-sorting.
- Guaranteed `O(log n)` worst-case performance (unlike an unbalanced BST which can degrade to `O(n)`).

This implementation treats the list as a **set of unique integers**; attempting to add a duplicate value has no effect.

## 2. Usage

```csharp
public class Example
{
    public void Demo()
    {
        var list = new AvlList();

        // Add elements (order does not matter)
        list.Add(30);
        list.Add(10);
        list.Add(20);
        list.Add(40);
        list.Add(10); // duplicate, ignored

        // Check membership
        bool hasTwenty = list.Contains(20); // true
        bool hasFifty = list.Contains(50);  // false

        // Retrieve sorted data
        int[] sorted = list.ToSortedArray(); // { 10, 20, 30, 40 }

        // Remove an element
        bool removed = list.Remove(20); // true

        // Current number of elements
        int size = list.Count; // 3

        // Empty the list
        list.Clear();
    }
}
```

## 3. Detailed Explanation

### Node structure
Each node stores an integer `Value`, references to its `Left` and `Right` children, and a cached `Height`. Caching the height allows the balance factor to be computed in constant time.

### Balance factor
The **balance factor** of a node is `height(left subtree) - height(right subtree)`. A node is balanced when this value is in the range `[-1, 1]`.

### Rotations
When an operation makes a node's balance factor `+2` or `-2`, one of four cases occurs, each fixed with a rotation:
- **Left-Left:** single right rotation.
- **Right-Right:** single left rotation.
- **Left-Right:** left rotation on the left child, then right rotation.
- **Right-Left:** right rotation on the right child, then left rotation.

The `Rebalance` method updates a node's height, computes its balance factor, and applies the appropriate rotation.

### Insertion (`Add`)
Insertion follows the standard BST rule: recurse left for smaller values, right for larger values, and stop for equal values (duplicates are ignored). As the recursion unwinds, each ancestor node is rebalanced. A `ref bool` flag reports whether a new node was actually created so the count can be updated accurately.

### Deletion (`Remove`)
Deletion locates the target node. If it has zero or one child, the node is replaced by its (possibly null) child. If it has two children, the in-order successor (the smallest value in the right subtree) replaces the node's value, and the successor is then deleted from the right subtree. Ancestors are rebalanced during unwinding.

### Lookup (`Contains`)
An iterative binary search from the root: go left for smaller targets, right for larger, and return true on a match.

### Sorted retrieval (`ToSortedArray`)
An in-order traversal visits nodes in ascending order, collecting all values into an array.

## 4. Complexity Analysis

Let `n` be the number of elements in the list. Because the AVL tree keeps its height bounded by `O(log n)`:

| Operation          | Time Complexity | Space Complexity |
|--------------------|-----------------|------------------|
| `Add`              | O(log n)        | O(log n) (recursion stack) |
| `Remove`           | O(log n)        | O(log n) (recursion stack) |
| `Contains`         | O(log n)        | O(1)             |
| `ToSortedArray`    | O(n)            | O(n)             |
| `Count`            | O(1)            | O(1)             |
| `Clear`            | O(1)            | O(1)             |

The overall space used by the structure is `O(n)` to store all the nodes. The recursive insertion and deletion use stack space proportional to the tree height, which is `O(log n)`.
