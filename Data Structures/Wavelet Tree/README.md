# Wavelet Tree

## Introduction

A Wavelet Tree is a data structure that represents a sequence of elements over a given alphabet (or universe of values). It is particularly useful for performing various range queries on the sequence efficiently, especially when the alphabet size is not excessively large compared to the sequence length. It achieves succinctness and speed by recursively partitioning the sequence based on the median of the current alphabet range.

**When to use it:**

*   When you need to perform frequent range queries on a static sequence.
*   Queries include counting occurrences of a value in a range (Range Frequency), finding the k-th smallest element in a range (Quantile), and counting elements less than or equal to a value in a range (Rank).
*   The alphabet of the elements is manageable (e.g., integers within a reasonable range, or characters).

## Usage

```csharp
using System;
using System.Collections.Generic;

public class Example
{
    public static void Main(string[] args)
    {
        // Example with integers
        int[] data = { 1, 5, 2, 8, 3, 5, 2, 1, 9, 4 };
        int minVal = 0;
        int maxVal = 10;

        WaveletTree<int> wt = new WaveletTree<int>(data, minVal, maxVal);

        // Range Frequency: Count occurrences of 5 in range [1, 6]
        // Sequence: { 1, [5, 2, 8, 3, 5], 2, 1, 9, 4 }
        // Subarray: { 5, 2, 8, 3, 5 }
        // Count of 5: 2
        int freq = wt.RangeFrequency(1, 6, 5);
        Console.WriteLine($"Frequency of 5 in [1, 6]: {freq}"); // Output: 2

        // Quantile: Find the 3rd smallest element in range [0, 8]
        // Sequence: { [1, 5, 2, 8, 3, 5, 2, 1], 9, 4 }
        // Subarray: { 1, 5, 2, 8, 3, 5, 2, 1 }
        // Sorted subarray: { 1, 1, 2, 2, 3, 5, 5, 8 }
        // 3rd smallest: 2
        int k = 3;
        int quantile = wt.Quantile(0, 8, k);
        Console.WriteLine($"{k}-th smallest element in [0, 8]: {quantile}"); // Output: 2

        // Range Count Less Than or Equal: Count elements <= 3 in range [2, 7]
        // Sequence: { 1, 5, [2, 8, 3, 5, 2, 1], 9, 4 }
        // Subarray: { 2, 8, 3, 5, 2, 1 }
        // Elements <= 3: { 2, 3, 2, 1 }
        // Count: 4
        int rank = wt.RangeCountLessOrEqual(2, 7, 3);
        Console.WriteLine($"Count of elements <= 3 in [2, 7]: {rank}"); // Output: 4

        // Example with characters
        char[] charData = {'a', 'c', 'b', 'd', 'a', 'c', 'b', 'a'};
        char charMin = 'a';
        char charMax = 'd';

        WaveletTree<char> charWt = new WaveletTree<char>(charData, charMin, charMax);

        // Range Frequency: Count 'c' in [1, 5]
        // Subarray: {'c', 'b', 'd', 'a', 'c'}
        // Count of 'c': 2
        int charFreq = charWt.RangeFrequency(1, 5, 'c');
        Console.WriteLine($"Frequency of 'c' in [1, 5]: {charFreq}"); // Output: 2
    }
}
```

## Detailed Explanation

The `WaveletTree<T>` class implements the Wavelet Tree data structure. It is designed to be generic, accepting any type `T` that implements `IComparable<T>`.

### Construction (`WaveletTree(IEnumerable<T> sequence, T minVal, T maxVal)`)

1.  **Initialization**: The constructor takes the input `sequence`, and the minimum (`minVal`) and maximum (`maxVal`) values defining the alphabet. It calculates the `_alphabetSize` and `_midVal` for partitioning.
2.  **Recursive Partitioning**: The core of the construction is a recursive process:
    *   At each node, the elements of the current subsequence are partitioned into two groups: those less than `_midVal` (going to the left child) and those greater than or equal to `_midVal` (going to the right child).
    *   A `_rank` array (prefix sums of bits) is maintained at each node. `_rank[i]` stores the number of elements that went to the left child among the first `i` elements processed at this node. This allows for efficient mapping of indices between parent and child nodes.
    *   The `_left` and `_right` children are recursively constructed with the corresponding subsequences and adjusted alphabet ranges.
