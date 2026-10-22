# Cyclic Buffer

## Introduction
The Cyclic Buffer is a fixed-size data structure that efficiently manages a collection of elements in a circular manner. It is particularly useful in scenarios where data needs to be processed in a first-in-first-out (FIFO) order, and older data can be overwritten when the buffer reaches its capacity. This structure is ideal for applications such as streaming data, buffering, and managing resources in real-time systems.

## Usage
Here is an example of how to use the Cyclic Buffer:
```csharp
CyclicBuffer<int> buffer = new CyclicBuffer<int>(5);
buffer.Add(1);
buffer.Add(2);
buffer.Add(3);
buffer.Add(4);
buffer.Add(5);
Console.WriteLine(buffer.IsFull); // True
buffer.Add(6); // Overwrites the oldest data (1)
Console.WriteLine(buffer.Get(0)); // Outputs 2
Console.WriteLine(buffer.Count); // Outputs 5
```

## Detailed Explanation
The `CyclicBuffer` class maintains an internal array to store the elements. It uses two pointers, `head` and `tail`, to track the positions for adding and retrieving elements. When an item is added, it is placed at the `tail` position, and the `tail` pointer is incremented in a circular manner using modulo arithmetic. If the buffer is full, the `head` pointer is also incremented to overwrite the oldest data. The `Count` property keeps track of the number of elements currently in the buffer, while the `IsFull` property checks if the buffer has reached its capacity. The `Get` method allows retrieval of elements based on their index relative to the `head` position.

## Complexity Analysis
- **Time Complexity:**
  - `Add(T item)`: O(1) - Adding an item is done in constant time.
  - `Get(int index)`: O(1) - Retrieving an item by index is also done in constant time.
- **Space Complexity:** O(n) - The space complexity is linear with respect to the buffer size, where n is the capacity of the buffer.