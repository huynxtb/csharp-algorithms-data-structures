# Cycle Sort

### 1. Introduction
Cycle Sort is an in-place, unstable sorting algorithm. It is theoretically optimal in terms of the total number of writes to the original array. It minimizes memory writes by decomposing the array permutation into cycles, rotating each cycle to place elements in their correct positions. It is highly useful when memory write operations are expensive (e.g., EEPROM or Flash memory).

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        int[] array = { 5, 1, 8, 3, 5, 2 };
        CycleSorter.Sort(array);
        // Output: 1, 2, 3, 5, 5, 8
        Console.WriteLine(string.Join(", ", array));
    }
}
```

### 3. Detailed Explanation
The algorithm works by iterating through the array and treating each element as the start of a cycle:
1. It finds the correct index `pos` where the current `item` should be placed by counting how many elements in the remaining array are smaller than it.
2. If the element is already in the correct position, it moves to the next index.
3. If there are duplicate elements, it increments `pos` to place the element after its duplicates.
4. It writes the `item` to its correct position and retrieves the displaced element, which becomes the new `item` to place.
5. This process repeats (rotating the cycle) until the displaced element is returned to the starting position of the cycle.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Worst Case**: $O(n^2)$ - Requires nested loops to count smaller elements for each cycle rotation.
  - **Average Case**: $O(n^2)$
  - **Best Case**: $O(n^2)$ - Still performs comparisons to verify positions even if the array is sorted.
- **Space Complexity**: $O(1)$ auxiliary - Sorts the array in-place using a constant amount of extra memory.