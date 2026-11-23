# Sparse Set

## Introduction
The Sparse Set is a data structure designed to efficiently map non-contiguous integer keys to values. It is particularly useful when dealing with sparse data, where the keys are not densely packed. This structure allows for fast insertion, deletion, and lookup operations while minimizing memory usage.

## Usage
```csharp
SparseSet<string> sparseSet = new SparseSet<string>();
sparseSet.Insert(0, "Value1");
sparseSet.Insert(2, "Value2");
sparseSet.Remove(0);
string value = sparseSet.Get(2);
```

## Detailed Explanation
The Sparse Set uses two lists: one for storing the values and another for storing the indices of those values. When a key is inserted, if it is new, it is added to the values list, and its index is stored in the indices list. If a key is removed, its index in the indices list is set to -1, marking it as deleted. The `Get` method retrieves the value associated with a key, throwing an exception if the key does not exist.

## Complexity Analysis
- **Insert:** O(1) on average, but can be O(n) in the worst case when resizing the indices list.
- **Remove:** O(1)
- **Get:** O(1)
- **Space Complexity:** O(n) where n is the number of unique keys inserted.