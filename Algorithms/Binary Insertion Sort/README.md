# Binary Insertion Sort

## Introduction

Binary Insertion Sort is a variant of the classic Insertion Sort algorithm that optimizes the process of finding the correct insertion position for each element by using a binary search. This combination reduces the number of comparisons needed to locate the right index within the sorted portion of the array. It is best suited for small to moderately sized arrays where stability and simplicity are desirable.

Use Binary Insertion Sort when you want a simple, in-place, stable sorting algorithm that is more efficient than regular Insertion Sort for larger arrays, but you don't need complexities like those found in advanced algorithms such as QuickSort or MergeSort.

## Usage

To use this algorithm, call the static `Sort` method with an array of integers you want to sort in ascending order:

int[] array = { 29, 10, 14, 37, 13 };
BinaryInsertionSort.Sort(array);
// array is now sorted: { 10, 13, 14, 29, 37 }

## Detailed Explanation

The algorithm iterates through the array, beginning with the second element:

1. It picks the element at the current index (`key`).
2. Employs binary search to find the correct index where this `key` should be inserted within the already sorted part of the array to the left.
3. Shifts all elements after this insertion position to the right by one step to make room for the `key`.
4. Inserts the `key` at its correct position.

The binary search efficiently locates the insertion index by repeatedly comparing the `key` to the middle element of the current search range, halving the search space each time. This optimization reduces the number of comparisons compared to a linear search for the insertion point.

## Complexity Analysis

- Time Complexity:
  - Best Case: O(n log n) when the array is nearly sorted. The binary search minimizes comparisons, but shifting elements still takes time.
  - Average and Worst Case: O(n^2) due to the element shifting after insertion.

- Space Complexity: O(1) as it sorts the array in-place without needing extra storage.

This implementation is stable because it inserts elements after existing equal elements, preserving their relative order.