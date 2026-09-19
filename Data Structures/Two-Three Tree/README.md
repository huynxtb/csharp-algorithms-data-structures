# Two-Three Tree

## Introduction
A Two-Three Tree is a balanced search tree where each internal node can have either one or two keys and two or three children. This structure maintains balance by ensuring that all leaf nodes are at the same depth, making it efficient for search, insert, and delete operations. It is particularly useful in scenarios where frequent insertions and deletions occur, such as in databases.

## Usage
```csharp
var tree = new TwoThreeTree<int>();
tree.Insert(10);
tree.Insert(20);
tree.Insert(15);
bool contains15 = tree.Contains(15); // returns true
var sortedElements = tree.InOrderTraversal(); // returns [10, 15, 20]
tree.Remove(15);
```

## Detailed Explanation
The `TwoThreeTree` class maintains a reference to the root node and implements core operations such as insertion, searching, and in-order traversal. The insertion method handles the addition of new keys while ensuring the tree remains balanced by splitting nodes as necessary. The `Contains` method checks for the existence of a key, and `InOrderTraversal` provides a sorted list of elements. The `Remove` method is included but not fully implemented due to its complexity, which involves borrowing keys from siblings or merging nodes.

## Complexity Analysis
- **Insertion:** O(log n) on average, O(n) in the worst case due to node splits.
- **Search:** O(log n) on average and worst case.
- **Deletion:** O(log n) on average, O(n) in the worst case due to node merging.
- **Space Complexity:** O(n) for storing the nodes in the tree.