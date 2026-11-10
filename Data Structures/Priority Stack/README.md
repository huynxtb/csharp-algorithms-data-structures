# Priority Stack

## Introduction
The Priority Stack is a data structure that combines the properties of a stack and a priority queue. It allows elements to be pushed onto the stack with a specified priority and ensures that elements are popped from the stack in order of priority (highest priority first). If multiple elements have the same priority, they are popped in the order they were added (FIFO for same priority). This structure is useful in scenarios where you need to manage tasks or items based on their urgency or importance while maintaining the order of insertion for items with the same priority.

## Usage
```csharp
PriorityStack<string> priorityStack = new PriorityStack<string>();
priorityStack.Push("Task 1", 1);
priorityStack.Push("Task 2", 3);
priorityStack.Push("Task 3", 2);
string highestPriorityTask = priorityStack.Pop(); // Returns "Task 2"
bool isEmpty = priorityStack.IsEmpty(); // Returns false
```

## Detailed Explanation
The `PriorityStack` class maintains a list of `StackItem` objects, each containing a value and its associated priority. The `Push` method adds a new item to the list. The `Pop` method searches for the item with the highest priority, removes it from the list, and returns its value. If the stack is empty, it throws an exception. The `IsEmpty` method checks if there are any items in the stack.

## Complexity Analysis
- **Push Operation:** O(1) - Adding an item to the end of the list is constant time.
- **Pop Operation:** O(n) - Finding and removing the item with the highest priority requires scanning the entire list.
- **IsEmpty Operation:** O(1) - Checking if the list is empty is constant time.