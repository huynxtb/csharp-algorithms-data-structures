# Recursive Tree Traversal (Pre-order)

### 1. Introduction
Pre-order traversal is a depth-first search (DFS) algorithm used to visit all nodes in a binary tree. The traversal order is **Root**, **Left subtree**, then **Right subtree**. It is commonly used to create a copy of a tree, serialize/deserialize a tree structure, or evaluate prefix expressions.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        // Constructing a binary tree:
        //        1
        //       / \
        //      2   3
        TreeNode<int> root = new TreeNode<int>(1);
        root.Left = new TreeNode<int>(2);
        root.Right = new TreeNode<int>(3);

        // Traverse and print values
        PreOrderTraversal<int>.Traverse(root, val => Console.Write(val + " "));
        // Output: 1 2 3
    }
}
```

### 3. Detailed Explanation
The implementation uses recursion to traverse the binary tree:
1. **Base Case**: If the current node (`root`) is null, the method returns immediately.
2. **Visit Root**: The `visitAction` delegate is executed on the current node's value.
3. **Recurse Left**: The method calls itself passing the `Left` child node.
4. **Recurse Right**: The method calls itself passing the `Right` child node.

### 4. Complexity Analysis
- **Time Complexity**: $O(N)$ where $N$ is the total number of nodes in the tree, as each node is visited exactly once.
- **Space Complexity**: $O(H)$ where $H$ is the height of the tree, representing the maximum depth of the call stack. In the worst case (skewed tree), space complexity is $O(N)$. In the best case (balanced tree), space complexity is $O(\log N)$.