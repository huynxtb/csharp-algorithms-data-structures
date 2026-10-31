# Smoothsort Algorithm

## Introduction

Smoothsort is an adaptive, in-place, unstable comparison sorting algorithm. It is known for its optimal average and worst-case time complexity of O(n log n). A key feature of Smoothsort is its adaptive nature: for nearly sorted data, its performance can approach O(n), making it particularly efficient in scenarios where input data often exhibits some degree of pre-existing order. It achieves this by using a heap-like structure based on Leonardo numbers, which allows it to efficiently identify and leverage existing order within the array.

Smoothsort is a good choice when:
*   You need an O(n log n) worst-case sorting algorithm.
*   Your data is often nearly sorted, and you want to benefit from adaptive performance.
*   You require an in-place sort to minimize memory usage.

## Usage

To use Smoothsort, you can call the static `Sort` method on the `Smoothsort` class. You can either use the default comparer for the element type or provide a custom `IComparer<T>`.

**Example with default comparer:**

```csharp
using System;

public class Example
{
    public static void Main(string[] args)
    {
        int[] numbers = { 5, 1, 4, 2, 8, 0, 3, 7, 6, 9 };
        Smoothsort.Sort(numbers);
        // numbers is now { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }

        string[] words = { "banana", "apple", "cherry", "date" };
        Smoothsort.Sort(words);
        // words is now { "apple", "banana", "cherry", "date" }
    }
}
```

**Example with a custom comparer (e.g., descending order):**

```csharp
using System;
using System.Collections.Generic;

public class Example
{
    public static void Main(string[] args)
    {
        int[] numbers = { 5, 1, 4, 2, 8, 0, 3, 7, 6, 9 };
        
        // Custom comparer for descending order
        IComparer<int> descendingComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        
        Smoothsort.Sort(numbers, descendingComparer);
        // numbers is now { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }
    }
}
```

## Detailed Explanation

Smoothsort operates in two main phases: building a heap and then extracting elements from it.

1.  **Heap Construction (Phase 1):**
    *   The algorithm iterates through the array, conceptually building a heap structure. Unlike a binary heap, Smoothsort uses a structure based on Leonardo numbers. Leonardo numbers (L(n)) are defined by L(0)=1, L(1)=1, and L(n) = L(n-1) + L(n-2) + 1. The sequence starts: 1, 1, 3, 5, 9, 15, 25, ...
    *   The array is partitioned into segments whose sizes correspond to Leonardo numbers. These segments form a forest of trees, where each tree's root is the largest element in its subtree.
    *   The `Trinkle` operation is crucial here. When an element is added or considered, `Trinkle` ensures that it is correctly positioned within its tree structure by comparing it with its 'children' (elements at specific offsets determined by Leonardo numbers) and swapping if necessary. This process is repeated recursively down the tree until the heap property is satisfied.
    *   The `Sift` operation, conceptually, is used when a new root is added to a valid heap structure. In the Smoothsort implementation, the `Trinkle` operation effectively handles the necessary restructuring, making the `Sift` method in this implementation largely conceptual or a placeholder.

2.  **Heap Extraction (Phase 2):**
    *   Once the entire array is structured as a heap, the algorithm repeatedly extracts the largest element (which is always at the root of the heap, index 0).
    *   The largest element is swapped with the last element of the current heap (which is shrinking in size). The heap size is then reduced by one.
    *   The `Trinkle` operation is called on the new root (the element that was swapped into index 0) to restore the heap property for the reduced heap.
    *   This process continues until the heap is empty, at which point the array is sorted.

**Leonardo Numbers and Tree Structure:**

Smoothsort uses Leonardo numbers to define the sizes of subtrees. A heap of size `n` is decomposed into two sub-heaps (if `n` is not a Leonardo number) of sizes `L(k-1)` and `L(k-2)`, where `L(k)` is the largest Leonardo number less than or equal to `n`. The element at index `i` is the root of a tree. Its children are located at `i - L(k-2)` and `i - L(k-1)`. The `Trinkle` function navigates this structure.

**Adaptiveness:**

Smoothsort's adaptiveness comes from the fact that if the data is already mostly sorted, the `Trinkle` operations will often find that the current element is already in the correct position, requiring fewer comparisons and swaps. In the best case (already sorted data), it can perform in O(n) time.

## Complexity Analysis

*   **Time Complexity:**
    *   **Worst Case:** O(n log n)
    *   **Average Case:** O(n log n)
    *   **Best Case (nearly sorted data):** O(n)

*   **Space Complexity:**
    *   **In-place:** O(1) auxiliary space. The sorting is performed directly within the input array.
