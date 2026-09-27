# Median Maintenance

## Introduction
The Median Maintenance algorithm efficiently maintains the median of a stream of integers using two heaps: a max-heap for the lower half of the numbers and a min-heap for the upper half. This is particularly useful in scenarios where you need to continuously process incoming data and frequently retrieve the median value.

## Usage
```csharp
var medianMaintenance = new MedianMaintenance();
medianMaintenance.Insert(1);
medianMaintenance.Insert(5);
medianMaintenance.Insert(2);
var median = medianMaintenance.GetMedian(); // Returns 2
medianMaintenance.Insert(3);
median = medianMaintenance.GetMedian(); // Returns 2.5
```

## Detailed Explanation
The `MedianMaintenance` class uses two priority queues (heaps) to maintain the lower and upper halves of the data stream. The max-heap stores the smaller half of the numbers, while the min-heap stores the larger half. When a new number is inserted, it is added to the appropriate heap based on its value. After each insertion, the heaps are balanced to ensure that the size difference is at most one. The median is then calculated based on the sizes of the heaps: if the max-heap has more elements, the median is the top of the max-heap; if both heaps are of equal size, the median is the average of the tops of both heaps.

## Complexity Analysis
- **Insertion:** O(log n) due to the heap operations.
- **Get Median:** O(1) since it only involves accessing the top elements of the heaps.
- **Space Complexity:** O(n) for storing the elements in the heaps.