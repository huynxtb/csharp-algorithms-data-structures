# Counting Sort Algorithm

## 1. Introduction

Counting Sort is a non-comparative integer sorting algorithm particularly useful when sorting integers within a limited range. Instead of comparing elements, it counts the number of occurrences of each distinct element, then calculates the position of each element in the sorted output. This makes the algorithm efficient with a time complexity of O(n + k), where n is the number of elements and k is the range of input values.

It is especially beneficial when the range of input integers is not significantly larger than the number of elements to be sorted.

## 2. Usage

To use the Counting Sort implementation, call the static `Sort` method by passing an integer array. The method returns a new sorted array without mutating the original.

Example:

int[] unsorted = { 4, 2, 2, 8, 3, 3, 1 };
int[] sorted = CountingSort.Sort(unsorted);
// sorted now contains: { 1, 2, 2, 3, 3, 4, 8 }

## 3. Detailed Explanation

- **Determine Range:** The algorithm starts by finding the maximum value in the input array to determine the range of the count array.
- **Counting Occurrences:** It initializes a count array of length `max + 1` with zeros and counts the occurrences of each integer from the input array.
- **Reconstruction:** Using the count array, it reconstructs the sorted array by placing each integer the number of times it appeared.

The algorithm works efficiently because it directly uses the counts to place elements rather than comparing elements to determine order.

## 4. Complexity Analysis

- **Time Complexity:**
  - Finding the maximum value: O(n)
  - Counting array population: O(n)
  - Constructing the sorted output: O(n + k), where k is the range (max element value)
  Overall complexity is O(n + k).

- **Space Complexity:**
  - A count array of size k (maximum element value + 1) is used.
  - An additional array for output of size n.
  The total space complexity is O(n + k).

This makes Counting Sort highly efficient when the range k is not significantly larger than n.
