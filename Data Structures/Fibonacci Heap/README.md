# Fibonacci Heap

## Introduction
A Fibonacci Heap is a priority queue data structure consisting of a collection of heap-ordered trees. It provides faster amortized running times for many operations compared to binary heaps or binomial heaps. It is particularly useful in network optimization algorithms, such as Dijkstra's shortest path algorithm and Prim's minimum spanning tree algorithm, where `DecreaseKey` operations are executed frequently.

## Usage

```csharp
using System;

class Program
{
    static void Main()
    {
        var heap = new FibonacciHeap<int, string>();

        // Insert elements
        var node1 = heap.Insert(10, "Ten");
        var node2 = heap.Insert(5, "Five");
        var node3 = heap.Insert(20, "Twenty");

        // Get minimum
        var minNode = heap.Minimum();
        Console.WriteLine($"Min: {minNode.Key} -> {minNode.Value}"); // Output: 5 -> Five

        // Decrease key
        heap.DecreaseKey(node3, 2);
        Console.WriteLine($"New Min: {heap.Minimum().Key}"); // Output: 2

        // Extract minimum
        var extracted = heap.ExtractMin();
        Console.WriteLine($"Extracted: {extracted.Key}"); // Output: 2
    }
}
```

## Detailed Explanation
The Fibonacci Heap maintains a collection of trees that satisfy the min-heap property (the key of a child is always greater than or equal to the key of its parent). Unlike binomial heaps, the structure of trees in a Fibonacci Heap is more flexible. 

- **Insert**: Adds a new node to the root list of the heap in $O(1)$ time.
- **ExtractMin**: Removes the minimum node, moves all its children to the root list, and then consolidates the root list by linking trees of equal degree until every tree has a unique degree. This keeps the size of the heap balanced.
- **DecreaseKey**: Decreases the key of a node. If the min-heap property is violated, the node is cut from its parent and moved to the root list. A cascading cut is performed on its ancestors to maintain structural balance.
- **Delete**: Removes a node by cutting it from its parent, moving its children to the root list, and removing it from the root list.

## Complexity Analysis

| Operation | Amortized Complexity | Worst-case Complexity |
|---|---|---|
| **Insert** | $O(1)$ | $O(1)$ |
| **Minimum** | $O(1)$ | $O(1)$ |
| **ExtractMin** | $O(\log n)$ | $O(n)$ |
| **DecreaseKey** | $O(1)$ | $O(\log n)$ |
| **Delete** | $O(\log n)$ | $O(n)$ |
| **Union** | $O(1)$ | $O(1)$ |

- **Space Complexity**: $O(n)$ where $n$ is the number of elements in the heap.