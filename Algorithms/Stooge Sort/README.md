# Stooge Sort

## 1. Introduction
Stooge Sort is an inefficient recursive sorting algorithm that uses a divide-and-conquer approach. It divides the array into three overlapping segments of size $2n/3$. It serves primarily as an educational tool for complexity analysis and recurrence relations rather than practical sorting.

## 2. Usage
```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int[] array = { 9, 3, 7, 5, 6, 2, 8, 4 };
        StoogeSort.Sort(array);
        Console.WriteLine(string.Join(", ", array));

        Span<int> span = stackalloc int[] { 40, 10, 30, 20 };
        StoogeSort.Sort(span, (a, b) => b.CompareTo(a));
    }
}
```

## 3. Detailed Explanation
The algorithm operates via the following recursive steps:
1. If the element at the beginning is greater than the element at the end, swap them.
2. If the current range contains 3 or more elements:
   - Compute one-third of the range: $t = \lfloor(end - start + 1) / 3\rfloor$.
   - Recursively sort the initial two-thirds: `[start, end - t]`.
   - Recursively sort the trailing two-thirds: `[start + t, end]`.
   - Recursively sort the initial two-thirds again: `[start, end - t]`.

## 4. Complexity Analysis
- **Time Complexity**:
  - Best, Average, Worst Case: $O(n^{\log_{1.5}{3}}) \approx O(n^{2.7095})$. Recurrence relation is $T(n) = 3T(2n/3) + O(1)$.
- **Space Complexity**:
  - Worst Case: $O(\log_{1.5}{n})$ stack space due to recursive call depth.