# Interval Tree

### 1. Introduction
An Interval Tree is an augmented binary search tree (BST) designed for storing and querying intervals. It allows efficient searching for intervals that overlap with a given query interval. This data structure is commonly used in scheduling algorithms, geometric databases, and windowing queries in computer graphics.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        IntervalTree<int> tree = new IntervalTree<int>();

        // Insert intervals
        tree.Insert(new Interval<int>(15, 20));
        tree.Insert(new Interval<int>(10, 30));
        tree.Insert(new Interval<int>(17, 19));
        tree.Insert(new Interval<int>(5, 20));
        tree.Insert(new Interval<int>(12, 15));
        tree.Insert(new Interval<int>(30, 40));

        // Search for any overlap
        Interval<int> query = new Interval<int>(6, 7);
        Interval<int>? match = tree.SearchAny(query);
        if (match.HasValue)
        {
            Console.WriteLine($"Found overlap: {match.Value}");
        }

        // Search for all overlaps
        var allMatches = tree.SearchAll(new Interval<int>(14, 16));
        foreach (var interval in allMatches)
        {
            Console.WriteLine($"Overlapping interval: {interval}");
        }
    }
}
```

### 3. Detailed Explanation
- **Interval Representation**: The `Interval<T>` struct represents a closed interval `[low, high]` where `low <= high`.
- **BST Ordering**: The tree is ordered by the `Low` value of each interval. 
- **Augmentation**: Each node stores a `Max` value, which represents the maximum `High` value among all intervals in the subtree rooted at that node. This value is updated during insertion.
- **Search Pruning**:
  - When searching for overlaps, if the left child's `Max` value is less than the query's `Low` value, we can safely skip searching the left subtree because no interval in it can overlap with the query.
  - When searching the right subtree, if the current node's `Low` value is greater than the query's `High` value, we can skip the right subtree because all nodes in it will have `Low` values greater than the query's `High` value.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Insertion**: $O(\log N)$ average, $O(N)$ worst-case (unbalanced tree).
  - **SearchAny**: $O(\log N)$ average, $O(N)$ worst-case.
  - **SearchAll**: $O(\min(N, K \log N))$ where $K$ is the number of overlapping intervals found.
- **Space Complexity**: $O(N)$ to store $N$ intervals in the tree.