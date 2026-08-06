# Red-Black Tree

### 1. Introduction
A Red-Black Tree is a self-balancing binary search tree where each node has an extra bit representing color (`Red` or `Black`). These colors ensure the tree remains approximately balanced during insertions and deletions, preventing worst-case $O(n)$ search times associated with unbalanced binary search trees.

Use a Red-Black Tree when you need a sorted associative container with guaranteed $O(\log n)$ performance for lookups, insertions, and deletions, such as in memory-constrained environments or database indexing.

### 2. Usage

```csharp
using System;

class Program
{
    static void Main()
    {
        RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

        // Insertion
        tree.Add("apple", 10);
        tree.Add("banana", 20);
        tree.Add("cherry", 30);

        // Retrieval
        if (tree.TryGetValue("banana", out int value))
        {
            Console.WriteLine($"Found banana: {value}");
        }

        // Update via Indexer
        tree["apple"] = 15;

        // Deletion
        bool removed = tree.Remove("cherry");
        Console.WriteLine($"Removed cherry: {removed}");

        // Iteration
        foreach (var kvp in tree)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}
```

### 3. Detailed Explanation
This implementation uses a sentinel node (`nil`) to represent leaf nodes, simplifying boundary conditions during rotations and recoloring. 

* **Insertion**: Nodes are initially inserted as `Red`. If this violates the Red-Black properties (e.g., a Red node has a Red parent), `InsertFixup` performs rotations and recoloring based on the color of the node's uncle.
* **Deletion**: Standard BST deletion is performed. If the deleted node is `Black`, it violates the black-height property. `DeleteFixup` restores balance by performing rotations and recoloring based on sibling node configurations.
* **Rotations**: `LeftRotate` and `RightRotate` modify pointer references locally to adjust tree height while preserving BST ordering.

### 4. Complexity Analysis

| Operation | Time Complexity | Space Complexity |
| :--- | :--- | :--- |
| **Search** | $O(\log n)$ | $O(1)$ |
| **Insertion** | $O(\log n)$ | $O(1)$ |
| **Deletion** | $O(\log n)$ | $O(1)$ |
| **Space Complexity** | - | $O(n)$ |