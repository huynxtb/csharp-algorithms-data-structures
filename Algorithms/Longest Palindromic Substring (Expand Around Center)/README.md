# Longest Palindromic Substring (Expand Around Center)

## 1. Introduction

The **Longest Palindromic Substring** problem asks: given a string, find the longest contiguous substring that reads the same forwards and backwards. A palindrome is a sequence that is symmetric around its center (e.g., `racecar`, `abba`).

The **Expand Around Center** approach is an elegant and intuitive algorithm that leverages the observation that every palindrome has a center. For a string of length `n`, there are `2n - 1` possible centers: `n` centers for odd-length palindromes (each character) and `n - 1` centers for even-length palindromes (each gap between adjacent characters).

**When to use it:**
- When you need a simple, readable O(n²) solution without the complexity of Manacher's algorithm.
- For inputs up to ~10,000 characters where O(n²) is acceptable.
- When memory usage must remain constant (O(1) auxiliary space).

## 2. Usage

```csharp
public class Example
{
    public void Run()
    {
        var solver = new LongestPalindromicSubstring();

        string result1 = solver.FindLongestPalindrome("babad");   // "bab" (or "aba")
        string result2 = solver.FindLongestPalindrome("cbbd");    // "bb"
        string result3 = solver.FindLongestPalindrome("racecar"); // "racecar"
        string result4 = solver.FindLongestPalindrome("");         // ""
        string result5 = solver.FindLongestPalindrome("a");        // "a"
    }
}
```

## 3. Detailed Explanation

The implementation is composed of two methods:

### `FindLongestPalindrome(string input)`
1. Handles edge cases: `null`, empty, or single-character inputs are returned immediately.
2. Iterates over each index `i` in the string, treating it as a potential center.
3. For each `i`, invokes `ExpandAroundCenter` twice:
   - Once with `(i, i)` to check for **odd-length** palindromes (single-character center).
   - Once with `(i, i + 1)` to check for **even-length** palindromes (center between two characters).
4. Tracks the maximum palindrome length and computes its starting index using:
   
   `startIndex = i - (maxLength - 1) / 2`

5. Returns the substring from `startIndex` of length `maxLength`.

### `ExpandAroundCenter(string input, int left, int right)`
1. Moves `left` and `right` outward from the given center as long as:
   - Both indices are within the string's bounds.
   - `input[left] == input[right]` (case-sensitive comparison).
2. When mismatch or out-of-bounds is encountered, the loop terminates, having moved one step too far.
3. Returns the palindrome's length: `right - left - 1`.

By considering both odd and even center configurations, the algorithm guarantees that every possible palindrome is examined.

## 4. Complexity Analysis

| Operation | Time Complexity | Space Complexity |
|-----------|-----------------|------------------|
| `FindLongestPalindrome` | **O(n²)** | **O(1)** auxiliary (excluding output) |
| `ExpandAroundCenter` | **O(n)** per call | **O(1)** |

- **Time:** For each of the `2n - 1` centers, expansion can take up to O(n) time in the worst case (e.g., strings like `"aaaaaa"`), leading to O(n²) overall.
- **Space:** Only a few integer variables are used. The output substring itself requires O(n) space but is not counted as auxiliary space.
