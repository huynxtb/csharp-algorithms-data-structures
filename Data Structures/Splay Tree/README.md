# Splay Tree

### 1. Introduction
A Splay Tree is a self-adjusting binary search tree. Recently accessed elements are quick to access again because operations like lookup, insertion, and deletion trigger a "Splay" operation. This operation performs tree rotations to move the target node (or the last accessed node if the target is not found) to the root of the tree.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        SplayTree<string, int> tree = new SplayTree<string, int>();

        // Insert elements
        tree.Insert("apple", 10);
        tree.Insert("banana", 20);
        tree.Insert("cherry", 30);

        // Retrieve elements
        if (tree.TryGetValue("banana", out int value))
        {
            Console.WriteLine($"Found banana: {value}");
        }

        // Delete elements
        bool deleted = tree.Delete("apple");
        Console.WriteLine($"Deleted apple: {deleted}");

        // Get minimum and maximum
        Console.WriteLine($"Min key: {tree.GetMinimum()}");
        Console.WriteLine($"Max key: {tree.GetMaximum()}");
    }
}
```

### 3. Detailed Explanation
- **Splay Operation**: The core mechanism. When a node is accessed, it is rotated to the root using three types of steps:
  - **Zig**: Performed when the node's parent is the root. A single rotation is executed.
  - **Zig-Zig**: Performed when both the node and its parent are left children, or both are right children. The parent is rotated first, then the node.
  - **Zig-Zag**: Performed when the node is a right child and its parent is a left child (or vice versa). The node is rotated first, then rotated again in the opposite direction.
- **Insert**: Standard BST insertion followed by a splay of the new node to the root. If the key already exists, its value is updated and the node is splayed.
- **TryGetValue**: Searches for the key. If found, the node is splayed to the root. If not found, the last accessed node is splayed to the root.
- **Delete**: Splays the target node to the root. Once at the root, the node is removed, leaving two subtrees. The maximum node of the left subtree is splayed to its root (ensuring it has no right child), and the right subtree is attached as its right child.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Insert**: Amortized $O(\log n)$, Worst-case $O(n)$
  - **Search (TryGetValue)**: Amortized $O(\log n)$, Worst-case $O(n)$
  - **Delete**: Amortized $O(\log n)$, Worst-case $O(n)$
- **Space Complexity**: $O(n)$ to store the nodes.