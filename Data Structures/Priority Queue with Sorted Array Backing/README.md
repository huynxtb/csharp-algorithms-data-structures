# Priority Queue with Sorted Array Backing

## Introduction

A **Priority Queue with Sorted Array Backing** is a data structure that maintains elements in sorted order within a dynamic array. The highest priority element (the largest value) can be accessed and removed efficiently from the end of the array in O(1) time. New elements are inserted into their correct sorted position using binary search to find the insertion point, followed by shifting elements to maintain order.

This data structure is ideal when:
- You need frequent access to the highest (or lowest) priority element.
- The number of dequeue/peek operations significantly exceeds enqueue operations.
- You want a simple, array-based implementation without the complexity of a heap.

## Usage

```csharp
// Create a new priority queue
PriorityQueueSortedArray pq = new PriorityQueueSortedArray();

// Enqueue elements
pq.Enqueue(10);
pq.Enqueue(30);
pq.Enqueue(20);
pq.Enqueue(5);

// Check if the queue is empty
bool empty = pq.IsEmpty(); // false

// Peek at the highest priority element without removing it
int top = pq.Peek(); // 30

// Dequeue the highest priority element
int highest = pq.Dequeue(); // 30
int nextHighest = pq.Dequeue(); // 20

// Check the count
int count = pq.Count; // 2

// Convert to array for inspection
int[] elements = pq.ToArray(); // [5, 10]
```

## Detailed Explanation

### Internal Storage
The priority queue uses an internal integer array (`_array`) that acts as a dynamic array. A `_count` field tracks the number of elements currently stored. The array is stored in **ascending order**, meaning the smallest element is at index 0 and the largest (highest priority) element is at index `_count - 1`.

### Enqueue Operation
When a new element is enqueued:
1. If the internal array is full, it is resized to double its current capacity.
2. A **binary search** (`FindInsertPosition`) is performed to find the correct index where the new element should be inserted to maintain sorted order.
3. All elements from the insertion index onward are shifted one position to the right.
4. The new element is placed at the insertion index.
5. The count is incremented.

### Dequeue Operation
When the highest priority element is dequeued:
1. The element at index `_count - 1` (the largest element) is retrieved.
2. The count is decremented.
3. If the count drops to one-quarter of the array's capacity, the array is resized to half its capacity to conserve memory.
4. The retrieved element is returned.

### Peek Operation
The `Peek` method simply returns the element at index `_count - 1` without modifying the queue.

### Dynamic Resizing
The array doubles in size when full (during `Enqueue`) and halves when the count drops to one-quarter of the capacity (during `Dequeue`). A minimum capacity of 4 is enforced.

## Complexity Analysis

| Operation | Time Complexity | Space Complexity |
|-----------|----------------|------------------|
| **Enqueue** | O(n) | O(1) amortized |
| **Dequeue** | O(1) | O(1) amortized |
| **Peek** | O(1) | O(1) |
| **IsEmpty** | O(1) | O(1) |
| **Overall Space** | — | O(n) |

### Notes on Complexity
- **Enqueue** is O(n) because, although the binary search to find the insertion position is O(log n), shifting elements to make room for the new element takes O(n) in the worst case.
- **Dequeue** is O(1) because the highest priority element is always at the end of the sorted array, so no shifting is needed.
- **Peek** is O(1) as it simply accesses the last element.
- The dynamic resizing uses an amortized O(1) cost per operation due to the doubling/halving strategy.
- The overall space complexity is O(n) where n is the number of elements stored.