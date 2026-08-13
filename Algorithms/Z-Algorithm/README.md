# Z-Algorithm

## Introduction

The Z-Algorithm is an efficient string-searching algorithm that finds all occurrences of a pattern within a text in linear time. It achieves this by pre-calculating a Z-array for a concatenated string. The Z-array for a string `S` of length `N` is an array `Z` of length `N` where `Z[i]` is the length of the longest substring starting from `S[i]` which is also a prefix of `S`.

This implementation provides two main functionalities:
1.  `CalculateZArray(string s)`: Computes the Z-array for a given string.
2.  `Search(string text, string pattern)`: Finds all occurrences of a `pattern` within a `text` using the Z-array.

It is particularly useful when you need to perform multiple pattern searches on the same text or when the pattern length is significant compared to the text length.

## Usage

```csharp
using System;
using System.Collections.Generic;
using Algorithms.String;

public class Example
{
    public static void Main(string[] args)
    {
        string text = "ABABDABACDABABCABAB";
        string pattern = "ABABCABAB";

        // Search for pattern occurrences
        List<int> occurrences = ZAlgorithm.Search(text, pattern);

        Console.WriteLine($"Pattern '{pattern}' found at indices:");
        if (occurrences.Count > 0)
        {
            foreach (int index in occurrences)
            {
                Console.WriteLine(index);
            }
        }
        else
        {
            Console.WriteLine("No occurrences found.");
        }

        // Example of calculating Z-array directly
        string s = "aabcaabxaaaz";
        int[] zArray = ZAlgorithm.CalculateZArray(s);
        Console.WriteLine($"\nZ-array for '{s}':");
        for (int i = 0; i < zArray.Length; i++)
        {
            Console.WriteLine($"Z[{i}] = {zArray[i]}");
        }
    }
}
```

## Detailed Explanation

### `CalculateZArray(string s)`

This method constructs the Z-array for a given string `s` in linear time. It uses a sliding window approach (`[l, r]`) that represents the current Z-box (a substring that matches a prefix of `s`).

1.  **Initialization**: `z` array is created, `l` and `r` are initialized to 0.
2.  **Iteration**: The algorithm iterates from `i = 1` to `n-1` (where `n` is the length of `s`).
3.  **Inside Z-box**: If `i` falls within the current Z-box (`i <= r`), the value of `z[i]` can be at least `min(r - i + 1, z[i - l])`. This is because the substring `s[i...r]` is identical to `s[0...r-i]`, and `z[i-l]` tells us the length of the prefix match starting at `s[i-l]`. We take the minimum to ensure we don't go beyond the current Z-box (`r`).
4.  **Outside Z-box or Extending**: If `i` is outside the Z-box (`i > r`) or if the initial guess for `z[i]` (from step 3) needs to be extended, a `while` loop compares characters `s[z[i]]` and `s[i + z[i]]` to find the actual length of the prefix match starting at `s[i]`. This loop expands `z[i]` as long as characters match and stay within string bounds.
5.  **Updating Z-box**: If the current match starting at `i` extends beyond the current Z-box (`i + z[i] - 1 > r`), the Z-box is updated by setting `l = i` and `r = i + z[i] - 1`.

### `Search(string text, string pattern)`

This method leverages `CalculateZArray` to find all occurrences of `pattern` in `text`.

1.  **Edge Cases**: Handles null strings, empty patterns (which match everywhere), and patterns longer than the text.
2.  **Concatenation**: A new string `combined` is formed by concatenating `pattern`, a sentinel character (e.g., `$`, assumed not to be in `text` or `pattern`), and `text`. The sentinel ensures that matches do not cross the boundary between `pattern` and `text` in an unintended way.
3.  **Z-Array Calculation**: The `CalculateZArray` method is called on the `combined` string.
4.  **Finding Matches**: The algorithm iterates through the `z` array starting from index `m + 1` (where `m` is the length of `pattern`). If `z[i]` is equal to `m`, it signifies that the substring of `combined` starting at index `i` matches the prefix of `combined` (which is the `pattern`) for `m` characters. This means the `pattern` has been found in the `text` starting at the corresponding index in the original `text`. The starting index in `text` is calculated as `i - m - 1`.
5.  **Result**: All such starting indices are collected and returned in a `List<int>`.

## Complexity Analysis

### `CalculateZArray(string s)`

*   **Time Complexity**: O(N), where N is the length of the string `s`. Although there is a nested `while` loop, the `r` pointer (right boundary of the Z-box) only moves forward. The total number of character comparisons is proportional to N.
*   **Space Complexity**: O(N), for storing the Z-array.

### `Search(string text, string pattern)`

*   **Time Complexity**: O(N + M), where N is the length of the `text` and M is the length of the `pattern`. This is because the `CalculateZArray` is called on a string of length `N + M + 1`, and the subsequent scan of the Z-array takes O(N + M) time.
*   **Space Complexity**: O(N + M), for storing the concatenated string and its Z-array.
