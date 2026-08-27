# Comb Sort

### 1. Introduction
Comb Sort is an in-place comparison-based sorting algorithm that improves upon Bubble Sort. While Bubble Sort always compares adjacent items (gap size of 1), Comb Sort uses a gap size larger than 1 and shrinks it by a factor on each iteration. This eliminates "turtles"—small values near the end of the list that slow down Bubble Sort significantly.

### 2. Usage
```csharp
using System;
using System.Collections.Generic;
using Algorithms.Sorting;

class Program
{
    static void Main()
    {
        List<int> data = new List<int> { 8, 4, 1, 56, 3, -44, 23, -6, 28, 0 };
        
        // Sort in-place using default comparer
        CombSort.Sort(data);
        
        Console.WriteLine(string.Join(", ", data));
        // Output: -44, -6, 0, 1, 3, 8, 23, 28, 56
    }
}
```

### 3. Detailed Explanation
- **Gap Initialization**: The algorithm starts with a gap equal to the length of the list.
- **Shrink Factor**: On each step, the gap is divided by a shrink factor of `1.3` (empirically found to be the most efficient value) and cast to an integer. If the gap falls below 1, it is clamped to 1.
- **Comparisons and Swaps**: The algorithm compares elements separated by the current gap. If they are out of order, they are swapped.
- **Termination**: The process repeats until the gap size reaches 1 and an entire pass is completed without any swaps, ensuring the list is fully sorted.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Best Case**: $O(n \log n)$ when the list is already sorted or nearly sorted.
  - **Average Case**: $O(n^2 / 2^p)$ where $p$ is the number of increments, which practically behaves close to $O(n \log n)$ or $O(n^2)$ depending on the data distribution.
  - **Worst Case**: $O(n^2)$ in the worst-case scenarios.
- **Space Complexity**: $O(1)$ auxiliary space because the sorting is performed in-place.