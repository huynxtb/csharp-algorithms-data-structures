# Skip Array

## Introduction

The Skip Array is a dynamic data structure that combines the properties of sorted arrays and linked lists to achieve efficient search, insertion, and deletion operations. It utilizes multiple levels of linked lists, where each higher level acts as an "express lane" to skip over elements in the levels below. This probabilistic structure allows for faster average-case performance compared to traditional sorted arrays or linked lists, especially for large datasets.

**When to use it:**

*   When you need a dynamic collection that maintains sorted order.
*   When frequent search, insert, and delete operations are expected.
*   When average-case performance is more critical than worst-case guarantees (though worst-case is still generally good).
*   As an alternative to balanced trees (like AVL or Red-Black trees) when a simpler implementation is desired or when probabilistic guarantees are acceptable.

## Usage

```csharp
using System;

public class Example
{
    public static void Main(string[] args)
    {
        var skipArray = new SkipArray<int>();

        // Adding elements
        skipArray.Add(10);
        skipArray.Add(5);
        skipArray.Add(20);
        skipArray.Add(15);
        skipArray.Add(25);

        Console.WriteLine("Skip Array after additions:");
        foreach (var item in skipArray)
        {
            Console.Write(item + " "); // Output: 5 10 15 20 25 
        }
        Console.WriteLine();

        // Searching for elements
        Console.WriteLine("Contains 15: " + skipArray.Contains(15)); // Output: Contains 15: True
        Console.WriteLine("Contains 12: " + skipArray.Contains(12)); // Output: Contains 12: False

        // Removing elements
        bool removed = skipArray.Remove(15);
        Console.WriteLine("Removed 15: " + removed); // Output: Removed 15: True
        removed = skipArray.Remove(100);
        Console.WriteLine("Removed 100: " + removed); // Output: Removed 100: False

        Console.WriteLine("Skip Array after removal:");
        foreach (var item in skipArray)
        {
            Console.Write(item + " "); // Output: 5 10 20 25 
        }
        Console.WriteLine();

        Console.WriteLine("Count: " + skipArray.Count); // Output: Count: 4
        Console.WriteLine("Is Empty: " + skipArray.IsEmpty); // Output: Is Empty: False
    }
}
```

## Detailed Explanation

The `SkipArray<T>` is implemented using a probabilistic multi-level linked list structure. Each node in the skip array has a `Value`, a `Next` pointer to the next node on the same level, a `Down` pointer to the corresponding node on the level below, and a `Span` which indicates how many nodes are skipped by the `Next` pointer on the current level. A sentinel `head` node is used at the top-left of the structure.

1.  **Node Structure:**
    *   `Value`: The data stored in the node.
    *   `Next`: Points to the next node in the same linked list (level).
    *   `Down`: Points to the node directly below it in the next lower level.
    *   `Span`: Represents the number of nodes between the current node and `Next` node on the lowest level. This is crucial for efficient traversal and span adjustments.

2.  **Levels and Probabilities:**
    *   The skip array has multiple levels of linked lists. The lowest level contains all elements in sorted order.
    *   When a new element is added, its height (number of levels it participates in) is determined probabilistically. A coin flip (simulated by `random.NextDouble() < PROBABILITY`) decides if the node should be promoted to the next level. This continues until the coin flip fails or the maximum level is reached.
    *   If a new node's height exceeds the current maximum height of the skip array, new levels are added to the `head` of the structure.

3.  **`Add(T item)` Operation:**
    *   The algorithm traverses from the highest level of the `head` node downwards. At each level, it moves `Next` as long as the next node's value is less than the item to be inserted.
    *   During this traversal, it keeps track of the last node visited at each level before moving down. These nodes are stored in an `update` list, which essentially stores the predecessors of the insertion point at each level.
    *   After determining the levels for the new node, it iterates from the lowest relevant level upwards. For each level, it creates a new node, links it into the list using the predecessor from the `update` list, and sets its `Down` pointer to the node created at the level below.
    *   Crucially, the `Span` values are updated. When a node is inserted, the `Span` of the predecessor node is adjusted, and the new node's `Span` is calculated based on the original `Span` of the predecessor and the `Span` of the node it was inserted before.

4.  **`Remove(T item)` Operation:**
    *   Similar to `Add`, the algorithm traverses the skip array to find the node to be removed and records the predecessors at each level in the `update` list.
    *   It then checks if the item actually exists at the lowest level.
    *   If found, it iterates from the lowest relevant level upwards. At each level where the node exists, it unlinks the node by adjusting the `Next` pointer of its predecessor and updates the `Span` of the predecessor.
    *   The `Span` of nodes that were "skipped over" by the removed node on higher levels are decremented.
    *   Finally, any empty levels at the top of the skip array are removed.

5.  **`Contains(T item)` Operation:**
    *   This operation is similar to the traversal part of `Add` and `Remove`. It starts from the highest level and moves `Next` as far as possible without overshooting the target item. Then, it moves `Down` to the next level and repeats the process.
    *   If at any point the `Next` node's value matches the item, `true` is returned. If the traversal reaches the end of the lowest level without finding the item, `false` is returned.

6.  **`IEnumerable<T>` Implementation:**
    *   The `GetEnumerator` method provides a way to iterate through the elements of the skip array in sorted order. It first traverses down to the lowest level and then iterates through the linked list at that level.

## Complexity Analysis

Let `n` be the number of elements in the Skip Array.

*   **Time Complexity:**
    *   **Search (`Contains`)**: Average: O(log n), Worst: O(n)
    *   **Insertion (`Add`)**: Average: O(log n), Worst: O(n)
    *   **Deletion (`Remove`)**: Average: O(log n), Worst: O(n)
    *   **Iteration (`GetEnumerator`)**: O(n)

*   **Space Complexity:**
    *   **Overall**: O(n)
    *   The probabilistic nature of the levels means that, on average, the number of nodes is proportional to `n`. In the worst case, all nodes might be promoted to all levels, leading to O(n log n) or even O(n^2) space if not carefully managed, but the probability of this is extremely low. The `Span` property adds a constant amount of space per node.
