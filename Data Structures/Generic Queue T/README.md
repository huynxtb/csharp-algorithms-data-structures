# Generic Queue<T>

## Introduction
The `Queue<T>` class implements a First-In-First-Out (FIFO) queue data structure using a custom singly linked list. It is useful in scenarios where you need to process items in the order they were added, such as task scheduling or breadth-first search algorithms.

## Usage
```csharp
Queue<int> queue = new Queue<int>();
queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);
int first = queue.Dequeue(); // first will be 1
int peek = queue.Peek(); // peek will be 2
int count = queue.Count; // count will be 2
bool isEmpty = queue.IsEmpty; // isEmpty will be false
```

## Detailed Explanation
The `Queue<T>` class maintains two pointers, `head` and `tail`, to track the front and back of the queue. The internal `Node<U>` class represents each element in the queue, containing a value and a reference to the next node. The `Enqueue` method adds a new node to the end of the queue, while the `Dequeue` method removes the node from the front. The `Peek` method allows you to view the front item without removing it. The `Count` property provides the number of items in the queue, and `IsEmpty` checks if the queue is empty.

## Complexity Analysis
- **Enqueue:** O(1) - Adding an item to the tail of the queue is done in constant time.
- **Dequeue:** O(1) - Removing an item from the head of the queue is also done in constant time.
- **Peek:** O(1) - Accessing the front item is done in constant time.
- **Space Complexity:** O(n) - The space used is proportional to the number of items in the queue.