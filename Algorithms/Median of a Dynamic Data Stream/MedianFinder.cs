using System;
using System.Collections.Generic;

public class MedianFinder {
    private readonly PriorityQueue<int, int> maxHeap; // Max-heap for the lower half
    private readonly PriorityQueue<int, int> minHeap; // Min-heap for the upper half

    public MedianFinder() {
        maxHeap = new PriorityQueue<int, int>();
        minHeap = new PriorityQueue<int, int>();
    }

    public void AddNum(int num) {
        // Add to maxHeap (lower half)
        maxHeap.Enqueue(num, -num);

        // Balance the heaps: maxHeap can have at most one more element than minHeap
        if (maxHeap.Count > minHeap.Count + 1) {
            minHeap.Enqueue(maxHeap.Dequeue(), maxHeap.Peek());
        }

        // Ensure all elements in minHeap are greater than those in maxHeap
        if (minHeap.Count > 0 && maxHeap.Peek() > minHeap.Peek()) {
            minHeap.Enqueue(maxHeap.Dequeue(), minHeap.Peek());
            maxHeap.Enqueue(minHeap.Dequeue(), -minHeap.Peek());
        }
    }

    public double FindMedian() {
        if (maxHeap.Count > minHeap.Count) {
            return maxHeap.Peek();
        } else {
            return (maxHeap.Peek() + minHeap.Peek()) / 2.0;
        }
    }
}