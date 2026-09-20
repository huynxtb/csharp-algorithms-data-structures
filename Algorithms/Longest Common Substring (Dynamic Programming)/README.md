# Longest Common Substring (Dynamic Programming)

## Introduction

The **Longest Common Substring (LCS)** algorithm finds the longest contiguous sequence of characters that appears in the same order in two given strings. Unlike the Longest Common Subsequence problem, the characters in a common substring must be consecutive in both strings.

This implementation uses a **Dynamic Programming (DP)** approach to efficiently solve the problem in polynomial time. It is commonly used in:

- **Bioinformatics:** Comparing DNA or protein sequences for shared segments.
- **Plagiarism Detection:** Identifying copied passages between documents.
- **Data Deduplication:** Finding repeated content across datasets.
- **Text Comparison and Diff Tools:** Highlighting similar sections in files.
- **Spell Checking and Autocomplete:** Matching partial input against known words.

## Usage

Below is a clear example of how to use the `LongestCommonSubstring` class:

```csharp
using System;

class Program
{
    static void Main()
    {
        LongestCommonSubstring lcs = new LongestCommonSubstring();

        // Example 1: Finding the longest common substring
        string result1 = lcs.FindLongestCommonSubstring("abcdef", "fbdamn");
        // result1 is an empty string since no contiguous match longer than 1 exists
        // Actually for "abcdef" and "zbcdf", the result would be "bcd"

        string result2 = lcs.FindLongestCommonSubstring("ABABC", "BABCBA");
        // result2 is "BABC"

        // Example 2: Getting only the length
        int length = lcs.GetLongestCommonSubstringLength("aaaa", "aaaa");
        // length is 4

        // Example 3: No common substring
        string result3 = lcs.FindLongestCommonSubstring("abc", "xyz");
        // result3 is "" (empty string)

        // Example 4: Single character match
        string result4 = lcs.FindLongestCommonSubstring("a", "a");
        // result4 is "a"

        // Example 5: Handling empty strings
        string result5 = lcs.FindLongestCommonSubstring("", "hello");
        // result5 is "" (empty string)

        Console.WriteLine(result2); // Output: BABC
        Console.WriteLine(length);  // Output: 4
    }
}
```

## Detailed Explanation

### How the Algorithm Works

The algorithm uses a 2D DP table of dimensions `(m+1) x (n+1)`, where `m` and `n` are the lengths of `text1` and `text2` respectively.

1. **Initialization:** A 2D array `dp` is created where `dp[i][j]` represents the length of the longest common substring ending at `text1[i-1]` and `text2[j-1]`. All values are initialized to 0.

2. **Filling the Table:** For each pair of indices `(i, j)` from 1 to m and 1 to n respectively:
   - If `text1[i-1] == text2[j-1]`, then `dp[i][j] = dp[i-1][j-1] + 1`. This extends the common substring ending at the previous characters by one.
   - If the characters do not match, `dp[i][j] = 0` because the contiguous substring is broken.

3. **Tracking the Maximum:** While filling the table, the algorithm keeps track of:
   - `maxLength`: The length of the longest common substring found so far.
   - `endingIndex`: The ending position (in `text1`) of the longest common substring.

4. **Reconstructing the Result:** Once the table is fully filled, the longest common substring is extracted from `text1` using `Substring(endingIndex - maxLength, maxLength)`.

### Input Validation

- **Null inputs** cause an `ArgumentNullException` to be thrown with a descriptive message.
- **Empty strings** are handled gracefully by returning an empty string (or 0 for the length method).

### Example Walkthrough

For `text1 = "aaaa"` and `text2 = "aaaa"`:

|   |   | a | a | a | a |
|---|---|---|---|---|---|
|   | 0 | 0 | 0 | 0 | 0 |
| a | 0 | 1 | 1 | 1 | 1 |
| a | 0 | 1 | 2 | 2 | 2 |
| a | 0 | 1 | 2 | 3 | 3 |
| a | 0 | 1 | 2 | 3 | 4 |

The maximum value in the table is 4, and the resulting substring is `"aaaa"`.

## Complexity Analysis

| Operation | Time Complexity | Space Complexity |
|-----------|----------------|------------------|
| `FindLongestCommonSubstring` | O(m × n) | O(m × n) |
| `GetLongestCommonSubstringLength` | O(m × n) | O(m × n) |

- **Time Complexity: O(m × n)** — The algorithm iterates through every cell of the 2D DP table exactly once, where `m` is the length of `text1` and `n` is the length of `text2`.
- **Space Complexity: O(m × n)** — The 2D DP table requires `(m+1) × (n+1)` space. For very large strings, a space-optimized version using two 1D arrays (O(min(m, n))) could be implemented, but the current implementation prioritizes clarity.

This implementation is well-suited for strings of moderate length (up to approximately 1000 characters each), as the table size remains manageable at roughly 1 million entries.