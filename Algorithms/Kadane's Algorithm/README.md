# Kadane's Algorithm

### 1. Introduction
Kadane's Algorithm is an iterative dynamic programming algorithm used to solve the Maximum Subarray Problem. The goal is to find the contiguous subarray within a one-dimensional array of numbers that has the largest sum. It is commonly used in data analysis, computer vision, and financial analysis to find maximum profit intervals.

### 2. Usage
```csharp
using System;
using Algorithms.Subarray;

public class Program
{
    public static void Main()
    {
        int[] numbers = { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
        ReadOnlySpan<int> span = numbers.AsSpan();

        SubarrayResult<int> result = KadaneAlgorithm.FindMaximumSubarray(span);

        Console.WriteLine($"Max Sum: {result.Sum}"); // Output: 6
        Console.WriteLine($"Start Index: {result.StartIndex}"); // Output: 3
        Console.WriteLine($"End Index: {result.EndIndex}"); // Output: 6
    }
}
```

### 3. Detailed Explanation
The algorithm maintains two values during iteration:
- `maxEndingHere`: The maximum subarray sum ending at the current position.
- `maxSoFar`: The maximum subarray sum found so far.

At each step, the algorithm decides whether to add the current element to the existing subarray or start a new subarray at the current element. This decision is made by comparing `maxEndingHere + current` with `current`. If starting a new subarray yields a larger value, the starting index tracker is updated. If `maxEndingHere` exceeds `maxSoFar`, the global maximum and the ending index are updated.

This implementation supports all-negative inputs by returning the maximum single element instead of defaulting to 0.

### 4. Complexity Analysis
- **Time Complexity**: $O(N)$ where $N$ is the number of elements in the input span. The algorithm performs a single pass over the data.
- **Space Complexity**: $O(1)$ auxiliary space. The algorithm uses a constant amount of memory to track indices and sums, utilizing `ReadOnlySpan<T>` to avoid memory allocations.