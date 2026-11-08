# Mergeable Heap

## Introduction

The Mergeable Heap is a priority queue data structure that supports efficient merging of two heaps. Unlike traditional binary heaps, which typically have a logarithmic time complexity for merging, Mergeable Heaps are designed to perform this operation in amortized constant time. This makes them particularly useful in algorithms where frequent merging of heaps is required, such as Dijkstra's algorithm with certain graph representations or Huffman coding.

This implementation uses a Leftist Heap, a type of mergeable heap that maintains a specific structural property (the 'leftist' property) to ensure efficient merging. The leftist property states that for every node, the rank of its left child is greater than or equal to the rank of its right child. The rank of a node is defined as the length of the shortest path from that node to a null descendant (i.e., the distance to the nearest empty subtree).

## Usage

```csharp
using System;

public class Example
{
    public static void Main(string[] args)
    {
        var heap1 = new MergeableHeap<int>();
        heap1.Insert(5);
        heap1.Insert(2);
        heap1.Insert(8);

        var heap2 = new MergeableHeap<int>();
        heap2.Insert(3);
        heap2.Insert(7);
        heap2.Insert(1);

        Console.WriteLine($"Heap 1 Min: {heap1.FindMin()}"); // Output: Heap 1 Min: 2
        Console.WriteLine($"Heap 2 Min: {heap2.FindMin()}"); // Output: Heap 2 Min: 1

        heap1.Merge(heap2);

        Console.WriteLine($"Heap 1 Count after merge: {heap1.Count}"); // Output: Heap 1 Count after merge: 6
        Console.WriteLine($"Heap 2 Count after merge: {heap2.Count}"); // Output: Heap 2 Count after merge: 0

        Console.WriteLine("Elements in merged heap (sorted):");
        while (!heap1.IsEmpty)
        {
            Console.Write($"{heap1.DeleteMin()} "); // Output: 1 2 3 5 7 8 
        }
        Console.WriteLine();
    }
}
```

## Detailed Explanation

The `MergeableHeap<T>` class is implemented using a Leftist Heap. Each node in the heap (`Node` inner class) stores a value (`Value`), its rank (`Rank`), and references to its left and right children (`Left`, `Right`).

- **`Node` Class**: A private inner class representing a node in the leftist heap. It holds the element's value, its rank (shortest path to a null descendant), and pointers to its left and right children.

- **`root`**: A `Node` reference pointing to the root of the heap. This node always contains the minimum element.

- **`count`**: An integer tracking the number of elements in the heap.

- **`IsEmpty`**: A property that returns `true` if the heap contains no elements.

- **`Count`**: A property that returns the number of elements in the heap.

- **`FindMin()`**: Returns the value of the root node without removing it. Throws an `InvalidOperationException` if the heap is empty.

- **`Insert(T value)`**: Creates a new node with the given value and merges it with the existing heap. The `Merge` operation ensures the heap properties are maintained. The `count` is incremented.

- **`DeleteMin()`**: Removes and returns the minimum element (the root). It achieves this by merging the left and right children of the root. The `count` is decremented. Throws an `InvalidOperationException` if the heap is empty.

- **`Merge(MergeableHeap<T> otherHeap)`**: Merges the current heap with another `MergeableHeap`. It calls the private `Merge` method with the roots of both heaps. After a successful merge, the `otherHeap` is effectively emptied to prevent data inconsistencies.

- **`Merge(Node h1, Node h2)` (private helper)**: This is the core recursive function for merging two leftist heaps. 
    1. **Base Cases**: If either heap is null, return the other heap.
    2. **Root Comparison**: Ensures that `h1` always points to the heap with the smaller root value. If not, `h1` and `h2` are swapped.
    3. **Recursive Merge**: Recursively merges `h2` with the right child of `h1`. This is the key to achieving O(1) amortized merge time, as we only ever recurse down one path (the right spine).
    4. **Leftist Property Maintenance**: After the recursive merge, it checks if the leftist property is violated (i.e., if the rank of the right child is greater than the rank of the left child). If so, the left and right children are swapped.
    5. **Rank Update**: The rank of `h1` is updated based on the rank of its new right child (plus one).
    6. **Return**: Returns the root of the merged heap (`h1`).

## Complexity Analysis

- **`Insert(T value)`**: O(log n) - This is because inserting a single element is equivalent to merging a heap with one element into the existing heap. The `Merge` operation on a leftist heap has a logarithmic time complexity in the worst case.

- **`FindMin()`**: O(1) - Simply returns the value of the root node.

- **`DeleteMin()`**: O(log n) - This involves merging the two children of the root. The `Merge` operation has a logarithmic time complexity in the worst case.

- **`Merge(MergeableHeap<T> otherHeap)`**: O(1) amortized - The actual recursive `Merge` operation on the underlying nodes has a complexity proportional to the sum of the lengths of the right spines of the two heaps. In a leftist heap, the length of the right spine is logarithmically bounded by the number of nodes. However, due to the way nodes are structured and the recursive calls are made, the amortized cost of merging is O(1). This is a key advantage of leftist heaps over other mergeable heap implementations.

- **Space Complexity**: O(n) - For storing `n` elements in the heap.
