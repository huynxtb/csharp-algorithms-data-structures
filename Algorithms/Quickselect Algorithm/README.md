# Quickselect Algorithm

## Introduction
The Quickselect algorithm is an efficient method for finding the k-th smallest element in an unsorted list. It is related to the Quicksort sorting algorithm and utilizes a partitioning approach to narrow down the search space. Quickselect is particularly useful when you need to find an element without fully sorting the array, making it faster in many scenarios.

## Usage
Here is an example of how to use the Quickselect algorithm in C#:
```csharp
int[] numbers = { 3, 6, 2, 7, 5, 1, 4 };  
int k = 3;  
int kthSmallest = Quickselect.Select(numbers, k);  
Console.WriteLine(kthSmallest);  // Output: 3
```

## Detailed Explanation
The Quickselect algorithm works by selecting a pivot element from the array and partitioning the other elements into two sub-arrays according to whether they are less than or greater than the pivot. The algorithm then recursively narrows down the search to one of the sub-arrays based on the position of the pivot relative to k. If the pivot's index matches k, the pivot is the k-th smallest element. If k is less than the pivot's index, the search continues in the left sub-array; otherwise, it continues in the right sub-array. This process continues until the k-th smallest element is found.

## Complexity Analysis
- **Time Complexity:** Average case is O(n), and the worst case is O(n^2) (when the smallest or largest element is consistently chosen as the pivot).
- **Space Complexity:** O(1) for the in-place partitioning, but O(log n) for the recursion stack in the average case.