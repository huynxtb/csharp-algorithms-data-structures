# Interval Heap

## Introduction
The Interval Heap is a specialized data structure that efficiently manages a collection of intervals, allowing for quick insertion, retrieval, and removal of the minimum interval based on the starting point. It is particularly useful in scenarios where intervals need to be processed in order of their starting times, such as in scheduling problems or interval merging tasks.

## Usage
```csharp
var intervalHeap = new IntervalHeap();
intervalHeap.Insert((1, 5));
intervalHeap.Insert((2, 6));
intervalHeap.Insert((0, 3));
var minInterval = intervalHeap.RemoveMin(); // Returns (0, 3)
bool isEmpty = intervalHeap.IsEmpty(); // Returns false
int size = intervalHeap.Size(); // Returns 2
```

## Detailed Explanation
The `IntervalHeap` class maintains a list of intervals and provides methods to insert new intervals, remove the minimum interval, check if the heap is empty, and get the size of the heap. The heap is structured as a binary heap, ensuring that the minimum interval can be accessed and removed in logarithmic time. The `Insert` method adds a new interval to the end of the list and then restores the heap property by moving the new interval up the tree. The `RemoveMin` method retrieves the minimum interval (the root of the heap), replaces it with the last interval, and then restores the heap property by moving the new root down the tree.

## Complexity Analysis
- **Insert:** O(log n) - The time complexity for inserting an interval is logarithmic due to the need to maintain the heap property.
- **RemoveMin:** O(log n) - Removing the minimum interval also requires logarithmic time to restore the heap property.
- **IsEmpty:** O(1) - Checking if the heap is empty is a constant time operation.
- **Size:** O(1) - Getting the size of the heap is also a constant time operation.