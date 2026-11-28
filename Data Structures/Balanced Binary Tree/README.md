# Balanced Binary Tree

## Introduction
A Balanced Binary Tree is a data structure that maintains sorted data in a way that allows for efficient insertion, deletion, and retrieval operations while keeping the height of the tree minimized. This structure is particularly useful when frequent insertions and deletions are required, as it ensures that operations remain efficient.

## Usage
```csharp
BalancedBinaryTree tree = new BalancedBinaryTree();
tree.Insert(10);
tree.Insert(20);
tree.Insert(30);
tree.Delete(20);
bool found = tree.Search(10);
List<int> inOrder = tree.InOrderTraversal();
List<int> preOrder = tree.PreOrderTraversal();
List<int> postOrder = tree.PostOrderTraversal();
```

## Detailed Explanation
The implementation consists of a private `Node` class that represents each node in the tree, containing the value, left and right children, and the height of the node. The main class `BalancedBinaryTree` provides methods for inserting, deleting, and searching for values, as well as performing in-order, pre-order, and post-order traversals. The tree remains balanced after each insertion and deletion using rotations based on the balance factor of the nodes.

## Complexity Analysis
- **Insertion:** O(log n)
- **Deletion:** O(log n)
- **Search:** O(log n)
- **Space Complexity:** O(n) for storing the nodes in the tree.