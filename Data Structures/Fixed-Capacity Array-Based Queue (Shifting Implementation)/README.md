# Fixed-Capacity Array-Based Queue (Shifting Implementation)

## Introduction

The `FixedCapacityArrayQueue<T>` is a generic implementation of a Queue data structure that uses a fixed-size array for storage. This specific implementation demonstrates a basic queue behavior where elements are added to the rear and removed from the front. A key characteristic of this implementation is its `Dequeue` operation, which explicitly shifts all remaining elements one position forward in the array to maintain a contiguous block of data at the beginning of the array. This approach is simpler to understand than a circular buffer but can be less efficient for frequent dequeues.

**When to use it:**
*   When you need a queue with a predefined, unchangeable maximum size.
*   For educational purposes to understand fundamental queue operations and array manipulation.
*   In scenarios where the overhead of shifting elements is acceptable, or dequeue operations are infrequent compared to enqueue operations.

## Usage

```csharp
// Example usage of FixedCapacityArrayQueue<T>

// Create a queue with a capacity of 5
FixedCapacityArrayQueue<int> intQueue = new FixedCapacityArrayQueue<int>(5);

// Enqueue elements
intQueue.Enqueue(10);
intQueue.Enqueue(20);
intQueue.Enqueue(30);

Console.WriteLine($"Current count: {intQueue.Count}"); // Output: Current count: 3
Console.WriteLine($"Is empty: {intQueue.IsEmpty()}");   // Output: Is empty: False
Console.WriteLine($"Is full: {intQueue.IsFull()}");     // Output: Is full: False

// Peek at the front element
Console.WriteLine($"Front element: {intQueue.Peek()}"); // Output: Front element: 10

// Dequeue elements
int first = intQueue.Dequeue();
Console.WriteLine($"Dequeued: {first}");             // Output: Dequeued: 10
Console.WriteLine($"New front element: {intQueue.Peek()}"); // Output: New front element: 20
Console.WriteLine($"Current count: {intQueue.Count}"); // Output: Current count: 2

intQueue.Enqueue(40);
intQueue.Enqueue(50);
intQueue.Enqueue(60);

Console.WriteLine($"Is full: {intQueue.IsFull()}");     // Output: Is full: True

// Attempt to enqueue into a full queue will throw an exception
try
{
    intQueue.Enqueue(70);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Error: {ex.Message}"); // Output: Error: Queue is full.
}

// Dequeue all elements
while (!intQueue.IsEmpty())
{
    Console.WriteLine($"Dequeued: {intQueue.Dequeue()}");
}

Console.WriteLine($"Is empty: {intQueue.IsEmpty()}");   // Output: Is empty: True

// Attempt to dequeue from an empty queue will throw an exception
try
{
    intQueue.Dequeue();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Error: {ex.Message}"); // Output: Error: Queue is empty.
}
```

## Detailed Explanation

The `FixedCapacityArrayQueue<T>` class is designed to manage a collection of elements of type `T` with a fixed maximum capacity. It uses a private array `_items` of type `T[]` to store these elements.

*   **Constructor (`FixedCapacityArrayQueue(int capacity)`):** Initializes the queue. It takes an integer `capacity` to set the maximum number of elements the queue can hold. This capacity is stored in the `_capacity` field and is immutable after creation. An `ArgumentOutOfRangeException` is thrown if the provided capacity is not positive.

*   **Internal State (`_items`, `_count`):**
    *   `_items`: The underlying array that stores the queue elements. Its size is fixed to `_capacity` upon construction.
    *   `_count`: An integer representing the current number of elements present in the queue. The elements are always stored contiguously from index `0` up to `_count - 1`.

*   **Properties (`Count`, `Capacity`):**
    *   `Count`: A read-only property that returns the current number of elements (`_count`).
    *   `Capacity`: A read-only property that returns the maximum capacity of the queue (`_capacity`).

*   **Methods:**
    *   **`IsEmpty()`:** Returns `true` if `_count` is 0, indicating the queue has no elements.
    *   **`IsFull()`:** Returns `true` if `_count` is equal to `_capacity`, indicating the queue has reached its maximum capacity.
    *   **`Enqueue(T item)`:** Adds an `item` to the rear of the queue. It first checks if the queue is full using `IsFull()`. If it is, an `InvalidOperationException` is thrown. Otherwise, the `item` is placed at the index indicated by `_count`, and `_count` is incremented.
    *   **`Dequeue()`:** Removes and returns the element from the front of the queue. It first checks if the queue is empty using `IsEmpty()`. If it is, an `InvalidOperationException` is thrown. If not empty, the element at index `0` is stored in `dequeuedItem`. Then, a loop iterates from index `0` up to `_count - 2`, shifting each element one position to the left (`_items[i] = _items[i + 1]`). This effectively overwrites the element at index `0` and moves all subsequent elements forward. Finally, the last position that was occupied by an element (`_count - 1`) is set to its default value (`default(T)`), and `_count` is decremented before `dequeuedItem` is returned.
    *   **`Peek()`:** Returns the element at the front of the queue (index `0`) without removing it. It checks for emptiness and throws an `InvalidOperationException` if the queue is empty. Otherwise, it returns `_items[0]`.

## Complexity Analysis

*   **`Enqueue(T item)`:**
    *   **Time Complexity:** O(1). Adding an element involves a constant number of operations (checking capacity, array assignment, incrementing count).
    *   **Space Complexity:** O(1) (for the operation itself, not considering the underlying array).

*   **`Dequeue()`:**
    *   **Time Complexity:** O(N), where N is the number of elements currently in the queue (`_count`). This is because, after removing the front element, all remaining N-1 elements must be shifted one position forward in the array.
    *   **Space Complexity:** O(1) (for the operation itself).

*   **`Peek()`:**
    *   **Time Complexity:** O(1). Accessing the first element of an array is a constant-time operation.
    *   **Space Complexity:** O(1).

*   **`IsEmpty()`:**
    *   **Time Complexity:** O(1).
    *   **Space Complexity:** O(1).

*   **`IsFull()`:**
    *   **Time Complexity:** O(1).
    *   **Space Complexity:** O(1).

*   **`Count` Property:**
    *   **Time Complexity:** O(1).

*   **`Capacity` Property:**
    *   **Time Complexity:** O(1).

*   **Overall Space Complexity:** O(C), where C is the fixed capacity of the queue, due to the storage required for the internal array `_items`.
