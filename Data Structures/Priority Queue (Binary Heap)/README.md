# Priority Queue using a Binary Heap

## 1. Introduction
A Priority Queue is a specialized data structure where elements are associated with a priority. In a min-heap implementation, the element with the lowest priority value is served first. This structure is ideal for algorithms like Dijkstra's shortest path, Prim's minimum spanning tree, and task scheduling systems.

## 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        PriorityQueue<string, int> pq = new PriorityQueue<string, int>();

        pq.Enqueue("Low priority task", 3);
        pq.Enqueue("High priority task", 1);
        pq.Enqueue("Medium priority task", 2);

        while (!pq.IsEmpty())
        {
            string task = pq.Dequeue();
            Console.WriteLine($"Processing: {task}");
        }
    }
}
```

## 3. Detailed Explanation
This implementation uses a binary min-heap stored in a dynamic array (`List<T>`). 
- **Enqueue**: Appends the element to the end of the list and performs a `HeapifyUp` operation to restore the heap property by swapping the element with its parent until the parent's priority is lower than or equal to the element's priority.
- **Dequeue**: Replaces the root element (index 0) with the last element in the list, removes the last element, and performs a `HeapifyDown` operation. This restores the heap property by swapping the new root with its smallest child recursively.
- **Peek**: Returns the root element at index 0 in $O(1)$ time.

## 4. Complexity Analysis
- **Enqueue**: $O(\log n)$ time complexity to bubble up the element.
- **Dequeue**: $O(\log n)$ time complexity to bubble down the new root.
- **Peek**: $O(1)$ time complexity to access the root.
- **IsEmpty / Count**: $O(1)$ time complexity.
- **Space Complexity**: $O(n)$ auxiliary space to store the elements in the heap.