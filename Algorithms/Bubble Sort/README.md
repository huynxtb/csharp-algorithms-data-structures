# Bubble Sort

### 1. Introduction
Bubble Sort is a simple, comparison-based sorting algorithm. It repeatedly steps through the list, compares adjacent elements, and swaps them if they are in the wrong order. The pass through the list is repeated until the list is sorted. It is best used for educational purposes or for small datasets where simplicity is preferred over performance.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        int[] data = { 5, 3, 8, 4, 2 };
        BubbleSorter.Sort(data);
        // data is now { 2, 3, 4, 5, 8 }
    }
}
```

### 3. Detailed Explanation
The implementation uses a generic method `Sort<T>` constrained to types implementing `IComparable<T>`. 
- The outer loop tracks the pass number. After each pass, the largest unsorted element bubbles up to its correct position at the end of the array.
- The inner loop compares adjacent elements `array[j]` and `array[j + 1]` up to the unsorted boundary `n - i - 1`.
- An optimization flag `swapped` tracks if any elements were exchanged during a pass. If no swaps occur, the array is already sorted, and the algorithm terminates early.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Worst Case**: O(N²) when the array is sorted in reverse order.
  - **Average Case**: O(N²).
  - **Best Case**: O(N) when the array is already sorted (due to the early termination optimization).
- **Space Complexity**: O(1) auxiliary space because the sorting is performed in-place.