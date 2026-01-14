# Heapify Algorithm for Max-Heap Construction

## Introduction

The Heapify algorithm transforms an arbitrary array of integers into a max-heap in-place. A max-heap is a complete binary tree where every parent node's value is greater than or equal to the values of its children. This property is fundamental for priority queue implementations and is a crucial first step in the Heapsort algorithm (though sorting is not covered here).

This implementation uses a zero-based indexed array to represent the heap and efficiently converts any unsorted array into a max-heap with an overall time complexity of O(n).

## Usage

Usage example:

int[] numbers = { 3, 9, 2, 1, 4, 5 };
MaxHeap.Heapify(numbers);
// After this call, 'numbers' is rearranged to satisfy max-heap property

Since the code only provides static methods inside a single class `MaxHeap`, you can directly call `Heapify` with any integer array to convert it into a max-heap.

## Detailed Explanation

- **Heap Representation:** The heap is stored as a zero-based indexed array. For a node at index `i`, its left child is at `2*i + 1` and right child at `2*i + 2`.

- **Heapify Process:**
  The process starts from the last internal node (non-leaf) which is at index `n/2 - 1` and goes backwards to the root (index 0).

- **SiftDown Operation:**
  For each node, if it violates the max-heap property (i.e., smaller than one or both of its children), we swap it with the largest child and recursively apply `SiftDown` on the child index. This ensures the subtree rooted at that node satisfies max-heap property.

- **Efficiency:**
  By starting from the bottom of the tree, we ensure that larger subtrees are heapified only once their smaller subtrees are already heapified, leading to linear time complexity.

## Complexity Analysis

- **Time Complexity:** O(n)
  - The heapify algorithm is more efficient than a naive approach (that inserts nodes one by one) which is O(n log n).
  - The sift-down operation is run on about half the nodes, but the amount of work decreases exponentially as you move down the tree, resulting in an overall linear time complexity.

- **Space Complexity:** O(1)
  - The algorithm works in-place, requiring no additional memory proportional to input size.

This makes the heapify method suitable for efficiently building max-heaps in large datasets or when performance and memory usage are critical.