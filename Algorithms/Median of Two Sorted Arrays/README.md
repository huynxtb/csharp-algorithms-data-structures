# Introduction
The Median of Two Sorted Arrays algorithm is used to find the median of two sorted arrays. It merges the two input arrays and calculates the median. The median of two sorted arrays is the middle value in the sorted array that results from merging the two input arrays.

# Usage
```csharp
int[] nums1 = { 1, 3 }; int[] nums2 = { 2 }; double median = MedianOfTwoSortedArrays.FindMedianSortedArrays(nums1, nums2);
Console.WriteLine(median); // Output: 2
```

# Detailed Explanation
The implementation first merges the two input arrays into a single array. It then checks if the length of the merged array is even or odd. If the length is even, the median is the average of the two middle elements. If the length is odd, the median is the middle element.

# Complexity Analysis
* Time complexity: O(m + n), where m and n are the lengths of the input arrays.
* Space complexity: O(m + n), where m and n are the lengths of the input arrays.