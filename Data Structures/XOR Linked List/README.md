# XOR Linked List

## 1. Introduction
An XOR Linked List is a memory-efficient variation of a doubly linked list. Instead of storing separate memory addresses for the previous and next nodes, each node stores the bitwise XOR of the addresses of its predecessor and successor. This reduces the memory footprint of the list's pointer overhead by half while still allowing bidirectional traversal.

Use this data structure in memory-constrained environments where bidirectional traversal is required, and the garbage collector's object overhead needs to be minimized.

## 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        using (var list = new XorLinkedList<int>())
        {
            list.AddLast(10);
            list.AddLast(20);
            list.AddFirst(5);

            // Forward Traversal: 5, 10, 20
            foreach (var val in list.ForwardTraversal())
            {
                Console.WriteLine(val);
            }

            list.RemoveLast();

            // Backward Traversal: 10, 5
            foreach (var val in list.BackwardTraversal())
            {
                Console.WriteLine(val);
            }
        }
    }
}
```

## 3. Detailed Explanation
Because C# is a managed language, the Garbage Collector (GC) can move objects in memory, which invalidates raw memory addresses. To implement a true XOR Linked List, this implementation uses unmanaged memory via `Marshal.AllocHGlobal` to allocate nodes. This ensures that node addresses remain fixed in memory.

Each node contains a value and a single `IntPtr` named `Link`. The `Link` field is calculated as:
`Link = Address(Previous Node) ^ Address(Next Node)`

During traversal:
- To find the next node: `Next = Prev ^ Current.Link`
- To find the previous node: `Prev = Current.Link ^ Next`

Since the memory is allocated on the unmanaged heap, the class implements `IDisposable` and a finalizer to prevent memory leaks.

## 4. Complexity Analysis
- **Time Complexity**:
  - `AddFirst` / `AddLast`: O(1)
  - `RemoveFirst` / `RemoveLast`: O(1)
  - `Traversal` (Forward/Backward): O(n)
- **Space Complexity**: O(n) total space, but uses only one pointer per node instead of two, saving `sizeof(IntPtr)` bytes per node compared to a standard doubly linked list.