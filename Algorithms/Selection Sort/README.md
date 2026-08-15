# Introduction
Selection Sort is a simple sorting algorithm that works by repeatedly finding the minimum element from the unsorted part and putting it at the beginning of the sorted part. It maintains two subarrays in a given array: the subarray which is already sorted, and the remaining subarray which is unsorted.

# Usage
```csharp
List<int> numbers = new List<int> { 64, 34, 25, 12, 22, 11, 90 }; 
SortingAlgorithms.SelectionSort(numbers);
Console.WriteLine(string.Join(" ", numbers));
```

# Detailed Explanation
The Selection Sort algorithm works as follows:
1. The outer loop (`for (int i = 0; i < list.Count - 1; i++)`) iterates over each element in the list.
2. The inner loop (`for (int j = i + 1; j < list.Count; j++)`) finds the minimum element in the unsorted part of the list.
3. If a smaller element is found, its index is stored in `minIndex`.
4. After the inner loop finishes, the minimum element is swapped with the first element of the unsorted part.
5. This process continues until the entire list is sorted.

# Complexity Analysis
* Time Complexity: O(n^2) in all cases (best, average, worst).
* Space Complexity: O(1) because it only uses a constant amount of additional space.