# Double-Ended Queue (Deque)

### 1. Introduction
A Deque (Double-Ended Queue) is a sequence container that allows insertion and deletion of elements at both the front and the rear ends. It combines the capabilities of a Stack (LIFO) and a Queue (FIFO). Use a Deque when you need efficient O(1) insertions and deletions at both ends, such as in sliding window algorithms, undo/redo buffers, or job-stealing scheduling algorithms.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        Deque<int> deque = new Deque<int>();

        // Add elements to both ends
        deque.AddFront(10);
        deque.AddRear(20);
        deque.AddFront(5);
        deque.AddRear(30);

        // Peek elements
        Console.WriteLine($"Front: {deque.PeekFront()}"); // Output: 5
        Console.WriteLine($"Rear: {deque.PeekRear()}");   // Output: 30

        // Remove elements
        Console.WriteLine($"Removed Front: {deque.RemoveFront()}"); // Output: 5
        Console.WriteLine($"Removed Rear: {deque.RemoveRear()}");   // Output: 30

        Console.WriteLine($"Count: {deque.Count}"); // Output: 2
    }
}
```

### 3. Detailed Explanation
This implementation uses a dynamically-resizing circular array to achieve O(1) performance for all operations.
- **Circular Indexing**: The `_head` and `_tail` pointers wrap around the array boundaries using modulo arithmetic (`% _array.Length`). This avoids shifting elements when items are added or removed.
- **Pointers**: `_head` points to the index of the first element. `_tail` points to the index where the next rear element will be inserted.
- **Resizing**: When the array is full (`_count == _array.Length`), the capacity is doubled. The elements are copied to the new array in their logical order starting from index 0, resetting `_head` to 0 and `_tail` to `_count`.
- **Memory Safety**: Removed elements are overwritten with `default(T)` to prevent memory leaks (loitering) of reference types.

### 4. Complexity Analysis
- **Time Complexity**:
  - `AddFront` / `AddRear`: $O(1)$ amortized. Resizing takes $O(N)$ time but occurs infrequently.
  - `RemoveFront` / `RemoveRear`: $O(1)$ constant time.
  - `PeekFront` / `PeekRear`: $O(1)$ constant time.
- **Space Complexity**: $O(N)$ where $N$ is the current capacity of the internal array.