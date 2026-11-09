# Introduction
The Heap is a specialized tree-based data structure that satisfies the heap property. In a min-heap, for any given node, the value of the node is less than or equal to the values of its children, while in a max-heap, the value of the node is greater than or equal to the values of its children. Heaps are commonly used to implement priority queues and for efficient sorting algorithms like heap sort.

# Usage
```csharp
var minHeap = new Heap<int>();
minHeap.Add(10);
minHeap.Add(5);
minHeap.Add(20);
int min = minHeap.Peek(); // Returns 5
int extractedMin = minHeap.Extract(); // Removes and returns 5
int count = minHeap.Count; // Returns 2
bool isEmpty = minHeap.IsEmpty; // Returns false
```

# Detailed Explanation
The `Heap<T>` class is a generic implementation of a heap data structure. It uses a list to store elements and maintains the heap property using two main operations: `HeapifyUp` and `HeapifyDown`. The `Add` method inserts a new element and ensures the heap property is maintained by moving the element up the tree if necessary. The `Extract` method removes the root element and restores the heap property by moving the new root down the tree. The class accepts a custom comparer to allow for both min-heap and max-heap configurations.

# Complexity Analysis
- **Add**: O(log n) - The height of the heap is logarithmic in relation to the number of elements, and we may need to traverse this height to maintain the heap property.
- **Extract**: O(log n) - Similar to Add, we may need to traverse the height of the heap to restore the heap property after removing the root.
- **Peek**: O(1) - Accessing the root element is a constant time operation.
- **Space Complexity**: O(n) - The space used is proportional to the number of elements in the heap.