# Self-Balancing Randomized Binary Search Tree (RBST)

## 1. Introduction
A **Randomized Binary Search Tree (RBST)** is a binary search tree that uses randomization to keep itself balanced without storing explicit balance factors, heights, or colors. On every insertion and deletion, a probabilistic decision is made about which node becomes the local root of a subtree. The expected height of the tree is O(log n) regardless of the insertion order.

Use an RBST when you need a dynamic ordered set/multiset supporting:
- Fast search, insertion, and deletion (expected O(log n)).
- Ordered iteration (in-order traversal for sorted output).
- Handling of duplicate values.
- A self-balancing structure that is simpler to implement than AVL or Red-Black trees while still avoiding worst-case degeneration for typical inputs.

## 2. Usage
```csharp
public static class Example
{
    public static void Run()
    {
        // Create the tree (optionally pass a seed for reproducible results).
        var tree = new RandomizedBST<int>(seed: 42);

        // Insert values (duplicates allowed).
        int[] values = { 50, 30, 70, 20, 40, 30, 60, 80 };
        foreach (int v in values)
        {
            tree.Insert(v);
        }

        // Search for elements.
        bool hasForty = tree.Contains(40);   // true
        bool hasNinety = tree.Contains(90);  // false

        // Remove a single occurrence of a value.
        bool removed = tree.Remove(30);      // true (one of the two 30s removed)

        // In-order traversal returns sorted elements.
        System.Collections.Generic.List<int> sorted = tree.InOrderTraversal();

        System.Console.WriteLine("Contains 40: " + hasForty);
        System.Console.WriteLine("Contains 90: " + hasNinety);
        System.Console.WriteLine("Removed a 30: " + removed);
        System.Console.WriteLine("Count: " + tree.Count);
        System.Console.WriteLine("Sorted: " + string.Join(", ", sorted));
    }
}
```

## 3. Detailed Explanation
The implementation relies on two fundamental operations, **split** and **merge**, together with subtree size counters.

- **Node & Encapsulation:** Each `Node` stores a value, left/right child references, and the `Size` of its subtree. The `Node` class is a `private sealed` nested type, so callers cannot manipulate the internal structure. Only well-defined public methods (`Insert`, `Remove`, `Contains`, `InOrderTraversal`, `Clear`, `Count`) are exposed.

- **Subtree Sizes:** Every node maintains the number of nodes in its subtree. Sizes drive the probability decisions that keep the tree balanced and are recalculated (`UpdateSize`) whenever structure changes.

- **Insertion:** When inserting into a subtree of size `s`, with probability `1/(s+1)` the new value becomes the root of that subtree (via `InsertAtRoot`), otherwise it recurses left or right depending on comparison. Making a node the root uses **Split**, which divides an existing subtree into a part with values less than the new value and a part with values greater than or equal to it. This mirrors inserting into a truly random BST, guaranteeing expected logarithmic height.

- **Deletion:** To delete a value, the tree is traversed to find the target node. That node is then replaced by the **randomized merge** of its two children. `Merge` combines two subtrees (where all left values are less than all right values) by randomly choosing which side's root becomes the new root, proportional to subtree sizes—again preserving the random-tree property.

- **Search:** A standard iterative BST descent comparing the target with the current node.

- **In-Order Traversal:** A recursive left-node-right walk that produces elements in ascending sorted order.

- **Duplicates:** Equal values are routed to the right subtree during insertion, so multiple occurrences coexist; `Remove` deletes exactly one matching occurrence.

## 4. Complexity Analysis
Let `n` be the number of elements in the tree.

| Operation           | Expected Time | Worst Case | Space (auxiliary) |
|---------------------|---------------|------------|-------------------|
| Insert              | O(log n)      | O(n)       | O(log n) recursion |
| Remove              | O(log n)      | O(n)       | O(log n) recursion |
| Contains (search)   | O(log n)      | O(n)       | O(1)              |
| InOrderTraversal    | O(n)          | O(n)       | O(n) output + O(log n) recursion |
| Count               | O(1)          | O(1)       | O(1)              |

- **Expected height** is O(log n) because the randomized root selection makes the structure equivalent to a randomly built BST, independent of input order.
- **Worst case** O(n) can occur but is exponentially unlikely due to the randomization.
- **Overall space** for storing the tree is O(n).
