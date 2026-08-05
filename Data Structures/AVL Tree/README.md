# AVL Tree

## 1. Introduction
An AVL Tree (Adelson-Velsky and Landis Tree) is a self-balancing binary search tree (BST). In an AVL tree, the heights of the two child subtrees of any node differ by at most one. If at any time they differ by more than one, rebalancing is done to restore this property. It is ideal for lookup-intensive applications where search times must remain strictly bounded.

## 2. Usage
```csharp
using System;

public class Program
{
    public static void Main()
    {
        AvlTree<int> tree = new AvlTree<int>();

        // Insert elements
        tree.Insert(10);
        tree.Insert(20);
        tree.Insert(30); // Triggers left rotation
        tree.Insert(5);

        // Check existence
        bool containsTwenty = tree.Contains(20);

        // Delete elements
        tree.Delete(20);

        // Traverse elements in sorted order
        foreach (int val in tree.InOrderTraversal())
        {
            Console.WriteLine(val);
        }
    }
}
```

## 3. Detailed Explanation
The implementation maintains balance using four types of rotations:
- **Left-Left (LL) / Single Right Rotation**: Executed when a node becomes left-heavy and its left child is also left-heavy.
- **Right-Right (RR) / Single Left Rotation**: Executed when a node becomes right-heavy and its right child is also right-heavy.
- **Left-Right (LR) Rotation**: Executed when a node is left-heavy and its left child is right-heavy. A left rotation is performed on the left child, followed by a right rotation on the node itself.
- **Right-Left (RL) Rotation**: Executed when a node is right-heavy and its right child is left-heavy. A right rotation is performed on the right child, followed by a left rotation on the node itself.

During deletion, if the target node has two children, it is replaced by its in-order successor (the minimum node in its right subtree). The tree is then rebalanced from the deletion point up to the root.

## 4. Complexity Analysis
- **Time Complexity**:
  - **Insert**: O(log n)
  - **Delete**: O(log n)
  - **Contains**: O(log n)
  - **InOrderTraversal**: O(n)
- **Space Complexity**: O(n) to store the nodes. Auxiliary space for recursive call stacks is O(log n).