# Optimal Merge Pattern

## Introduction
The Optimal Merge Pattern is an algorithm used to efficiently merge multiple sorted arrays or lists into a single sorted array. It minimizes the number of comparisons needed during the merging process, making it particularly useful in scenarios where multiple sorted datasets need to be combined.

## Usage
```csharp
int[][] arrays = new int[][]
{
    new int[] { 1, 4, 7 },
    new int[] { 2, 5, 8 },
    new int[] { 3, 6, 9 }
};
OptimalMergePattern omp = new OptimalMergePattern();
int[] mergedArray = omp.MergeSortedArrays(arrays);
```

## Detailed Explanation
The implementation uses a custom priority queue to manage the merging of sorted arrays. Each entry in the queue represents the current index of an array being processed. The algorithm repeatedly extracts the minimum element from the queue, adds it to the merged result, and then pushes the next element from the same array back into the queue if available. This process continues until all elements from all arrays have been merged.

## Complexity Analysis
- **Time Complexity:** O(N log k), where N is the total number of elements across all arrays and k is the number of arrays. Each insertion and extraction operation from the priority queue takes logarithmic time relative to the number of arrays.
- **Space Complexity:** O(k), where k is the number of arrays, as we maintain a priority queue of size k.