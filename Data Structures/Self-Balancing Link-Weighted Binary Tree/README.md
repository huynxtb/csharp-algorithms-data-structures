# Self-Balancing Link-Weighted Binary Tree

## Introduction
The Self-Balancing Link-Weighted Binary Tree is a data structure that maintains a balance between the heights of its left and right subtrees while allowing for link weights that influence balance decisions. This tree structure is particularly useful when you need efficient insertion, deletion, and search operations, along with the ability to retrieve the path with the maximum total weight from the root to any leaf node.

## Usage
```csharp
var tree = new SelfBalancingLinkWeightedBinaryTree();
tree.Insert(10, 5);
tree.Insert(20, 10);
tree.Insert(5, 2);
tree.Delete(10);
var maxPath = tree.GetMaxWeightedPath();
```

## Detailed Explanation
The implementation consists of a private `Node` class that holds the value, weight, left and right children, and height of each node. The main class provides methods for inserting and deleting nodes while maintaining balance through rotations. The `GetMaxWeightedPath` method traverses the tree to find the path from the root to a leaf node that has the maximum total weight. Balancing is achieved using AVL tree principles, ensuring that the tree remains balanced after each insertion or deletion.

## Complexity Analysis
- **Insertion:** O(log n) on average, O(n) in the worst case (when the tree becomes unbalanced).
- **Deletion:** O(log n) on average, O(n) in the worst case.
- **Search:** O(log n) on average, O(n) in the worst case.
- **GetMaxWeightedPath:** O(n) as it may need to traverse all nodes in the tree.
- **Space Complexity:** O(n) for storing the nodes in the tree.