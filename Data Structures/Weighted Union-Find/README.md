# Introduction
The Weighted Union-Find algorithm, also known as Disjoint Set Union (DSU), is a data structure that efficiently manages a partition of a set into disjoint subsets. It supports two primary operations: `Union`, which merges two subsets, and `Find`, which determines the subset an element belongs to. This structure is particularly useful in scenarios like network connectivity, image processing, and clustering.

# Usage
```csharp
UnionFind uf = new UnionFind(10);
uf.Union(1, 2);
uf.Union(2, 3);
int root = uf.Find(3); // Returns the root of the set containing 3
```

# Detailed Explanation
The `UnionFind` class maintains two arrays: `parent` and `size`. The `parent` array keeps track of the parent of each element, while the `size` array maintains the size of each tree. The `Find` method uses path compression to flatten the tree structure, ensuring that future queries are faster. The `Union` method attaches the smaller tree under the root of the larger tree, which keeps the overall tree flat and optimizes future operations.

# Complexity Analysis
- **Find Operation:** Average time complexity is O(α(n)), where α is the Inverse Ackermann function, which grows very slowly. 
- **Union Operation:** Average time complexity is also O(α(n)). 
- **Space Complexity:** O(n) for the `parent` and `size` arrays.