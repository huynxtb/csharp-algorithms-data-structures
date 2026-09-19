# Ternary Heap (Tournament Tree)

## 1. Introduction
A **Ternary Heap** is a generalization of the classical binary heap in which every internal node may have up to **three children** rather than two. Like a binary heap it is a *complete tree* stored implicitly in an array, and it maintains the *min-heap property*: every parent's value is less than or equal to the values of its children.

Because a ternary heap is shallower than a binary heap for the same number of elements (height ≈ log₃ n instead of log₂ n), it performs fewer comparisons on the path from a leaf to the root. This makes **insertion** and **decrease-key** operations slightly faster, at the cost of doing more comparisons per level during **extract-min** (since three children must be inspected instead of two). It is often used as a drop-in replacement for a binary heap in algorithms such as **Dijkstra's shortest path**, **Prim's MST**, and any priority-queue driven workload where wider fan-out yields better cache behavior.

## 2. Usage
```csharp
using System;

public class Example
{
    public static void Run()
    {
        var heap = new TernaryHeap<int>();

        heap.Insert(42);
        heap.Insert(7);
        heap.Insert(15);
        heap.Insert(3);
        heap.Insert(29);

        Console.WriteLine($"Count: {heap.Count}");     // 5
        Console.WriteLine($"Min:   {heap.PeekMin()}"); // 3

        while (!heap.IsEmpty)
        {
            Console.WriteLine(heap.ExtractMin());     // 3, 7, 15, 29, 42
        }

        // Decrease-key example
        heap.Insert(100);
        heap.Insert(50);
        heap.Insert(200);
        heap.DecreaseKey(2, 1); // reduces element at index 2 to 1
        Console.WriteLine(heap.PeekMin()); // 1
    }
}
```

## 3. Detailed Explanation
The heap is stored in a resizable `List<T>` where the element at index `0` is the root. For a node at index `i`:

- **Parent index:** `(i - 1) / 3`
- **Child indices:** `3*i + 1`, `3*i + 2`, `3*i + 3`

### Insert
1. Append the new element to the end of the list (the next available leaf position).
2. Perform **heapify-up**: repeatedly compare the element to its parent and swap them while the element is smaller. This restores the min-heap property along the path to the root.

### ExtractMin
1. Save the root (`_items[0]`) as the minimum to return.
2. Move the last element in the list to index `0` and remove the last slot.
3. Perform **heapify-down**: examine the (up to three) children of the current node, find the smallest, and swap if any child is smaller. Continue downward until the heap property holds.

### PeekMin
Simply returns `_items[0]` in constant time, throwing when the heap is empty.

### DecreaseKey
Replaces the value at a given index with a smaller value, then triggers heapify-up from that index. An exception is raised if the new value is not strictly less than or equal to the current one.

### Helper Methods
- `Swap(i, j)` exchanges two elements in the backing store.
- `Compare(a, b)` delegates to `IComparable<T>.CompareTo`, keeping ordering logic centralized.
- `GetParentIndex(i)` computes `(i - 1) / 3`.

## 4. Complexity Analysis
Let **n** be the number of elements in the heap.

| Operation      | Time Complexity | Notes                                                       |
|----------------|-----------------|-------------------------------------------------------------|
| `Insert`       | O(log₃ n)       | Height is log base 3; up to one comparison/swap per level.  |
| `ExtractMin`   | O(log₃ n)       | Each level costs up to 3 comparisons (constant factor).     |
| `PeekMin`      | O(1)            | Direct array access.                                        |
| `DecreaseKey`  | O(log₃ n)       | Bubbles the modified element upward toward the root.        |
| `Count`/`IsEmpty` | O(1)         | Backed by `List<T>.Count`.                                  |
| `Clear`        | O(n)            | Empties the underlying list.                                |

**Space Complexity:** O(n) — a single contiguous array stores every element with no per-node pointer overhead, giving the ternary heap excellent memory locality.
