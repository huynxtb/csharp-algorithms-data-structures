# Introduction
The Longest Common Subsequence (LCS) algorithm is a dynamic programming approach used to find the longest contiguous or non-contiguous substring common to two strings. It is commonly used in data comparison and analysis tasks.

# Usage
```csharp
string str1 = "ABCBDAB";
string str2 = "BDCABA";
string lcs = LongestCommonSubsequence.CalculateLCS(str1, str2);
Console.WriteLine(lcs);
```

# Detailed Explanation
The implementation works by creating a 2D array `dp` where `dp[i, j]` represents the length of the LCS between the first `i` characters of `str1` and the first `j` characters of `str2`. The LCS is then constructed by backtracking through the `dp` array.

# Complexity Analysis
* Time complexity: O(m * n), where m and n are the lengths of the input strings.
* Space complexity: O(m * n), for the `dp` array.