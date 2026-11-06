# Weight-Balanced Tree

## Introduction
A Weight-Balanced Tree is a self-balancing binary search tree that maintains a dynamic set of items in sorted order. It balances itself based on the sizes of its subtrees, ensuring efficient search, insertion, and deletion operations. This data structure is particularly useful when frequent updates and queries are required on a sorted dataset.

## Usage
```csharp
WeightBalancedTree<int> tree = new WeightBalancedTree<int>();
tree.Insert(10);
tree.Insert(5);
tree.Insert(15);

bool exists = tree.Search(10); // returns true

tree.Delete(10);
exists = tree.Search(10); // returns false
```

## Detailed Explanation
The implementation consists of a nested `Node` class that holds the value, left and right children, and the size of the subtree rooted at that node. The main operations include:
- **Insert**: Adds a new value while maintaining the binary search tree property and updating sizes. It then balances the tree if necessary.
- **Delete**: Removes a value, replacing it with its in-order successor if needed, and balances the tree afterward.
- **Search**: Checks for the existence of a value in the tree.
- **Balance**: Ensures the tree remains balanced based on the sizes of its subtrees, performing rotations as needed.

## Complexity Analysis
- **Insertion**: O(log n) on average, O(n) in the worst case.
- **Deletion**: O(log n) on average, O(n) in the worst case.
- **Search**: O(log n) on average, O(n) in the worst case.
- **Space Complexity**: O(n) for storing the nodes in the tree.