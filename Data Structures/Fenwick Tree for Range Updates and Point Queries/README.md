# Fenwick Tree for Range Updates and Point Queries

## Introduction

The Fenwick Tree (also known as Binary Indexed Tree) is a data structure that provides efficient methods for cumulative frequency/interval summation queries. This is a specialized variant designed to allow efficient range updates and point queries on an integer array. Instead of the classic Fenwick Tree that supports point updates and prefix sum queries, this variant enables updates on ranges [l, r] by adding a value to each element in the range and querying the exact value at a single index after all updates.

This data structure is useful when you need to perform frequent range increments and single point queries without repeatedly updating each element individually, which would be inefficient.

## Usage

var fenw = new FenwickTreeRangeUpdatePointQuery(10); // 10 elements, zero-based indexing
fenw.RangeUpdate(2, 5, 3);   // Add 3 to elements at indices 2,3,4,5
fenw.RangeUpdate(0, 3, 2);   // Add 2 to elements at indices 0,1,2,3
int valAt3 = fenw.PointQuery(3); // Query the value at index 3

## Detailed Explanation

- **Internal Structure**: The class uses a single Fenwick tree array `fenw` with size `n+1` internally, leveraging one-based indexing for Fenwick operations while exposing zero-based indexing externally.

- **Range Updates**: The trick to supporting range updates with Fenwicks is to maintain a difference array implicitly. To add a value `v` over [l, r], the Fenwick tree is updated at index `l` with `+v` and at index `r+1` with `-v` (if in range). This way, prefix sums computed by the Fenwick tree hold the cumulative increments up to any index.

- **Point Queries**: A point query is then simply the prefix sum at that index in the Fenwick tree, which accumulates all range increments affecting it.

- **Operations**:
  - `InternalUpdate`: Adds a delta to Fenwick tree nodes along the update path.
  - `InternalPrefixSum`: Calculates the sum of increments up to an index.
  - `RangeUpdate`: Applies increments over a range using the dual Fenwick tree update approach.
  - `PointQuery`: Returns the value at the exact index after all increments.

## Complexity Analysis

- **Time Complexity**:
  - `RangeUpdate(left, right, value)`: O(log n) because it performs two Fenwick tree updates.
  - `PointQuery(index)`: O(log n) due to a Fenwick prefix sum computation.

- **Space Complexity**:
  - O(n) for the Fenwick tree storage array.

The approach ensures that both updates and queries scale logarithmically with the size of the array, suitable for large datasets with many such operations.