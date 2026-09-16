# Pancake Sorting Algorithm

## Introduction

Pancake Sort is a sorting algorithm that uses only flip (reversal) operations to sort an array. The name comes from the analogy of sorting a stack of pancakes by using a spatula to flip and rearrange the top k pancakes repeatedly. Unlike most sorting algorithms that use comparisons and swaps, Pancake Sort manipulates contiguous prefixes of the array.

### When to Use It

Pancake Sort is primarily used in:
- **Theoretical computer science** and algorithmic study
- **Situations with restricted operations** where only prefix reversals are allowed
- **Teaching sorting algorithms** and algorithm design principles
- **Scenarios where minimizing the number of operations** is critical (though it's not optimal in practice)

## Usage

```csharp
using System;
using System.Collections.Generic;
using Algorithms.Sorting;

class Program
{
    static void Main()
    {
        // Example 1: Sorting an integer array
        int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6 };
        PancakeSort.Sort(numbers);
        Console.WriteLine("Sorted numbers: " + string.Join(", ", numbers));
        // Output: Sorted numbers: 1, 1, 2, 3, 4, 5, 6, 9

        // Example 2: Sorting with a generic type
        string[] words = { "pancake", "apple", "zebra", "banana" };
        PancakeSort.Sort(words);
        Console.WriteLine("Sorted words: " + string.Join(", ", words));
        // Output: Sorted words: apple, banana, pancake, zebra

        // Example 3: Sorting with a custom comparer (descending order)
        int[] descending = { 3, 1, 4, 1, 5, 9, 2, 6 };
        PancakeSort.Sort(descending, Comparer<int>.Create((a, b) => b.CompareTo(a)));
        Console.WriteLine("Sorted descending: " + string.Join(", ", descending));
        // Output: Sorted descending: 9, 6, 5, 4, 3, 2, 1, 1

        // Example 4: Getting the flip sequence without modifying the original array
        int[] original = { 3, 1, 4, 1, 5 };
        var flips = PancakeSort.GetFlipSequence(original, Comparer<int>.Default);
        Console.WriteLine("Flip sequence: " + string.Join(", ", flips));
        Console.WriteLine("Original array unchanged: " + string.Join(", ", original));
    }
}
```

## Detailed Explanation

### How Pancake Sort Works

1. **Initialization**: Start with an unsorted portion covering the entire array (indices 0 to n-1).

2. **Main Loop**: For each iteration with unsorted size from n down to 2:
   - Find the index of the maximum element in the unsorted portion (0 to unsortedSize-1)
   - If the maximum is already at index unsortedSize-1 (its correct sorted position), skip to the next iteration
   - If the maximum is not at index 0:
     - Perform a flip at position (maxIndex + 1) to bring the maximum to index 0
   - Perform a flip at position unsortedSize to move the maximum from index 0 to its final sorted position at index unsortedSize-1
   - Reduce the unsorted portion size by 1

3. **Termination**: When unsorted size reaches 1, the array is completely sorted.

### Core Operations

**Flip(array, k)**: Reverses the first k elements of the array in-place.
- Time: O(k)
- Swaps k/2 pairs of elements

**FindMaxIndex(array, endExclusive, comparer)**: Scans the range [0, endExclusive) and returns the index of the maximum element.
- Time: O(endExclusive) comparisons

### Optimization: Skip Unnecessary Flips

The implementation includes an optimization to skip flips when the maximum element is already in its correct sorted position. This reduces the number of flips performed while maintaining the same worst-case behavior.

### Example Trace

Sorting [3, 1, 4, 1, 5] in ascending order:

```
Initial:      [3, 1, 4, 1, 5]
Max in [0,5):  5 at index 4
  Flip(5):     [5, 1, 4, 1, 3]
  Flip(5):     [3, 1, 4, 1, 5]

Max in [0,4):  4 at index 2
  Flip(3):     [4, 1, 3, 1, 5]
  Flip(4):     [1, 3, 1, 4, 5]

Max in [0,3):  3 at index 1
  Flip(2):     [3, 1, 1, 4, 5]
  Flip(3):     [1, 1, 3, 4, 5]

Max in [0,2):  1 at index 0 or 1
  Already in correct position or requires minimal flips

Result:       [1, 1, 3, 4, 5]
```

## Complexity Analysis

### Time Complexity

- **Comparisons**: O(n²) in the worst case
  - Outer loop runs (n-1) iterations
  - Each iteration performs a linear search for the maximum: O(n) comparisons
  - Total: (n-1) + (n-2) + ... + 1 = O(n²)

- **Flips**: O(n) in the worst case
  - At most 2 flips per iteration
  - (n-1) iterations
  - However, each flip is O(k) where k is the flip size
  - Total work for flips: O(n²) in the absolute worst case

### Space Complexity

- **In-place Sort variants**: O(1) auxiliary space
  - Only uses local variables for loop counters and temporary values
  - Sorts the array in-place with no additional data structures

- **GetFlipSequence**: O(n) auxiliary space
  - Creates a copy of the input array: O(n)
  - Maintains a list of flip sizes: O(n) in worst case

### Practical Performance

- Pancake Sort is not efficient in practice compared to QuickSort or MergeSort
- It is primarily of theoretical interest
- The number of flips is bounded by 2n - 3 for any array
- Worst-case arrays are rare, making average performance better than worst-case analysis suggests
