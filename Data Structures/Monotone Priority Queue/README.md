# Introduction
The Monotone Priority Queue is a type of queue data structure that maintains a set of elements, each with a priority, and ensures that the queue remains monotone, meaning that the priority of the elements either always increases or always decreases. This data structure is useful in scenarios where elements need to be processed in a specific order based on their priority.

# Usage
```csharp
MonotonePriorityQueue<int> queue = new MonotonePriorityQueue<int>(true);
queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);
Console.WriteLine(queue.Dequeue()); // prints 1
Console.WriteLine(queue.Peek()); // prints 2
```

# Detailed Explanation
The implementation of the Monotone Priority Queue uses a list to store the elements. The `Enqueue` method checks if the new element maintains the monotone property before adding it to the queue. The `Dequeue` method removes and returns the front element of the queue, while the `Peek` method returns the front element without removing it. The `IsEmpty` method checks if the queue is empty.

# Complexity Analysis
* Time complexity of `Enqueue`: O(1) amortized, O(n) in the worst case when the queue needs to be resized
* Time complexity of `Dequeue`: O(n) in the worst case when the queue needs to be shifted
* Time complexity of `Peek`: O(1)
* Time complexity of `IsEmpty`: O(1)
* Space complexity: O(n) where n is the number of elements in the queue