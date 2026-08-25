# Recursive Tree Traversal (In-order)

## Introduction
In-order traversal is a depth-first tree traversal algorithm. It visits nodes in the following order: left subtree, root node, right subtree. When applied to a Binary Search Tree (BST), in-order traversal retrieves the values in non-decreasing sorted order.

## Usage
```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Constructing the tree:
        //      4
        //     / \
        //    2   5
        //   / \
        //  1   3
        TreeNode<int> root = new TreeNode<int>(4,
            new TreeNode<int>(2,
                new TreeNode<int>(1),
                new TreeNode<int>(3)
            ),
            new TreeNode<int>(5)
        );

        List<int> result = BinaryTreeTraversal.InOrderTraversal(root);
        Console.WriteLine(string.Join(", ", result)); // Output: 1, 2, 3, 4, 5
    }
}
```

## Detailed Explanation
The implementation uses a helper method `InOrderHelper` to perform the recursion:
1. **Base Case**: If the current node is `null`, the recursion halts and returns.
2. **Recursive Step**: 
   - Recursively traverse the left subtree.
   - Visit the current node by adding its value to the result list.
   - Recursively traverse the right subtree.

## Complexity Analysis
- **Time Complexity**: $O(N)$ where $N$ is the number of nodes in the tree, as every node is visited exactly once.
- **Space Complexity**: $O(H)$ where $H$ is the height of the tree, representing the maximum call stack depth. In the worst case (skewed tree), this is $O(N)$. In the best case (balanced tree), this is $O(\log N)$.