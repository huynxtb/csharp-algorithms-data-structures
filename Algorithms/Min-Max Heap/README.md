# Min-Max Heap

## Introduction
The Min-Max Heap is a complete binary tree that efficiently supports both minimum and maximum queries in O(log n) time. It combines the functionality of a min-heap and a max-heap into a single data structure, making it suitable for scenarios where both extremes of a dataset need to be managed simultaneously.

## Usage
```csharp
MinMaxHeap<int> minMaxHeap = new MinMaxHeap<int>();
minMaxHeap.Insert(5);
minMaxHeap.Insert(10);
minMaxHeap.Insert(3);
minMaxHeap.Insert(8);
Console.WriteLine($