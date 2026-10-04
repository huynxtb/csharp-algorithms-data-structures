# Sparse Table for Range Maximum Query (RMQ)

## Introduction

A **Sparse Table** is a powerful static data structure designed to efficiently answer **Range Maximum Query (RMQ)** problems. Given an array, the sparse table allows you to find the maximum value in any contiguous subarray in **O(1) time** after performing **O(n log n)** preprocessing. Unlike dynamic data structures that support updates, the sparse table is immutable after construction, making it ideal for scenarios where the data is fixed and queries are frequent.

### When to Use
- Finding maximum values in ranges of fixed, immutable data
- Applications requiring millions of range queries with minimal latency
- Competitive programming and algorithm challenges
- Static analysis problems where real-time updates are not required

## Usage

```csharp
using System;
using System.Collections.Generic;
using Algorithms.DataStructures;

// Create a sparse table from an array of integers
int[] data = { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3 };
var sparseTable = new SparseTableRMQ<int>(data);

// Query the maximum value in range [2, 5] (indices inclusive)
int maxValue = sparseTable.Query(2, 5);  // Returns 9
Console.WriteLine($"Max in [2, 5]: {maxValue}");

// Query with index tracking
int maxVal = sparseTable.QueryIndex(0, 9, out int maxIndex);
Console.WriteLine($"Max in [0, 9]: {maxVal} at index {maxIndex}");  // Max: 9, Index: 5

// Get sparse table properties
Console.WriteLine($"Number of elements: {sparseTable.Count}");
Console.WriteLine($"Number of levels: {sparseTable.Levels}");

// Works with any comparable type
string[] words = { "apple", "zebra", "banana", "mango" };
var stringTable = new SparseTableRMQ<string>(words);
string maxWord = stringTable.Query(1, 3);  // Returns "zebra"
```

## Detailed Explanation

### How It Works

The sparse table stores precomputed maximum values for all subarrays whose lengths are powers of 2.

**Table Structure:**
- `table[k][i]` contains the maximum value in the subarray `arr[i..i+2^k-1]`
- Level 0 stores individual elements: `table[0][i] = arr[i]`
- Each subsequent level k is built from level k-1 using the recurrence:
  ```
  table[k][i] = max(table[k-1][i], table[k-1][i + 2^(k-1)])
  ```

**Preprocessing Phase (O(n log n)):**
1. Initialize `table[0]` with all elements from the input array
2. For each level k from 1 to log₂(n):
   - For each valid starting index i:
     - Combine two overlapping subarrays of half the desired length
     - Store the maximum of the two in `table[k][i]`
3. Precompute a logarithm lookup table `log2[i]` for O(1) access during queries

**Query Phase (O(1)):**
1. Given a range [left, right], calculate the range length: `len = right - left + 1`
2. Find `k = floor(log₂(len))`
3. The query uses the overlapping intervals trick:
   - Two overlapping intervals of length 2^k cover the entire range [left, right]
   - Interval 1: `table[k][left]`
   - Interval 2: `table[k][right - 2^k + 1]`
4. Return the maximum of these two values

### Key Features

- **Generic Implementation:** Works with any type implementing `IComparable<T>`
- **Constant-Time Queries:** After preprocessing, each query runs in O(1) time
- **Index Tracking:** The `QueryIndex` method returns both the maximum value and its leftmost index
- **Immutable:** Data is set at construction and cannot be modified
- **Input Validation:** Throws appropriate exceptions for null or empty data and invalid query ranges

## Complexity Analysis

### Time Complexity

| Operation | Complexity | Notes |
|-----------|------------|-------|
| Construction | O(n log n) | Building all levels of the sparse table |
| Query | O(1) | Constant-time lookup with precomputed log table |
| QueryIndex | O(1) | Same as Query, with additional index tracking |

### Space Complexity

- **Table Storage:** O(n log n) - The sparse table with log₂(n) + 1 levels, each up to n entries
- **Log Lookup:** O(n) - Precomputed logarithm values
- **Total:** O(n log n)

### Why This Works

The key insight is that any contiguous range can be covered by at most two overlapping intervals of the same power-of-2 length:

```
Range [left, right] with length L:
- Find k = floor(log₂(L))
- Interval A: [left, left + 2^k - 1]
- Interval B: [right - 2^k + 1, right]
- These intervals overlap but cover the entire range
- max(range) = max(max(A), max(B))
```

Since both intervals have their maxima precomputed in the sparse table, the query is O(1).

### Comparison with Other Structures

| Structure | Build Time | Query Time | Update | Use Case |
|-----------|------------|------------|--------|----------|
| Sparse Table | O(n log n) | O(1) | Not supported | Static, frequent queries |
| Segment Tree | O(n) | O(log n) | O(log n) | Dynamic, mixed operations |
| Sqrt Decomposition | O(n) | O(√n) | O(√n) | Moderate range sizes |
| Naive Array | O(n) | O(n) | O(1) | Small ranges, updates needed |