3.  **Base Case**: The recursion stops when the alphabet range contains only one distinct value (`_minVal.CompareTo(_maxVal) == 0`) or when the subsequence is empty.

### Range Frequency (`RangeFrequency(int left, int right, T value)`)

1.  **Bounds Checking**: Validates the `left` and `right` indices.
2.  **Recursive Search**: The `RangeFrequencyInternal` method is called.
    *   If the current node represents a single value alphabet, it checks if `value` matches and returns the range size or 0.
    *   It determines which child (`_left` or `_right`) the `value` belongs to based on `_midVal`.
    *   It uses the `_rank` array to map the query range `[left, right]` to the corresponding range in the chosen child node.
    *   The query is recursively passed down to the appropriate child until the leaf node representing the `value` is reached.

### Quantile (`Quantile(int left, int right, int k)`)

1.  **Bounds Checking**: Validates `left`, `right`, and `k`.
2.  **Recursive Search**: The `QuantileInternal` method is called.
    *   If the current node represents a single value alphabet, that value is returned.
    *   It calculates how many elements in the query range `[left, right]` go to the left child (`leftCount`) and how many go to the right child (`rightCount`).
    *   If `k` is less than or equal to `leftCount`, the k-th smallest element must be in the left subtree. The query is recursively passed to the `_left` child with the mapped range and the same `k`.
    *   Otherwise, the k-th smallest element is in the right subtree. The query is passed to the `_right` child with the mapped range, and `k` is adjusted by subtracting `leftCount` (`k - leftCount`).

### Range Count Less Or Equal (`RangeCountLessOrEqual(int left, int right, T value)`)

1.  **Bounds Checking**: Validates the `left` and `right` indices.
2.  **Recursive Search**: The `RangeCountLessOrEqualInternal` method is called.
    *   If the current node represents a single value alphabet, it checks if `value` is greater than or equal to this value and returns the range size or 0.
    *   It handles edge cases where `value` is outside the current node's alphabet range.
    *   If `value` is less than `_midVal`, the count must come entirely from the left subtree. The query is recursively passed to `_left` with the mapped range.
    *   If `value` is greater than or equal to `_midVal`, the count includes all elements that went to the left subtree within the range (`leftElementsCount`), plus the count of elements less than or equal to `value` in the right subtree. The query is recursively passed to `_right` with the mapped range and `value`.

### Helper Methods

*   `GetAlphabetSize`: Attempts to determine the size of the alphabet by converting `minVal` and `maxVal` to `long`. This is a simplification and might require adjustments for non-numeric `T` types.
*   `GetMidpointValue`: Calculates the value corresponding to the midpoint index. Similar to `GetAlphabetSize`, this relies on numeric conversion and might need customization for complex types.

## Complexity Analysis

Let $N$ be the length of the sequence and $\Sigma$ be the size of the alphabet.

*   **Construction**: $O(N \log \Sigma)$ time. Each element is processed at each level of the tree, and the tree has $\log \Sigma$ levels. $O(N \log \Sigma)$ space for storing the rank arrays.
*   **Range Frequency (`RangeFrequency`)**: $O(\log \Sigma)$ time. Each query traverses a path from the root to a leaf.
*   **Quantile (`Quantile`)**: $O(\log \Sigma)$ time. Similar to Range Frequency, it traverses a path down the tree.
*   **Range Count Less Or Equal (`RangeCountLessOrEqual`)**: $O(\log \Sigma)$ time. Each query involves a constant number of operations per level, traversing down the tree.
*   **Space Complexity**: $O(N \log \Sigma)$ in total, as each of the $\log \Sigma$ levels stores $N$ bits (implicitly via rank arrays).
