# Introduction
The Longest Increasing Subsequence (LIS) algorithm determines the length of the longest subsequence of a given sequence such that all elements of the subsequence are sorted in increasing order. This algorithm is useful when you need to find the longest sequence that is strictly increasing.

# Usage
```csharp
int[] nums = { 10, 22, 9, 33, 21, 50, 41, 60, 80 }; 
int length = LongestIncreasingSubsequence.LengthOfLIS(nums); 
Console.WriteLine(length); // Output: 6
```

# Detailed Explanation
The implementation uses dynamic programming to achieve an optimal time complexity of O(n^2), where n is the number of elements in the input array. It initializes an array `dp` where `dp[i]` represents the length of the longest increasing subsequence ending at index `i`. It then iterates over the input array, updating `dp[i]` whenever it finds a previous element that is smaller than the current element. Finally, it returns the maximum value in the `dp` array, which represents the length of the longest increasing subsequence.

# Complexity Analysis
* Time complexity: O(n^2), where n is the number of elements in the input array.
* Space complexity: O(n), where n is the number of elements in the input array.