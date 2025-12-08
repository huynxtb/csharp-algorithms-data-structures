# Fenwick Tree for Frequency Counting

## Introduction

A Fenwick Tree, also known as a Binary Indexed Tree, is a data structure that efficiently maintains prefix sums and updates on a dynamic array. Specifically, the Fenwick Tree for Frequency Counting is designed to store frequencies of elements in a dynamic array and quickly compute cumulative frequencies (prefix sums) as well as update frequencies of elements.

This data structure excels in scenarios where you need to repeatedly update counts and query prefix sums in logarithmic time, such as frequency counting, cumulative frequency tables, and range queries.

## Usage

The FenwickTreeFrequency class offers two primary operations:
- `Update(int index, int delta)`: Updates the frequency of the element at a 1-based index by adding `delta`.
- `Query(int index)`: Returns the cumulative frequency up to the 1-based index.

Here's a simple example demonstrating usage:

// Initialize Fenwick Tree for array size 10
FenwickTreeFrequency fenw = new FenwickTreeFrequency(10);

// Update frequency of elements
fenw.Update(3, 5);  // Add frequency 5 at index 3
fenw.Update(7, 2);  // Add frequency 2 at index 7

// Query cumulative frequencies
int sumUpTo3 = fenw.Query(3);  // Returns 5
int sumUpTo7 = fenw.Query(7);  // Returns 7
int sumUpTo10 = fenw.Query(10); // Returns 7

Console.WriteLine($"Sum up to 3: {sumUpTo3}");
Console.WriteLine($"Sum up to 7: {sumUpTo7}");
Console.WriteLine($"Sum up to 10: {sumUpTo10}");

## Detailed Explanation

- **Internal Structure:** Internally, the Fenwick Tree uses an integer array of size `size + 1`, since it uses 1-based indexing for simpler arithmetic.
- **Update Operation:** To update the frequency of an element at position `index`, the algorithm adds the `delta` to all relevant tree nodes by moving from the current index upwards using `index += index & (-index)` until it exceeds the array size.
- **Query Operation:** To get the prefix sum up to `index`, the algorithm accumulates sums from the tree nodes moving backward by `index -= index & (-index)` until index becomes 0.
- **1-Based Indexing:** It is critical that the user provides and uses 1-based indexing when calling methods.

This design ensures both updates and prefix sum queries execute in **O(log n)** time, making the structure highly efficient for dynamic frequency counting.

## Complexity Analysis

| Operation | Time Complexity | Space Complexity |
|-----------|-----------------|-----------------|
| Update    | O(log n)        | -               |
| Query     | O(log n)        | -               |
| Construction | O(n) or O(1) based on initialization | O(n) for fenwickTree array |

Here, `n` is the size of the input array.

The Fenwick Tree uses extra linear space proportional to the size of the input, but operations are logarithmic in time, making it suitable for large datasets with frequent updates and queries.
