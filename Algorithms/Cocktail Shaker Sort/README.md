# Cocktail Shaker Sort

## 1. Introduction
Cocktail Shaker Sort, also known as Bidirectional Bubble Sort, Shaker Sort, or Ripple Sort, is a variation of Bubble Sort. It is a comparison sort algorithm that is stable and performs slightly better than standard Bubble Sort. It works by traversing the list in both directions (left-to-right and then right-to-left) in each pass. In the forward pass, it pushes the largest unsorted element to its correct position at the end of the unsorted section. In the backward pass, it pushes the smallest unsorted element to its correct position at the beginning of the unsorted section. This bidirectional movement helps elements reach their final positions faster than a single-direction Bubble Sort. It is generally used for educational purposes or on small lists, as more efficient algorithms like Merge Sort or Quick Sort are preferred for larger datasets.

## 2. Usage
To use the `CocktailShakerSorter`, simply call one of its static `Sort` methods, providing an `IList<T>` to be sorted. You can optionally provide a custom `IComparer<T>` for specific sorting logic.

```csharp
using System;
using System.Collections.Generic;

public class Example
{
    public static void Main(string[] args)
    {
        // Example 1: Sorting a list of integers using default comparer
        List<int> numbers = new List<int> { 5, 1, 4, 2, 8, 0, 9, 3, 7, 6 };
        Console.WriteLine("Original List: " + string.Join(", ", numbers));
        CocktailShakerSorter.Sort(numbers);
        Console.WriteLine("Sorted List (default): " + string.Join(", ", numbers)); // Output: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9

        Console.WriteLine();

        // Example 2: Sorting a list of strings using a custom comparer (descending order)
        List<string> words = new List<string> { "banana", "apple", "cherry", "date" };
        Console.WriteLine("Original Words: " + string.Join(", ", words));
        CocktailShakerSorter.Sort(words, Comparer<string>.Create((s1, s2) => s2.CompareTo(s1)));
        Console.WriteLine("Sorted Words (descending): " + string.Join(", ", words)); // Output: date, cherry, banana, apple

        Console.WriteLine();

        // Example 3: Empty list
        List<int> emptyList = new List<int>();
        CocktailShakerSorter.Sort(emptyList);
        Console.WriteLine("Empty List (sorted): " + string.Join(", ", emptyList)); // Output: (empty)

        // Example 4: Single element list
        List<int> singleElementList = new List<int> { 42 };
        CocktailShakerSorter.Sort(singleElementList);
        Console.WriteLine("Single Element List (sorted): " + string.Join(", ", singleElementList)); // Output: 42
    }
}
```

## 3. Detailed Explanation
The `CocktailShakerSorter` class provides two static `Sort` methods:
- `Sort<T>(IList<T> list)`: This overload uses the default comparer for type `T`, requiring `T` to implement `IComparable<T>`.
- `Sort<T>(IList<T> list, IComparer<T> comparer)`: This overload allows specifying a custom `IComparer<T>` for sorting.

Both methods first perform guard checks for null or single-element lists, returning immediately as such lists are considered sorted.

The core algorithm operates within a `do-while` loop:
1.  **Initialization**: `left` and `right` pointers define the current unsorted section of the list. `swapped` is a flag to track if any swaps occurred in a pass.
2.  **Forward Pass (Left-to-Right)**:
    -   The loop iterates from `left` to `right - 1`.
    -   It compares adjacent elements (`list[i]` and `list[i + 1]`). If `list[i]` is greater than `list[i + 1]` (according to the comparer), they are swapped.
    -   If a swap occurs, `swapped` is set to `true`.
    -   After this pass, the largest element in the current unsorted section is guaranteed to be at `list[right]`.
    -   If no swaps occurred in this pass, the list is already sorted, and the algorithm breaks early.
    -   The `right` boundary is then decremented because the element at `list[right]` is now in its final sorted position.
3.  **Backward Pass (Right-to-Left)**:
    -   `swapped` is reset to `false`.
    -   The loop iterates from `right` down to `left + 1`.
    -   It compares adjacent elements (`list[i]` and `list[i - 1]`). If `list[i]` is smaller than `list[i - 1]`, they are swapped.
    -   If a swap occurs, `swapped` is set to `true`.
    -   After this pass, the smallest element in the current unsorted section is guaranteed to be at `list[left]`.
    -   The `left` boundary is then incremented because the element at `list[left]` is now in its final sorted position.
4.  **Loop Termination**: The `do-while` loop continues as long as `swapped` is `true` (meaning elements were moved in the previous two passes) and `left` has not crossed `right`. If no swaps occur in a full forward and backward pass, the list is sorted.

A private `Swap` helper method is used to exchange elements efficiently.

## 4. Complexity Analysis
-   **Time Complexity**:
    -   **Worst Case**: O(n^2). This occurs when the list is sorted in reverse order. In each pass, elements move one position at a time.
    -   **Average Case**: O(n^2). Similar to Bubble Sort, it requires multiple passes over the data.
    -   **Best Case**: O(n). This occurs when the list is already sorted. The algorithm will perform one forward pass, detect no swaps, and terminate early.
-   **Space Complexity**:
    -   **Worst Case**: O(1). Cocktail Shaker Sort is an in-place sorting algorithm, meaning it only requires a constant amount of additional memory for temporary variables (like `temp` for swapping and loop counters), regardless of the input size.
