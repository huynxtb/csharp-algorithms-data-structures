# Interpolation Search

## 1. Introduction

Interpolation Search is a search algorithm used for finding a target element in a **sorted array** of uniformly distributed values. It is an improvement over Binary Search for such datasets, as it uses the formula of linear interpolation to estimate the probable position of the target value, rather than simply dividing the search space in half. This results in faster average search times when the data is distributed evenly.

It is ideal for scenarios where the data is sorted and values are uniformly distributed across the search space.

## 2. Usage

Here's an example of how to use the Interpolation Search method:

int[] sortedArray = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
int target = 70;

int index = InterpolationSearchAlgorithm.InterpolationSearch(sortedArray, target);

if (index != -1)
{
    Console.WriteLine($"Element found at index {index}.");
}
else
{
    Console.WriteLine("Element not found.");
}

## 3. Detailed Explanation

The `InterpolationSearch` method takes a sorted array and a target value. It initializes two pointers:

- `low` pointing to the start of the array.
- `high` pointing to the end of the array.

While the target is within the range defined by the values at `low` and `high`, the algorithm estimates a probable position `pos` of the target using the formula:

pos = low + ((target - sortedArray[low]) * (high - low)) / (sortedArray[high] - sortedArray[low])

This formula assumes uniform distribution of array values and uses linear interpolation to guess where the target might be.

- If the element at `pos` matches the target, `pos` is returned.
- If the element at `pos` is less than the target, search continues from `pos + 1` to `high`.
- If the element at `pos` is greater than the target, search continues from `low` to `pos - 1`.

If the search space is exhausted without locating the target, the method returns -1.

Edge cases are handled such as:
- Empty or null arrays.
- Division by zero when `sortedArray[low]` equals `sortedArray[high]`.

## 4. Complexity Analysis

- **Time Complexity:**
  - Average case: O(log log n) if the data is uniformly distributed.
  - Worst case: O(n) when distribution is skewed or if all elements are identical.

- **Space Complexity:** O(1), as the algorithm uses only a few variables and performs the search in place.

This implementation is a clean, reusable static method contained within a single class file, with detailed commenting for clarity.