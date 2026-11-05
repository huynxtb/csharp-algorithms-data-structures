# Introduction
An AVL Tree is a self-balancing binary search tree where the difference between heights of left and right subtrees cannot be more than one for all nodes. It is used when frequent insertions and deletions are required while maintaining sorted data.

# Usage
```csharp
AVLTree avlTree = new AVLTree();
avlTree.Insert(10);
avlTree.Insert(20);
avlTree.Insert(30);
avlTree.Delete(20);
avlTree.InOrderTraversal(); // Outputs: 10 30
bool isBalanced = avlTree.IsBalanced(); // Checks if the tree is balanced
```

# Detailed Explanation
The AVL Tree maintains balance through rotations during insertions and deletions. Each node keeps track of its height, and the balance factor is calculated as the difference between the heights of the left and right subtrees. If the balance factor exceeds 1 or -1, rotations are performed to restore balance. The tree supports efficient operations in O(log n) time complexity.

# Complexity Analysis
- **Insertion:** O(log n)
- **Deletion:** O(log n)
- **Lookup:** O(log n)
- **Space Complexity:** O(n) for storing nodes.