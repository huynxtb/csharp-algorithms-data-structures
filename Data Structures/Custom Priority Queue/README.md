# Custom Priority Queue

## Introduction

A **Priority Queue** is an abstract data structure where each element has an associated priority. Unlike a standard queue that follows FIFO (First-In-First-Out), a priority queue serves elements based on their priority. The element with the highest (or lowest) priority is always at the front and is dequeued first.

This implementation uses a **binary heap** (max-heap or min-heap) to efficiently manage priorities. It's ideal for scenarios where you need to:
- Process tasks based on priority (task scheduling)
- Implement Dijkstra's shortest path algorithm
- Manage event-driven simulations
- Handle resource allocation with priority levels

## Usage

Here's a clear example demonstrating how to use the Custom Priority Queue:

```csharp
using System;

public class Program
{
    public static void Main()
    {
        // Create a max-heap priority queue (highest priority first)
        CustomPriorityQueue<string> maxQueue = new CustomPriorityQueue<string>(isMaxHeap: true);
        
        // Enqueue elements with priorities
        maxQueue.Enqueue("Low priority task", 1);
        maxQueue.Enqueue("High priority task", 10);
        maxQueue.Enqueue("Medium priority task", 5);
        
        // Check the count
        Console.WriteLine($"Queue count: {maxQueue.Count}");
        
        // Peek at the highest priority element
        Console.WriteLine($"Next task: {maxQueue.Peek()}");
        
        // Dequeue elements in priority order
        while (!maxQueue.IsEmpty)
        {
            string task = maxQueue.Dequeue();
            Console.WriteLine($"Processing: {task}");
        }
        
        // Create a min-heap priority queue (lowest priority first)
        CustomPriorityQueue<int> minQueue = new CustomPriorityQueue<int>(isMaxHeap: false);
        
        minQueue.Enqueue(100, 5);
        minQueue.Enqueue(200, 2);
        minQueue.Enqueue(300, 8);
        
        Console.WriteLine($"Smallest priority value: {minQueue.Dequeue()}");
    }
}
```

## Detailed Explanation

The Custom Priority Queue is implemented using a **binary heap** stored in a dynamic array (List). The heap can be configured as either a max-heap (highest priority first) or min-heap (lowest priority first).

### Key Components:

1. **HeapNode Class**: An internal class that encapsulates both the value and its priority.

2. **Heap Storage**: A `List<HeapNode>` that stores elements in heap order, where:
   - Parent at index `i`
   - Left child at index `2*i + 1`
   - Right child at index `2*i + 2`

3. **Enqueue Operation**:
   - Add the new element at the end of the list
   - Restore heap property by "bubbling up" (HeapifyUp)
   - Compare with parent and swap if necessary

4. **Dequeue Operation**:
   - Remove the root element (highest/lowest priority)
   - Move the last element to the root
   - Restore heap property by "bubbling down" (HeapifyDown)
   - Compare with children and swap with the appropriate child

5. **HeapifyUp**: Moves an element up the tree until the heap property is satisfied.

6. **HeapifyDown**: Moves an element down the tree, swapping with the highest/lowest priority child until the heap property is satisfied.

7. **ShouldSwap**: Determines whether two elements should be swapped based on whether it's a max-heap or min-heap.

## Complexity Analysis

### Time Complexity:

- **Enqueue**: O(log n)
  - Adding an element and heapifying up takes logarithmic time relative to the number of elements

- **Dequeue**: O(log n)
  - Removing the root and heapifying down takes logarithmic time

- **Peek**: O(1)
  - Accessing the root element is constant time

- **IsEmpty**: O(1)
  - Checking if the list is empty is constant time

- **Count**: O(1)
  - Getting the count of elements is constant time

### Space Complexity:

- **Overall**: O(n)
  - The dynamic array stores n elements, where n is the number of items in the queue
  - No additional space is required beyond the storage for the elements

The binary heap implementation provides an excellent balance between insertion and removal efficiency, making it suitable for real-time applications where priority-based processing is critical.