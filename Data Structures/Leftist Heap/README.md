# Leftist Heap (Leftist Tree)

## 1. Introduction
A **Leftist Heap** (also known as a **Leftist Tree**) is a priority queue data structure implemented as a binary tree with a structural bias toward the left. It is specifically designed to support efficient, sublinear merge operations ($O(\log n)$) between two heaps, making it a powerful alternative to standard binary heaps (which require $O(n)$ time to merge).

### When to use:
- When your application frequently combines or merges priority queues (e.g., discrete event simulations, multi-threaded task scheduling where workers merge queues).
- When you need predictable $O(\log n)$ bounds on `Insert`, `ExtractMin`, and `Merge` operations.

---

## 2. Usage

```csharp
using System;
using DataStructures.LeftistHeap;

class Program
{
    static void Main()
    {
        // Create two leftist heaps
        var heap1 = new LeftistHeap<int>();
        heap1.Insert(10);
        heap1.Insert(25);
        heap1.Insert(5);

        var heap2 = new LeftistHeap<int>();
        heap2.Insert(30);
        heap2.Insert(15);
        heap2.Insert(3);

        // Merge heap2 into heap1 in O(log n) time
        heap1.Merge(heap2);

        Console.WriteLine($"Total elements: {heap1.Count}"); // 6
        Console.WriteLine($"Minimum element: {heap1.Peek()}"); // 3

        // Extract elements in ascending order
        while (!heap1.IsEmpty)
        {
            Console.Write($"{heap1.ExtractMin()} ");
        }
        // Output: 3 5 10 15 25 30
    }
}
```

---

## 3. Detailed Explanation

### Invariants:
1. **Min-Heap Property**: For every node $X$, the value of $X$ is less than or equal to the values of its children.
2. **Null Path Length (NPL)**: The NPL of node $X$ is defined as the length of the shortest path from $X$ to a node with fewer than two children (external/null node). An empty node has $\text{Npl} = -1$, a leaf has $\text{Npl} = 0$.
3. **Leftist Property**: For every internal node $X$, $\text{Npl}(X.\text{Left}) \ge \text{Npl}(X.\text{Right})$.

### Core Operations:
- **`Merge(h1, h2)`**: The fundamental operation. Compare the roots of $h_1$ and $h_2$. The tree with the larger root is recursively merged with the right subtree of the tree with the smaller root. After merging, if the leftist property is violated (i.e., $\text{Npl}(\text{left}) < \text{Npl}(\text{right})$), swap the left and right children. Finally, update the root's NPL: $\text{Npl}(root) = \text{Npl}(root.\text{Right}) + 1$.
- **`Insert(item)`**: Treat the single item as a 1-node heap and call `Merge`.
- **`ExtractMin()`**: Remove the root and merge the root's left and right subtrees.

---

## 4. Complexity Analysis

| Operation | Time Complexity | Space Complexity |
| :--- | :--- | :--- |
| **Peek** | $O(1)$ | $O(1)$ |
| **Insert** | $O(\log n)$ | $O(\log n)$ recursion stack |
| **ExtractMin / Pop** | $O(\log n)$ | $O(\log n)$ recursion stack |
| **Merge** | $O(\log n)$ | $O(\log n)$ recursion stack |
| **Clear** | $O(1)$ | $O(1)$ |
| **Traversal (IEnumerable)** | $O(n)$ | $O(n)$ |