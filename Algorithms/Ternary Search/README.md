# Ternary Search Algorithm

## 1. Introduction

Ternary search is a divide-and-conquer search algorithm used for finding a target element within a sorted array or to identify an extremum (maximum or minimum) in a unimodal function. Unlike binary search which splits the search space into two parts, ternary search splits it into three parts. It is particularly useful when the search domain can be split into three segments and is often employed in optimization problems involving unimodal functions.

When searching for an element in a sorted array, the algorithm compares the target to two mid points to reduce the search space efficiently.

---

## 2. Usage

// Example usage of the TernarySearch class
int[] sortedArray = { 1, 4, 7, 9, 15, 18, 22 };
int targetValue = 15;

int index = TernarySearch.Search(sortedArray, targetValue);

// index will be 4 as 15 is at 0-based position 4 in the array

---

## 3. Detailed Explanation

The implementation is iterative and maintains two pointers, `left` and `right`, indicating the current search interval. Each iteration:

- Calculates two mid points:
  - `mid1 = left + (right - left) / 3`
  - `mid2 = right - (right - left) / 3`

- Compares the target to elements at these mid points.
  - If the target matches `arr[mid1]` or `arr[mid2]`, return that index immediately.

- Otherwise, narrow the search range depending on target's relation to `arr[mid1]` and `arr[mid2]`:
  - If target is less than `arr[mid1]`, the target lies in the left third section, so update `right = mid1 - 1`.
  - If target is greater than `arr[mid2]`, the target lies in the right third section, so update `left = mid2 + 1`.
  - Otherwise, it lies in the middle third, so update `left = mid1 + 1` and `right = mid2 - 1`.

Repeat this process until the target is found or the search space is empty (`left > right`). If not found, return `-1`.

This method ensures that the search space is reduced by roughly one third each iteration, similar to binary search's halving.

---

## 4. Complexity Analysis

- **Time Complexity:** O(log_3 n), where n is the number of elements in the array. Each iteration reduces the search space by about one-third.

- **Space Complexity:** O(1), since the search is performed iteratively without any additional memory allocation besides a few variables.

---

This implementation is clean, efficient, and easily reusable within any C# project requiring fast search on sorted integer arrays.