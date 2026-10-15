# Pigeonhole Sort

## Introduction
Pigeonhole Sort is a non-comparative sorting algorithm that is particularly efficient for sorting lists of elements where the number of elements and the range of key values are approximately the same. It works by creating a set of 'pigeonholes' (auxiliary storage) corresponding to the range of values in the input array. Elements are then distributed into their respective pigeonholes and collected in order to produce a sorted array.

## Usage
```csharp
int[] array = { 8, 3, 2, 7, 4, 6, 5 }; 
SortingAlgorithms.PigeonholeSort(array);
// array is now sorted: { 2, 3, 4, 5, 6, 7, 8 }
```

## Detailed Explanation
The algorithm begins by checking if the input array is null or empty, returning immediately if so. It then finds the minimum and maximum values in the array to determine the range of pigeonholes needed. An array of pigeonholes is created, initialized to zero. Each element from the input array is placed into its corresponding pigeonhole based on its value. Finally, the elements are collected from the pigeonholes back into the original array in sorted order.

## Complexity Analysis
- **Time Complexity:** O(n + k), where n is the number of elements in the input array and k is the range of the input values (max - min + 1). This makes it efficient for small ranges of values.
- **Space Complexity:** O(k), for the pigeonhole array used to store the counts of each value.