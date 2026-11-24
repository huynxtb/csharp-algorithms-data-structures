# Median of a Dynamic Data Stream

## Introduction
The Median of a Dynamic Data Stream algorithm efficiently computes the median of a stream of integers that can change over time. It allows for the insertion and removal of elements while maintaining the ability to quickly retrieve the current median. This algorithm is particularly useful in scenarios where data is continuously flowing, such as in real-time analytics or streaming applications.

## Usage
```csharp
MedianFinder medianFinder = new MedianFinder();
medianFinder.AddNum(1);
medianFinder.AddNum(2);
Console.WriteLine(medianFinder.FindMedian()); // Output: 1.5
medianFinder.AddNum(3);
Console.WriteLine(medianFinder.FindMedian()); // Output: 2
```

## Detailed Explanation
The implementation uses two heaps to maintain the balance of the numbers:
- A max-heap (`maxHeap`) to store the lower half of the numbers.
- A min-heap (`minHeap`) to store the upper half of the numbers.

When a new number is added, it is first pushed into the max-heap. If the max-heap exceeds the size of the min-heap by more than one, the largest element from the max-heap is moved to the min-heap to maintain balance. Additionally, if the smallest element in the min-heap is less than the largest element in the max-heap, elements are swapped between the heaps to ensure the max-heap contains only elements less than or equal to those in the min-heap.

The median is then calculated based on the sizes of the heaps:
- If the max-heap has more elements, the median is the top of the max-heap.
- If both heaps are of equal size, the median is the average of the tops of both heaps.

## Complexity Analysis
- **Time Complexity:**
  - `AddNum(int num)`: O(log n) due to heap operations.
  - `FindMedian()`: O(1) as it only accesses the tops of the heaps.
- **Space Complexity:** O(n) for storing the elements in the heaps.