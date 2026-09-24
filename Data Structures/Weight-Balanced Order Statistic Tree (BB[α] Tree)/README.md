# Weight-Balanced Order Statistic Tree (BB[α] Tree)

## Introduction

A **Weight-Balanced Order Statistic Tree** is an augmented self-balancing binary search tree that combines two key capabilities:

1. **Order Statistic Queries**: Efficient rank and select operations that allow you to find the k-th smallest element or the rank position of any element in O(log n) time.
2. **Weight-Based Balancing**: Uses the BB[α] (Nievergelt-Reingold) balance criterion, which maintains balance by ensuring that the ratio of subtree sizes stays within a bounded interval defined by parameter α (typically α ≈ 0.292 = 1 - √2/2).

### When to Use

- **Ordered Data with Rank/Select Queries**: When you need to maintain a sorted collection and frequently query elements by position or compute the rank of elements.
- **Balanced Tree with Weight Consideration**: When you want a self-balancing tree that considers subtree sizes rather than height (as in AVL trees), providing more nuanced balance properties.
- **Statistical Analysis**: In applications like computing medians, percentiles, or order statistics in dynamic datasets.

### Key Features

- **O(log n) Insert, Remove, Contains**: Standard BST operations with automatic rebalancing.
- **O(log n) Select(k)**: Return the k-th smallest element (0-indexed).
- **O(log n) Rank(value)**: Return the number of elements strictly less than a given value.
- **O(n) In-Order Traversal**: Iterative traversal with explicit stack (no recursion).
- **Duplicate Support**: The implementation allows and correctly handles duplicate values.

## Usage

```csharp
using System;
using System.Collections.Generic;
using DataStructures.Trees;

// Create a tree for integers
var tree = new WeightBalancedOrderStatisticTree<int>();

// Insert values
tree.Insert(50);
tree.Insert(30);
tree.Insert(70);
tree.Insert(20);
tree.Insert(40);
tree.Insert(60);
tree.Insert(80);

// Check size
Console.WriteLine($"Tree size: {tree.Count}"); // Output: 7

// Check membership
bool exists = tree.Contains(40); // true

// Get the k-th smallest element (0-indexed)
int thirdSmallest = tree.Select(2); // Returns 40 (0:20, 1:30, 2:40)

// Get the rank of an element
int rankOf50 = tree.Rank(50); // Returns 4 (elements < 50: 20, 30, 40)

// Get min and max
int minVal = tree.Min(); // 20
int maxVal = tree.Max(); // 80

// In-order traversal
foreach (int value in tree.InOrderTraversal())
{
    Console.WriteLine(value); // Prints: 20, 30, 40, 50, 60, 70, 80
}

// Remove an element
bool removed = tree.Remove(40); // true
Console.WriteLine($"Tree size after removal: {tree.Count}"); // 6

// Clear the tree
tree.Clear();
Console.WriteLine($"Tree size after clear: {tree.Count}"); // 0
```

## Detailed Explanation

### Weight-Balanced Tree (BB[α] Tree) Concept

Unlike AVL trees that use height constraints, BB[α] trees use **weight constraints**. The weight of a node is defined as:
```
weight(node) = size(node) + 1
```
where `size(node)` is the number of nodes in the subtree rooted at that node.

The **balance invariant** requires that for every internal node:
```
Alpha * weight(parent) <= weight(left_child) <= (1 - Alpha) * weight(parent)
Alpha * weight(parent) <= weight(right_child) <= (1 - Alpha) * weight(parent)
```

When this invariant is violated (a child is too light), rebalancing occurs.

### Order Statistic Augmentation

Every node stores its **subtree size**. This allows:

1. **Select(k)**: Navigate to the k-th smallest by checking if k is in the left subtree, at the current node, or in the right subtree.
2. **Rank(value)**: During a search for `value`, accumulate the count of nodes smaller than `value` to determine its rank.

### Rebalancing Strategy

When a node's weight violates the balance invariant:

1. **Single Rotation** (Left or Right): Used when the heavier grandchild is on the outer side relative to the unbalanced child.
2. **Double Rotation** (Left-Right or Right-Left): Used when the heavier grandchild is on the inner side, providing better balance properties.

The decision is made using parameter **β ≈ 0.35** (or derived from α). If the inner grandchild's weight exceeds β times the unbalanced child's weight, a double rotation is preferred.

### Deletion

Deletion uses the standard BST approach:
- If the node has no children, remove it directly.
- If it has one child, replace it with that child.
- If it has two children, replace it with the in-order successor (leftmost node in the right subtree), then recursively delete the successor.
- After each removal step, sizes are updated and rebalancing is applied along the path back to the root.

### Complexity Analysis

The weight-balanced property ensures **O(log n) height**. Specifically:
- The height h satisfies: h ≤ log₂(n) / log₂(1/α) ≈ 1.44 * log₂(n)
- This guarantees all BST operations (Insert, Remove, Contains, Select, Rank) are **O(log n)**.

## Complexity Analysis

### Time Complexity

| Operation | Complexity | Notes |
|-----------|------------|-------|
| **Insert(value)** | O(log n) | Insertion followed by rebalancing up to root. |
| **Remove(value)** | O(log n) | Search, removal (with successor handling if needed), and rebalancing up to root. |
| **Contains(value)** | O(log n) | Standard BST search. |
| **Select(k)** | O(log n) | Navigate using subtree sizes; O(log n) path length. |
| **Rank(value)** | O(log n) | Search-based rank computation along O(log n) path. |
| **Min() / Max()** | O(log n) | Traverse to leftmost/rightmost node. |
| **InOrderTraversal()** | O(n) | Visits each node once using iterative stack-based traversal. |
| **Clear()** | O(1) | Simple root reset. |
| **Count (property)** | O(1) | Direct access to root's size. |

### Space Complexity

| Aspect | Complexity | Notes |
|--------|------------|-------|
| **Tree Storage** | O(n) | Each of n elements stored in a node. |
| **Recursion/Stack** | O(log n) | During insertion, deletion, and search operations; iterative traversal uses explicit O(log n) stack. |
| **InOrderTraversal** | O(log n) | Explicit stack for iterative traversal (not using system call stack). |

### Balance Properties

The BB[α] tree guarantees:
- **Height**: h ≤ log₁/α(n) ≈ 1.44 * log₂(n)
- **Rebalancing Cost**: O(1) rotations per insertion/deletion (amortized).
- **No Height Rebuilds**: Unlike some trees, BB[α] does not require global rebuilds.
