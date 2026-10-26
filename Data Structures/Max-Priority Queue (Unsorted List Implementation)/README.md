# Max-Priority Queue (Unsorted List Implementation)

## Introduction

The `MaxPriorityQueue<T>` is a data structure that stores elements of type `T`, where `T` must implement `IComparable<T>` to define its natural ordering. This implementation uses a simple, unsorted `System.Collections.Generic.List<T>` as its internal storage. It prioritizes clarity and direct representation of priority queue logic over optimal performance. In a max-priority queue, the element with the highest value (according to `IComparable<T>`) is considered the highest priority.

This implementation is suitable for scenarios where:
*   The number of elements is relatively small.
*   The frequency of `Enqueue` operations is high compared to `DequeueMax` and `PeekMax` operations, and the overhead of maintaining a sorted structure or a heap is not desired.
*   Educational purposes, to understand the fundamental operations of a priority queue without complex underlying structures.

## Usage

Here's a C# code example demonstrating how to use the `MaxPriorityQueue<T>`:

```csharp
using System;

public class Example
{
    public static void Main(string[] args)
    {
        // Create a max-priority queue for integers
        MaxPriorityQueue<int> pq = new MaxPriorityQueue<int>();

        // Enqueue elements
        pq.Enqueue(10);
        pq.Enqueue(30);
        pq.Enqueue(20);
        pq.Enqueue(5);
        pq.Enqueue(40);

        Console.WriteLine($"Priority Queue Count: {pq.Count}"); // Output: Priority Queue Count: 5

        // Peek at the highest priority element
        if (!pq.IsEmpty)
        {
            Console.WriteLine($"Highest priority element (peek): {pq.PeekMax()}"); // Output: Highest priority element (peek): 40
        }

        // Dequeue elements in priority order
        Console.WriteLine("Dequeuing elements:");
        while (!pq.IsEmpty)
        {
            Console.WriteLine(pq.DequeueMax());
        }
        // Output:
        // Dequeuing elements:
        // 40
        // 30
        // 20
        // 10
        // 5

        Console.WriteLine($"Priority Queue is empty: {pq.IsEmpty}"); // Output: Priority Queue is empty: True

        // Attempting to dequeue from an empty queue will throw an exception
        try
        {
            pq.DequeueMax();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Output: Error: Cannot dequeue from an empty priority queue.
        }
    }
}
```

## Detailed Explanation

The `MaxPriorityQueue<T>` class is designed to manage elements based on their priority, where higher values indicate higher priority. It uses a `List<T>` internally, which means elements are not kept in any particular order during insertion.

*   **`_elements`**: A private `List<T>` that stores all the elements currently in the priority queue. This list is not sorted.

*   **`Count` Property**: Returns the current number of elements in the `_elements` list. This is a direct property access to the list's `Count`.

*   **`IsEmpty` Property**: Returns `true` if the `_elements` list is empty (i.e., `Count` is 0), and `false` otherwise.

*   **`Enqueue(T item)` Method**: This method simply adds the `item` to the end of the `_elements` list. No sorting or reordering is performed, making this operation very fast.

*   **`DequeueMax()` Method**: This is the core operation for retrieving the highest priority element. 
    1.  It first checks if the queue is empty. If so, it throws an `InvalidOperationException`.
    2.  It then iterates through the entire `_elements` list to find the index of the element with the maximum value (using `CompareTo`).
    3.  Once the maximum element is found, it is stored in a temporary variable.
    4.  The element at the identified maximum index is removed from the `_elements` list using `RemoveAt`.
    5.  Finally, the stored maximum element is returned.

*   **`PeekMax()` Method**: Similar to `DequeueMax()`, this method finds the highest priority element.
    1.  It checks for an empty queue and throws an `InvalidOperationException` if necessary.
    2.  It iterates through the `_elements` list to find the index of the maximum element.
    3.  It returns the maximum element found without removing it from the list.

## Complexity Analysis

Let `n` be the number of elements in the priority queue.

*   **`Enqueue(T item)`**:
    *   **Time Complexity**: O(1). Adding an element to the end of a `List<T>` is typically a constant time operation, assuming no resizing is needed. If resizing occurs, it can be O(n) in the worst case, but amortized O(1).
    *   **Space Complexity**: O(1) (amortized, due to potential list resizing).

*   **`DequeueMax()`**:
    *   **Time Complexity**: O(n). This is because the method must iterate through the entire unsorted list to find the maximum element, and then `RemoveAt` can take O(n) time in the worst case if the element is at the beginning of the list.
    *   **Space Complexity**: O(1).

*   **`PeekMax()`**:
    *   **Time Complexity**: O(n). Similar to `DequeueMax()`, it requires a linear scan of the entire list to find the maximum element.
    *   **Space Complexity**: O(1).

*   **`Count` and `IsEmpty`**:
    *   **Time Complexity**: O(1).
    *   **Space Complexity**: O(1).
