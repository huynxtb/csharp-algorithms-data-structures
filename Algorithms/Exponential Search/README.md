## 1. Introduction
Exponential Search, also known as doubling search, galloping search, or stride search, is an algorithm for finding a target value in a sorted array. It is particularly useful for unbounded arrays (arrays of infinite size) or very large arrays where the target element is likely to be found near the beginning. The algorithm first finds a range where the target element might reside and then performs a standard binary search within that range.

## 2. Usage
```csharp
using System;

public class Program
{
    public static void Main(string[] args)
    {
        int[] sortedArray = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35 };

        // Search for an existing value
        int valueToFind = 19;
        int index = ExponentialSearch.Search(sortedArray, valueToFind);
        if (index != -1)
        {
            Console.WriteLine($"Value {valueToFind} found at index: {index}"); // Expected: 7
        }
        else
        {
            Console.WriteLine($"Value {valueToFind} not found.");
        }

        // Search for a non-existing value
        int nonExistentValue = 10;
        index = ExponentialSearch.Search(sortedArray, nonExistentValue);
        if (index != -1)
        {
            Console.WriteLine($"Value {nonExistentValue} found at index: {index}");
        }
        else
        {
            Console.WriteLine($"Value {nonExistentValue} not found."); // Expected: not found
        }

        // Search for the first element
        int firstValue = 1;
        index = ExponentialSearch.Search(sortedArray, firstValue);
        if (index != -1)
        {
            Console.WriteLine($"Value {firstValue} found at index: {index}"); // Expected: 0
        }
        else
        {
            Console.WriteLine($"Value {firstValue} not found.");
        }

        // Search for a value smaller than the first element
        int smallerValue = 0;
        index = ExponentialSearch.Search(sortedArray, smallerValue);
        if (index != -1)
        {
            Console.WriteLine($"Value {smallerValue} found at index: {index}");
        }
        else
        {
            Console.WriteLine($"Value {smallerValue} not found."); // Expected: not found
        }

        // Search in an empty array
        int[] emptyArray = new int[0];
        int valueInEmpty = 5;
        index = ExponentialSearch.Search(emptyArray, valueInEmpty);
        if (index != -1)
        {
            Console.WriteLine($"Value {valueInEmpty} found in empty array at index: {index}");
        }
        else
        {
            Console.WriteLine($"Value {valueInEmpty} not found in empty array."); // Expected: not found
        }
    }
}
```

## 3. Detailed Explanation
The `ExponentialSearch` class provides a static generic method `Search<T>(T[] array, T value)` to find an element in a sorted array.

1.  **Edge Case Handling**: The method first checks for `null` or empty arrays, returning `-1` immediately if either condition is met. It also handles the case where the `value` is smaller than the first element of the array (returning `-1`) or if the `value` is exactly the first element (returning `0`).

2.  **Range Finding (Exponential Step)**: If the `value` is not the first element and potentially exists in the array, the algorithm proceeds to find a suitable range for a binary search. It starts with a `bound` of `1`. It then repeatedly doubles this `bound` (`1, 2, 4, 8, ...`) as long as `bound` is within the array limits and `array[bound]` is less than the `value`. This step quickly narrows down the potential location of the `value`.

3.  **Binary Search**: Once the loop terminates, the `value` is guaranteed to be within the range `[bound / 2, min(bound, array.Length - 1)]`. The `left` boundary for the binary search is `bound / 2`, and the `right` boundary is the minimum of `bound` (the last checked exponential step) and `array.Length - 1` (the actual end of the array). A standard binary search is then performed within this determined range.

4.  **Return Value**: The `BinarySearch` helper method returns the index of the `value` if found, or `-1` otherwise. This result is then returned by the `Search` method.

## 4. Complexity Analysis

*   **Time Complexity**:
    *   **Worst Case**: O(log N), where N is the number of elements in the array. This occurs when the element is at the end of the array or not present. The exponential step takes O(log N) time to find the range, and the subsequent binary search also takes O(log N) time within that range.
    *   **Best Case**: O(1). This occurs if the target element is the first element of the array.
    *   **Average Case**: O(log N).

*   **Space Complexity**:
    *   **Worst Case**: O(1). The algorithm uses a constant amount of extra space for variables regardless of the input array size.