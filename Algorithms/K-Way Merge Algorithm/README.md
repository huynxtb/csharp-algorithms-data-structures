# K-Way Merge Algorithm

## 1. Introduction

The K-Way Merge Algorithm is an efficient method for combining `K` already sorted lists (or arrays) into a single, comprehensive sorted list. It is particularly useful in scenarios where data is naturally partitioned into multiple sorted segments, such as in external sorting (where data is too large to fit into memory and is sorted in chunks), or when merging results from parallel sorting processes.

The core idea is to always pick the smallest available element from the `K` lists. To do this efficiently, a min-heap (or priority queue) is used to keep track of the smallest element currently at the "front" of each of the `K` lists.

## 2. Usage

To use the `KWayMerge` algorithm, you simply need to call the static `Merge` method, passing an `IEnumerable<IList<T>>` where `T` is a comparable type (e.g., `int`, `string`, custom objects implementing `IComparable<T>`).

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class Example
{
    public static void Main(string[] args)
    {
        // Example 1: Merging lists of integers
        List<IList<int>> intLists = new List<IList<int>>
        {
            new List<int> { 1, 3, 5, 7 },
            new List<int> { 2, 4, 6, 8 },
            new List<int> { 0, 9, 10, 11 }
        };
        List<int> mergedInts = KWayMerge.Merge(intLists);
        Console.WriteLine("Merged Integers: " + string.Join(", ", mergedInts));
        // Expected output: Merged Integers: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11

        // Example 2: Merging lists of strings with different lengths and an empty list
        List<IList<string>> stringLists = new List<IList<string>>
        {
            new List<string> { "apple", "banana" },
            new List<string> { "cherry" },
            new List<string>(), // An empty list is handled gracefully
            new List<string> { "date", "elderberry", "fig" }
        };
        List<string> mergedStrings = KWayMerge.Merge(stringLists);
        Console.WriteLine("Merged Strings: " + string.Join(", ", mergedStrings));
        // Expected output (alphabetical): Merged Strings: apple, banana, cherry, date, elderberry, fig

        // Example 3: Merging a single list
        List<IList<double>> singleList = new List<IList<double>>
        {
            new List<double> { 10.5, 20.1, 30.9 }
        };
        List<double> mergedSingle = KWayMerge.Merge(singleList);
        Console.WriteLine("Merged Single List: " + string.Join(", ", mergedSingle));
        // Expected output: Merged Single List: 10.5, 20.1, 30.9

        // Example 4: Merging only empty lists
        List<IList<char>> emptyLists = new List<IList<char>>
        {
            new List<char>(),
            new List<char>(),
            new List<char>()
        };
        List<char> mergedEmpty = KWayMerge.Merge(emptyLists);
        Console.WriteLine("Merged Empty Lists (Count): " + mergedEmpty.Count);
        // Expected output: Merged Empty Lists (Count): 0
    }
}
```

## 3. Detailed Explanation

The `KWayMerge` implementation consists of three main components:

1.  **`HeapNode<T>`**: A private nested class that wraps an element from one of the input lists. Besides the `Value` itself, it stores `ListIndex` (the index of the list it came from) and `ElementIndex` (its position within that list). This is crucial for knowing which element to add to the heap next after an element is extracted.

2.  **`MinHeap<T>`**: A private generic implementation of a binary min-heap. It uses a `List<T>` as its backing store. Key operations are:
    *   `Add(T item)`: Inserts an item and calls `HeapifyUp` to maintain the heap property.
    *   `ExtractMin()`: Removes and returns the smallest item (root), replaces it with the last item, and calls `HeapifyDown` to restore the heap property.
    *   `HeapifyUp(int index)`: Moves an element up the heap until its parent is smaller or it reaches the root.
    *   `HeapifyDown(int index)`: Moves an element down the heap by repeatedly swapping it with its smallest child until both children are larger or it becomes a leaf.

3.  **`KWayMerge.Merge<T>(IEnumerable<IList<T>> sortedLists)`**: The main public method that orchestrates the merge process:
    *   It first converts the input `IEnumerable<IList<T>>` into a `List<IList<T>>` (`actualLists`). This allows for `O(1)` indexed access to the original lists, which is necessary when retrieving the next element from a specific list.
    *   It initializes a `MinHeap<HeapNode<T>>`. For each non-empty input list, it creates a `HeapNode` for its first element and adds it to the min-heap. This populates the heap with the smallest available element from each list.
    *   It then enters a loop that continues as long as the min-heap is not empty:
        *   It extracts the `minNode` (the smallest element overall) from the heap.
        *   The `Value` of this `minNode` is added to the `result` list.
        *   It calculates the `nextElementIndex` for the list from which `minNode` originated.
        *   If that list still has more elements (i.e., `nextElementIndex` is within bounds), a new `HeapNode` for the *next* element from that specific list is created and added back into the min-heap. This ensures that the heap always contains the next available element from each list that still has elements.
    *   Finally, the `result` list, containing all elements in sorted order, is returned.

This approach ensures that at each step, the globally smallest element is picked, and the heap efficiently manages the candidates for the next smallest element from all `K` lists.

## 4. Complexity Analysis

Let `K` be the number of sorted input lists.
Let `N` be the total number of elements across all `K` lists (i.e., `N = n_1 + n_2 + ... + n_K`, where `n_i` is the number of elements in list `i`).

*   **Time Complexity:**
    *   **Conversion to `List<IList<T>>`**: `O(K)` to iterate through the `IEnumerable` and store references to the `K` lists.
    *   **Heap Initialization**: In the worst case, all `K` lists are non-empty. Adding `K` elements to a min-heap, where each `Add` operation takes `O(log K)` time, results in `O(K log K)` for initialization.
    *   **Main Merge Loop**: The loop runs `N` times, as each element from the input lists is extracted exactly once.
        *   Each iteration involves an `ExtractMin` operation, which takes `O(log K)` time.
        *   Potentially, an `Add` operation (for the next element from the source list) is performed, also taking `O(log K)` time.
        *   Thus, the total time for the main loop is `O(N log K)`.
    *   **Overall Time Complexity**: Combining these, the dominant factor is `O(N log K)`.

*   **Space Complexity:**
    *   **`result` list**: Stores all `N` elements, requiring `O(N)` space.
    *   **`actualLists`**: Stores `K` references to the input lists, requiring `O(K)` space.
    *   **`minHeap`**: At any point, the heap stores at most `K` `HeapNode` objects (one from each list). Each `HeapNode` stores a reference to an element of type `T` and two integers. Therefore, the heap requires `O(K)` space.
    *   **Overall Space Complexity**: Combining these, the total space complexity is `O(N + K)`.

This makes the K-Way Merge Algorithm highly efficient, especially when `K` is relatively small compared to `N` (e.g., `K` is much smaller than `N/log N`).