# Segment Tree for Range Maximum Query

## Introduction
The Segment Tree is a data structure that allows for efficient range queries and updates on an array. It is particularly useful for scenarios where you need to perform multiple queries on an array and update elements frequently. This implementation focuses on range maximum queries, allowing you to find the maximum value in a specified range and update values in the original array efficiently.

## Usage
```csharp
int[] input = { 1, 3, 5, 7, 9, 11 };
SegmentTree segmentTree = new SegmentTree(input);

// Perform a range maximum query from index 1 to 3
int maxInRange = segmentTree.RangeMaxQuery(1, 3);

// Update the value at index 1 to 10
segmentTree.Update(1, 10);

// Perform the range maximum query again
maxInRange = segmentTree.RangeMaxQuery(1, 3);
```

## Detailed Explanation
The Segment Tree is built from the input array by recursively dividing the array into halves and storing the maximum values in a binary tree structure. Each node in the tree represents the maximum value of a segment of the array. The `Build` method constructs the tree, while the `RangeMaxQuery` method retrieves the maximum value in a specified range by traversing the tree. The `Update` method allows for modifying an element in the original array and updating the corresponding nodes in the tree to reflect this change.

## Complexity Analysis
- **Build:** O(n) - The tree is built in linear time.
- **Range Maximum Query:** O(log n) - Each query traverses the height of the tree.
- **Update:** O(log n) - Each update modifies the tree along the path from the leaf to the root.