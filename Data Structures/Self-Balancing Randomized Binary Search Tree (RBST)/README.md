# Self-Balancing Randomized Binary Search Tree (RBST)

## Introduction
The Self-Balancing Randomized Binary Search Tree (RBST) is a data structure that maintains a balanced binary search tree while supporting efficient operations such as insertion, deletion, and search. It is particularly useful when you need to maintain a dynamic set of items and perform frequent insertions and deletions while ensuring logarithmic average time complexity for these operations.

## Usage
```csharp
SelfBalancingRBST<int> tree = new SelfBalancingRBST<int>();
tree.Insert(10);
tree.Insert(20);
tree.Insert(5);
bool found = tree.Search(10); // returns true
List<int> sortedElements = tree.InOrderTraversal(); // returns [5, 10, 20]
tree.Delete(10);
sortedElements = tree.InOrderTraversal(); // returns [5, 20]
```

## Detailed Explanation
The RBST uses a randomized priority for each node to maintain balance. When inserting or deleting nodes, the tree is restructured using rotations to ensure that the height of the tree remains logarithmic relative to the number of nodes. The `Insert` method adds a new value while maintaining the binary search property, and the `Delete` method removes a value and restructures the tree accordingly. The `Search` method checks for the existence of a value, and `InOrderTraversal` retrieves the elements in sorted order.

## Complexity Analysis
- **Insertion:** Average O(log n), Worst O(n) (rare cases)
- **Deletion:** Average O(log n), Worst O(n) (rare cases)
- **Search:** Average O(log n), Worst O(n) (rare cases)
- **Space Complexity:** O(n) for storing the nodes in the tree.