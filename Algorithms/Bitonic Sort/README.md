# Bitonic Sort

## 1. Introduction

Bitonic Sort is a parallel comparison-based sorting algorithm originally designed by Ken Batcher in 1968. It is primarily known for producing sorting networks where the sequence of comparisons is predefined and independent of data, making it exceptionally well-suited for hardware implementations (FPGAs, ASICs) and massively parallel architectures like modern GPUs.

A **bitonic sequence** is a sequence of values $x_0, x_1, \dots, x_{n-1}$ such that either:
1. $x_0 \le \dots \le x_k \ge \dots \ge x_{n-1}$ for some $k$, or
2. It can be cyclically shifted to satisfy condition 1.

While traditional Bitonic Sort strictly requires the sequence length $N$ to be a power of two ($N = 2^k$), this implementation generalizes the divide-and-conquer strategy to handle arbitrary array sizes $N$ in-place without allocating auxiliary memory for padding.

## 2. Usage

```csharp
using System;
using AdvancedAlgorithms.Sorting;

class Program
{
    static void Main()
    {
        // Example with arbitrary array length (non-power of two)
        int[] numbers = { 35, 12, 89, 4, 17, 23, 9, 64, 42, 1, 56 };

        // Sort in ascending order
        BitonicSorter.Sort(numbers);
        Console.WriteLine(string.Join(", ", numbers));
        // Output: 1, 4, 9, 12, 17, 23, 35, 42, 56, 64, 89

        // Sort in descending order
        BitonicSorter.Sort(numbers, SortDirection.Descending);
        Console.WriteLine(string.Join(", ", numbers));
        // Output: 89, 64, 56, 42, 35, 23, 17, 12, 9, 4, 1

        // Custom object and comparator usage
        string[] words = { "apple", "Banana", "cherry", "date", "Elderberry" };
        BitonicSorter.Sort(words, StringComparer.OrdinalIgnoreCase, SortDirection.Ascending);
        Console.WriteLine(string.Join(", ", words));
    }
}
```

## 3. Detailed Explanation

Bitonic sort operates in two main phases:
1. **Bitonic Sequence Construction (`BitonicSortRecursive`)**:
   Recursively sorts the first half of the sequence in monotonic order (e.g., ascending) and the second half in the opposite order (e.g., descending). Concatenating these two halves creates a bitonic sequence.
2. **Bitonic Merge (`BitonicMergeRecursive`)**:
   Given a bitonic sequence, elements separated by an offset (the greatest power of two $k < n$) are compared and swapped if out of order. After this step, the original sequence is split into two halves where every element in the first half is less than (or equal to) every element in the second half, and each half is itself a bitonic sequence. Recursively applying the merge step yields a completely sorted array.

### Arbitrary Length Handling
To avoid allocating memory to pad the array to the nearest $2^k$, the algorithm dynamically splits partitions using $k = 2^{\lfloor \log_2(n-1) \rfloor}$ (the greatest power of two strictly smaller than $n$). The comparison step compares indices $i$ and $i + k$ for $0 \le i < n - k$. Both halves ($k$ and $n - k$) are then recursively merged.

## 4. Complexity Analysis

| Metric | Complexity |
| :--- | :--- |
| **Best-Case Time Complexity** | $O(\log^2 N)$ parallel / $O(N \log^2 N)$ sequential |
| **Average-Case Time Complexity** | $O(\log^2 N)$ parallel / $O(N \log^2 N)$ sequential |
| **Worst-Case Time Complexity** | $O(\log^2 N)$ parallel / $O(N \log^2 N)$ sequential |
| **Space Complexity** | $O(\log N)$ auxiliary recursion stack space (In-place) |
| **Stability** | Not Stable |