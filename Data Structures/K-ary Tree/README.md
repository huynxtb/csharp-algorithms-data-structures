# K-ary Tree

## 1. Introduction

A K-ary Tree is a generalization of a binary tree (where K=2) and a ternary tree (where K=3). In a K-ary tree, each node can have up to `K` children. This data structure is particularly useful for representing hierarchical data where the branching factor is greater than two but bounded. Examples include file systems (where a directory can have multiple subdirectories), organizational charts, or game trees where each state can lead to a limited number of next states.

This implementation provides a flexible and robust K-ary tree structure, allowing for dynamic node insertion, systematic traversal, and efficient searching.

## 2. Usage

Here's a C# example demonstrating how to use the `KaryTree` implementation:

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        // Create a 3-ary tree (each node can have up to 3 children)
        KaryTree<int> tree = new KaryTree<int>(3);

        // Set the root of the tree
        tree.SetRoot(10);
        Console.WriteLine("Root set: 10");

        // Add children to the root
        tree.AddChild(10, 20); // Add 20 as child of 10
        tree.AddChild(10, 30); // Add 30 as child of 10
        tree.AddChild(10, 40); // Add 40 as child of 10
        Console.WriteLine("Children added to 10: 20, 30, 40");

        // Try to add a 4th child to 10 (should fail as K=3)
        bool added = tree.AddChild(10, 50);
        Console.WriteLine($"Attempt to add 50 to 10 (K=3): {added} (Expected: False)");

        // Add children to node 20
        tree.AddChild(20, 60);
        tree.AddChild(20, 70);
        Console.WriteLine("Children added to 20: 60, 70");

        // Add a child to node 30
        tree.AddChild(30, 80);
        Console.WriteLine("Child added to 30: 80");

        // Perform Pre-order Traversal
        List<int> preOrderResult = tree.PreOrderTraversal();
        Console.Write("\nPre-order Traversal: ");
        foreach (int val in preOrderResult)
        {
            Console.Write($"{val} ");
        }
        Console.WriteLine(); // Expected: 10 20 60 70 30 80 40

        // Search for a value
        KaryTreeNode<int> foundNode = tree.Search(70);
        if (foundNode != null)
        {
            Console.WriteLine($"\nNode with value 70 found: {foundNode.Value}");
        }
        else
        {
            Console.WriteLine("\nNode with value 70 not found.");
        }

        foundNode = tree.Search(99);
        if (foundNode != null)
        {
            Console.WriteLine($"Node with value 99 found: {foundNode.Value}");
        }
        else
        {
            Console.WriteLine("Node with value 99 not found.");
        }

        // Demonstrate adding child using a node reference
        KaryTreeNode<int> node40 = tree.Search(40);
        if (node40 != null)
        {
            tree.AddChild(node40, 90);
            Console.WriteLine("Child 90 added to node 40 using node reference.");
        }

        // Pre-order Traversal again to see new node
        preOrderResult = tree.PreOrderTraversal();
        Console.Write("Pre-order Traversal (after adding 90): ");
        foreach (int val in preOrderResult)
        {
            Console.Write($"{val} ");
        }
        Console.WriteLine(); // Expected: 10 20 60 70 30 80 40 90
    }
}
```

## 3. Detailed Explanation

The K-ary Tree implementation consists of two main classes: `KaryTreeNode<T>` and `KaryTree<T>`.

### `KaryTreeNode<T>`

This class represents a single node within the K-ary tree. It is generic (`<T>`) to allow storing any data type. It has two key properties:

*   `Value`: Stores the actual data associated with the node.
*   `Children`: A `List<KaryTreeNode<T>>` that holds references to its child nodes. The size of this list is constrained by the `K` value defined in the `KaryTree`.

The constructor simply initializes the node with a given value and an empty list of children.

### `KaryTree<T>`

This class manages the overall tree structure and provides methods for tree operations. It also uses generics (`<T>`).

*   **Constructor (`KaryTree(int k)`):**
    *   Initializes the tree with a specified maximum number of children (`K`) that any node can have. It throws an `ArgumentOutOfRangeException` if `K` is not a positive integer, ensuring valid tree configuration.

*   **`SetRoot(T value)`:**
    *   Establishes the very first node of the tree. It can only be called once. If a root already exists, it throws an `InvalidOperationException` to prevent accidental overwriting of the tree's foundation.

*   **`AddChild(KaryTreeNode<T> parentNode, T childValue)`:**
    *   This is the core method for adding a child. It takes an existing `KaryTreeNode<T>` object as the parent. It performs two crucial checks:
        1.  **Null Parent:** If `parentNode` is `null`, it returns `false`, indicating that the child cannot be added to a non-existent parent.
        2.  **K Limit:** It checks if the `parentNode` already has `K` children. If so, it returns `false`, enforcing the K-ary tree constraint.
    *   If checks pass, a new `KaryTreeNode<T>` is created with `childValue` and added to the `parentNode.Children` list.

*   **`AddChild(T parentValue, T childValue)`:**
    *   This is a convenience method for users who only have the `value` of the parent node, not its direct object reference. It first calls the `Search` method to find the `KaryTreeNode<T>` corresponding to `parentValue`. If the parent is found, it then delegates to the `AddChild(KaryTreeNode<T>, T)` method. It also handles the case where the tree is empty (no root).

*   **`PreOrderTraversal()`:**
    *   Initiates a pre-order traversal of the tree. It returns a `List<T>` containing the values of the nodes in the order they were visited. It calls a private recursive helper method `PreOrderTraversalRecursive` starting from the `Root`.

*   **`PreOrderTraversalRecursive(KaryTreeNode<T> node, List<T> result)`:**
    *   This private recursive method implements the pre-order logic:
        1.  **Base Case:** If the current `node` is `null`, it returns.
        2.  **Visit Node:** It adds the `node.Value` to the `result` list.
        3.  **Recurse Children:** It then iterates through all `node.Children` and recursively calls itself for each child.

*   **`Search(T value)`:**
    *   This method searches for a node with a specific `value` within the tree. It uses a Breadth-First Search (BFS) algorithm, which is suitable for finding nodes in a tree structure. It works as follows:
        1.  **Empty Tree:** If the `Root` is `null`, it immediately returns `null`.
        2.  **Queue Initialization:** It uses a `Queue<KaryTreeNode<T>>` to manage nodes to visit. The `Root` is enqueued first.
        3.  **BFS Loop:** While the queue is not empty, it dequeues a `current` node. If `current.Value` matches the `value` being searched for (using `Equals` for generic type comparison), the `current` node is returned. Otherwise, all children of the `current` node are enqueued for future processing.
        4.  **Not Found:** If the loop finishes and the value is not found, it returns `null`.

## 4. Complexity Analysis

Let `N` be the total number of nodes in the K-ary tree and `H` be the height of the tree.

### Time Complexity

*   **`KaryTree(int k)` (Constructor):** O(1)
    *   Initializes properties with constant time operations.
*   **`SetRoot(T value)`:** O(1)
    *   Assigns a value to the root and performs a null check.
*   **`AddChild(KaryTreeNode<T> parentNode, T childValue)`:** O(1)
    *   Assumes the `parentNode` reference is already available. It involves constant time checks and adding to a list (amortized O(1)).
*   **`AddChild(T parentValue, T childValue)`:** O(N) in the worst case.
    *   This method calls `Search(parentValue)`, which might traverse the entire tree to find the parent node.
*   **`PreOrderTraversal()`:** O(N)
    *   Every node in the tree is visited exactly once.
*   **`Search(T value)`:** O(N) in the worst case.
    *   In the worst-case scenario (e.g., the value is not in the tree, or it's the last node visited), the BFS algorithm might have to visit all `N` nodes.

### Space Complexity

*   **`KaryTreeNode<T>`:** O(K) per node.
    *   Each node stores its `Value` and a `List<KaryTreeNode<T>>` for its children. In the worst case, this list can hold `K` references.
*   **`KaryTree<T>`:** O(N)
    *   The tree itself stores all `N` nodes.
*   **`PreOrderTraversal()`:** O(H) for the recursion stack.
    *   The space used by the recursion stack is proportional to the height of the tree. In a skewed tree (like a linked list), `H` can be `N`. In a balanced tree, `H` is `log_K(N)`.
*   **`Search(T value)`:** O(W) for the queue.
    *   The space used by the queue in BFS is proportional to the maximum width of the tree (the maximum number of nodes at any single level). In a complete K-ary tree, the widest level can contain up to `K^(H-1)` nodes. In the worst case (a very flat tree), `W` can be `N` (e.g., a root with `N-1` children).