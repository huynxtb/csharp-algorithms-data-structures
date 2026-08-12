# Quickselect Algorithm

## 1. Introduction

Quickselect is a selection algorithm to find the k-th smallest element in an unordered list. It is related to the Quicksort sorting algorithm. Like Quicksort, it is a divide-and-conquer algorithm, and in the average case, it has linear time complexity, making it highly efficient for finding specific order statistics (like the median, minimum, or maximum) without fully sorting the entire list. It operates in-place, modifying the input list during its execution.

Use Quickselect when you need to find an element with a specific rank (e.g., the 5th smallest, the median) from a large dataset and full sorting is unnecessary or too slow.

## 2. Usage

To use the `Quickselect` algorithm, call the static `Select` method, providing the list and the 0-indexed rank `k` of the element you wish to find.

```csharp
using System;
using System.Collections.Generic;

public class Example
{
    public static void Main(string[] args)
    {
        List<int> numbers = new List<int> { 3, 2, 1, 5, 4, 6, 8, 7 };
        Console.WriteLine("Original list: " + string.Join(", ", numbers));

        // Find the 0-th smallest element (minimum)
        int min = Quickselect.Select(numbers, 0);
        Console.WriteLine($"0-th smallest element (min): {min}"); // Expected: 1

        // Find the 3rd smallest element
        int thirdSmallest = Quickselect.Select(numbers, 3);
        Console.WriteLine($"3rd smallest element: {thirdSmallest}"); // Expected: 4

        // Find the (Count-1)-th smallest element (maximum)
        int max = Quickselect.Select(numbers, numbers.Count - 1);
        Console.WriteLine($"{(numbers.Count - 1)}-th smallest element (max): {max}"); // Expected: 8

        // Example with strings
        List<string> words = new List<string> { "apple", "banana", "cherry", "date", "fig" };
        Console.WriteLine("\nOriginal words: " + string.Join(", ", words));
        string medianWord = Quickselect.Select(words, words.Count / 2);
        Console.WriteLine($"Median word: {medianWord}"); // Expected: cherry

        // Example of invalid input handling
        try
        {
            Quickselect.Select(new List<int>(), 0);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\nError: {ex.Message}"); // Expected: The input list cannot be empty.
        }

        try
        {
            Quickselect.Select(numbers, 10);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Expected: k must be a non-negative integer less than the list's count.
        }
    }
}
```

## 3. Detailed Explanation

The Quickselect algorithm works as follows:

1.  **Input Validation**: The `Select` method first validates the input list and the `k` value. It throws `ArgumentNullException` for a null list, `ArgumentException` for an empty list, and `ArgumentOutOfRangeException` if `k` is out of bounds (less than 0 or greater than or equal to the list's count).

2.  **Recursive Selection**: The core logic is implemented in the `QuickselectRecursive` helper method, which takes the list, a `left` index, a `right` index (defining the current sub-array), and `k`.

3.  **Base Case**: If the `left` and `right` indices are the same, it means the sub-array contains only one element, which must be the k-th smallest element, so it's returned.

4.  **Partitioning**: The algorithm uses a `Partition` helper method (specifically, Lomuto's partitioning scheme) to rearrange the elements within the current sub-array (`list[left...right]`).
    *   A pivot element is chosen (in this implementation, the last element `list[right]`).
    *   The sub-array is reordered such that all elements less than or equal to the pivot are moved to its left, and all elements greater than the pivot are moved to its right.
    *   The `Partition` method returns the final index of the pivot element (`pivotIndex`).

5.  **Pivot Check and Recursion**: After partitioning:
    *   If `k` is equal to `pivotIndex`, it means the pivot element is exactly the k-th smallest element, and it is returned.
    *   If `k` is less than `pivotIndex`, the k-th smallest element must be in the left sub-array (elements smaller than the pivot). The algorithm then recursively calls `QuickselectRecursive` on the left sub-array (`list[left...pivotIndex-1]`).
    *   If `k` is greater than `pivotIndex`, the k-th smallest element must be in the right sub-array (elements larger than the pivot). The algorithm recursively calls `QuickselectRecursive` on the right sub-array (`list[pivotIndex+1...right]`).

This process effectively narrows down the search space in each step until the k-th element is found.

## 4. Complexity Analysis

*   **Time Complexity**:
    *   **Average Case**: O(N), where N is the number of elements in the list. This is because, on average, each partitioning step reduces the problem size by a constant factor, leading to a linear sum of work across all levels of recursion.
    *   **Worst Case**: O(N^2). This occurs when the pivot selection consistently results in highly unbalanced partitions (e.g., always picking the smallest or largest element as the pivot). In such scenarios, the problem size only reduces by one element in each step, similar to bubble sort.

*   **Space Complexity**:
    *   **Average Case**: O(log N). This is due to the recursion stack depth. With good pivot choices, the recursion depth is logarithmic.
    *   **Worst Case**: O(N). This occurs in the worst-case partitioning scenario where the recursion depth can be linear, consuming stack space proportional to the number of elements.