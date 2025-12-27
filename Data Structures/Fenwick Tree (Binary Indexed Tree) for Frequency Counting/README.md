# Fenwick Tree (Binary Indexed Tree) for Frequency Counting

## 1. Introduction

The Fenwick Tree, also known as a Binary Indexed Tree, is a data structure that efficiently supports prefix sum queries and updates on a list of frequencies or values. It is particularly useful in scenarios where there are frequent updates and queries on cumulative frequencies or sums.

Use this data structure when you need to:
- Quickly update the frequency or value at a specific position.
- Compute prefix sums or cumulative frequencies up to a given index.

The Fenwick Tree provides both update and query operations in O(log n) time, where n is the size of the input array.

## 2. Usage

// Initialize from an array of frequencies
int[] frequencies = {1, 3, 4, 8, 6, 1, 4, 2};
FenwickTree fenwickTree = new FenwickTree(frequencies);

// Update the frequency at index 3 (0-based) by adding 5
fenwickTree.Update(3, 5);

// Query the prefix sum up to index 5 (0-based)
int prefixSum = fenwickTree.Query(5);

// prefixSum now represents the sum of elements frequencies[0..5]

## 3. Detailed Explanation

- Internally, the Fenwick Tree uses a 1-based indexed array to store partial cumulative sums.
- The `Update` method adds a given delta to the frequency at a specified index, then updates all affected nodes in the Fenwick Tree in O(log n) time.
- The `Query` method computes the prefix sum from index 0 up to a given index by traversing back through the Fenwick Tree nodes, also in O(log n) time.
- The constructor that accepts an array initializes the Fenwick Tree efficiently by inserting all frequencies one by one.

The key technique involves bit manipulation to move to parent nodes in the tree using the operation `index += index & (-index)` for updates and `index -= index & (-index)` for queries.

## 4. Complexity Analysis

| Operation                    | Time Complexity | Space Complexity |
|-----------------------------|-----------------|------------------|
| Initialization from array    | O(n log n)      | O(n)             |
| Update (increment frequency) | O(log n)       | O(n)             |
| Query (prefix sum)           | O(log n)        | O(n)             |

The Fenwick Tree is memory efficient, using only a single array of size n+1. Its logarithmic update and query times make it suitable for large datasets with many updates and queries.
