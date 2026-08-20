# Insertion Sort

### 1. Introduction
Insertion Sort is a simple, comparison-based sorting algorithm that builds the final sorted array one item at a time. It is highly efficient for small data sets or arrays that are already partially sorted. It is stable (preserves the relative order of equal elements) and operates in-place, requiring minimal memory overhead.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        int[] data = { 5, 2, 9, 1, 5, 6 };
        InsertionSort.Sort(data);
        Console.WriteLine(string.Join(", ", data));
    }
}
```

### 3. Detailed Explanation
The algorithm splits the input array conceptually into a sorted and an unsorted part. 
- It starts from the second element (index 1), assuming the first element is already sorted.
- The current element (`key`) is compared with the elements in the sorted partition (to its left).
- Elements in the sorted partition that are greater than the `key` are shifted one position to the right to make room.
- The `key` is inserted into its correct relative position.
- This process repeats for all elements until the entire array is sorted.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Best Case**: $O(n)$ when the array is already sorted (only one comparison per element, no shifts).
  - **Average Case**: $O(n^2)$ when elements are randomly distributed.
  - **Worst Case**: $O(n^2)$ when the array is sorted in reverse order.
- **Space Complexity**: $O(1)$ auxiliary space because the sorting is performed in-place.