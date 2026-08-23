# ArrayStack<T>

### 1. Introduction
An `ArrayStack<T>` is a Last-In, First-Out (LIFO) data structure implemented using a dynamically resizing array. It is ideal for scenarios requiring fast access to the most recently added elements, such as undo mechanisms, expression evaluation, and depth-first search algorithms.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        ArrayStack<int> stack = new ArrayStack<int>();

        stack.Push(10);
        stack.Push(20);
        stack.Push(30);

        Console.WriteLine($"Top element: {stack.Peek()}"); // Output: 30
        Console.WriteLine($"Popped: {stack.Pop()}");       // Output: 30
        Console.WriteLine($"Count: {stack.Count}");         // Output: 2
    }
}
```

### 3. Detailed Explanation
- **Internal Storage**: Elements are stored in a contiguous private array `_items`. A pointer `_size` tracks the current number of elements and points to the next insertion index.
- **Dynamic Resizing**: When `Push` is called and `_size` equals the array length, the internal array capacity doubles. This minimizes the frequency of allocation operations.
- **Memory Management**: During `Pop`, the reference to the removed element is set to `default` to allow the garbage collector to reclaim memory for reference types.
- **Safety**: Both `Pop` and `Peek` throw an `InvalidOperationException` if invoked on an empty stack.

### 4. Complexity Analysis
- **Time Complexity**:
  - `Push`: $O(1)$ amortized. $O(N)$ when resizing occurs.
  - `Pop`: $O(1)$.
  - `Peek`: $O(1)$.
  - `Count` / `IsEmpty`: $O(1)$.
- **Space Complexity**: $O(N)$ where $N$ is the capacity of the internal array.