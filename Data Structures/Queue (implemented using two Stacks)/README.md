# Queue (implemented using two Stacks)

## Introduction
This is a Queue data structure implemented using two Stack data structures. A queue follows the First-In, First-Out (FIFO) principle, while a stack follows the Last-In, First-Out (LIFO) principle. This implementation demonstrates how to use two stacks to achieve the behavior of a queue.

## Usage
```csharp
Queue<int> queue = new Queue<int>();
queue.Enqueue(1);
queue.Enqueue(2);
int first = queue.Dequeue(); // returns 1
int front = queue.Peek(); // returns 2
int count = queue.Count; // returns 1
bool isEmpty = queue.IsEmpty; // returns false
```

## Detailed Explanation
The `Queue<T>` class uses two stacks (`stack1` and `stack2`) for its internal storage. The `Enqueue` method simply pushes items onto `stack1`. For `Dequeue` and `Peek`, if `stack2` is empty, all items from `stack1` are popped and pushed onto `stack2`, reversing their order. This allows the oldest item to be accessed first. The `Count` property returns the total number of items in both stacks, while `IsEmpty` checks if the queue has any items.

## Complexity Analysis
- **Enqueue:** O(1) - Adding an item to `stack1` is constant time.
- **Dequeue:** Amortized O(1) - Moving items from `stack1` to `stack2` takes time proportional to the number of items, but this is spread out over multiple operations.
- **Peek:** Amortized O(1) - Similar reasoning as `Dequeue` applies here.
- **Space Complexity:** O(n) - The space used is proportional to the number of elements in the queue.