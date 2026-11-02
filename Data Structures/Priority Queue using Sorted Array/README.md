# Priority Queue using Sorted Array

## Introduction

The `PriorityQueueSortedArray<T>` is a data structure that manages a collection of elements, where each element has a priority. It allows for efficient retrieval and removal of the element with the highest priority. This implementation uses a sorted array to store the elements, ensuring that the highest-priority element is always at the end of the array for O(1) access. Elements are kept in ascending order, so the maximum element is at the last index.

This data structure is suitable when you need to frequently access the highest-priority item and insertions, while less frequent, can tolerate a linear time complexity. It's a good choice for scenarios where the number of elements is relatively small or when the cost of maintaining a more complex heap structure is not justified.

## Usage

```csharp
using System;

public class Example
{
    public static void Main(string[] args)
    {
        // Create a priority queue for integers
        var pq = new PriorityQueueSortedArray<int>();

        // Insert elements
        pq.Insert(10);
        pq.Insert(30);
        pq.Insert(20);
        pq.Insert(5);

        Console.WriteLine($"Is empty: {pq.IsEmpty()}"); // Output: Is empty: False
        Console.WriteLine($"Count: {pq.Count}");       // Output: Count: 4

        // Peek at the highest priority element
        Console.WriteLine($"Peek Max: {pq.PeekMax()}"); // Output: Peek Max: 30

        // Remove elements in priority order
        Console.WriteLine($"Removed Max: {pq.RemoveMax()}"); // Output: Removed Max: 30
        Console.WriteLine($"Removed Max: {pq.RemoveMax()}"); // Output: Removed Max: 20
        Console.WriteLine($"Removed Max: {pq.RemoveMax()}"); // Output: Removed Max: 10
        Console.WriteLine($"Removed Max: {pq.RemoveMax()}"); // Output: Removed Max: 5

        Console.WriteLine($"Is empty: {pq.IsEmpty()}"); // Output: Is empty: True

        // Example with custom objects (must implement IComparable)
        var pqStrings = new PriorityQueueSortedArray<string>();
        pqStrings.Insert("apple");
        pqStrings.Insert("banana");
        pqStrings.Insert("cherry");
        Console.WriteLine($"Max string: {pqStrings.PeekMax()}"); // Output: Max string: cherry
    }
}
```

## Detailed Explanation

The `PriorityQueueSortedArray<T>` class maintains its elements in an internal array, `_elements`, which is always kept sorted in ascending order. The `_count` variable tracks the number of elements currently in the queue.

*   **Constructor:** Initializes the internal array with a default capacity or a user-specified capacity. It sets the `_count` to zero.
*   **`IsEmpty()`:** Returns `true` if `_count` is 0, indicating an empty queue, and `false` otherwise.
*   **`Insert(T element)`:**
    1.  Checks for null elements and throws an `ArgumentNullException` if found.
    2.  Calls `EnsureCapacity()` to make sure there's enough space in the array. If the array is full, it doubles its size.
    3.  It then iterates through the existing elements to find the correct insertion point (`insertIndex`) where the new element should be placed to maintain the sorted order (ascending).
    4.  `ShiftElements()` is called to move all elements from `insertIndex` onwards one position to the right, creating space for the new element.
    5.  The new `element` is placed at `insertIndex`.
    6.  `_count` is incremented.
*   **`RemoveMax()`:**
    1.  Checks if the queue is empty and throws an `InvalidOperationException` if it is.
    2.  Since the array is sorted in ascending order, the maximum element is always the last element (`_elements[_count - 1]`).
    3.  This element is stored in a temporary variable `maxElement`.
    4.  The last position in the array is set to `default(T)` to clear the reference (for garbage collection).
    5.  `_count` is decremented.
    6.  The `maxElement` is returned.
*   **`PeekMax()`:**
    1.  Checks if the queue is empty and throws an `InvalidOperationException` if it is.
    2.  Returns the last element (`_elements[_count - 1]`) without removing it.
*   **`EnsureCapacity()`:** If the number of elements (`_count`) equals the current array length, it calls `Resize()` to double the array's capacity.
*   **`Resize(int newCapacity)`:** Creates a new array with the specified `newCapacity`, copies the existing elements to the new array, and updates `_elements` to refer to the new, larger array.
*   **`ShiftElements(int startIndex, int shiftAmount)`:** This helper method shifts elements in the array starting from `startIndex` by `shiftAmount` positions to the right. This is crucial for making space during insertion while maintaining the sorted order.

The `T : IComparable<T>` constraint ensures that the elements stored in the priority queue can be compared with each other, which is necessary for maintaining the sorted order.