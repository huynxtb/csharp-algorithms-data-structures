# Floyd's Cycle-Finding Algorithm (Tortoise and Hare)

## Introduction
Floyd's Cycle-Finding Algorithm, also known as the Tortoise and Hare algorithm, is an efficient method for detecting cycles in a sequence, particularly in singly linked lists. It uses two pointers that traverse the list at different speeds to determine if a cycle exists and to find the starting point and length of the cycle if it does.

## Usage
Here is a simple example of how to use the Floyd's Cycle-Finding Algorithm:
```csharp
Node<int> head = new Node<int>(1);
Node<int> second = new Node<int>(2);
Node<int> third = new Node<int>(3);
Node<int> fourth = new Node<int>(4);
head.Next = second;
second.Next = third;
third.Next = fourth;
fourth.Next = second; // Creates a cycle

bool hasCycle = FloydsCycleFinding.HasCycle(head);
Node<int> cycleStart = FloydsCycleFinding.FindCycleStart(head);
int cycleLength = FloydsCycleFinding.FindCycleLength(head);
```

## Detailed Explanation
The algorithm works by initializing two pointers, the 'tortoise' and the 'hare'. The tortoise moves one step at a time while the hare moves two steps. If there is a cycle, these two pointers will eventually meet. Once a meeting point is found, the starting point of the cycle can be determined by resetting one pointer to the head and moving both pointers one step at a time until they meet again. The length of the cycle can be calculated by moving one pointer around the cycle until it meets the other pointer again.

## Complexity Analysis
- **Time Complexity:** O(N), where N is the number of nodes in the linked list. The algorithm traverses the list at most twice.
- **Space Complexity:** O(1), as it only uses a fixed number of pointers regardless of the input size.