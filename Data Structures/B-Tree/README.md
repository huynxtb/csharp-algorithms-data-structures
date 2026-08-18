# B-Tree

## 1. Introduction
A B-Tree is a self-balancing search tree data structure that maintains sorted data and allows searches, sequential access, insertions, and deletions in logarithmic time. It is optimized for systems that read and write large blocks of data, making it commonly used in databases and file systems.

## 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        // Initialize a B-Tree with a minimum degree of 3
        BTree<int, string> btree = new BTree<int, string>(3);

        // Insert key-value pairs
        btree.Insert(10, "Ten");
        btree.Insert(20, "Twenty");
        btree.Insert(5, "Five");

        // Search for a value
        string value = btree.Search(20);
        Console.WriteLine($"Found: {value}"); // Output: Twenty

        // Delete a key
        bool deleted = btree.Delete(10);
        Console.WriteLine($"Deleted 10: {deleted}"); // Output: True
    }
}
```

## 3. Detailed Explanation
This implementation uses a nested `BTreeNode` class containing lists for keys, values, and child nodes. 
* **Insertion**: Traverses down the tree. If a node is full (contains $2t - 1$ keys), it is split into two nodes, and the median key is pushed up to the parent. This ensures that nodes never exceed their capacity.
* **Search**: Performs a binary-like search within the keys of a node. If the key is not found, it recurses into the appropriate child node.
* **Deletion**: Follows the CLRS algorithm. It handles cases where keys are in leaf nodes or internal nodes. If a child node has fewer than $t$ keys, it borrows a key from a sibling or merges with a sibling to maintain the B-Tree invariant (minimum $t - 1$ keys per node).

## 4. Complexity Analysis
* **Time Complexity**:
  * **Search**: $O(\log n)$
  * **Insertion**: $O(\log n)$
  * **Deletion**: $O(\log n)$
* **Space Complexity**: $O(n)$ to store $n$ elements.