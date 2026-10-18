# Min-Heap Data Structure

## 1. Introduction

A Min-Heap is a specialized tree-based data structure that satisfies the heap property: if `P` is a parent node of `C`, then the value of `P` is less than or equal to the value of `C`. This property ensures that the smallest element is always at the root of the heap. It is a complete binary tree, meaning all levels are fully filled except possibly the last level, which is filled from left to right.

Min-Heaps are commonly used to implement priority queues, where elements are retrieved based on their priority (the smallest element having the highest priority). They are also fundamental to algorithms like Heap Sort and for efficiently finding the minimum element in a collection.

## 2. Usage

Here's how to use the `MinHeap<T>` class in C#:

```csharp
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        // Example 1: Min-Heap of integers using default comparer
        MinHeap<int> minHeap = new MinHeap<int>();
        Console.WriteLine("Is heap empty? " + minHeap.IsEmpty); // Output: True

        minHeap.Insert(10);
        minHeap.Insert(4);
        minHeap.Insert(15);
        minHeap.Insert(20);
        minHeap.Insert(1);

        Console.WriteLine("Heap count: " + minHeap.Count); // Output: 5
        Console.WriteLine("Min element (peek): " + minHeap.PeekMin()); // Output: 1

        Console.WriteLine("Extracting min: " + minHeap.ExtractMin()); // Output: 1
        Console.WriteLine("New min element (peek): " + minHeap.PeekMin()); // Output: 4

        minHeap.Insert(2);
        Console.WriteLine("Min element after inserting 2: " + minHeap.PeekMin()); // Output: 2

        while (!minHeap.IsEmpty)
        {
            Console.WriteLine("Extracted: " + minHeap.ExtractMin());
        }
        // Expected output: 2, 4, 10, 15, 20

        Console.WriteLine("Is heap empty? " + minHeap.IsEmpty); // Output: True

        // Example 2: Min-Heap of custom objects using a custom comparer
        Console.WriteLine("\n--- Custom Object Min-Heap ---");
        var people = new MinHeap<Person>(new PersonAgeComparer());
        people.Insert(new Person("Alice", 30));
        people.Insert(new Person("Bob", 25));
        people.Insert(new Person("Charlie", 35));
        people.Insert(new Person("David", 20));

        Console.WriteLine("Youngest person (peek): " + people.PeekMin().Name); // Output: David
        Console.WriteLine("Extracting youngest: " + people.ExtractMin().Name); // Output: David
        Console.WriteLine("New youngest person (peek): " + people.PeekMin().Name); // Output: Bob
    }
}

// Custom class for demonstration
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

// Custom comparer for Person objects based on Age
public class PersonAgeComparer : IComparer<Person>
{
    public int Compare(Person x, Person y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1; // null is considered smaller
        if (y == null) return 1;
        return x.Age.CompareTo(y.Age);
    }
}
```

## 3. Detailed Explanation

This `MinHeap<T>` implementation uses a `List<T>` internally to represent the complete binary tree. The root of the heap is always at index `0`. For any node at index `i`:
- Its left child is at `2*i + 1`.
- Its right child is at `2*i + 2`.
- Its parent is at `(i - 1) / 2` (using integer division).

The class is generic (`<T>`), allowing it to store any type of elements. It supports custom comparison logic by accepting an `IComparer<T>` in its constructor. If no comparer is provided, it defaults to `Comparer<T>.Default`, which requires `T` to implement `IComparable<T>` or `IComparable`.

### Core Operations:

1.  **`Insert(T item)`**: 
    - The new `item` is added to the end of the `_heap` list, maintaining the complete binary tree property.
    - To restore the min-heap property, the `HeapifyUp()` method is called. This method starts from the newly added item's position and iteratively compares it with its parent. If the item is smaller than its parent, they are swapped. This process continues upwards until the item is at the root or is no longer smaller than its parent, ensuring the smallest element bubbles up to its correct position.

2.  **`ExtractMin()`**: 
    - First, it checks if the heap is empty and throws an `InvalidOperationException` if it is.
    - The smallest item (which is always at the root, index `0`) is stored to be returned.
    - The last element in the `_heap` list is moved to the root position.
    - The last element is then removed from the list.
    - To restore the min-heap property, the `HeapifyDown()` method is called. This method starts from the new root and iteratively compares it with its children. It finds the smaller of its two children. If the current node is larger than its smaller child, they are swapped. This process continues downwards until the node is a leaf or is no longer larger than its children, ensuring the element sinks down to its correct position.

3.  **`PeekMin()`**: 
    - This method simply returns the element at the root (index `0`) without modifying the heap. It also throws an `InvalidOperationException` if the heap is empty.

4.  **`Count`**: 
    - A property that returns the number of elements currently in the heap, directly reflecting the `_heap.Count`.

5.  **`IsEmpty`**: 
    - A property that returns `true` if the heap contains no elements, `false` otherwise.

## 4. Complexity Analysis

Let `N` be the number of elements in the heap.

### Time Complexity:

-   **`Insert(T item)`**: O(log N)
    -   Adding an element to the `List<T>` is O(1) amortized.
    -   `HeapifyUp()` involves traversing up the tree from a leaf to the root in the worst case. The height of a complete binary tree with N nodes is O(log N). Thus, `Insert` is O(log N).

-   **`ExtractMin()`**: O(log N)
    -   Accessing the root, moving the last element, and removing the last element from `List<T>` are all O(1).
    -   `HeapifyDown()` involves traversing down the tree from the root to a leaf in the worst case. The height of the tree is O(log N). Thus, `ExtractMin` is O(log N).

-   **`PeekMin()`**: O(1)
    -   Accessing the root element at index `0` is a constant-time operation.

-   **`Count`**: O(1)
    -   Retrieving the count from `List<T>` is a constant-time operation.

-   **`IsEmpty`**: O(1)
    -   Checking if `List<T>` is empty is a constant-time operation.

### Space Complexity:

-   **Overall**: O(N)
    -   The heap stores all `N` elements in an internal `List<T>`, which requires space proportional to the number of elements.