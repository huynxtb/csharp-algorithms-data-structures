# Introduction
The Top-K Frequent Elements algorithm is used to find the top K frequent elements in an unsorted array of integers. This algorithm is useful when you need to identify the most common elements in a dataset.

# Usage
```csharp
var topKFrequentElements = new TopKFrequentElements();
var nums = new int[] { 1, 1, 1, 2, 2, 3 };
var k = 2;
var result = topKFrequentElements.TopKFrequent(nums, k);
Console.WriteLine(string.Join(" ", result));
```

# Detailed Explanation
The implementation of the Top-K Frequent Elements algorithm involves two main steps:
1.  **Frequency Counting:** Create a dictionary to store the frequency of each element in the input array.
2.  **Priority Queue:** Use a priority queue to find the top K frequent elements. The priority queue is sorted in descending order based on the frequency of the elements.

# Complexity Analysis
*   **Time Complexity:** O(n log k), where n is the number of elements in the input array and k is the number of top frequent elements to find. The time complexity is dominated by the priority queue operations.
*   **Space Complexity:** O(n), where n is the number of elements in the input array. The space complexity is dominated by the dictionary used for frequency counting and the priority queue.